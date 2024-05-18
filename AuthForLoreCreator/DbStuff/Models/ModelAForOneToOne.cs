using SharedForLoreCreator.Models;

namespace AuthForLoreCreator.DbStuff.Models;

public class ModelAForOneToOne : BaseModel
{
    public string Name { get; set; }

    public ModelBForOneToOne? PartnerFromB { get; set; }
}
