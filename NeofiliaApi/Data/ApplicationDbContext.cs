using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NeofiliaDomain;




public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{

    public DbSet<Pub> Locals => Set<Pub>();
    public DbSet<PubTable> Tables => Set<PubTable>();
    public DbSet<PubMenu> Menus => Set<PubMenu>();
    public DbSet<TableReward> Rewards => Set<TableReward>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        

        base.OnModelCreating(modelBuilder);

        #region Seeding User and Roles
        //Seeding a 'Administrator' role to AspNetRoles table
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole
            {
                Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                Name = "Admin",
                NormalizedName = "ADMIN".ToUpper()
            },
            new IdentityRole
            {
                Id = "2c5e174e-3b0e-446f-86af-483d56fd6109",
                Name = "LocalManager",
                NormalizedName = "LOCALMANAGER".ToUpper()
            });

        //a hasher to hash the password before seeding the user to the db
        var hasher = new PasswordHasher<ApplicationUser>();


        //Seeding the User to AspNetUsers table
        modelBuilder.Entity<ApplicationUser>().HasData(
            new ApplicationUser
            {
                Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", // primary key
                UserName = "nofilia_admin@libero.it",
                NormalizedUserName = "NOFILIA_ADMIN@LIBERO.IT",
                Email = "nofilia_admin@libero.it",
                NormalizedEmail = "NOFILIA_ADMIN@LIBERO.IT",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Pa$$w0rd")
            });

        //Seeding the relation between our user and role to AspNetUserRoles table
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
            });
        #endregion
        
    }
}
