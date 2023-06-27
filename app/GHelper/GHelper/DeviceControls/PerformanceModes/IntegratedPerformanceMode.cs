﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace GHelper.DeviceControls.PerformanceModes;

public partial class IntegratedPerformanceMode : ObservableObject, IPerformanceMode
{
    [ObservableProperty] private Guid _id;
    [ObservableProperty] private string _title;
    [ObservableProperty] private string _description;
    [ObservableProperty] private string _icon;
    [ObservableProperty] private PerformanceModeType _type;
}