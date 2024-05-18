using SharedForLoreCreator.Models;

namespace AuthForLoreCreator.DbStuff.Models;

public class ModelBForOneToOne : BaseModel
{
    public string Name {  get; set; }
    public ModelAForOneToOne? PartnerFromA { get; set; }
}
