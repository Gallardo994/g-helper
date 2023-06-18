﻿using System;
using GHelper.Serialization;
using Ninject;
using Serilog;

namespace GHelper.DeviceControls;

public class AsusAcpi : IAcpi
{
    private readonly IAcpiHandleProvider _acpiHandleProvider;
    
    const uint ControlCode = 0x0022240C;
    const uint Devs = 0x53564544;

    public bool IsAvailable => _acpiHandleProvider.TryGet(out _);
    
    [Inject]
    public AsusAcpi(IAcpiHandleProvider acpiHandleProvider)
    {
        _acpiHandleProvider = acpiHandleProvider;
    }

    public int DeviceSet(uint deviceId, int status, string logName)
    {
        var serializer = new BinarySerializer();
        serializer.WriteUint(Devs);
        serializer.WriteUint(sizeof(uint) * 2);
        serializer.WriteUint(deviceId);
        serializer.WriteUint((uint) status);

        var callStatus = CallMethod(serializer);

        Log.Debug(logName + " = " + status + " : " + (callStatus == 1 ? "OK" : callStatus));
        return callStatus;
    }
    
    
    private int CallMethod(BinarySerializer serializer)
    {
        var outBuffer = new byte[20];
        CallDeviceIoControl(ControlCode, serializer.ToArray(), outBuffer);
        return BitConverter.ToInt32(outBuffer, 0);
    }

    private void CallDeviceIoControl(uint dwIoControlCode, byte[] lpInBuffer, byte[] lpOutBuffer)
    {
        if (!_acpiHandleProvider.TryGet(out var handle))
        {
            Log.Error("Failed to get handle to ACPI device");
            return;
        }
        
        uint lpBytesReturned = 0;
        Native.DeviceIoControl(
            handle,
            dwIoControlCode,
            lpInBuffer,
            (uint)lpInBuffer.Length,
            lpOutBuffer,
            (uint)lpOutBuffer.Length,
            ref lpBytesReturned,
            IntPtr.Zero
        );
    }
}