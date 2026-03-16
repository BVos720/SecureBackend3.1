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
        public class Enviroment2DTests
        {
            [TestMethod]
            public void IsLevelProgressGeldig_MetVerplichtVelden()
            {
                var levelProgress = new LevelProgress
                {
                    LevelProgressId = Guid.NewGuid(),
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
        }

        [TestClass]
        public class Object2DTests
        {
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
        }
    }
}
