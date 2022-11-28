﻿using System.ComponentModel.DataAnnotations;

namespace eCommerceTicketsWebApplication.DTOS
{
    public class WithdrawDTO
    {
        [Display(Name = "Amount")]
        [Required(ErrorMessage = "Amount input is required")]
        public decimal Amount { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "Email address is required")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
