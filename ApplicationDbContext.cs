using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCTask.Entities;

namespace MVCTask {
    public class ApplicationDbContext(DbContextOptions options): IdentityDbContext(options) {

        //override protected void OnModelCreating(ModelBuilder modelBuilder) {
        //    base.OnModelCreating(modelBuilder);

           
        //}

        public DbSet<Entities.Task> Tasks { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<FilesAttach> FilesAttach { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Address> Addresses { get; set; }

    }
}
