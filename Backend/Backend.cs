using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Backend.Commands;
using Backend.Commands.aelita;
using Backend.Commands.xana;
using LyokoPluginLoader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;

namespace Backend
{
    public class Backend
    {
        private static Backend instance;
        private List<Command> Commands;
        private Listener _listener;

        private Backend(int port, IApplicationLifetime lifetime)
        {
            Commands = new List<Command>(){new Xana(), new Aelita()};
            var help = new Help( ref Commands);
            _listener = new Listener(Commands,port);
            PluginLoader loader = new PluginLoader(Path.Combine(Directory.GetCurrentDirectory(),"Plugins"));
            LoaderInfo.DevMode = true;
            lifetime.ApplicationStopping.Register((() => loader.DisableAll()));  
        }

        public static Backend Initialize(int port, IApplicationLifetime lifetime)
        {
            if (instance == null)
            {
                return instance = new Backend(port, lifetime);
            }
            else
            {
                return instance;
            }
        }
    }
}