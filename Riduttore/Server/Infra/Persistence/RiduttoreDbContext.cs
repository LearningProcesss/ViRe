using Microsoft.EntityFrameworkCore;

public class RiduttoreDbContext : DbContext
{
    public DbSet<Video> Videos { get; set; }
    public RiduttoreDbContext(DbContextOptions<RiduttoreDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VideoEntityTypeConfiguration());
    }

    public override int SaveChanges()
    {
        // var entries = ChangeTracker
        //     .Entries()
        //     .Where(e => e.Entity is Video && (e.State == EntityState.Added || e.State == EntityState.Modified));

        // foreach (var entityEntry in entries)
        // {
        //     if (entityEntry.State == EntityState.Added)
        //     {
        //         ((Video)entityEntry.Entity).CreatedOn = DateTime.Now;
        //     }
        // }

        return base.SaveChanges();
    }
}