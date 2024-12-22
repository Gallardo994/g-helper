using System;
using System.ServiceProcess;

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

    public AsusServicesControl(IAsusServicesControlCommandLoop commandLoop)
    {
        _commandLoop = commandLoop;
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
        _commandLoop.Enqueue(new AsusServicesEnableCommand(_services));
    }

    public void Disable()
    {
        _commandLoop.Enqueue(new AsusServicesDisableCommand(_services));
    }
}