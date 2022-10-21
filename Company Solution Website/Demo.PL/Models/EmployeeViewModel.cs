﻿using Microsoft.AspNetCore.Http;
using Demo.DAL.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required!")]
        [MaxLength(50, ErrorMessage = "Maximum Length of Name is 50 Chars")]
        [MinLength(5, ErrorMessage = "Minimum Length of Name is 5 Chars")]
        public string Name { get; set; }

        [Range(22, 60, ErrorMessage = "Age must be between 22 and 60")]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,10}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}-[a-zA-Z]{1,40}$"
           , ErrorMessage = "Address must be like '123-Street-Region-City'")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        [Range(4000, 8000, ErrorMessage = "Salary Must be between 4000 and 8000")]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }


        public string ImageName { get; set; }

        public IFormFile Image { get; set; }

        public int DepartmentId { get; set; }

        public DepartmentViewModel Department { get; set; }

    }
}
