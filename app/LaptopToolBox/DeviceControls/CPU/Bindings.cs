using Ninject.Modules;

namespace LaptopToolBox.DeviceControls.CPU;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<ICpuGeneralInfoProvider>().To<WmiCpuGeneralInfoProvider>().InSingletonScope();
    }
}