﻿namespace BudgetManager.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string StandardEmail { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
