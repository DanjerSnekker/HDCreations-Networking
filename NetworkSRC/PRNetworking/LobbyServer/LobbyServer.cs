using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PlayTimePackets;
using MatchmakingServer;

namespace LobbyServer
{
    public class LobbyServer
    {
        static void Main(string[] args)
        {
            string name = "THE KILLERS";
            int port = 3301;
            int roomCode = RandomRoomCode(1000, 9999);
            Guid hostID = Guid.Empty;
            Player HostPlayer = new Player("host", true);

            if (args.Length == 4)
            {
                name = args[0];
                port = Int32.Parse(args[1]);
                hostID = new Guid(args[2]);
                roomCode = Int32.Parse(args[3]);
            }

            HostPlayer = new Player("Host", hostID);
            //Console.WriteLine("lobby name is " + name + " with port number: " + port + " Host ID: " + hostID + " Roomcode: " + roomCode);

            Socket MainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            MainSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3300));
            LobbyInformationPacket lp = new LobbyInformationPacket(name, 1000, port, HostPlayer, HostPlayer.ID);
            //Console.WriteLine(lp.hostID);
            MainSocket.Send(new LobbyInformationPacket(name, roomCode, port, HostPlayer, HostPlayer.ID).Serialize());
            // send display lobby packet here to main server to send it to the player...cuz new lobby has been created??

            Console.WriteLine("Connected To Main Server & Waiting For Clients");

            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            listeningSocket.Listen(2);
            listeningSocket.Blocking = false;

            List<Client> clients = new List<Client>();

            //there are two while loops becuz something is breaking the second one
            //first one is working but its connecting an unknown client in the beginning!?

            while (true)
            {
                //Console.WriteLine("spam this tests!");
                try
                {
                    clients.Add(new Client(listeningSocket.Accept()));
                    Console.WriteLine("A Client Joined Lobby " + name);
                    // MainSocket.Send(new DisplayLobbiesPacket().Serialize()); // this if for tyesting
                }
                catch (SocketException ex)
                {
                    //Console.WriteLine("No New Client Found");
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
                if(clients.Count > 2) // if a third person connects..then kick them here cuz lobby would be full by 2 people..
                {
                    clients[clients.Count - 1].Socket.Send(new KickRequestPacket("Lobby Is Full", clients[2].Player).Serialize());
                    clients[clients.Count - 1].Socket.Disconnect(false);
                    clients.RemoveAt(clients.Count - 1);
                    Console.WriteLine(" Kicking Third Person");
                }

                //Console.WriteLine("Spam This Test");
                for (int i = 0; i < clients.Count; i++)    
                {
                    //client packet loop
                    if (clients[i].Socket.Available > 0)
                    {
                        try
                        {
                            byte[] recievedBuffer = new byte[clients[i].Socket.Available];
                            clients[i].Socket.Receive(recievedBuffer);
                            BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
                            switch (pb.Type)
                            {
                                case BasePacket.PacketType.Message:

                                    MessagePacket packet = (MessagePacket)new MessagePacket().DeSerialize(recievedBuffer);
                                    Console.WriteLine($"someone is saying {packet.Message}");
                                    for (int e = 0; e < clients.Count; e++)
                                    {
                                        if (e != i)
                                        {
                                            clients[e].Socket.Send(recievedBuffer);
                                        }
                                    }
                                    break;

                                //Check for Kick Request
                                case BasePacket.PacketType.KickRequest:
                                    KickRequestPacket krp = (KickRequestPacket)new KickRequestPacket().DeSerialize(recievedBuffer);
                                    Console.WriteLine("Recieved Kick Request + ");
                                    /*for (int e = 0; e < clients.Count; e++)
                                    {
                                        if (e != i)
                                        {
                                            clients[e].Socket.Send(recievedBuffer);
                                        }
                                    }
                                    clients.Remove(clients[i]);*/
                                    break;
                                case BasePacket.PacketType.StartGame:
                                    StartGamePacket sgp = (StartGamePacket)new StartGamePacket().DeSerialize(recievedBuffer);
                                    Console.WriteLine("Starting Game");
                                    for (int e = 0; e < clients.Count; e++)
                                    { 
                                        clients[e].Socket.Send(recievedBuffer);             
                                    }
                                    break;
                            }
                            //Check for Leave Request (If Host Close Lobby)
                            //Check for Start
                            for (int e = 0; e < clients.Count; e++)
                            {
                                if (e != i)
                                {
                                    clients[e].Socket.Send(recievedBuffer);
                                }
                            }
                        }
                        catch (SocketException ex)
                        {
                            if (ex.SocketErrorCode != SocketError.WouldBlock) throw;
                        }
                    }
                }
            }
            while (true)
            {
                //server packet loop;
                try
                {
                    byte[] recievedBuffer = new byte[MainSocket.Available];
                    MainSocket.Receive(recievedBuffer);
                    //Look for packet from main
                    BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
                    switch (pb.Type)
                    {
                        case BasePacket.PacketType.DisplayLobby:
                           // MainSocket.Send(new DisplayLobbiesPacket().Serialize());
                            break;
                    }

                    // Check for roomcode request
                    // Check for port request
                    // Check for name request
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock) Console.WriteLine(ex);//throw;
                }
            }
        }

        static int RandomRoomCode(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        static void KickPartner(List<Client> clients)
        {
            clients.Remove(clients[1]);
        }

        public void GetRoomCode()
        {

        }

        public void GetPort()
        {

        }
        public void GetName()
        {

        }
        public void CloseLobby()
        {

        }
        public void GetLobbyInfo()
        {

        }
    }
}
