using LoreCreator.DbStuff.Repositories;
using LoreCreator.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedForLoreCreator.Models;
using System.Security.Claims;

namespace LoreCreator.Controllers;

public class AuthController : Controller
{
    private UserRepository _userRepository;

    public const string AUTH_KEY = "loreCreatorAuth";

    public AuthController(UserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetRole(int roleId)
    {
        if(!_userRepository.IsRoleExistAsync(roleId).Result)
        {
            return RedirectToAction("Index", "LoreCreator");
        }
        return View(_userRepository.GetRoleAsync(roleId).Result);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Profile(int? id)
    {
        if(id is null)
        {
            var strId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            int yourId;
            if (!int.TryParse(strId, out yourId))
            {
                return RedirectToAction("Index", "LoreCreator");
            }
            id = yourId;
        }
        if(!_userRepository.IsExistAsync(id.Value).Result)
        {
            ModelState.AddModelError("Profile2", "user is not exist");
        }
        var a = _userRepository.GetByIdAsync(id.Value).Result;
        return View(new ProfileViewModel()
        {
            Id = a.Id,
            Name = a.Name,
            Email = a.Email,
            Roles = a.Roles
        });
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel loginViewModel)
    {
        if(!ModelState.IsValid)
        {
            return View(loginViewModel);
        }
        if( !LoginSupport(loginViewModel))
        {
            ModelState.AddModelError("LoginFirstStepError", "Something wrong with Login");
            return View(loginViewModel);
        }
        return RedirectToAction("Index","LoreCreator" );
    }

    [HttpGet]
    public IActionResult Registration()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Registration(RegisterViewModel registerViewModel)
    {
        if(!ModelState.IsValid)
        {
            return View(registerViewModel);
        }
        if( !_userRepository.AddAsync(registerViewModel).Result)
        {
            ModelState.AddModelError("RegistrationFirstStepError", "Something wrong with Registration");
            return View(registerViewModel);
        }
        if ( !LoginSupport(new() { Password = registerViewModel.Password, Email = registerViewModel.Email }))
        {
            ModelState.AddModelError("RegistrationLastStepError","Something wrong with Logging");
            return View(registerViewModel);
        }
        return RedirectToAction("Index","LoreCreator" );
    }

   [NonAction]
    private bool LoginSupport(LoginViewModel loginViewModel)
    {
        SharedUser? user = _userRepository.GetUserByEmailAndPasswordAsync(loginViewModel).Result;
        if (user is null)
        {
            return false;
        }
        List<PermissionTypes> listOfPermissions = _userRepository.GetUserPermissionsAsync(user.Id).Result;
        string permissionsInString = string.Join(' ', listOfPermissions.ConvertAll(x => ((int)x).ToString()));
        List<Claim> claims = new()
        {
            new("id", user.Id.ToString()),
            new("name", user.Name),
            new("permissions", permissionsInString)
        };
        ClaimsIdentity identity = new(claims, AUTH_KEY);
        ClaimsPrincipal principal = new(identity);
        Response.Cookies.Append("id", user.Id.ToString());
        HttpContext.SignInAsync(AUTH_KEY, principal).Wait();
        return true;
    }
}
