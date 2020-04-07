using System;
using AdxApp.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AdxAppTests.Models
{
    [TestClass]
    public class ModelFactoryTests
    {
        [TestMethod]
        public void Create_Properties_Model_Instance_With_Random_Values()
        {
            // Arrange
            var modelFactory = new ModelFactory();

            // Act
            var model = modelFactory.CreateWithRandomValues<Properties>();

            // Assert
            model.CapabilityType.Should().NotBeNullOrWhiteSpace();
            model.DataName.Should().NotBeNullOrWhiteSpace();
            model.DeviceId.Should().NotBeNullOrWhiteSpace();
            model.InterfaceId.Should().NotBeNullOrWhiteSpace();
            model.InterfaceName.Should().NotBeNullOrWhiteSpace();
            model.ModelId.Should().NotBeNullOrWhiteSpace();
            model.SemanticType.Should().NotBeNullOrWhiteSpace();
            model.PayloadTimeStamp.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMinutes(30));
        }

        [TestMethod]
        public void Create_Pluto_Model_Instance_With_Random_Values()
        {
            // Arrange
            var modelFactory = new ModelFactory();

            // Act
            var model = modelFactory.CreateWithRandomValues<Pluto>();

            // Assert
            model.CanNetwork.Should().BeNull();
            model.Can0Load.Should().BeInRange(0, 100000);
            model.Can1Load.Should().BeInRange(0, 100000);
            model.CpuLoad.Should().BeInRange(0, 100000);
            model.DiskUsage.Should().BeInRange(0, 100000);
            model.MemoryUsage.Should().BeInRange(0, 100000);
            model.QueueMemoryUsage.Should().BeInRange(0, 100000);
        }

        [TestMethod]
        public void Create_And_Serialize_Telemetry_With_Random_Values()
        {
            // Arrange
            var modelFactory = new ModelFactory();

            // Act
            var model = modelFactory.CreateTelemetryWithRandomValues();

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(model, new JsonSerializerSettings { ContractResolver = contractResolver});

            // Assert
            model.Payload.Pluto.CanNetwork.Count.Should().Be(2);
        }
    }
}
