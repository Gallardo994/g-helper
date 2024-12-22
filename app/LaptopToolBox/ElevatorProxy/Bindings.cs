using Ninject.Modules;

namespace LaptopToolBox.ElevatorProxy;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<IElevatorProxy>().To<ExeElevatorProxy>().InSingletonScope();
    }
}