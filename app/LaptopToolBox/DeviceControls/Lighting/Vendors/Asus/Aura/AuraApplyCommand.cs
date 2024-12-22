using System.Threading.Tasks;
using LaptopToolBox.Commands;
using LaptopToolBox.DeviceControls.Usb;

namespace LaptopToolBox.DeviceControls.Lighting.Vendors.Asus.Aura;

public class AuraApplyCommand : IBackgroundCommand
{
    private readonly IUsb _usb;
    private readonly IHid _hid;
    private readonly byte[] _message;

    private readonly byte[] _messageSet;
    private readonly byte[] _messageApply;

    public AuraApplyCommand(IUsb usb, IHid hid, byte[] message)
    {
        _usb = usb;
        _hid = hid;

        _messageSet = new byte[]{ _usb.LightingHidId, 0xb5, 0, 0, 0 };
        _messageApply = new byte[]{ _usb.LightingHidId, 0xb4 };
        
        _message = message;
    }

    public void Execute()
    {
        var devices = _hid.GetHidDevicesBlocking(_usb.VendorId, _usb.DeviceIds);
            
        Parallel.ForEach(devices, device =>
        {
            device.OpenDevice();
            
            device.WriteFeatureData(_message);
            device.WriteFeatureData(new byte[]{ _usb.LightingHidId, 0xb5, 0, 0, 0 });
            device.WriteFeatureData(new byte[]{ _usb.LightingHidId, 0xb4 });
            
            device.CloseDevice();
        });
    }
}