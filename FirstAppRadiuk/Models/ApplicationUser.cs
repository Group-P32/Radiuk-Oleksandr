using Microsoft.AspNet.Identity.EntityFramework;

namespace FirstAppRadiuk.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Year { get; set; }
    }
}