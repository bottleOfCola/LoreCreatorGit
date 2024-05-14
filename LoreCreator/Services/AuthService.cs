using LoreCreator.DbStuff.Repositories;
using SharedForLoreCreator.Models;

namespace LoreCreator.Services;

public class AuthService
{
    private UserRepository _userRepository;
    private IHttpContextAccessor _contextAccessor;

    public AuthService(UserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _contextAccessor = httpContextAccessor;
    }
    public int GetCurrentUserId()
    {
        return int.Parse(_contextAccessor.HttpContext.User.Claims.First(x => x.Type == "id").Value);
    }

    public async Task<SharedUser> GetCurrentUserAsync()
    {
        return await _userRepository.GetByIdAsync(GetCurrentUserId());
    }

    public async Task<bool> IsUserHavePermissionAsync(PermissionTypes permissionType)
    {
        int id = int.Parse(_contextAccessor.HttpContext.User.Claims.First(x => x.Type == "id").Value);
        return await _userRepository.IsUserHavePermissionAsync(permissionType, GetCurrentUserId());
    }
}