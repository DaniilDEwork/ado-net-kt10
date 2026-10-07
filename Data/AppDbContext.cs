using Kt10Validation.Models;
using Microsoft.EntityFrameworkCore;

namespace Kt10Validation.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
}
