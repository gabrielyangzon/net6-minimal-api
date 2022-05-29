using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

    }

   

}
