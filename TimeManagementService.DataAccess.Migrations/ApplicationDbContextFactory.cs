using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TimeManagementService.DataAccess.Contexts;

namespace TimeManagementService.DataAccess.Migrations;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseNpgsql(builder =>
        {
            builder.MigrationsAssembly(typeof(ApplicationDbContextFactory).Assembly.FullName);
        });
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}