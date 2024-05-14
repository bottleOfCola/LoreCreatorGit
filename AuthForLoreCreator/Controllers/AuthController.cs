using AuthForLoreCreator.DbStuff.Repositories;
using SharedForLoreCreator.Models;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AuthForLoreCreator.ViewModels;
using System.Reflection;
using AuthForLoreCreator.DbStuff.Models;

namespace AuthForLoreCreator.Controllers;

public class AuthController : Controller
{
    private UserRepository _userRepository;
    private RoleRepository _roleRepository;
    private PermissionTypeRepository _permissionTypeRepository;

    public AuthController(UserRepository userRepository, RoleRepository roleRepository, PermissionTypeRepository permissionTypeRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _permissionTypeRepository = permissionTypeRepository;
    }

    public IActionResult Index() => StatusCode(StatusCodes.Status200OK);

    public IActionResult AnyUser() => _userRepository.Any()? StatusCode(StatusCodes.Status302Found) : StatusCode(StatusCodes.Status404NotFound);

    [HttpPost]
    public IActionResult AddUser(UserViewModel user)
    {
        if(!_userRepository.EmailIsExist(user.Email) || !_userRepository.NameIsExist(user.Name))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        _userRepository.Add(new()
        {
            Name = user.Name,
            Email = user.Email,
            Roles = new() { _roleRepository.GetByName("User") },
            Password = user.Password
        });

        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpPost]
    public IActionResult ChangeNameUser(int id, string name)
    {
        if(!_userRepository.isExist(id))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }

        _userRepository.ChangeName(id, name);

        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        if(!_userRepository.isExist(id))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        _userRepository.Delete(id);
        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpGet]
    public IActionResult GetUser(int id)
    {
        SharedUser user = _userRepository.GetById(id);
        if(user is null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        Response.StatusCode = StatusCodes.Status302Found;
        Response.ContentType = "application/json";
        return Json(user);
    }

    [HttpGet]
    public IActionResult GetUserByEmailAndPassword(string email, string password)
    {
        SharedUser user = _userRepository.GetByEmailAndPassword(email, password);
        if(user is null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        Response.StatusCode = StatusCodes.Status302Found;
        Response.ContentType = "application/json";
        return Json(user);
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        if(!_userRepository.Any())
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        List<SharedUser> users = new();
        foreach(SharedUser user in _userRepository.GetAll())
        {
            users.Add(user);
        }
        Response.StatusCode = StatusCodes.Status200OK;
        Response.ContentType = "application/json";
        return Json(users);
    }

    [HttpGet]
    public IActionResult IsUserExist(int id) => _userRepository.isExist(id) ? StatusCode(StatusCodes.Status302Found) : StatusCode(StatusCodes.Status404NotFound);

    [HttpPost]
    public IActionResult AddRole(string name, List<PermissionTypes> permissions)
    {
        if(_roleRepository.isExistByName(name))
        {
            return StatusCode(StatusCodes.Status302Found);
        }
        if(permissions.Contains(PermissionTypes.Unknown))
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }
        _roleRepository.Add(new()
        {
            Name = name,
            Permissions = _permissionTypeRepository.GetManyByEnumList(permissions).ToList()
        });
        return StatusCode(StatusCodes.Status202Accepted);
    }

    [HttpGet]
    public IActionResult GetUserPermissions(int userId)
    {
        var roles = _userRepository.GetById(userId).Roles.Select(x => x.Id);
        if (roles is null) return StatusCode(StatusCodes.Status404NotFound);
        List<PermissionTypes> permissions = new();
        foreach(var role in roles)
        {
            permissions.AddRange(_roleRepository.GetById(role).Permissions.Select(y => y.Id));
        }
        Response.StatusCode = StatusCodes.Status302Found;
        Response.ContentType = "application/json";
        return Json(permissions);
    }
    [HttpGet]
    public IActionResult IsUserHavePermission(int userId, int permission)
    {
        if(!_userRepository.isExist(userId))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        if(!_permissionTypeRepository.isExist(permission))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        List<Role> roles = _userRepository.GetById(userId).Roles;
        foreach(var role in roles)
        {
            if(_roleRepository.isHavePermission(role.Id, (PermissionTypes)permission))
            {
                return StatusCode(StatusCodes.Status200OK);
            }
        }
        return StatusCode(StatusCodes.Status204NoContent);
    }

    [HttpGet]
    public IActionResult IsRoleExist(int roleId)
    {
        return _roleRepository.isExist(roleId) ? StatusCode(StatusCodes.Status302Found) : StatusCode(StatusCodes.Status404NotFound);
    }

    [HttpGet]
    public IActionResult GetRole(int roleId)
    {
        if(!_roleRepository.isExist(roleId))
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        Response.StatusCode = StatusCodes.Status302Found;
        return Json(_roleRepository.GetById(roleId));
    }
}