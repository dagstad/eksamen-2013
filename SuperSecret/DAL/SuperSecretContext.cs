using SuperSecret.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using SuperSecret.ViewModels;

namespace SuperSecret.DAL
{
    public class SuperSecretContext : DbContext
    {
        public SuperSecretContext()
            : base("SuperSecretContext")
        {
        }

        public DbSet<Crime> Crimes { get; set; }
        public DbSet<Suspect> Suspects { get; set; }
        public DbSet<TypeOfCrime> TypeOfCrimes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<FilePath> FilePaths { get; set; }




        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }
    }
}

