using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MySecureBackend.WebApi.Controllers;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

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


        [TestMethod]
        public void IsKindMetNegatiefLeeftijd_Ongeldig()
        {
            var kind = new Kind
            {
                KindID = Guid.NewGuid(),
                Naam = "Lisa",
                Leeftijd = -5, // Negatieve leeftijd is ongeldig
                OuderID = Guid.NewGuid()
            };
            var context = new ValidationContext(kind);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(kind, context, results, true);
            Assert.IsFalse(isValid); // leeftijd moet tussen 0 en 18
        }

        // Test 1: OuderController geeft 404 als er geen ouder is voor de ingelogde gebruiker
        [TestMethod]
        public async Task OuderController_GetAsync_GeenOuderGevonden_Returns404()
        {
            var mockOuder = new Mock<IOuder>();
            var mockAuth = new Mock<IAuthenticationService>();

            mockAuth.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("user-123");
            mockOuder.Setup(x => x.SelectByAccountIdAsync("user-123")).ReturnsAsync(null as Ouder);

            var controller = new OuderController(mockOuder.Object, mockAuth.Object);

            var response = await controller.GetAsync();

            Assert.IsInstanceOfType<NotFoundObjectResult>(response.Result);
        }

        // Test 2: OuderController geeft Forbid als de ouder toebehoort aan een andere gebruiker
        [TestMethod]
        public async Task OuderController_GetByIdAsync_AndereGebruiker_ReturnsForbid()
        {
            var mockOuder = new Mock<IOuder>();
            var mockAuth = new Mock<IAuthenticationService>();

            var ouderID = Guid.NewGuid();
            var ouder = new Ouder { OuderID = ouderID, Naam = "Jan", AccountID = "andere-user" };

            mockAuth.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("user-123");
            mockOuder.Setup(x => x.SelectAsync(ouderID)).ReturnsAsync(ouder);

            var controller = new OuderController(mockOuder.Object, mockAuth.Object);

            var response = await controller.GetByIdAsync(ouderID);

            Assert.IsInstanceOfType<ForbidResult>(response.Result);
        }

        // Test 3: KindController geeft 404 als er geen ouder is voor de ingelogde gebruiker
        [TestMethod]
        public async Task KindController_GetAsync_GeenOuderGevonden_Returns404()
        {
            var mockKind = new Mock<IKind>();
            var mockOuder = new Mock<IOuder>();
            var mockSettings = new Mock<ISettings>();
            var mockAuth = new Mock<IAuthenticationService>();

            mockAuth.Setup(x => x.GetCurrentAuthenticatedUserId()).Returns("user-123");
            mockOuder.Setup(x => x.SelectByAccountIdAsync("user-123")).ReturnsAsync(null as Ouder);

            var controller = new KindController(mockKind.Object, mockOuder.Object, mockSettings.Object, mockAuth.Object);

            var response = await controller.GetAsync();

            Assert.IsInstanceOfType<NotFoundObjectResult>(response.Result);
        }
        }
    }
}
