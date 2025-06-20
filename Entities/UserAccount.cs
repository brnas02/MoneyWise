﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DWebProjetoFinal.Entities {

    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class UserAccount {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "UserName is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de conta.")]
        public string Role { get; set; }

        [MaxLength(255, ErrorMessage = "Max 255 characters allowed.")]
        public string? ProfileImagePath { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
