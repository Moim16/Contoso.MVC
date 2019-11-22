using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Contoso.Models;
using Contoso.MVC.WebServiceAccess.Base; //interesa este porque alli esta la clase que implementa la interfaz para obtener info de estudiante
using Contoso.MVC.WebServiceAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Contoso.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Contoso.MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly IWebApCalls _webApCalls;
        /*Aplicando Inyeccion de dependencia a la interfaz*/
        public StudentController(IWebApCalls webApi)
        {
            _webApCalls = webApi;
        }
        // GET: Student
       /*Como estaba antes
        * public ActionResult Index()
        {
            return View();
        }*/
        /*Se agrega el atributo authorize*/
        [Authorize(Roles="Estudiante")]
        public async Task<ActionResult> Index()
        {
            var students = await _webApCalls.GetStudents();
            return View(students);
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Student/Create
    /*    public ActionResult Create()
        {
            /*Se manda una instancia de un estudiante vacia para crear un estudiante nuevo*/
            /*El model binder debe saber a que entidad esta asociada*/
            //return View();
        /*}*/

     public ActionResult Create()
     {
        /*Se manda una instancia de un estudiante vacia para crear un estudiante nuevo*/
        /*El model binder debe saber a que entidad esta asociada*/
        return View(new Student());/*Esto es necesario para crear la vista de Agregar estudiante*/
     }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        /*Como estaba antes*/
        /*public ActionResult Create(IFormCollection collection)
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
        public async  Task<ActionResult> Create(Student estudiante)
        {
            try
            {
                // TODO: Add insert logic here
                if (estudiante != null)
                {
                    await _webApCalls.AddStudent(estudiante);
                    return RedirectToAction(nameof(Index));
                }
                return View(new Student());
               
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Edit/5
        /* Como estaba antes */
        /*  public ActionResult Edit(int id)
          {
              return View();
          }*/
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            //  var student = await _webApCalls.GetStudents();
            var student = await _webApCalls.GetStudent(id);
            if (student != null)
            {
                return View(student);
            }
            return NotFound();

        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        /* Como estaba antes   public ActionResult Edit(int id, IFormCollection collection)
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

        public async Task<ActionResult> Edit(int id, Student estudianteMod)
        {
            try
            {
                // TODO: Add update logic here
                if (id == 0)
                {
                    return BadRequest();
                }
                var estudiantex = await _webApCalls.GetStudent(id);
                var estudiante = estudiantex.FirstOrDefault();

                if (estudiante != null)
                {
                    estudiante.Nombres = estudianteMod.Nombres;
                    estudiante.Apellidos = estudianteMod.Apellidos;
                    estudiante.Edad = estudianteMod.Edad;
                    // Si ya maneja modulo seguridad para obtener usuario estudiante.UsuarioModificacion = HttpContext.User.Identity.Name;
                    estudiante.UsuarioModificacion = "Admin";
                    estudiante.FechaModificacion = DateTime.Now;
                    await _webApCalls.UpdateStudent(estudiante);
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();

            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        /*Como estaba originalmente  public ActionResult Delete(int id)
          {
              return View();
          }*/
        public async Task<ActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            var student = await _webApCalls.GetStudent(id);
            if (student != null)
            {
                return View(student);
            }
            return NotFound();
        }

        // POST: Student/Delete/5
      //Como estaba antes  [HttpPost]
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
      /*Como estaba antes  public ActionResult Delete(int id, IFormCollection collection)
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
          public async Task<ActionResult> DeletePost(int id)
        {
            try
            {
                // TODO: Add delete logic here
                if (id == 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                var student = await _webApCalls.GetStudent(id);
                if (student != null)
                {
                    await _webApCalls.DeleteStudent(id);
                    return RedirectToAction(nameof(Index));
                }
                //return RedirectToAction(nameof(Index));
                return NotFound();
            }
            catch
            {
                return View();
            }
        }
    }
}