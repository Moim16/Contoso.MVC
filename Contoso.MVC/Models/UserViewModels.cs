﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.MVC.Models
{
    public class CreateModel
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    /*Primer ViewModel para Editar*/
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<AppUser> Members{ get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
    /*Se agrega otro ViewModel para Editar*/
    public class RoleModificationModel
    {
        [Required]
        public string  RoleName { get; set; }
        public string RoleId { get; set; }
        public string[]  IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
