using System.Collections.Generic;
using LaptopToolBox.ElevatorProxy;
using Serilog;

namespace LaptopToolBox.VendorServices.Vendors.Asus;

public class AsusServicesEnableCommand : IAsusServiceCommand
{
    private readonly string[] _services;
    private readonly IElevatorProxy _elevatorProxy;
    
    public AsusServicesEnableCommand(string[] services, IElevatorProxy elevatorProxy)
    {
        _services = services;
        _elevatorProxy = elevatorProxy;
    }
    
    public void Execute()
    {
        var result = _elevatorProxy.Execute("services_control", new Dictionary<string, string>
        {
            { "action", "enable" },
            { "services", string.Join(",", _services) },
        });
        
        Log.Information("Enable services result: {Result}", result);
    }
}