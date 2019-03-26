using System.Collections.Generic;
using Backend.Commands;
using Backend.Commands.aelita;
using Backend.Commands.xana;

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