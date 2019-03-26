using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Threading.Tasks;
using Backend.Commands;
using LyokoAPI.Events;
using LyokoAPI.VirtualStructures;
using LyokoAPI.VirtualStructures.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace Backend
{
    public class Listener
    {
        private HubConnection connection;
        private List<Command> Commands;

        public Listener(ICollection<Command> commands,int port)
        {
            Commands = new List<Command>(commands);
            /*ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;*/
            connection = new HubConnectionBuilder().WithUrl($"http://localhost:{port}/MiniLyokoHub" ).Build();
            
            StartConnection();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0,5) * 1000);
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
            
            connection.On("CommandInputEvent", (string command) => OnSignalRCommand(command));
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
        
        private void OnCommandOutput(string output)
        {
            connection.InvokeAsync("CommandOutputEvent", output);
        }
       
        private void OnLogger(string message)
        {
            connection.InvokeAsync("LoggerEvent", message);
        }

        private void OnCommand(string commandstring)
        {
            string[] commandargs = commandstring.Split(".");
            var commandname = commandargs[0];
            if (commandargs.Length < 2)
            {
                commandargs = new string[]{};
            }
            else
            {
                commandargs = commandargs.ToList().GetRange(1, commandargs.Length).ToArray();
            }

            var command = Commands.FirstOrDefault(commandd => commandd.Name == commandname);
            command?.Run(commandargs);
            if (command == null)
            {
                CommandOutputEvent.Call(commandstring,"Command not recognized!");
            }
            
        }
        
        
        
        
        

    }
}