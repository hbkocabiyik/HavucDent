﻿namespace HavucDent.Application.DTOs
{
    public class CreateUserDto
    {
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TcKimlikNo { get; set; }
        public string SgkNo { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime SalaryPaymentDate { get; set; }
        public int AnnualLeaveDays { get; set; }
        public int? CommissionRate { get; set; }
        public int? LaboratoryCommissionRate { get; set; }
    }   
}