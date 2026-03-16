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
            public void IsDeWereldGroterDan750_TrueOrFalse()
            {
                var testEnvironmentWereldGroote = new Level2D
                {
                    Name = "TestWereld",
                    MaxLenght = 800,
                    MaxHeight = 100

                };

                var dossier = new ValidationContext(testEnvironmentWereldGroote);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(testEnvironmentWereldGroote, dossier, results, true);

                Assert.IsFalse(isValid);

            }



            [TestMethod]
            public void IsWereldNaamGroterDan25_TrueOrFalse()
            {
                var testEnvironmentNaam = new Level2D
                {
                    Name = "TestWereldMetEenLangeNaamDieGroterIsDan25Tekens",
                    MaxLenght = 600,
                    MaxHeight = 100
                };

                var dossier = new ValidationContext(testEnvironmentNaam);
                var results = new List<ValidationResult>();

                bool isValid = Validator.TryValidateObject(testEnvironmentNaam, dossier, results, true);

                Assert.IsFalse(isValid);
            }
        }

        [TestClass]
        public class Object2DTests
        {
            [TestMethod]
            public void IsObject2DScaleKleinerDan1_TrueOrFalse()
            {
                               var testObject2DScale = new Patient
                {
                    GUID = Guid.NewGuid(),
                    PrefabID = 1,
                    PositionX = 0,
                    PositionY = 0,
                    ScaleX = 0,
                    ScaleY = 0,
                    RotationZ = 0,
                    SortingLayer = 1,
                    EnviromentGUID = Guid.NewGuid()
                };
                var dossier = new ValidationContext(testObject2DScale);
                var results = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(testObject2DScale, dossier, results, true);
                Assert.IsFalse(isValid);

            }
        }
    }
}
