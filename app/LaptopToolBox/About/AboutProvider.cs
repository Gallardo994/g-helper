using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LaptopToolBox.About;

public partial class AboutProvider : ObservableObject, IAboutProvider
{
    [ObservableProperty] private ObservableCollection<IAboutItem> _items;
    
    public AboutProvider()
    {
        Items = new ObservableCollection<IAboutItem>
        {
            new AboutItem
            {
                Title = "GHelper",
                Description = "Lightweight Armoury Crate alternative for Asus laptops",
                Link = "https://github.com/seerge/g-helper",
                LicenseLink = "https://github.com/seerge/g-helper/blob/main/LICENSE",
            },
            new AboutItem
            {
                Title = "NvAPIWrapper",
                Description = "NvAPIWrapper is a .Net wrapper for NVIDIA public API, capable of managing all aspects of a display setup using NVIDIA GPUs",
                Link = "https://github.com/falahati/NvAPIWrapper",
                LicenseLink = "https://github.com/falahati/NvAPIWrapper/blob/master/LICENSE",
            },
            new AboutItem
            {
                Title = "hidlibrary",
                Description = "This library enables you to enumerate and communicate with Hid compatible USB devices in .NET",
                Link = "https://github.com/mikeobrien/HidLibrary",
                LicenseLink = "https://github.com/mikeobrien/HidLibrary/blob/master/LICENSE",
            },
            new AboutItem
            {
                Title = "Serilog",
                Description = "Serilog is a diagnostic logging library for .NET applications",
                Link = "https://github.com/serilog/serilog",
                LicenseLink = "https://github.com/serilog/serilog/blob/main/LICENSE",
            },
            new AboutItem
            {
                Title = "Ninject",
                Description = "Ninject is a lightning-fast, ultra-lightweight dependency injector for .NET applications",
                Link = "https://github.com/ninject/Ninject",
                LicenseLink = "https://github.com/ninject/Ninject/blob/main/LICENSE.txt",
            },
            new AboutItem
            {
                Title = "Newtonsoft.Json",
                Description = "Popular high-performance JSON framework for .NET",
                Link = "https://github.com/JamesNK/Newtonsoft.Json",
                LicenseLink = "https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md",
            },
            new AboutItem
            {
                Title = "WindowsAppSDK",
                Description = "Windows App SDK is a set of libraries, frameworks, components, and tools for Windows platform",
                Link = "https://github.com/microsoft/windowsappsdk",
                LicenseLink = "https://github.com/microsoft/WindowsAppSDK/blob/main/LICENSE",
            },
            new AboutItem
            {
                Title = "Interprocess",
                Description = "Cloudtoid Interprocess is a cross-platform shared memory queue for fast communication between processes",
                Link = "https://github.com/cloudtoid/interprocess",
                LicenseLink = "https://github.com/cloudtoid/interprocess/blob/main/LICENSE",
            },
            new AboutItem
            {
                Title = "CommunityToolkit.Mvvm",
                Description = ".NET Community Toolkit is a collection of helpers and APIs that work for all .NET developers and are agnostic of any specific UI platform",
                Link = "https://github.com/CommunityToolkit/dotnet",
                LicenseLink = "https://github.com/CommunityToolkit/dotnet/blob/main/License.md",
            },
            new AboutItem
            {
                Title = "NAudio",
                Description = "NAudio is an open source .NET audio library",
                Link = "https://github.com/naudio/NAudio",
                LicenseLink = "https://github.com/naudio/NAudio/blob/master/license.txt",
            },
            new AboutItem
            {
                Title = "Vanara",
                Description = "This project contains various .NET assemblies that contain P/Invoke functions, interfaces, enums and structures from Windows libraries",
                Link = "https://github.com/dahall/vanara",
                LicenseLink = "https://github.com/dahall/Vanara/blob/master/LICENSE",
            },
            new AboutItem
            {
                Title = "TaskScheduler",
                Description = "The original .NET wrapper for the Windows Task Scheduler that aggregates the multiple versions and provides localized controls for editing.",
                Link = "https://github.com/dahall/taskscheduler",
                LicenseLink = "https://github.com/dahall/TaskScheduler/blob/master/license.md",
            }
        };
    }
}