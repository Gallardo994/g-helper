using Ninject.Modules;

namespace LaptopToolBox.ViewModels;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<UpdatesViewModel>().ToSelf().InSingletonScope();
        Bind<AuraViewModel>().ToSelf().InSingletonScope();
        Bind<PerformanceModeViewModel>().ToSelf().InSingletonScope();
        Bind<TrayViewModel>().ToSelf().InSingletonScope();
        Bind<AutoStartViewModel>().ToSelf().InSingletonScope();
        Bind<BatteryLimitViewModel>().ToSelf().InSingletonScope();
        Bind<DeviceInformationViewModel>().ToSelf().InSingletonScope();
        Bind<StartMinimizedViewModel>().ToSelf().InSingletonScope();
        Bind<VendorServicesViewModel>().ToSelf().InSingletonScope();
        Bind<CpuFanViewModel>().ToSelf().InSingletonScope();
        Bind<GpuFanViewModel>().ToSelf().InSingletonScope();
        Bind<CpuMonitorViewModel>().ToSelf().InSingletonScope();
        Bind<SensorsViewModel>().ToSelf().InSingletonScope();
        Bind<PowerConsumersViewModel>().ToSelf().InSingletonScope();

        Bind<ModifyPerformanceProfileViewModel>().ToSelf().InTransientScope();
    }
}