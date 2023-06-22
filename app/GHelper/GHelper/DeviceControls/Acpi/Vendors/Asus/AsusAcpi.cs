﻿using System;
using GHelper.Serialization;
using Serilog;

namespace GHelper.DeviceControls.Acpi.Vendors.Asus;

public class AsusAcpi : IAcpi
{
    private readonly AsusAcpiHandleProvider _acpiHandleProvider;

    public bool IsAvailable => _acpiHandleProvider.TryGet(out _);
    
    public AsusAcpi()
    {
        _acpiHandleProvider = new AsusAcpiHandleProvider();
    }

    public int DeviceSet(uint deviceId, int status)
    {
        return BitConverter.ToInt32(DeviceSetWithBuffer(deviceId, status), 0);
    }
    
    public byte[] DeviceSetWithBuffer(uint deviceId, int status)
    {
        var serializer = new BinarySerializer();
        serializer.WriteUint(0x53564544);
        serializer.WriteUint(sizeof(uint) * 2);
        serializer.WriteUint(deviceId);
        serializer.WriteUint((uint) status);

        return CallMethod(serializer);
    }
    
    public int DeviceGet(uint deviceId)
    {
        return BitConverter.ToInt32(DeviceGetWithBuffer(deviceId), 0);
    }
    
    public byte[] DeviceGetWithBuffer(uint deviceId)
    {
        var serializer = new BinarySerializer();
        serializer.WriteUint(0x53545344);
        serializer.WriteUint(sizeof(uint));
        serializer.WriteUint(deviceId);

        return CallMethod(serializer);
    }
    
    private byte[] CallMethod(BinarySerializer serializer)
    {
        var outBuffer = new byte[20];
        CallDeviceIoControl(0x0022240C, serializer.ToArray(), outBuffer);
        return outBuffer;
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
            handle.DangerousGetHandle(),
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