using System;
using System.Collections.Generic;
using System.Reflection;

namespace AdxApp.Models
{
    public class ModelFactory
    {
        private readonly TelemetryGenerator _telemetryGenerator = new TelemetryGenerator();

        public Telemetry CreateTelemetryWithRandomValues(int canNetworkArrayCount = 2)
        {
            var properties = CreateWithRandomValues<Properties>();
            var canNetwork = new List<CanNetwork>();

            for (var i = 0; i < canNetworkArrayCount; i++)
            {
                canNetwork.Add(CreateWithRandomValues<CanNetwork>());
            }

            var pluto = CreateWithRandomValues<Pluto>();
            pluto.CanNetwork = canNetwork;

            var machine = CreateWithRandomValues<Machine>();
            var motorCurrent = CreateWithRandomValues<MotorCurrent>();
            var motorVoltage = CreateWithRandomValues<MotorVoltage>();
            var supplyVoltage24V = CreateWithRandomValues<SupplyVoltage24V>();
            
            var infeedConveyor = CreateWithRandomValues<InfeedConveyor>();
            infeedConveyor.MotorCurrent = motorCurrent;
            infeedConveyor.MotorVoltage = motorVoltage;
            infeedConveyor.SupplyVoltage24V = supplyVoltage24V;

            var motorCurrent2 = CreateWithRandomValues<MotorCurrent2>();
            var motorVoltage2 = CreateWithRandomValues<MotorVoltage2>();
            var supplyVoltage24V2 = CreateWithRandomValues<SupplyVoltage24V2>();

            var outfeedConveyor = CreateWithRandomValues<OutfeedConveyor>();
            outfeedConveyor.MotorCurrent = motorCurrent2;
            outfeedConveyor.MotorVoltage = motorVoltage2;
            outfeedConveyor.SupplyVoltage24V = supplyVoltage24V2;

            var motorCurrent3 = CreateWithRandomValues<MotorCurrent3>();
            var motorVoltage3 = CreateWithRandomValues<MotorVoltage3>();
            var supplyVoltage24V3 = CreateWithRandomValues<SupplyVoltage24V3>();

            var mainConveyor = CreateWithRandomValues<MainConveyor>();
            mainConveyor.MotorCurrent = motorCurrent3;
            mainConveyor.MotorVoltage = motorVoltage3;
            mainConveyor.SupplyVoltage24V = supplyVoltage24V3;

            var xrayGenerator = CreateWithRandomValues<XrayGenerator>();
            var coolingCurrent = CreateWithRandomValues<CoolingCurrent>();

            var xraySensor = CreateWithRandomValues<XraySensor>();
            xraySensor.CoolingCurrent = coolingCurrent;

            var payload = new Payload
            {
                Pluto = pluto,
                Machine = machine,
                InfeedConveyor = infeedConveyor,
                OutfeedConveyor = outfeedConveyor,
                MainConveyor = mainConveyor,
                XrayGenerator = xrayGenerator,
                XraySensor = xraySensor
            };

            var telemetry = CreateWithRandomValues<Telemetry>();
            telemetry.Properties = properties;
            telemetry.Payload = payload;

            return telemetry;
        }

        public T CreateWithRandomValues<T>() where T : IModel, new()
        {
            var model = new T();

            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                if (!prop.CanWrite)
                {
                    continue;
                }

                model.GetType().InvokeMember(
                    prop.Name,
                    BindingFlags.SetProperty,
                    Type.DefaultBinder,
                    model,
                    new object[] {GetRandomValue(prop.PropertyType) });

            }

            return model;
        }

        private object GetRandomValue(Type randomValueType)
        {
            if (randomValueType == typeof(string))
            {
                return _telemetryGenerator.GenerateRandomString(maxLength: 10);
            }

            if (randomValueType == typeof(int))
            {
                return _telemetryGenerator.GenerateRandomInt(0, 100000);
            }

            if (randomValueType == typeof(float))
            {
                return _telemetryGenerator.GenerateRandomFloat(0, 100000);
            }

            if (randomValueType == typeof(DateTime))
            {
                return _telemetryGenerator.GenerateRandomDateTime();
            }

            return null;
        }
    }
}
