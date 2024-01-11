using AutoMapper;
using Data.ViewModel;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Services.Role;

namespace NewsSite.Areas.AdminPanel.Controllers;


[Area("AdminPanel")]
[Authorize(Roles = "News")]
public class RoleController : Controller
{

    private readonly RoleManager<ApplicationRoles> _roleManager;
    private readonly UserManager<ApplicationUsers> _userManager;
    private readonly IRoleService _roleService;
    private readonly IMapper _mapper;

    public RoleController(RoleManager<ApplicationRoles> roleManager, IMapper mapper,
        UserManager<ApplicationUsers> userManager, IRoleService roleService)
    {
        _roleManager = roleManager;
        _mapper = mapper;
        _userManager = userManager;
        _roleService = roleService;
    }





    #region Index

    public IActionResult Index()
    {
        List<TreeViewModel> nodes = new List<TreeViewModel>();

        nodes.Add(new TreeViewModel
        {
            id = "abc",
            parent = "#",
            text = "اجزای سیستم",
        });

        foreach (ApplicationRoles subnode in _roleManager.Roles.Where(x => x.RoleLevel != "0"))
        {
            nodes.Add(new TreeViewModel
            {
                id = subnode.Id.ToString(),
                parent = subnode.RoleLevel,
                text = subnode.FaName,
            });
        }
        //////
        ViewBag.Json = JsonConvert.SerializeObject(nodes);
        ViewBag.ViewTitle = "نمایش درختی اجزای سیستم";

        return View();
    }
    #endregion



    #region Create

    [HttpGet]
    public IActionResult Create(string id, string parentname)
    {
        ViewBag.parentname = parentname;
        ViewBag.id = id;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ApplicationRoleViewModel model)
    {
        if (ModelState.IsValid)
        {
            var mapModel = _mapper.Map<ApplicationRoles>(model);
            IdentityResult roleResult = await _roleManager.CreateAsync(mapModel);

            if (roleResult.Succeeded)
            {
                TempData["SuccessMessage"] = "زیر مجموعه جدید با موفقیت افزوده شد";
                return RedirectToAction("Index");
            }
        }

        return View(model);
    }
    #endregion


    #region AccessRight

    [HttpGet]
    public async Task<IActionResult> AccessRight(string Id)
    {
        List<TreeViewModel> nodes = new List<TreeViewModel>();

        nodes.Add(new TreeViewModel
        {
            id = "abc",
            parent = "#",
            text = "اجزای سیستم"
        });

        foreach (ApplicationRoles subnode in _roleManager.Roles.Where(r => r.RoleLevel != "0"))
        {
            nodes.Add(new TreeViewModel
            {
                id = subnode.Id.ToString(),
                parent = subnode.RoleLevel.ToString(),
                text = subnode.FaName
            });
        }
        ///////////////////////////////////////
        ViewBag.Json = JsonConvert.SerializeObject(nodes);
        ApplicationUsers user = await _userManager.FindByIdAsync(Id);

        if (user != null)
        {
            ViewBag.userId = Id;
            string getRoleid = _roleService.GetRoleId(Id);

            if (getRoleid.Length > 0)
            {
                ViewBag.roleList = getRoleid.Substring(0, getRoleid.Length - 1);
            }
            ViewBag.ViewTitle = "ثبت دسترسی برای " + user.FirstName + " " + user.LastName;
            return View();
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> AccessRight(string selectedItems, string userId)
    {
        List<TreeViewModel> items = JsonConvert.DeserializeObject<List<TreeViewModel>>(selectedItems);

        ApplicationUsers user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            //Delete Role
            var roles = await _userManager.GetRolesAsync(user);
            IdentityResult delRoleResult = await _userManager.RemoveFromRolesAsync(user, roles);

            if (delRoleResult.Succeeded)
            {
                for (int i = 0; i <= items.Count - 1; i++)
                {
                    ApplicationRoles approle = await _roleManager.FindByIdAsync(items[i].id);

                    if (approle != null)
                    {
                        IdentityResult roleresult = await _userManager.AddToRoleAsync(user, approle.Name);
                        if (roleresult.Succeeded)
                        {
                            TempData["SuccessMessage"] = " دسترسی اطلاعات برای " + user.FirstName + " " + user.LastName + " با موفقیت ثبت شد! ";
                        }
                    }
                }
            }
        }
        return RedirectToAction("Index", "User");
    }
    #endregion

}

