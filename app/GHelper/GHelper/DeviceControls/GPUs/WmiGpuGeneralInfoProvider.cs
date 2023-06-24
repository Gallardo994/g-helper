﻿using System.Collections.ObjectModel;
using System.Management;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GHelper.DeviceControls.GPUs;

public partial class WmiGpuGeneralInfoProvider : ObservableObject, IGpuGeneralInfoProvider
{
    [ObservableProperty] private ObservableCollection<IGpuGeneralInfo> _gpuGeneralInfoCollection;

    public WmiGpuGeneralInfoProvider()
    {
        Refresh();
    }

    public void Refresh()
    {
        GpuGeneralInfoCollection = new ObservableCollection<IGpuGeneralInfo>();
        
        using var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
        
        foreach (var obj in searcher.Get())
        {
            var gpuGeneralInfo = new GpuGeneralInfo
            {
                DeviceName = obj["Name"].ToString(),
                DeviceId = obj["DeviceID"].ToString(),
                AdapterRam = uint.Parse(obj["AdapterRAM"].ToString()),
                AdapterDacType = obj["AdapterDACType"].ToString(),
                Monochrome = obj["Monochrome"].ToString() == "True",
                InstalledDisplayDrivers = obj["InstalledDisplayDrivers"].ToString(),
                DriverVersion = obj["DriverVersion"].ToString(),
                VideoProcessor = obj["VideoProcessor"].ToString(),
                VideoArchitecture = ushort.Parse(obj["VideoArchitecture"].ToString()),
                VideoMemoryType = ushort.Parse(obj["VideoMemoryType"].ToString()),
            };
            
            GpuGeneralInfoCollection.Add(gpuGeneralInfo);
        }
    }
}