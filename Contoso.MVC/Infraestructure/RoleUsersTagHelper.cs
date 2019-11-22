using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*Agregando*/
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Contoso.MVC.Models;


namespace Contoso.MVC.Infraestructure
{
    [HtmlTargetElement("td",Attributes="identity-role")]
    public class RoleUsersTagHelper:TagHelper
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public RoleUsersTagHelper(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;
        }
        /*Obteniendo Tag Helper del, Todo que lo pase en la vista lo pasara a la propiedad de abajo*/
        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        /*Como se hereda del Tag Helper hay que sobreescribir de la clase que heredo con tab mas barra espaciadora seguido despues de haber escrito override */
        /*Se agrega Async*/
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            /*Buscamos el rol en la b.d cuyo id es igual al que le pasas en tag-helper*/
            IdentityRole role = await roleManager.FindByIdAsync(Role);
            //return base.ProcessAsync(context, output);
            if (role != null)
            {
                foreach(var user in userManager.Users)
                {
                    /*Pregunta si ese usario pertenece a ese rol, si es asi se agrega a la lista
                     y esa se regresara en el Td de Index de RoleAdmin*/
                    if(user!=null && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }
            /*Se genera taghelperCOntent donde manda imprimir listado de usuarios*/
            /*El join ahorra codigo*/
            output.Content.SetContent(names.Count == 0 ? "Ningun Usuario" : string.Join(",", names));
        }
    }
}
