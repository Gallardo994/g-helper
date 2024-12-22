using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LaptopToolBox.ElevatorProxy;

public class ExeElevatorProxy : IElevatorProxy
{
    private const string ElevatorPath = "Elevator\\Elevator.exe";
    
    public int Execute(string command, Dictionary<string, string> args)
    {
        var arguments = new StringBuilder();
        arguments.Append(command);
        foreach (var (key, value) in args)
        {
            arguments.Append($" --{key}=\"{value}\"");
        }
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = ElevatorPath,
            Arguments = arguments.ToString(),
            UseShellExecute = true,
            Verb = "runas",
        };

        var process = Process.Start(processStartInfo);
        process?.WaitForExit();
        
        return process?.ExitCode ?? -1;
    }
}