﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MazzaWebSite.Models
{
    public class ManageViewModel
    {
        public List<User> Users { get; set; }

        public string QrCodeImageUrl { get; set; }

        public string ManualEntryKey { get; set; }

        public string Token { get; set; }

        public bool IsActive { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}