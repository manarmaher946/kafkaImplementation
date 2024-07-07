
using Microsoft.EntityFrameworkCore;
using ProducerImplementation.Models;

namespace backgroundImplementation.Data
{
    public class ApplicationDbcontext : DbContext
    {
        public ApplicationDbcontext(DbContextOptions<ApplicationDbcontext> options) : base(options)
        {
        }
        public DbSet<prince> Princes { get; set; }

    }
}
