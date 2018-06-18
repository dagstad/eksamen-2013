namespace SuperSecret.Migrations
{
    using SuperSecret.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<SuperSecret.DAL.SuperSecretContext>
    {


        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }


        protected override void Seed(SuperSecret.DAL.SuperSecretContext context)
        {


            var countries = new List<Country>
            {
                new Country {CountryName = "Norge"},
                new Country {CountryName = "Sverige"},
                new Country {CountryName = "Danmark"},
                new Country {CountryName = "Finnland"}
            };
            countries.ForEach(s => context.Countries.AddOrUpdate(p => p.CountryName, s));
            context.SaveChanges();

            var crimeTypes = new List<TypeOfCrime>
            {
                new TypeOfCrime { CrimeTypeName = "Datainnbrudd"},
                new TypeOfCrime { CrimeTypeName = "Databedrageri"},
                new TypeOfCrime { CrimeTypeName = "Informasjonsheleri"},
                new TypeOfCrime { CrimeTypeName = "Skadeverk"},
                new TypeOfCrime { CrimeTypeName = "Dokumentfalsk"},
                new TypeOfCrime { CrimeTypeName = "Piratkopiering"},
                new TypeOfCrime { CrimeTypeName = "Beskyttelsesbrudd (TV- og radiosignaler)"}
            };
            crimeTypes.ForEach(s => context.TypeOfCrimes.AddOrUpdate(p => p.CrimeTypeName, s));
            context.SaveChanges();

            var suspects = new List<Suspect>
            {
                new Suspect {CountryId = 1, PictureId = null ,Name ="Per", Alias ="Svarte Per", Age =35},
                new Suspect {CountryId = 1, PictureId = null ,Name ="Kalle", Alias ="Spøkelseskladden", Age =67},
                new Suspect {CountryId = 1, PictureId = null ,Name ="Pekka", Alias ="231-321", Age =59}
            };
            suspects.ForEach(s => context.Suspects.AddOrUpdate(p => p.Alias, s));
            context.SaveChanges();

            var crimes = new List<Crime>
            {
                new Crime { TypeOfCrimeId = 1, Description ="Datainbrudd bank", Date = DateTime.Parse("01.12.2012")},
                new Crime { TypeOfCrimeId = 2,   Description ="Lagt ut virus hos forsikringsselskap", Date = DateTime.Parse("02.02.2015")},
                new Crime { TypeOfCrimeId = 3,  Description ="Forfalsket pass", Date = DateTime.Parse("24.01.2014")},
                new Crime { TypeOfCrimeId = 1,   Description ="Lastet ned filmer", Date = DateTime.Parse("12.10.2013")},

            };

            crimes.ForEach(s => context.Crimes.AddOrUpdate(p => p.Description, s));
            context.SaveChanges();
        }
    }
}
