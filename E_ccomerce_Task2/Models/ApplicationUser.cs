using Microsoft.AspNetCore.Identity;

namespace E_ccomerce_Task2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set;}
        public string LastName { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? ZipCode { get; set; }
    }
}
