﻿using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
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

    public void KillServe()
    {
        try
        {
            foreach (Process proc in Process.GetProcessesByName("GenshinSwitch.WindowsService"))
            {
                try
                {
                    proc.Kill();
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
            isRunning = true;

            PipeSecurity ps = new();
            ps.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null!), PipeAccessRights.ReadWrite, AccessControlType.Allow));
            ps.SetAccessRule(new PipeAccessRule("Everyone", PipeAccessRights.ReadWrite, AccessControlType.Allow));

            while (isRunning)
            {
                try
                {
                    using NamedPipeServerStream pipeServer = new("GenshinSwitch.WindowsService", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 99999, 99999, ps);
                    pipeServer.WaitForConnection();
                    using StreamReader reader = new(pipeServer);

                    Debug.WriteLine("GenshinSwitch.WindowsService REACHED");

                    try
                    {
                        string? cmd = reader.ReadLine();
                        if (cmd == null)
                        {
                            continue;
                        }

                        Debug.WriteLine("Received message: " + cmd);

                        dynamic? cmdObj = JsonConvert.DeserializeObject(cmd!);
                        dynamic? retObj = CommandRunner.Run(cmdObj);

                        if (retObj != null)
                        {
                            using StreamWriter writer = new(stream: pipeServer);
                            string ret = JsonConvert.SerializeObject(retObj);
                            writer.WriteLine(ret);
                            writer.Flush();
                            Debug.WriteLine("Send message: " + ret);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        });
        thread.Start();
    }

    protected override void OnStop()
    {
        isRunning = false;
    }
}
