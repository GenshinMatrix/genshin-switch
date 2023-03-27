using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace GenshinSwitch.WindowsService;

internal partial class MainService : ServiceBase
{
    private Thread thread = null!;
    private IContainer components = null!;
    private bool isRunning = false;

    public MainService()
    {
        InitializeComponent();
    }

    public void StartServe()
    {
        OnStart(null!);
    }

    private void InitializeComponent()
    {
        components = new Container();
        ServiceName = "GenshinSwitch.WindowsService";
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    protected override void OnStart(string[] args)
    {
        thread = new Thread(() =>
        {
            try
            {
                isRunning = true;

                PipeSecurity ps = new();
                ps.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null!), PipeAccessRights.ReadWrite, AccessControlType.Allow));
                ps.SetAccessRule(new PipeAccessRule("Everyone", PipeAccessRights.ReadWrite, AccessControlType.Allow));

                while (isRunning)
                {
                    using NamedPipeServerStream pipeServer = new("GenshinSwitch.WindowsService", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 99999, 99999, ps);
                    pipeServer.WaitForConnection();
                    using StreamReader reader = new(pipeServer);
                    
                    try
                    {
                        string? message = reader.ReadLine();
                        if (message == null) continue;

                        Debug.WriteLine("Received message: " + message);

                        dynamic? obj = JsonConvert.DeserializeObject(message!);
                        CommandRunner.Run(obj);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        });
        thread.Start();
    }

    protected override void OnStop()
    {
        isRunning = false;
    }
}
