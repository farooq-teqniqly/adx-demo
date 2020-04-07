using System;
using AdxApp.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdxAppTests
{
    [TestClass]
    public class RandomStringGeneratorTests
    {
        [TestMethod]
        public void GenerateRandomString_Returns_String()
        {
            // Arrange
            var generator = new TelemetryGenerator();

            // Act
            var result = generator.GenerateRandomString();

            // Assert
            result.Length.Should().BeInRange(5, 256);
        }

        [TestMethod]
        public void GenerateRandomString_When_Given_Exact_Range_Returns_String_With_Length_In_Range()
        {
            var maxLengths = new []{10, 126};

            // Arrange
            var generator = new TelemetryGenerator();

            // Act

            foreach (var maxLength in maxLengths)
            {
                var result = generator.GenerateRandomString(maxLength, maxLength);

                // Assert
                result.Length.Should().Be(maxLength);
            }
            
        }

        [TestMethod]
        public void GenerateRandomString_When_Given_Range_Returns_String_With_Length_In_Range()
        {
            var maxLengths = new[] { 30, 49 };

            // Arrange
            var generator = new TelemetryGenerator();

            // Act

            foreach (var maxLength in maxLengths)
            {
                var result = generator.GenerateRandomString(maxLength: maxLength);

                // Assert
                result.Length.Should().BeInRange(5, maxLength);
            }

        }

        [TestMethod]
        public void GenerateRandomInt_Returns_Int()
        {
            // Arrange
            var generator = new TelemetryGenerator();

            // Act
            var result = generator.GenerateRandomInt();

            // Assert
            result.Should().BeInRange(int.MinValue, Int32.MaxValue);
        }

        [TestMethod]
        public void GenerateRandomInt_When_Given_Range_Returns_Int_Between_Range()
        {
            // Arrange
            var generator = new TelemetryGenerator();

            // Act
            var result = generator.GenerateRandomInt(1000, 2000);

            // Assert
            result.Should().BeInRange(1000, 2000);
        }

        [TestMethod]
        public void GenerateRandomFloat_Returns_Float()
        {
            // Arrange
            var generator = new TelemetryGenerator();

            // Act
            var result = generator.GenerateRandomFloat();

            // Assert
            result.Should().BeInRange(float.MinValue, float.MaxValue);
        }

        [TestMethod]
        public void GenerateRandomFloat_When_Given_Range_Returns_Float_Between_Range()
        {
            // Arrange
            var generator = new TelemetryGenerator();

            // Act
            var result = generator.GenerateRandomFloat((float)11.44532, (float)12.111231);

            // Assert
            result.Should().BeInRange((float)11.44532, (float)12.111231);
        }

        [TestMethod]
        public void GenerateRandomDateTime_Returns_DateTime()
        {
            // Arrange
            var generator = new TelemetryGenerator();

            // Act
            var result = generator.GenerateRandomDateTime();

            // Assert
            result.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(30));
        }

        [TestMethod]
        public void GenerateRandomDateTime_When_Given_Range_Returns_DateTime_Within_Range()
        {
            // Arrange
            var generator = new TelemetryGenerator();

            // Act
            var result = generator.GenerateRandomDateTime(120);

            // Assert
            result.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(120));
        }
    }
}
