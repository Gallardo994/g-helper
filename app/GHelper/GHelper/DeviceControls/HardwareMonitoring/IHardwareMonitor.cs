﻿using System;

namespace GHelper.DeviceControls.HardwareMonitoring;

public interface IHardwareMonitor
{
    public IHardwareReport HardwareReport { get; }
    public event Action<IHardwareReport> HardwareReportUpdated;
    
    public void StartMonitoring();
    public void StopMonitoring();
}