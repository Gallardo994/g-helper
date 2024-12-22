using System;
using System.ServiceProcess;
using LaptopToolBox.ElevatorProxy;

namespace LaptopToolBox.VendorServices.Vendors.Asus;

public class AsusServicesControl : IVendorServicesControl
{
    private readonly string[] _services = new[]
    {
        "AsusAppService",
        "ASUSLinkNear",
        "ASUSLinkRemote",
        "ASUSSoftwareManager",
        "ASUSSwitch",
        "ASUSSystemAnalysis",
        "ASUSSystemDiagnosis",
        "ArmouryCrateControlInterface",
        "AsusCertService",
        "ASUSOptimization",
    };
    
    private readonly IAsusServicesControlCommandLoop _commandLoop;
    private readonly IElevatorProxy _elevatorProxy;

    public AsusServicesControl(IAsusServicesControlCommandLoop commandLoop, IElevatorProxy elevatorProxy)
    {
        _commandLoop = commandLoop;
        _elevatorProxy = elevatorProxy;
    }
    
    public int CountRunningSlow()
    {
        var count = 0;
        
        foreach (var service in _services)
        {
            try
            {
                using var serviceController = new ServiceController(service);
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    count++;
                }
            }
            catch (InvalidOperationException)
            {
                // Service doesn't exist
            }
        }

        return count;
    }

    public void Enable()
    {
        _commandLoop.Enqueue(new AsusServicesEnableCommand(_services, _elevatorProxy));
    }

    public void Disable()
    {
        _commandLoop.Enqueue(new AsusServicesDisableCommand(_services, _elevatorProxy));
    }
}