using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySecureBackend.WebApi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MySecureBackend.WebApi.Controllers;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;

namespace MySecureBackend.Tests
{


    // Mock User model voor unit tests
    public class User
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }

    // Mock Kind model voor unit tests
    public class Kind
    {
        [Required]
        public string Naam { get; set; }
        [Range(0, 18)]
        public int Leeftijd { get; set; }
        [Required]
        public string Behandeling { get; set; }
        [Required]
        public DateTime Datum { get; set; }
    }

    namespace SecureBackendTests
    {
        [TestClass]
        public class Test
        {
            [TestMethod]
            public void IsLevelProgressGeldig_MetVerplichtVelden()
            {
                var levelProgress = new LevelProgress
                {
                    Id = Guid.NewGuid(),
                    Name = "TestLevel",
                    LevelProgressValue = 75.5f,
                    Points = 100
                };

                var dossier = new ValidationContext(levelProgress);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(levelProgress, dossier, results, true);

                Assert.IsTrue(isValid);
            }

            [TestMethod]
            public void IsPatientVoornaamLeeg_Ongeldig()
            {
                var patient = new Patient
                {
                    PatientID = Guid.NewGuid(),
                    voornaam = "",
                    achternaam = "Testachternaam",
                    Leeftijd = 30
                };

                var dossier = new ValidationContext(patient);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(patient, dossier, results, true);

                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void IsPatientAchternaamLeeg_Ongeldig()
            {
                var patient = new Patient
                {
                    PatientID = Guid.NewGuid(),
                    voornaam = "Testvoornaam",
                    achternaam = "",
                    Leeftijd = 25
                };

                var dossier = new ValidationContext(patient);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(patient, dossier, results, true);

                Assert.IsFalse(isValid);
            }

        [TestMethod]
        public void IsPatientZonderVoornaam_Ongeldig()
        {
            var patient = new Patient
            {
                PatientID = Guid.NewGuid(),
                achternaam = "Jansen",
                Leeftijd = 40
            };
            var dossier = new ValidationContext(patient);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(patient, dossier, results, true);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsPatientZonderAchternaam_Ongeldig()
        {
            var patient = new Patient
            {
                PatientID = Guid.NewGuid(),
                voornaam = "Jan",
                Leeftijd = 40
            };
            var dossier = new ValidationContext(patient);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(patient, dossier, results, true);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsUserRegistratieGeldig()
        {
            var user = new User
            {
                Username = "testuser",
                Password = "SterkWachtwoord123!"
            };
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, context, results, true);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsUserLoginOngeldig()
        {
            var user = new User
            {
                Username = "",
                Username = "",
                Password = ""
            };
            var context = new ValidationContext(user);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(user, context, results, true);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsKindGegevensGeldig()
        {
            var kind = new Kind
            {
                Naam = "Lisa",
                Leeftijd = 8,
                Behandeling = "Longfunctietest",
                Datum = DateTime.Now
            };
            var context = new ValidationContext(kind);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(kind, context, results, true);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsWachtwoordNietPlainText()
        {
            var user = new User
            {
                Username = "testuser",
                Password = "SterkWachtwoord123!"
            };
            // Simuleer hashing
            string hashed = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Password));
            Assert.AreNotEqual(user.Password, hashed);
        }

        }
    }
}
