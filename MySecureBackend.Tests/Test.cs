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
    namespace SecureBackendTests
    {
        [TestClass]
        public class Test
        {

            [TestMethod]
            public void IsOuderNaamLeeg_Ongeldig()
            {
                var ouder = new Ouder
                {
                    OuderID = Guid.NewGuid(),
                    Naam = ""
                };

                var dossier = new ValidationContext(ouder);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(ouder, dossier, results, true);

                Assert.IsFalse(isValid);
            }

            [TestMethod]
            public void IsOuderGegevens()
            {
                var ouder = new Ouder
                {
                    OuderID = Guid.NewGuid(),
                    Naam = "Jan de Vries",
                    AccountID = "Nagger"
                };

                var dossier = new ValidationContext(ouder);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(ouder, dossier, results, true);

                Assert.IsTrue(isValid);
            }

        [TestMethod]
        public void IsKindNaamLeeg_Ongeldig()
        {
            var kind = new MySecureBackend.WebApi.Models.Kind
            {
                KindID = Guid.NewGuid(),
                Naam = "",
                Leeftijd = 10
            };
            var dossier = new ValidationContext(kind);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(kind, dossier, results, true);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsKindGegevensGeldig()
        {
            var kind = new Kind
            {
                Naam = "Lisa",
                Leeftijd = 8,
                OuderID = Guid.NewGuid(),
            };
            var context = new ValidationContext(kind);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(kind, context, results, true);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsGameProgressGeldig_MetVerplichteVelden()
        {
            var gameProgress = new GameProgress
            {
                GameProgressID = Guid.NewGuid(),
                LevelProgress = 50.0f,
                Points = 100,
                BehandelingID = Guid.NewGuid()
            };
            var context = new ValidationContext(gameProgress);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(gameProgress, context, results, true);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsBehandelingGeldig_MetVerplichteVelden()
        {
            var behandeling = new Behandeling
            {
                BehandelingID = Guid.NewGuid(),
                Type = "Controle",
                Datum = DateTime.Now,
                Arts = "Dr. Jansen",
                KindID = Guid.NewGuid()
            };
            var context = new ValidationContext(behandeling);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(behandeling, context, results, true);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsBehandelingZonderType_Ongeldig()
        {
            var behandeling = new Behandeling
            {
                BehandelingID = Guid.NewGuid(),
                Type = null, // Type is verplicht
                Datum = DateTime.Now,
                Arts = "Dr. Jansen",
                KindID = Guid.NewGuid()
            };
            var context = new ValidationContext(behandeling);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(behandeling, context, results, true);
            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsBehandelingZonderArts_Ongeldig()
        {
            var behandeling = new Behandeling
            {
                BehandelingID = Guid.NewGuid(),
                Type = "Controle",
                Datum = DateTime.Now,
                Arts = null, // Arts is verplicht
                KindID = Guid.NewGuid()
            };
            var context = new ValidationContext(behandeling);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(behandeling, context, results, true);
            Assert.IsFalse(isValid);
        }
        }
    }
}
