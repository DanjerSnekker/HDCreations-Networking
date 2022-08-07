﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using PlayTimePackets;
using GamePackets;
using UnityEngine;


namespace GameServer
{
    internal class GameServer
    {
        static void Main(string[] args)
        {
            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listeningSocket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            listeningSocket.Listen(10);
            listeningSocket.Blocking = false;


            Console.WriteLine("We are currently expecting a few guests..");

            List<Client> clients = new List<Client>();

            Player serverPlayer = new Player("1");
            //string message = "Welcome my friend";

            bool playerCredentialsGrabbed = false;

            while (true)
            {
                try
                {
                    clients.Add(new Client(listeningSocket.Accept()));
                    Console.WriteLine($"Ah they have arrived.");

                    //clients[clients.Count - 1].Send(new MessagePacket(message, serverPlayer).Serialize());
                }
                catch (SocketException sE)
                {
                    if (sE.SocketErrorCode != SocketError.WouldBlock)
                        Console.WriteLine(sE);
                }

                for (int i = clients.Count - 1; i >= 0; i--)
                {
                    try
                    {
                        if (clients[i].Socket.Available > 0)
                        {
                            byte[] receivedBuffer = new byte[clients[i].Socket.Available];
                            clients[i].Socket.Receive(receivedBuffer);

                            GameBasePacket pb = new GameBasePacket().DeSerialize(receivedBuffer);

                            switch (pb.Type)
                            {
                                case GameBasePacket.PacketType.PlayerInfo:
                                    PlayerInfoPacket piPack = (PlayerInfoPacket)new PlayerInfoPacket().DeSerialize(receivedBuffer);
                                    Player tempPlayer = new Player(piPack.playerName, piPack.playerID);

                                    if (piPack.playerID == clients[i].Player.ID)
                                    {
                                        Client tempClient = new Client(clients[i].Socket, tempPlayer);
                                        clients[i] = tempClient;
                                        Console.WriteLine($"{clients[i].Player.Name} with ID {clients[i].Player.ID} has been added");
                                    }

                                    break;

                                default:
                                    break;
                            }

                            //Switch case looking for PlayerInfo packet. Upon receiving, create new Player obj using that info.
                            //Create temp playerobj, give it the socket of the current player and then replace the old player obj with the new one.
                            //Check that both players have names and if they do, send a spawnpoint packet to each player.
                            //Spawnpoint Packet contains playerID and spawnPos. 


                            //clients[i].Send(receivedBuffer);

                            for (int j = clients.Count - 1; j >= 0; j--)
                            {
                                if (i == j)
                                    continue;

                                //Console.WriteLine(receivedBuffer);

                                clients[j].Socket.Send(receivedBuffer);
                                TestPacket testPacket = (TestPacket)new TestPacket().DeSerialize(receivedBuffer);
                                Console.WriteLine($"{pb.Type} packet of {pb.objID} conaining {testPacket.objPos}, has been sent to the others");
                            }
                        }
                    }
                    catch (SocketException ex)
                    {

                    }
                }

                if (!playerCredentialsGrabbed)
                {
                    bool playersDefined = true;

                    for (int i = 0; i < clients.Count; i++)
                    {
                        if (clients[i].Player.Name == "Undefined")
                        {
                            playersDefined = false;
                            break;
                        }

                    }

                    if (playersDefined)
                    {
                        for (int i = 0; i < clients.Count; i++)
                        {
                            clients[0].Socket.Send(new SpawnPosPacket(clients[i].Player.ID, new Vector3(0 + i, 2, 0 + i)).Serialize());
                            clients[1].Socket.Send(new SpawnPosPacket(clients[i].Player.ID, new Vector3(0 + i * 2, 2, 0 + i * 2)).Serialize());
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
