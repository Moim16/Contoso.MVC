using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/* 1 Agregado*/
using Microsoft.AspNetCore.Authorization;
using Contoso.Models;
using Microsoft.AspNetCore.Identity;
/*4 Se agrega using de AppUser*/
using Contoso.MVC.Models;

namespace Contoso.MVC.Controllers
{
    [Authorize]/* 2 A este controlador no entrara cualquiera*/
    public class AccountController : Controller
    {
        /*3 Se agregan dos variables*/
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> SignInManager;
        /*SignIn permite loguear y tiene asi tambien metodo logout genera token de autenticacion*/
        /*5 Para poder usar las clases UserManager y SignInManager hay que hacer inyeccion de dependencia
         */
        public AccountController(UserManager<AppUser> userMgr,SignInManager<AppUser> signinMgr)
        {
            /*Las variables que creamos se la vamos a asignar a las que estamos inyectando*/
            userManager = userMgr;
            SignInManager = signinMgr;
            /*Se puede trabajar sin la inyeccion de dependencia pero se tendria que instanciar de forma Manual*/

        }

        /*6 Se agrega AllowAnonymous para que entre cualquiera, para que puedan entrar, para que para loguearse no requiera estar logueado*/
        [AllowAnonymous]
        /*7 Se crea controlador de Logueo esta es la version get*/
        public IActionResult Login(string returnurl)
        {
            ViewBag.returnUrl = returnurl;
            return View();
        }   
        /*8 Creando version postback de la Accion Login*/
        [HttpPost]
        [AllowAnonymous]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginModel model,string returnUrl)
        {
            if(ModelState.IsValid)
            {
                AppUser user = await userManager.FindByNameAsync(model.UserName);
                if(user!=null)
                {
                    /*Se palma a alguien que este adentro*/
                    await SignInManager.SignOutAsync();
                    /* Si es persistente el primer false es decir que los token de autenticacion sean persistentes, debe ser false, porque sino cerro bien otro podria entrar*/
                    /*El otro false es que si falla  user y contraseña que no se bloquee el acceso por intento a la primera*/
                    Microsoft.AspNetCore.Identity.SignInResult result = await SignInManager.PasswordSignInAsync(user, model.Password, false, false);
                    /*Cuando autentica llena la informacion sino esta nulo*/
                    ViewBag.UserName = HttpContext.User.Identity.Name;

                    if(result.Succeeded)
                    {
                        /*?? Sino es decir si es nulo ?? Valida que el valor que antepone o precede es nulo si es asi manda lo que esta a la derecha*/
                        return Redirect(returnUrl ?? "/");
                    }
                  
                }
                else
                {
                    ModelState.AddModelError(nameof(model.UserName),"Usuario inactivo");
                }
            }
            else
            {
                ModelState.AddModelError(nameof(model.UserName), "Usuario o Contraseña Invalida");
            }
            return View();
        }


        //Creando el metodo de logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
            
            // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        // GET: Account/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
        }

        // GET: Account/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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
        }

        // GET: Account/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Account/Delete/5
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

        /*Accion que retorna una vista cuando usuario no tiene permisos*/
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}