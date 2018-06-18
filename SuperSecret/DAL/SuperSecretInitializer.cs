using SuperSecret.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SuperSecret.DAL
{
    public class SuperSecretInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SuperSecretContext>
    {
        protected override void Seed(SuperSecretContext context)
        {

            var countries = new List<Country>
            {
                new Country {CountryName = "Norge"},
                new Country {CountryName = "Sverige"},
                new Country {CountryName = "Danmark"},
                new Country {CountryName = "Finnland"}
            };
            countries.ForEach(s => context.Countries.Add(s));
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
            crimeTypes.ForEach(s => context.TypeOfCrimes.Add(s));
            context.SaveChanges();
            var suspects = new List<Suspect>
            {
                new Suspect {CountryId = 1, PictureId = null ,Name ="Per", Alias ="Svarte Per", Age =35},
                new Suspect {CountryId = 1, PictureId = null ,Name ="Kalle", Alias ="Spøkelseskladden", Age =67},
                new Suspect {CountryId = 1, PictureId = null ,Name ="Pekka", Alias ="231-321", Age =59}
            };
            suspects.ForEach(s => context.Suspects.Add(s));
            context.SaveChanges();
            var crimes = new List<Crime>
            {
                new Crime { TypeOfCrimeId = 1,  Description ="Datainbrudd bank", Date = DateTime.Parse("2015-01-12")},
                new Crime { TypeOfCrimeId = 2,   Description ="Lagt ut virus hos forsikringsselskap", Date = DateTime.Parse("2015-02-02")},
                new Crime { TypeOfCrimeId = 3,   Description ="Forfalsket pass", Date = DateTime.Parse("2014-01-24")},
                new Crime { TypeOfCrimeId = 1,  Description ="Lastet ned filmer", Date = DateTime.Parse("2013-10-12")},

            };
            //kommentert ut for å teste uten SuspectId i crime.
            //var crimes = new List<Crime>
            //{
            //    new Crime { TypeOfCrimeId = 1, SuspectId=1,  Description ="Datainbrudd bank", Date = DateTime.Parse("2015-01-12")},
            //    new Crime { TypeOfCrimeId = 2, SuspectId=2,  Description ="Lagt ut virus hos forsikringsselskap", Date = DateTime.Parse("2015-02-02")},
            //    new Crime { TypeOfCrimeId = 3, SuspectId=3,  Description ="Forfalsket pass", Date = DateTime.Parse("2014-01-24")},
            //    new Crime { TypeOfCrimeId = 1, SuspectId=2,  Description ="Lastet ned filmer", Date = DateTime.Parse("2013-10-12")},

            //};
            crimes.ForEach(s => context.Crimes.Add(s));
            context.SaveChanges();
        }
    }
}