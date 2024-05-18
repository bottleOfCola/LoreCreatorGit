using AuthForLoreCreator.DbStuff.Models;
using SharedForLoreCreator.Repositories;

namespace AuthForLoreCreator.DbStuff.Repositories;

public class ModelBRepository : BaseDbRepository<ModelBForOneToOne, AuthForLoreCreatorDbContext>
{
    public ModelBRepository(AuthForLoreCreatorDbContext context) : base(context) { }

    public void AddAModel(int id, ModelAForOneToOne model)
    {
        if(GetById(id) is ModelBForOneToOne bModel)
        {
            bModel.PartnerFromA = model;
            _context.SaveChanges();
        }
    }
}
