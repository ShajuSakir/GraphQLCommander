using CommanderGQL.Models;
using Microsoft.EntityFrameworkCore;


namespace CommanderGQL.Data
{
    public class AppDbContext : DbContext
    {
        //use base keyword to pass the options to base class
        public AppDbContext(DbContextOptions options): base(options)
        {

        }

        //dbset is a representaion of model in DB
        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Platform>()
                .HasMany(p => p.Commands)
                .WithOne(p => p.Platform!) //its relationship with itslf. and nullable
                .HasForeignKey(p => p.PlatformId);

            modelBuilder
                .Entity<Command>()
                .HasOne(p => p.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.PlatformId);
        }
    }
}