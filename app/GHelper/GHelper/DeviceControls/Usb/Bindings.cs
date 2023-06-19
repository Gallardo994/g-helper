﻿using GHelper.DeviceControls.Usb.Vendors.Asus;
using Ninject.Modules;

namespace GHelper.DeviceControls.Usb;

public class Bindings : NinjectModule
{
    public override void Load()
    {
        Bind<IUsb>().To<AsusUsb>().InSingletonScope();
    }
}