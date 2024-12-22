using System;
using System.Diagnostics;
using System.IO;
using Microsoft.UI.Xaml;
using System.Reflection;
using System.Security.Principal;
using LaptopToolBox.Configs;
using LaptopToolBox.Helpers;
using LaptopToolBox.Initializers;
using LaptopToolBox.Injection;
using LaptopToolBox.IPC.Messages;
using LaptopToolBox.IPC.Publishers;
using Ninject;
using Serilog;

namespace LaptopToolBox
{
    public partial class App
    {
        public App()
        {
            var appDataLogPath = System.IO.Path.Combine(ApplicationHelper.AppDataFolder, "log.txt");
            if (!Directory.Exists(ApplicationHelper.AppDataFolder))
            {
                Directory.CreateDirectory(ApplicationHelper.AppDataFolder);
            }
            
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(appDataLogPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10, retainedFileTimeLimit: TimeSpan.FromDays(3))
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();
            
            Log.Information("Application started");
            
            UnhandledException += (sender, args) =>
            {
                Log.Error(args.Exception, "Unhandled exception in App");
            };
            
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Log.Error((Exception) args.ExceptionObject, "Unhandled exception in AppDomain");
            };
            
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            
            Services.ResolutionRoot = kernel;
            
            if (FocusSameInstance())
            {
                Log.Information("Another instance is running, exiting");
                ApplicationHelper.Exit();
                return;
            }
            
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Log.Debug("Application launched");

            Services.ResolutionRoot.Get<IConfig>().ReadFromLocalStorage();
            Services.ResolutionRoot.Get<IInitializersProvider>().InitializeAll();
        }
        
        private bool FocusSameInstance()
        {
            var currentProcess = Process.GetCurrentProcess();
            
            var processes = Process.GetProcessesByName(currentProcess.ProcessName);
            Log.Debug("Found {ProcessCount} processes with name {ProcessName}", processes.Length, currentProcess.ProcessName);
            
            foreach (var process in processes)
            {
                if (process.Id == currentProcess.Id)
                {
                    continue;
                }

                try
                {
                    var publisher = new IpcPublisher(process.Id);
                    var result = publisher.Publish(new IpcBringToFront());
                
                    Log.Debug("Published {Message} to process {ProcessId}, result: {Result}", typeof(IpcBringToFront), process.Id, result);
                }
                catch (Exception e)
                {
                    Log.Error(e, "Failed to publish {Message} to process {ProcessId}", typeof(IpcBringToFront), process.Id);
                }
            }
            
            return processes.Length > 1;
        }
    }
}
