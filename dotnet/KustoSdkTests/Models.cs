using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KustoSdkTests
{
    public interface IModel
    {

    }

    public class Properties : IModel
    {
        public string DeviceId { get; set; }
        public string ModelId { get; set; }
        public string InterfaceId { get; set; }
        public string InterfaceName { get; set; }
        public string DataName { get; set; }
        public string CapabilityType { get; set; }
        public string SemanticType { get; set; }
        public DateTime PayloadTimeStamp { get; set; }
        
    }

    public class CanNetwork : IModel
    {
        public int Id { get; set; }
        public string Iface { get; set; }
        public bool IsActive { get; set; }
        public int SkippedHeartbeats { get; set; }
        public string SwVersion { get; set; }
        public DateTime LastSeen { get; set; }
        public string HwVersion { get; set; }
        public string Name { get; set; }
        public int ErrorRegister { get; set; }
        public int DeviceType { get; set; }
    }

    public class Pluto : IModel
    {
        public List<CanNetwork> CanNetwork { get; set; }
        public int Can0Load { get; set; }
        public int Can1Load { get; set; }
        public float CpuLoad { get; set; }
        public float DiskUsage { get; set; }
        public float MemoryUsage { get; set; }
        public float QueueMemoryUsage { get; set; }
    }

    public class Machine : IModel
    {
        public float CabinetTemperature { get; set; }
        public float UpperCabinetTemperature { get; set; }
        public int Uptime { get; set; }
    }

    public class MotorCurrent : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class MotorVoltage : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class SupplyVoltage24V : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class InfeedConveyor : IModel
    {
        public int HeatsinkTemperature { get; set; }
        public MotorCurrent MotorCurrent { get; set; }
        public MotorVoltage MotorVoltage { get; set; }
        public SupplyVoltage24V SupplyVoltage24V { get; set; }
        public int ThermalLoad { get; set; }
    }

    public class MotorCurrent2 : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class MotorVoltage2 : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class SupplyVoltage24V2 : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class OutfeedConveyor : IModel
    {
        public int HeatsinkTemperature { get; set; }
        public MotorCurrent2 MotorCurrent { get; set; }
        public MotorVoltage2 MotorVoltage { get; set; }
        public SupplyVoltage24V2 SupplyVoltage24V { get; set; }
        public int ThermalLoad { get; set; }
    }

    public class MotorCurrent3 : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class MotorVoltage3 : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class SupplyVoltage24V3 : IModel
    {
        public float Scaling { get; set; }
        public int Value { get; set; }
    }

    public class MainConveyor : IModel
    {
        public int HeatsinkTemperature { get; set; }
        public MotorCurrent3 MotorCurrent { get; set; }
        public MotorVoltage3 MotorVoltage { get; set; }
        public SupplyVoltage24V3 SupplyVoltage24V { get; set; }
        public int ThermalLoad { get; set; }
    }

    public class XrayGenerator : IModel
    {
        public float OnTime { get; set; }
        public float Temperature { get; set; }
        public float TubeCurrent { get; set; }
        public float TubeVoltage { get; set; }
    }

    public class CoolingCurrent : IModel
    {
        public float Scaling { get; set; }
        public float Value { get; set; }
    }

    public class XraySensor : IModel
    {
        public float ActualTemperature { get; set; }
        public CoolingCurrent CoolingCurrent { get; set; }
        public float CoolingEffort { get; set; }
        public int OnTime { get; set; }
        public int XrayTime { get; set; }
    }

    public class Payload : IModel
    {
        public Pluto Pluto { get; set; }
        public Machine Machine { get; set; }
        public InfeedConveyor InfeedConveyor { get; set; }
        public OutfeedConveyor OutfeedConveyor { get; set; }
        public MainConveyor MainConveyor { get; set; }
        public XrayGenerator XrayGenerator { get; set; }
        public XraySensor XraySensor { get; set; }
    }

    public class Telemetry : IModel
    {
        public Properties Properties { get; set; }
        public string EdgeDeviceId { get; set; }
        public DateTime TimeStamp { get; set; }

        [JsonProperty("@version")]
        public int Version { get; set; }
        public string Fingerprint { get; set; }
        public string CorrelationId { get; set; }
        public Payload Payload { get; set; }
}
}
