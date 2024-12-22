namespace Elevator;

public interface IElevatorCommand
{
    public int Execute(Dictionary<string, string> args);
}