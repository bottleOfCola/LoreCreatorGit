using ChatForLoreCreator.DbStuff.Models;
using ChatForLoreCreator.DbStuff.Repositories;
using Microsoft.AspNetCore.SignalR;
using SharedForLoreCreator.Models;
using static System.Net.Mime.MediaTypeNames;

namespace ChatForLoreCreator.SignalRChatHubs;

public class ChatHub : Hub
{
    protected ChatRepository _chatRepository;
    protected MessageRepository _messageRepository;
    protected UserRepository _userRepository;

    public ChatHub(ChatRepository chatRepository, MessageRepository messageRepository, UserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public void SendMessage(string groupName, int userId, string text)
    {
        if (!_chatRepository.isExistByName(groupName))
        {
            Clients.Caller.SendAsync("Error", "This Group is not exist").Wait();
            return;
        }
        SharedUser? user = _userRepository.GetByIdAsync(userId).Result;
        if (user is null)
        {
            Clients.Caller.SendAsync("Error", "Your User is not exist").Wait();
            return;
        }
        if(!_userRepository.IsUserHavePermission(userId, PermissionTypes.AddMessage).Result)
        {
            Clients.Caller.SendAsync("Error", "Your User dont have permission").Wait();
            return;
        }
        Message message = new()
        {
            Chat = _chatRepository.GetById(_chatRepository.GetIdByName(groupName)),
            DateTime = DateTime.Now,
            UserId = userId,
            Text = text
        };
        _messageRepository.Add(message);
        Clients.Group(groupName).SendAsync("NewMessage", new SharedMessage(message.Id, message.DateTime, message.Text, user.Name)).Wait();
    }

    public void Enter(int userId, string groupName)
    {
        if(!_chatRepository.isExistByName(groupName))
        {
            Clients.Caller.SendAsync("Error", "This Group is not exist").Wait();
            return;
        }
        SharedUser? user = _userRepository.GetByIdAsync(userId).Result;
        if (user is null)
        {
            Clients.Caller.SendAsync("Error", "Your User is not exist").Wait();
            return;
        }
        Message message = new()
        {
            Chat = _chatRepository.GetById(_chatRepository.GetIdByName(groupName)),
            DateTime = DateTime.Now,
            UserId = 0,
            Text = $"{user.Name} присоединился!"
        };
        _messageRepository.Add(message);

        Groups.AddToGroupAsync(Context.ConnectionId, groupName.ToString()).Wait();
        Clients.Group(groupName).SendAsync("AddToGroup", message.Id, DateTime.Now, user.Name).Wait();
    }
    public void GetLastMessages(string groupName)
    {
        var oldMessages = _chatRepository.GetMessagesFromConcreteChat(_chatRepository.GetIdByName(groupName), 50)
                                        .Select(x =>
                                        {
                                            if(x.UserId == 0)
                                            {
                                                return new SharedMessage(x.Id, x.DateTime, x.Text, "System");
                                            }
                                            SharedUser? messageUser = _userRepository.GetByIdAsync(x.UserId).Result;
                                            return new SharedMessage(x.Id, x.DateTime, x.Text, messageUser?.Name ?? "deleted");
                                        })
                                        .ToList();

        Clients.Caller.SendAsync("OldMessages", oldMessages);
    }
    public void DeleteMessage(int userId, int messageId)
    {
        if(!_userRepository.IsExistAsync(userId).Result)
        {
            Clients.Caller.SendAsync("Error", "Your User is not exist").Wait();
            return;
        }
        if(!_userRepository.IsUserHavePermission(userId, PermissionTypes.RemoveMessage).Result)
        {
            Clients.Caller.SendAsync("Error", "Your User dont have permission").Wait();
            return;
        }
        if(!_messageRepository.isExist(messageId))
        {
            Clients.Caller.SendAsync("Error", "Your Message is not exist").Wait();
            return;
        }
        _messageRepository.Delete(messageId);
        Clients.Caller.SendAsync("Apply", "Message was deleted", messageId).Wait();
    }
}

record SharedMessage( int id, DateTime Time, string Text, string Name);