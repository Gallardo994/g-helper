﻿using Ninject.Modules;

namespace GHelper.AutoEco;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<IAutoEco>().To<AutoEco>().InSingletonScope();
    }
}