using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace MatchMakingServer
{
    public class Lobby
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public int Roomcode { get; set; }
        public Socket Socket { get; set; }

        public Lobby(Socket socket)
        {
            this.Socket = socket;
        }

        public Lobby(Socket socket,string name, int port, int roomcode)
        {
            this.Socket = socket;
            this.Name = name;
            this.Port = port;
            this.Roomcode = roomcode;
        }
    }
}
