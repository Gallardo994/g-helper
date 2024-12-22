using System.Collections.Generic;
using LaptopToolBox.ElevatorProxy;
using Serilog;

namespace LaptopToolBox.VendorServices.Vendors.Asus;

public class AsusServicesDisableCommand : IAsusServiceCommand
{
    private readonly string[] _services;
    private readonly IElevatorProxy _elevatorProxy;
    
    public AsusServicesDisableCommand(string[] services, IElevatorProxy elevatorProxy)
    {
        _services = services;
        _elevatorProxy = elevatorProxy;
    }
    
    public void Execute()
    {
        var result = _elevatorProxy.Execute("services_control", new Dictionary<string, string>
        {
            { "action", "disable" },
            { "services", string.Join(",", _services) },
        });
        
        Log.Information("Disable services result: {Result}", result);
    }
}