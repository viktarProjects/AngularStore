﻿using System.ComponentModel.DataAnnotations;

namespace AngularStore.WebAPI.Dto_s
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a - z])(?=.*[A - Z])(?=.*\\d)(?=.*[@$! % *? &])[A - Za - z\\d@$! % *? &]{8,}$",
            ErrorMessage ="Password must have 1 Uppercase, 1 Lowercase, 1 non-numeric character and at least 6 numbers")]
        public string Password { get; set; }
    }
}
