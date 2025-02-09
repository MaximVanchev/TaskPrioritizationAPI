using TaskPrioritizationAPI.Infrastructure.Data.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskPrioritizationAPI.Infrastructure.Data
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer(DatabaseConstants.Connection_String); // Replace with your actual connection string

            return new Context(optionsBuilder.Options);
        }
    }
}