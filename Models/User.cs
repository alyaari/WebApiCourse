using Microsoft.AspNetCore.Identity;

namespace WebApplication4.Models
{
    public class User  : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public bool? IsAdmin { get; set; }=false;
    }

    public class Role : IdentityRole<int>
    {
        public Role(int id, string name)
        {
            this.Id=id; 
            this.Name = name;
            this.NormalizedName = name.ToUpper();
        }
    }
    }
