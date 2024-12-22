using System.Collections.Generic;

namespace LaptopToolBox.ElevatorProxy;

public interface IElevatorProxy
{
    public int Execute(string command, Dictionary<string, string> args);
}