using System;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Serilog;

public static class ServiceControllerExtensions
{
    [DllImport("advapi32.dll", EntryPoint = "OpenSCManagerW", ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr OpenSCManager(string machineName, string databaseName, ScmAccessRights dwDesiredAccess);

    [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, ServiceAccessRights dwDesiredAccess);

    [DllImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ChangeServiceConfig(IntPtr hService, uint nServiceType, uint nStartType, uint nErrorControl, string lpBinaryPathName, string lpLoadOrderGroup, IntPtr lpdwTagId, [In] char[] lpDependencies, string lpServiceStartName, string lpPassword, string lpDisplayName);

    [DllImport("advapi32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseServiceHandle(IntPtr hSCObject);

    public enum ScmAccessRights
    {
        AllAccess = 0xF003F,
        Connect = 0x0001,
        CreateService = 0x0002,
        EnumerateService = 0x0004,
        Lock = 0x0008,
        QueryLockStatus = 0x0010,
        ModifyBootConfig = 0x0020
    }

    public enum ServiceAccessRights
    {
        AllAccess = 0xF01FF,
        ChangeConfig = 0x0002,
        EnumerateDependents = 0x0008,
        Interrogate = 0x0080,
        PauseContinue = 0x0040,
        QueryConfig = 0x0001,
        QueryStatus = 0x0004,
        Start = 0x0010,
        Stop = 0x0020
    }

    public static void SetStartMode(this ServiceController service, ServiceStartMode mode)
    {
        var scmHandle = OpenSCManager(null, null, ScmAccessRights.AllAccess);
        if (scmHandle == IntPtr.Zero)
        {
            var error = Marshal.GetLastWin32Error();
            Log.Error("Could not open service control manager (error {Error}).", error);
            return;
        }

        var serviceHandle = OpenService(scmHandle, service.ServiceName, ServiceAccessRights.AllAccess);
        if (serviceHandle == IntPtr.Zero)
        {
            var error = Marshal.GetLastWin32Error();
            Log.Error("Could not open service (error {Error}).", error);
            return;
        }

        try
        {
            var success = ChangeServiceConfig(serviceHandle, 0xFFFFFFFF, (uint)mode, 0xFFFFFFFF, null, null, IntPtr.Zero, null, null, null, null);

            if (!success)
            {
                var error = Marshal.GetLastWin32Error();
                Log.Error("Could not change service start type (error {Error}).", error);
            }
        }
        finally
        {
            CloseServiceHandle(serviceHandle);
            CloseServiceHandle(scmHandle);
        }
    }
}