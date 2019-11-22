using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/*Se agrega*/
using Contoso.MVC.Models;
using Microsoft.AspNetCore.Identity;
/*****/

namespace Contoso.MVC.Controllers
{
    public class AdminController : Controller
    {
        /*Se agrega*/
             private UserManager<AppUser> userManager;
        /*Para el Editar para el hash, luego se inyecta en el constructor*/
        private IPasswordHasher<AppUser> passHash;
        //Aplicando Inyeccion de dependencia a constructorc reado
        public AdminController(UserManager<AppUser> userMgr,IPasswordHasher<AppUser> ph)
        {
            userManager = userMgr;
            passHash = ph;
        }
        // GET: Admin
        /* public ActionResult Index()
         {
             return View();
         }*/
        /*Se Modifica*/
        public ActionResult Index()
       // public async Task<ActionResult> Index()
        {
            //var students = await userManager.Users;
            // return View(students);
            return View(userManager.Users);
         // return View(students);
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        /*  public ActionResult Create()
          {
              return View();
          }*/
          /*Se modifica*/
        public ActionResult Create()
        {
            return View(new CreateModel());
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*   public ActionResult Create(IFormCollection collection)
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

        public async Task<IActionResult> Create(CreateModel model)
        {
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    //return RedirectToAction(nameof(Index));
                    AppUser user = new AppUser();
                    user.UserName = model.Name;
                    user.Email = model.Email;
                    IdentityResult result = await userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        foreach(IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                //return RedirectToAction(nameof(Index));
                return View(model);
            }
            catch
            {
                return View();
            }
        }


        // GET: Admin/Edit/5
        /*  public ActionResult Edit(int id)
          {
              return View();
          }*/
        public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);
            if(user!=null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: Admin/Edit/5
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
        public async Task<ActionResult> Edit(string id,string name,string email, string pass)
        {
            try
            {
                // TODO: Add update logic here
                AppUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    user.Email = email;
                    user.UserName = name;
                    user.PasswordHash = passHash.HashPassword(user, pass);
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                else
                {
                    return NotFound();
                }
                return View("user");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        /* public ActionResult Delete(int id, IFormCollection collection)
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
         }*/

        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                // TODO: Add delete logic here
                AppUser user = await userManager.FindByIdAsync(id);
                if(user!=null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "El Usuario no se encontro");
                }
                return View("Index",userManager.Users);
            }
            catch
            {
                return View();
            }
        }
        /*Agregadp*/
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach(IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}