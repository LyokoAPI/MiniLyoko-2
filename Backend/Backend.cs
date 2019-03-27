using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Backend.Commands;
using Backend.Commands.aelita;
using Backend.Commands.xana;
using LyokoPluginLoader;

namespace Backend
{
    public class Backend
    {
        private static Backend instance;
        private List<Command> Commands;
        private Listener _listener;

        private Backend(int port)
        {
            Commands = new List<Command>(){new Xana(), new Aelita()};
            _listener = new Listener(Commands,port);
            PluginLoader loader = new PluginLoader(Path.Combine(Directory.GetCurrentDirectory(),"Plugins"));
        }

        public static Backend Initialize(int port)
        {
            if (instance == null)
            {
                return instance = new Backend(port);
            }
            else
            {
                return instance;
            }
        }
    }
}