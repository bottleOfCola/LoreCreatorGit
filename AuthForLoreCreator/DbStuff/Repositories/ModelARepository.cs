using AuthForLoreCreator.DbStuff.Models;
using SharedForLoreCreator.Repositories;

namespace AuthForLoreCreator.DbStuff.Repositories;

public class ModelARepository : BaseDbRepository<ModelAForOneToOne, AuthForLoreCreatorDbContext>
{
    public ModelARepository(AuthForLoreCreatorDbContext context) : base(context) { }

    public void AddBModel(int id, ModelBForOneToOne model)
    {
        if (GetById(id) is ModelAForOneToOne bModel)
        {
            bModel.PartnerFromB = model;
            _context.SaveChanges();
        }
    }
}
