using LyokoAPI.VirtualStructures;
using LyokoAPI.VirtualStructures.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace MiniLyokoV2.SignalR
{
    public class MiniLyokoHub : Hub
    {
        public void TowerActivationEvent(ITower tower)
        {
            Clients.All.SendAsync("TowerActivationEvent", tower);
        }

        public void TowerDeactivationEvent(ITower tower)
        {
            Clients.All.SendAsync("TowerDeactivationEvent", tower);
        }

        public void TowerHijackEvent(ITower tower, APIActivator oldActivator, APIActivator newActivator)
        {
            Clients.All.SendAsync("TowerHijackEvent", tower, oldActivator, newActivator);
        }

        public void CommandInputEvent(string command)
        {
            Clients.Others.SendAsync("CommandInputEvent", command);
        }

        public void CommandOutputEvent(string command)
        {
            Clients.All.SendAsync("CommandOutputEvent", command);
        }

        public void LoggerEvent(string message)
        {
            Clients.All.SendAsync("LoggerEvent", message);
        }
    }
}