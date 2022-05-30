using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.Accounts;

namespace WebAdvert.Web.Controllers;

public class AccountsController : Controller
{
    private readonly SignInManager<CognitoUser> _signInManager;
    private readonly UserManager<CognitoUser> _userManager;
    private readonly CognitoUserPool _cognitoUserPool;
    public AccountsController(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool cognitoUserPool)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _cognitoUserPool = cognitoUserPool;
    }
    public IActionResult Signup()
    {
        var model = new SignupModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Signup(SignupModel model)
    {
        if (ModelState.IsValid)
        {
            var user = _cognitoUserPool.GetUser(model.Email);
            if (user.Status != null)
            {
                ModelState.AddModelError("", "This user already exist.");
                return View(model);
            }

            user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);

            var userResult = await _userManager.CreateAsync(user, model.Password);
            if (!userResult.Succeeded)
            {
                foreach (var error in userResult.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(model);
            }

            return RedirectToAction(nameof(Confirm));
        }

        return View(model);
    }

    public IActionResult Confirm()
    {
        var model = new ConfirmModel();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Confirm(ConfirmModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Sorry this user is not found.");
            return View(model);
        }

        var confirmResult = await (_userManager as CognitoUserManager<CognitoUser>).ConfirmSignUpAsync(user, model.Code, true);
        if (!confirmResult.Succeeded)
        {
            foreach (var error in confirmResult.Errors)
                ModelState.AddModelError("", error.Description);
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> ReSendConfirmationCode(ConfirmModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        var result = await (_userManager as CognitoUserManager<CognitoUser>).ResendSignupConfirmationCodeAsync(user);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        return RedirectToAction(nameof(Confirm));
    }


    public IActionResult Login()
    {
        var model = new LoginModel();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email,
                        model.Password, model.RememberMe, false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid username and password combination");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }
}
