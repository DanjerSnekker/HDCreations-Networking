using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Sockets;
using PlayTimePackets;

namespace MatchmakingServer
{
    public class Client
    {
        public Player Player { get; private set; }
        public Socket Socket { get; private set; }
        public Client(Socket socket)
        {
            this.Socket = socket;
            Player = new Player("Undefined");
        }
        public Client(Socket socket, Player player)
        {
            this.Socket = socket;
            this.Player = player;
        }
    }
}

