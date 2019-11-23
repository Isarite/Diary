using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DiaryApp.Models
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Diary> Diaries { get; set; }
        public DbSet<DiaryPage> Pages { get; set; }
        public DbSet<Marking> Markings { get; set; }
        public new DbSet<User> Users { get; set; }
        //public new DbSet<IdentityRole> Roles { get;set;}
    
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasMany(u => u.diaries).WithOne(d => d.user);


            modelBuilder.Entity<Diary>().HasMany(d => d.pages).WithOne();
            modelBuilder.Entity<Diary>().HasKey(d => d.id);

            modelBuilder.Entity<DiaryPage>().HasKey(p => p.id);
            modelBuilder.Entity<DiaryPage>().HasMany(p => p.markings).WithOne();

            modelBuilder.Entity<Marking>().HasKey(m => m.id);

        }
    }
}
