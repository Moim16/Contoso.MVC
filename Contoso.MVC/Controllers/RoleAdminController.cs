using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/*1 agregando*/
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Contoso.MVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace Contoso.MVC.Controllers
{

    /*2*/
    [Authorize]
    public class RoleAdminController : Controller
    {
        /*3*/
        private RoleManager<IdentityRole> roleManager;
        private UserManager<AppUser> userManager;
        public RoleAdminController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        // GET: RoleAdmin
      /*  public ActionResult Index()
        {
            return View();
        }*/


        public ActionResult Index()
        {
            return View(roleManager.Roles);
        }

        // GET: RoleAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RoleAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoleAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*    public ActionResult Create(IFormCollection collection)
            {
                try
                {
                    // TODO: Add insert logic here

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }*/

            /*Con DataNotation podes hacer validacion de propiedades(Que si es requerido, el tamaño), se puede usar tambien
             * en parametros de las funciones*/
        public async Task<ActionResult> Create([Required]string name)
        {
            try
            {
                // TODO: Add insert logic here

                if (ModelState.IsValid)
                {
                    //  IdentityResult result = await roleManager.CreateAsync(new IdentityRole { Name = name });
                    /*Se comentarea linea anterior para pasar nombre en constructor*/
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }

                }
                //return RedirectToAction(nameof(Index));
                return View(name);
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleAdmin/Edit/5
        /*   public ActionResult Edit(int id)
           {
               return View();
           }*/

        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                List<AppUser> members = new List<AppUser>();
                List<AppUser>  nonMembers = new List<AppUser>();
                foreach(AppUser user in userManager.Users)
                {
                    var list = await userManager.IsInRoleAsync(user, role.Name) ? members : nonMembers;
                    list.Add(user);
                    /*IActionResult es una interfaz*/
                      /*ActionResult Abarca ciertos tipos
                        el IActionResult para tener mas alcance Abarca mas*/
                }
                return View(new RoleEditModel
                {
                    Role = role,
                    Members = members,
                    NonMembers = nonMembers
                });
            }
            return RedirectToAction(nameof(Index));
            //  return View();
        }

        // POST: RoleAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*   public ActionResult Edit(int id, IFormCollection collection)
           {
               try
               {
                   // TODO: Add update logic here

                   return RedirectToAction(nameof(Index));
               }
               catch
               {
                   return View();
               }
           }*/

        public async Task<IActionResult> Edit(RoleModificationModel roleMod)
        {
            try
            {
                // TODO: Add update logic here
                IdentityResult result;
                if(ModelState.IsValid)
                {
                    IdentityRole rol = await roleManager.FindByIdAsync(roleMod.RoleId);
                    if(rol!=null)
                    {
                        rol.Name = roleMod.RoleName;
                        result = await roleManager.UpdateAsync(rol);
                        if (!result.Succeeded)
                        {
                            AddErrorsFromResult(result);
                        }
                    }
                    //Si es nulo instancio arreglo vacio sino nada
                    foreach(string userId in roleMod.IdsToAdd??new string[]{ })
                    {

                        AppUser user = await userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            result = await userManager.AddToRoleAsync(user, roleMod.RoleName);
                            if (!result.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                        }
                    }
                    foreach (string userId in roleMod.IdsToDelete ?? new string[] { })
                    {

                        AppUser user = await userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            result = await userManager.RemoveFromRoleAsync(user, roleMod.RoleName);
                            if (!result.Succeeded)
                            {
                                AddErrorsFromResult(result);
                            }
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return await Edit(roleMod.RoleId);
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleAdmin/Delete/5
        public ActionResult Delete(int id)
          {
              return View();
          }
       /* public  async Task<IActionResult> Delete(int id)
        {
            return View();
        }*/

        // POST: RoleAdmin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}