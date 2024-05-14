using ChatForLoreCreator.DbStuff.Models;
using Microsoft.EntityFrameworkCore;
using SharedForLoreCreator.Repositories;

namespace ChatForLoreCreator.DbStuff.Repositories;

public class ChatRepository : BaseDbRepository<Chat, ChatForLoreCreatorDbContext>
{
    public ChatRepository(ChatForLoreCreatorDbContext context) : base(context)
    {
    }
    public IEnumerable<Message> GetMessagesFromConcreteChat(int id, int count)
    {
        var a = _entyties.Include(x => x.Messages).First(x => x.Id == id);
        var b = a.Messages
                                        .OrderByDescending(x => x.DateTime)
                                        .Take(count)
                                        .Reverse();
        return b;
    }
        

    public int GetIdByName(string name)
    {
        return _entyties.FirstOrDefault(x => x.Name == name).Id;
    }
    public bool isExistByName(string name)
    {
        return _entyties.Any(x => x.Name == name);
    }
    public void DeleteByName(string name)
    {
        Delete(GetIdByName(name));
    }
}
