using ChatForLoreCreator.DbStuff.Models;
using SharedForLoreCreator.Repositories;

namespace ChatForLoreCreator.DbStuff.Repositories;

public class MessageRepository : BaseDbRepository<Message, ChatForLoreCreatorDbContext>
{
    public MessageRepository(ChatForLoreCreatorDbContext context) : base(context)
    {
    }
}
