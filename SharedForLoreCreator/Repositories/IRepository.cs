using SharedForLoreCreator.Models;
namespace SharedForLoreCreator.Repositories;

public interface IRepository<DbModel> where DbModel : BaseModel
{
    public bool isExist(int id);

    public DbModel GetById(int id);

    public int Add(DbModel dbModel);

    public void Delete(int id);

    public bool Any();
}


public interface IRepositoryAsync<DbModel> where DbModel : BaseModel
{
    public Task<bool> IsExistAsync(int id);

    public Task<DbModel?> GetByIdAsync(int id);

    public Task<List<DbModel>> GetAllAsync();

    public Task<bool> AnyAsync();
}