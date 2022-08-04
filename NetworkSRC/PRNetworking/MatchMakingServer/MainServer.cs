//using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using PlayTimePackets;
using System.Diagnostics;
using MatchMakingServer;

namespace MatchmakingServer
{
    public class MainServer
    {
        static int portOffset = 1;

        static void Main(string[] args)
        {

            Socket LobbyListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            LobbyListeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3300));
            LobbyListeningSocket.Listen(20);
            LobbyListeningSocket.Blocking = false;

            //Console.WriteLine("Lobby Socket Listening On Port: " + 3300);

            Socket ClientListeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ClientListeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            ClientListeningSocket.Listen(10);
            ClientListeningSocket.Blocking = false;

            //Console.WriteLine("Client Socket Listening On Port: " + 3000);

            List<Client> ClientSockets = new List<Client>();
            List<Lobby> LobbySockets = new List<Lobby>();
            List<Lobby> LobbiesList = new List<Lobby>();

            List<string> LobbyNames = new List<string>();
            List<int> LobbyCodes = new List<int>();
            //bool lobbyCreated = false;

            Console.WriteLine("waiting for connection");


            while (true)
            {
                try
                {
                    ClientSockets.Add(new Client(ClientListeningSocket.Accept()));
                    Console.WriteLine("Client Connected");
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
                for (int i = 0; i < ClientSockets.Count; i++)
                {
                    if (ClientSockets[i].Socket.Available == ClientSockets.Count)
                    {
                        Console.WriteLine(ClientSockets[i].Socket.Available);
                    }
                }
                   Console.WriteLine(ClientSockets.Count);

                //Client Packet Loop
                for (int i = 0; i < ClientSockets.Count; i++)
                {
                    if (ClientSockets[i].Socket.Available > 0)
                    {
                        try
                        {
                            byte[] recievedBuffer = new byte[ClientSockets[i].Socket.Available];
                            ClientSockets[i].Socket.Receive(recievedBuffer);
                            BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
                            switch (pb.Type)
                            {
                                //Check for create lobby
                                case BasePacket.PacketType.CreateLobby:
                                    CreateLobbyPacket clp = (CreateLobbyPacket)new CreateLobbyPacket().DeSerialize(recievedBuffer);
                                    Random random = new Random();
                                    CreateLobby(clp.Name, ClientSockets[i].Player.ID, random.Next(1000, 9999));
                                    break;

                                //Check for Display Lobbies
                                case BasePacket.PacketType.DisplayLobby:
                                    DisplayLobbiesPacket dlp = (DisplayLobbiesPacket)new DisplayLobbiesPacket().DeSerialize(recievedBuffer);
                                    //Console.WriteLine("Recieved Request To Show Lobbies");
                                    //string lobbyname = string.Join(",", LobbyNames);
                                    ClientSockets[i].Socket.Send(new LobbyNamesPacket(LobbyNames, ClientSockets[i].Player).Serialize());
                                    Console.WriteLine("Showing Lobbies");
                                    break;

                                    //create player packet
                                case BasePacket.PacketType.CreatePlayer:
                                    CreatePlayerPacket cpp = (CreatePlayerPacket)new CreatePlayerPacket().DeSerialize(recievedBuffer);
                                    Socket tempSocket = ClientSockets[i].Socket;
                                    ClientSockets[i] = new Client(tempSocket, new Player(cpp.Name, cpp.Id));
                                    break;


                                    //check for roomcode input
                                case BasePacket.PacketType.JoinRequest:
                                    JoinRequestPacket jrp = (JoinRequestPacket)new JoinRequestPacket().DeSerialize(recievedBuffer);
                                   // Console.WriteLine("Recieved Join Lobby Request");
                                    for (int j = 0; j < LobbiesList.Count; j++)
                                    {
                                        if (LobbiesList[j].Name == jrp.RoomName)
                                        {
                                            Console.WriteLine("Client count then:");
                                            for (int e = 0; e < ClientSockets.Count; e++)
                                            {
                                                Console.WriteLine(ClientSockets[e]);
                                            }
                                            if (LobbiesList[j].Roomcode == jrp.RoomCode)
                                            {
                                                ClientSockets[i].Socket.Send(new LobbyInformationPacket(LobbiesList[j].Name, LobbiesList[j].Roomcode, LobbiesList[j].Port, null, Guid.Empty).Serialize());
                                                ClientSockets.Remove(ClientSockets[i]);
                                                //ClientSockets.Sort();
                                                Console.WriteLine("Client count now");
                                                for (int e = 0; e < ClientSockets.Count; e++)
                                                {
                                                    Console.WriteLine(ClientSockets[e]);
                                                }
                                                // Console.WriteLine("RoomCode Found! Sending Player To " + LobbiesList[i].Name);
                                            }
                                            else
                                            {
                                                //this is in case the room code inputted was invalid
                                                Console.WriteLine("Incorrect Room Code Inputted");
                                                ClientSockets[i].Socket.Send(new JoinRequestPacket("null", 0, "The Room You Entered Is Invalid", ClientSockets[i].Player).Serialize());
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                        catch (SocketException ex)
                        {
                            if (ex.SocketErrorCode != SocketError.WouldBlock) throw;
                        }
                    }
                }

                //Lobby Connect
                try
                {
                    LobbySockets.Add(new Lobby(LobbyListeningSocket.Accept()));
                    //Console.WriteLine("Lobby Created & Running");
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(ex);
                }
                //Lobby Packet Loop
                for (int i = 0; i < LobbySockets.Count; i++)
                {

                    byte[] recievedBuffer = new byte[LobbySockets[i].Socket.Available];
                    if (LobbySockets[i].Socket.Available > 1)
                    {
                        //recieving credentials from lobby
                        LobbySockets[i].Socket.Receive(recievedBuffer);
                        BasePacket pb = new BasePacket().DeSerialize(recievedBuffer);
                        switch (pb.Type)
                        {
                            case BasePacket.PacketType.Lobby:
                                LobbyInformationPacket lp = (LobbyInformationPacket)new LobbyInformationPacket().DeSerialize(recievedBuffer);
                                Console.WriteLine("creds recieved, grabbed: " + lp.Name + " with Lobby port: " + lp.LobbyPort + " RoomCode: " + lp.RoomCode);
                                //lobbyInformation.Add(new Lobby())
                                LobbiesList.Add(new Lobby(LobbySockets[i].Socket, lp.Name, lp.LobbyPort , lp.RoomCode));
                                LobbyNames.Add(lp.Name);
                                LobbyCodes.Add(lp.RoomCode);

                                for (int e = 0; e < ClientSockets.Count; e++)
                                {
                                    if (ClientSockets[e].Player.ID == lp.hostID)
                                    {
                                        ClientSockets[e].Socket.Send(lp.Serialize());
                                        //JoinClientToLobby(ClientSockets[e], lp.Name, lp.RoomCode, lp.LobbyPort);
                                        //Console.WriteLine(lp.player.ID + "Has Successfully Connected To " + lp.Name);
                                        ClientSockets.Remove(ClientSockets[e]);
                                    }
                                }
                                break;
                                // check for lobbypackey(close)
                        }
                    }
                }
            }

        }

        static void CreateLobby(string name, Guid clientId, int roomcode)
        {
            Process lobby = new Process();
            lobby.StartInfo.FileName = "LobbyServer.exe";
            lobby.StartInfo.Arguments = $"{name} {3300 + portOffset} {clientId} {roomcode}";
            lobby.Start();
            portOffset++;
        }

        static void RemoveLobbyFromList(Socket socket)
        {

        }

        static void JoinClientToLobby(Client client, string name, int roomcode, int port)
        {
            client.Socket.Send(new LobbyInformationPacket(name, roomcode, port, client.Player, client.Player.ID).Serialize());
        }

        static void AddClientSocket(Socket socket, List<Socket> sockets)
        {
            sockets.Add(socket);
        }
        static void RemoveClientSocket(Socket socket, List<Socket> sockets)
        {
            sockets.Remove(socket);
        }
        static void DisplayLobbies(List<Socket> listOfSockets)
        {
            for (int i = 0; i < listOfSockets.Count; i++)
            {
                //listOfSockets[i].Send(new DisplayLobbiesPacket().Serialize());
            }
        }
    }
}