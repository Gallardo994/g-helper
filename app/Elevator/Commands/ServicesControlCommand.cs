using System.ServiceProcess;
using Elevator.Helpers;
using Serilog;

namespace Elevator.Commands;

public class ServicesControlCommand : IElevatorCommand
{
    public int Execute(Dictionary<string, string> args)
    {
        if (!args.TryGetValue("action", out var action))
        {
            Log.Error("No action provided. Use --action to specify action. Available actions: enable, disable");
            return 2;
        }
        
        if (action != "disable" && action != "enable")
        {
            Log.Error("Invalid action provided. Use --action to specify action. Available actions: enable, disable");
            return 3;
        }
        
        if (!args.TryGetValue("services", out var services))
        {
            Log.Error("No services provided to disable. Use --services to specify services to disable");
            return 4;
        }
        
        var servicesArray = services?.Split(',');
        
        if (servicesArray == null || servicesArray.Length == 0)
        {
            Log.Error("Services array is empty. Use --services to specify services to disable. Example: --services=service1,service2");
            return 5;
        }
        
        if (action == "enable")
        {
            Enable(servicesArray);
        }
        else
        {
            Disable(servicesArray);
        }
        
        
        return 0;
    }
    
    private void Enable(string[] services)
    {
        foreach (var service in services)
        {
            try
            {
                Log.Information("Enabling service {ServiceName}", service);
                using var serviceController = new ServiceController(service);
                serviceController.SetStartMode(ServiceStartMode.Automatic);
                
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    Log.Information("Service {ServiceName} is already running", service);
                    continue;
                }
                
                serviceController.Start();
                Log.Information("Service {ServiceName} has been enabled", service);
            }
            catch (InvalidOperationException)
            {
                Log.Warning("Service {ServiceName} is not installed", service);
            }
        }
    }
    
    private void Disable(string[] services)
    {
        foreach (var service in services)
        {
            try
            {
                Log.Information("Disabling service {ServiceName}", service);
                using var serviceController = new ServiceController(service);
                serviceController.SetStartMode(ServiceStartMode.Disabled);
                
                if (serviceController.Status != ServiceControllerStatus.Running)
                {
                    Log.Information("Service {ServiceName} is not running", service);
                    continue;
                }
            
                serviceController.Stop();
                Log.Information("Service {ServiceName} has been disabled", service);
            }
            catch (InvalidOperationException)
            {
                Log.Warning("Service {ServiceName} is not installed", service);
            }
        }
    }
}