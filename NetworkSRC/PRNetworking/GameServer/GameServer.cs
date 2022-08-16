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
            List<bool> clientConnectionStatus = new List<bool>();

            //Player serverPlayer = new Player("1");

            while (true)
            {
                try
                {
                    clients.Add(new Client(listeningSocket.Accept()));
                    clientConnectionStatus.Add(false);
                    Console.WriteLine($"Ah they have arrived.");
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
                                case GameBasePacket.PacketType.ClientReady:
                                    ClientReadyPacket crPack = (ClientReadyPacket)new ClientReadyPacket().DeSerialize(receivedBuffer);
                                    
                                    clientConnectionStatus[i] = crPack.isReady;
                                  //  Console.WriteLine($"Client{i}'s connection status: {clientConnectionStatus[i]}");

                                    if (clientConnectionStatus.Count >= 2)
                                    {
                                        if (clientConnectionStatus.All(x => x))
                                        {
                                            //Console.WriteLine("All clients have connected");

                                            for (int j = 0; j < clients.Count; j++)
                                            {
                                                clients[j].Socket.Send(new PlayerInfoPacket($"Player{j + 1}").Serialize());
                                              //  Console.WriteLine($"Sending Player Designation to Player {j + 1}");
                                            }
                                        }
                                    }
                                    break;
                                
                                case GameBasePacket.PacketType.Instantiate:
                                    InstantiateObjPacket iPack = (InstantiateObjPacket)new InstantiateObjPacket().DeSerialize(receivedBuffer);
                                    //Console.WriteLine($"{iPack.Type} from {iPack.objID} is being sent to the other client");
                                    if (iPack.objID.Equals("Player1"))
                                    {
                                        clients[1].Socket.Send(receivedBuffer);
                                    }
                                    else if (iPack.objID.Equals("Player2"))
                                    {
                                        clients[0].Socket.Send(receivedBuffer);
                                    }
                                    break;
                            }

                            for (int j = clients.Count - 1; j >= 0; j--)
                            {
                                if (i == j)
                                    continue;

                                //Console.WriteLine($"{pb.Type} packet by {pb.objID}, has been sent to the others");
                                clients[j].Socket.Send(receivedBuffer);

                                //Debug line for the Test Packet
                                /*TestPacket testPacket = (TestPacket)new TestPacket().DeSerialize(receivedBuffer);
                                Console.WriteLine($"{pb.Type} packet of {pb.objID} conaining {testPacket.objPos}, has been sent to the others");*/
                            }
                        }
                    }
                    catch (SocketException ex)
                    {
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
