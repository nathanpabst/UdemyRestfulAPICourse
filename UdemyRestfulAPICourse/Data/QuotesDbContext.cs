using Microsoft.EntityFrameworkCore;
using UdemyRestfulAPICourse.Models;

namespace UdemyRestfulAPICourse.Data
{
    public class QuotesDbContext : DbContext 
    {
        public QuotesDbContext(DbContextOptions<QuotesDbContext> options) : base(options)
        {

        }
        public DbSet<Quote> Quotes { get; set; }
    }
}
