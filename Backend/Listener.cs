using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Threading.Tasks;
using Backend.Commands;
using LyokoAPI.Events;
using LyokoAPI.Events.EventArgs;
using LyokoAPI.VirtualStructures;
using LyokoAPI.VirtualStructures.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Backend
{
    public class Listener
    {
        private HubConnection connection;
        private List<Command> Commands;

        public Listener(ICollection<Command> commands, int port)
        {
            Commands = new List<Command>(commands);
            /*ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;*/
            connection = new HubConnectionBuilder().WithUrl($"http://localhost:{port}/MiniLyokoHub").Build();

            StartConnection();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            TowerActivationEvent.Subscribe(OnTowerActivation);
            TowerDeactivationEvent.Subscribe(OnTowerDeactivation);
            TowerHijackEvent.Subscribe(OnTowerHijack);
            CommandOutputEvent.Subscribe(OnCommandOutput);
            LyokoLogger.Subscribe(OnLogger);
            CommandInputEvent.Subscribe(OnCommand);
        }

        private async void StartConnection()
        {
            connection.On("CommandInputEvent", (string command) => { OnSignalRCommand(command); });
            await connection.StartAsync();
        }


        private void OnTowerActivation(ITower tower)
        {
            connection.InvokeAsync("TowerActivationEvent", tower);
        }

        private void OnTowerDeactivation(ITower tower)
        {
            connection.InvokeAsync("TowerDeactivationEvent", tower);
        }

        private void OnTowerHijack(ITower tower, APIActivator oldActivator, APIActivator newActivator)
        {
            connection.InvokeAsync("TowerHijackEvent", tower, oldActivator, newActivator);
        }


        private void OnSignalRCommand(string command)
        {
            CommandInputEvent.Call(command);
        }

        private void OnCommandOutput(string arg)
        {
            connection.InvokeAsync("CommandOutputEvent", arg);
        }

        private void OnLogger(string arg)
        {
            connection.InvokeAsync("LoggerEvent", arg);
        }

        private void OnCommand(string arg)
        {
            string[] commandargs = arg.Split(".");
            var commandname = commandargs[0];
            if (commandargs.Length > 1)
            {
                commandargs = commandargs.ToList().GetRange(1, commandargs.Length - 1).ToArray();
            }
            else
            {
                commandargs = new string[] { };
            }
            var command = Commands.Find(commandd => commandd.Name.Equals(commandname));
            command?.Run(commandargs);
        }
    }
}