using System;
using System.IO;
using Windows.ApplicationModel;
using LaptopToolBox.Helpers.Native;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Vanara.PInvoke;

namespace LaptopToolBox.Helpers;

public class WindowHelper
{
    public static IntPtr GetHandleOf(Window window)
    {
        return WinRT.Interop.WindowNative.GetWindowHandle(window);
    }
    
    public static AppWindow GetAppWindowOf(Window window)
    {
        var handle = GetHandleOf(window);
        var windowId = Win32Interop.GetWindowIdFromWindow(handle);
        return AppWindow.GetFromWindowId(windowId);
    }
    
    public static void SetRoundedCorners(Window window, DwmWindowCornerPreference preference)
    {
        var handle = GetHandleOf(window);
        DwmApi.DwmSetWindowAttribute(
            handle,
            DwmWindowAttribute.DwmwaWindowCornerPreference,
            ref preference,
            sizeof(uint));
    }

    public static void SetTransparent(Window window, bool state)
    {
        var handle = GetHandleOf(window);
        var extendedStyle = User32.GetWindowLong(handle, User32.WindowLongFlags.GWL_EXSTYLE);
    
        if (state)
        {
            User32.SetWindowLong(handle, User32.WindowLongFlags.GWL_EXSTYLE, extendedStyle | 0x80000);
        }
        else
        {
            User32.SetWindowLong(handle, User32.WindowLongFlags.GWL_EXSTYLE, extendedStyle & ~0x80000);
        }
    }

    public static void ConvertToOverlay(Window window)
    {
        var appWindow = GetAppWindowOf(window);
        appWindow.IsShownInSwitchers = false;

        var presenter = appWindow.Presenter as OverlappedPresenter;
            
        if (presenter == null)
        {
            throw new InvalidOperationException("Unable to extract presenter from AppWindow");
        }
            
        presenter.IsMaximizable = false;
        presenter.IsMinimizable = false;
        presenter.IsAlwaysOnTop = true;
        presenter.IsResizable = false;
        presenter.SetBorderAndTitleBar(true, false);
    }

    public static void SetWindowAlpha(Window window, byte alpha)
    {
        var handle = GetHandleOf(window);
        User32.SetLayeredWindowAttributes(handle, 0, alpha, User32.LayeredWindowAttributes.LWA_ALPHA);
    }

    public static DisplayArea GetDisplayArea(Window window)
    {
        var handle = GetHandleOf(window);
        var windowId = Win32Interop.GetWindowIdFromWindow(handle);
        return DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Nearest);
    }

    public static bool ShowWindow(IntPtr handle, ShowWindowCommand command) => User32.ShowWindow(handle, command);
    public static bool ShowWindow(HWND handle, ShowWindowCommand command) => User32.ShowWindow(handle, command);
    public static bool ShowWindow(Window window, ShowWindowCommand command) => ShowWindow(GetHandleOf(window), command);

    public static bool FocusWindow(IntPtr handle) => User32.SetForegroundWindow(handle);
    public static bool FocusWindow(HWND handle) => User32.SetForegroundWindow(handle);
    public static bool FocusWindow(Window window) => FocusWindow(GetHandleOf(window));
    
    public static bool IsMinimized(IntPtr handle) => User32.IsIconic(handle);
    public static bool IsMinimized(HWND handle) => User32.IsIconic(handle);
    public static bool IsMinimized(Window window) => User32.IsIconic(GetHandleOf(window));

    public static void SetIcon(IntPtr handle, string iconName)
    {
        var iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, iconName);
        var windowId = Win32Interop.GetWindowIdFromWindow(handle);
        var appWindow = AppWindow.GetFromWindowId(windowId);
        appWindow.SetIcon(iconPath);
    }
    
    public static void SetIcon(Window window, string iconName) => SetIcon(GetHandleOf(window), iconName);
}