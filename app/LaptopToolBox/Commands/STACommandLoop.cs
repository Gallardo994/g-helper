using System;
using LaptopToolBox.AppWindows;
using Microsoft.UI.Dispatching;
using Ninject;
using Serilog;

namespace LaptopToolBox.Commands;

public class STACommandLoop : ISTACommandLoop
{
    private readonly DispatcherQueue _dispatcherQueue;
    
    [Inject]
    public STACommandLoop(MainWindow mainWindow)
    {
        _dispatcherQueue = mainWindow.DispatcherQueue;
    }
    
    public void Enqueue(ISTACommand command)
    {
        void ActionWrapper()
        {
            try
            {
                command.Execute();
            } 
            catch (Exception e)
            {
                Log.Error(e, "Exception occurred in STACommandLoop for command {CommandType}", command.GetType());
            }
        }
        
        if (_dispatcherQueue.HasThreadAccess)
        {
            ActionWrapper();
            return;
        }
        
        _dispatcherQueue.TryEnqueue(ActionWrapper);
    }
    
    public void Enqueue(Action action)
    {
        Enqueue(new ActionCommand(action));
    }
}