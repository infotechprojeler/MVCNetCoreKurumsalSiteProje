﻿using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class LoginViewModel
    {
        [StringLength(50), Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Şifre"), StringLength(50), Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
