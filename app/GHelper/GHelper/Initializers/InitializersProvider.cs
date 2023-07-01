﻿using System.Collections.Generic;
using GHelper.Initializers.ConcreteInitializers;
using Ninject;

namespace GHelper.Initializers;

public class InitializersProvider : IInitializersProvider
{
    private readonly List<IInitializer> _initializers = new();

    [Inject]
    public InitializersProvider(
        BatteryInitializer batteryInitializer,
        VendorKeyRegisterInitializer vendorKeyRegisterInitializer,
        CpuControlInitializer cpuControlInitializer,
        PerformanceModeInitializer performanceModeInitializer,
        KeyboardBacklightInitializer keyboardBacklightInitializer,
        AutoOverdriveInitializer autoOverdriveInitializer,
        HardwareMonitorInitializer hardwareMonitorInitializer,
        BackgroundAppUpdateInitializer backgroundAppUpdateInitializer,
        BackgroundUpdatesInitializer backgroundUpdatesInitializer,
        NavigationBellInitializer navigationBellInitializer,
        
        MainWindowInitializer mainWindowInitializer,
        IpcSubscriberInitializer ipcSubscriberInitializer
        )
    {
        _initializers.Add(batteryInitializer);
        _initializers.Add(vendorKeyRegisterInitializer);
        _initializers.Add(cpuControlInitializer);
        _initializers.Add(performanceModeInitializer);
        _initializers.Add(keyboardBacklightInitializer);
        _initializers.Add(autoOverdriveInitializer);
        _initializers.Add(hardwareMonitorInitializer);
        _initializers.Add(backgroundAppUpdateInitializer);
        _initializers.Add(backgroundUpdatesInitializer);
        _initializers.Add(navigationBellInitializer);
        
        _initializers.Add(mainWindowInitializer);
        _initializers.Add(ipcSubscriberInitializer);
    }
    
    public void InitializeAll()
    {
        foreach (var initializer in _initializers)
        {
            initializer.Initialize();
        }
    }
}