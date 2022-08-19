using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Diagnostics;
using PlayTimePackets;
using MatchmakingServer;

namespace LobbyServer
{
    public class LobbyServer
    {
        static int portOffset = 100;
        static int currentport;
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
            bool someoneConnected = false;

            while (true)
            {
                try
                {
                    clients.Add(new Client(listeningSocket.Accept()));
                    Console.WriteLine("A Client Joined Lobby " + name);
                    someoneConnected = true;
                }
                catch (SocketException ex)
                {
                    //Console.WriteLine("No New Client Found");
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
                if (clients.Count > 2) // if a third person connects..then kick them here cuz lobby would be full by 2 people..
                {
                    clients[clients.Count - 1].Socket.Send(new KickRequestPacket("Lobby Is Full", clients[2].Player).Serialize());
                    clients.RemoveAt(clients.Count - 1);
                    Console.WriteLine(" Kicking Third Person");
                }
                if (someoneConnected && clients.Count == 0)
                {
                    MainSocket.Send(new LeaveRequestPacket(name, false, null).Serialize());
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
                                //check for chat
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
                                    for (int e = 0; e < clients.Count; e++)
                                    {
                                        Console.WriteLine(clients[e]);
                                        if (e != i)
                                        {
                                            clients[e].Socket.Send(recievedBuffer);
                                            clients.Remove(clients[e]);
                                        }
                                    }

                                    break;
                                //Check for Start
                                case BasePacket.PacketType.StartGame:
                                    StartGamePacket sgp = (StartGamePacket)new StartGamePacket().DeSerialize(recievedBuffer);
                                    Console.WriteLine("Starting Game");
                                    CreateGame();
                                    for (int e = 0; e < clients.Count; e++)
                                    {
                                        clients[e].Socket.Send(new StartGamePacket(currentport, clients[e].Player).Serialize());             
                                    }
                                    break;
                                
                                    //Check for Leave Request (If Host Close Lobby)
                                case BasePacket.PacketType.LeaveLobby:
                                    LeaveRequestPacket lrp = (LeaveRequestPacket)new LeaveRequestPacket().DeSerialize(recievedBuffer);
                                    if (lrp.isHost)
                                    {
                                        for (int e = 0; e < clients.Count; e++)
                                        {
                                            Console.WriteLine(clients.Count);
                                            clients[e].Socket.Send(new KickRequestPacket("Lobby Disbanded", clients[e].Player).Serialize());
                                            Console.WriteLine("removed " + clients[e]);
                                            clients.Remove(clients[e]);
                                            e--;
                                        }
                                        if (clients.Count == 0)
                                        {
                                            MainSocket.Send(recievedBuffer);
                                            Console.WriteLine("removed everyone");
                                        }
                                    }
                                    else
                                    {
                                        clients[i].Socket.Send(new KickRequestPacket("", clients[i].Player).Serialize());
                                        clients.Remove(clients[i]);
                                        Console.WriteLine("removing the only person who left...");
                                    }
                                    
                                    break;
                                //check if anyone closed the game or ALT-F4
                                case BasePacket.PacketType.PlayerShutDown:
                                    PlayerShutDownPacket psp = (PlayerShutDownPacket)new PlayerShutDownPacket().DeSerialize(recievedBuffer);
                                    clients.Remove(clients[i]);
                                    break;
                            }
                            
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
        }

        static void CreateGame()
        {
            Process game = new Process();
            game.StartInfo.FileName = "GameServer.exe";
            game.StartInfo.Arguments = $"{3300 + portOffset}";
            currentport = 3300 + portOffset;
            game.Start();
            portOffset++;
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
