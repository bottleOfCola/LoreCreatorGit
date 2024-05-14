using ChatForLoreCreator.DbStuff.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using SharedForLoreCreator.Models;
using System.Net.Http;
using System.Xml;

namespace ChatForLoreCreator.Controllers;

public class ChatController : Controller
{
    protected ChatRepository _chatRepository;
    protected MessageRepository _messageRepository;
    protected UserRepository _userRepository;

    public ChatController(ChatRepository chatRepository, MessageRepository messageRepository, UserRepository userRepository)
    {
        _chatRepository = chatRepository;
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    public IActionResult Index()
    {
        return StatusCode((int)HttpStatusCode.OK);
    }

    [HttpGet]
    public IActionResult AddChat(string name)
    {
        _chatRepository.Add(new() { Name = name });
        return StatusCode((int)HttpStatusCode.OK);
    }

    public void LALO()
    {
        foreach(var a in _chatRepository.GetAll())
        {
            _chatRepository.Delete(a.Id);
        }
    }

    [HttpDelete]
    public IActionResult DeleteChat(string name)
    {
        if(!_chatRepository.isExistByName(name))
        {
            return StatusCode((int)HttpStatusCode.NotFound);
        }
        _chatRepository.Delete(_chatRepository.GetIdByName(name));
        return StatusCode((int)HttpStatusCode.Accepted);
    }
    [HttpGet]
    public IActionResult GetChat(int id)
    {
        var chat = _chatRepository.GetById(id);
        if(chat is null)
        {
            return StatusCode((int)HttpStatusCode.NotFound);
        }
        Response.StatusCode = (int)HttpStatusCode.Found;
        return Json(chat.Name);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMessage(int messageId, int userId)
    {
        if (!await _userRepository.IsExistAsync(userId)) return StatusCode((int)HttpStatusCode.NotFound);
        if (!await _userRepository.IsUserHavePermission(userId, PermissionTypes.RemoveMessage)) return StatusCode((int)HttpStatusCode.Locked);
        _messageRepository.Delete(messageId);
        return StatusCode((int)HttpStatusCode.OK);
    }
}
