using Microsoft.EntityFrameworkCore;

namespace PasswordManager.Model
{
    public class WebsiteContext : DbContext
    {
        public WebsiteContext(DbContextOptions<WebsiteContext> options)
            : base(options)
        {
        }

        public DbSet<Website> Websites { get; set; }
    }
}