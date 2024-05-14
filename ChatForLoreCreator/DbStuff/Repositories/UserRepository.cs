using ChatForLoreCreator.Services;
using SharedForLoreCreator.Models;
using SharedForLoreCreator.Repositories;

namespace ChatForLoreCreator.DbStuff.Repositories;

public class UserRepository
{
    private AuthSericeWorker _authSericeWorker;
    public UserRepository(AuthSericeWorker authSericeWorker)
    {
        _authSericeWorker = authSericeWorker;
    }

    public async Task<bool> AnyAsync() => await _authSericeWorker.AnyAsync();

    public async Task<List<SharedUser>> GetAllAsync()
    {
        return await _authSericeWorker.GetAllAsync();
    }

    public async Task<SharedUser?> GetByIdAsync(int id)
    {
        return await _authSericeWorker.GetByIdAsync(id);
    }

    public async Task<bool> IsExistAsync(int id) => await _authSericeWorker.IsExistAsync(id);

    public async Task<bool> IsUserHavePermission(int userId, PermissionTypes permission) => await _authSericeWorker.IsUserHavePermissionAsync(userId, permission);
}
