using Elevator.Commands;
using Serilog;

namespace Elevator;

public static class Program
{
    public static string AppDataFolder => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LaptopToolBox");
    private static readonly Dictionary<string, IElevatorCommand> Commands = new()
    {
        ["services_control"] = new ServicesControlCommand(),
    };

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(Path.Combine(AppDataFolder, "elevator.txt"), rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: 10, retainedFileTimeLimit: TimeSpan.FromDays(3))
            .MinimumLevel.Debug()
            .CreateLogger();
        
        Log.Information("Elevator started");

        string? command = null;
        var parameters = new Dictionary<string, string>();
        
        foreach (var arg in args)
        {
            if (!arg.StartsWith("--"))
            {
                command = arg;
                continue;
            }
            
            var indexOfEqualSign = arg.IndexOf('=');
            if (indexOfEqualSign <= 0)
            {
                continue;
            }
            
            var paramName = arg.Substring(2, indexOfEqualSign - 2);
            var paramValue = arg.Substring(indexOfEqualSign + 1).Trim('\"');
            parameters.Add(paramName, paramValue);
        }
        
        if (command == null)
        {
            Log.Error("No command specified");
            return;
        }
        
        Log.Information("Command: {Command}", command);
        
        foreach (var (key, value) in parameters)
        {
            Log.Information("{Key}={Value}", key, value);
        }
        
        if (!Commands.TryGetValue(command, out var commandInstance))
        {
            Log.Error("Command {Command} not found", command);
            return;
        }
        
        var exitCode = 0;
        
        try
        {
            exitCode = commandInstance.Execute(parameters);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception occurred while executing command {Command}", command);
            exitCode = 1;
        }
        
        Log.Information("Command {Command} executed with exit code {ExitCode}", command, exitCode);
        Environment.Exit(exitCode);
    }
}