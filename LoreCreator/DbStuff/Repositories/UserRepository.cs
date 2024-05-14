using LoreCreator.Services;
using LoreCreator.ViewModels;
using SharedForLoreCreator.Repositories;
using SharedForLoreCreator.Models;

namespace LoreCreator.DbStuff.Repositories;

public class UserRepository : IRepositoryAsync<SharedUser>
{
    private AuthServiceWorker _authSericeWorker;
    public UserRepository(AuthServiceWorker authSericeWorker)
    {
        _authSericeWorker = authSericeWorker;
    }

    public async Task<bool> AddAsync(RegisterViewModel registerViewModel) => await _authSericeWorker.AddAsync(registerViewModel);

    public async Task<bool> DeleteAsync(int id) => await _authSericeWorker.DeleteAsync(id);

    public async Task<SharedUser?> GetUserByEmailAndPasswordAsync(LoginViewModel loginViewModel) => await _authSericeWorker.GetByEmailAndPasswordAsync(loginViewModel);

    public async Task<bool> AnyAsync() => await _authSericeWorker.AnyAsync();

    public async Task<List<SharedUser>> GetAllAsync() => await _authSericeWorker.GetAllAsync();

    public async Task<SharedUser?> GetByIdAsync(int id) => await _authSericeWorker.GetByIdAsync(id);

    public async Task<bool> IsUserHavePermissionAsync(PermissionTypes permissionType, int id) => await _authSericeWorker.IsUserHavePermissionAsync(id, permissionType);

    public async Task<bool> IsExistAsync(int id) => await _authSericeWorker.IsExistAsync(id);

    public async Task<List<PermissionTypes>> GetUserPermissionsAsync(int id) => await _authSericeWorker.GetUserPermissions(id);

    public async Task<bool> IsRoleExistAsync(int id) => await _authSericeWorker.IsRoleExistAsync(id);

    public async Task<SharedRole?> GetRoleAsync(int id) => await _authSericeWorker.GetRoleAsync(id);
}
