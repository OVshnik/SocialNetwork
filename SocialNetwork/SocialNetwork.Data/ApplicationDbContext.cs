using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using SocialNetwork.Data.Models;

namespace SocialNetwork.Data
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }
    }
}
