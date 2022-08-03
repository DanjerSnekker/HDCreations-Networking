using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using PlayTimePackets;

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

            List<Socket> clients = new List<Socket>();

            Player serverPlayer = new Player("1");
            string message = "Welcome my friend";

            while (true)
            {
                try
                {
                    clients.Add(listeningSocket.Accept());
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
                        if (clients[i].Available > 0)
                        {
                            byte[] receivedBuffer = new byte[clients[i].Available];
                            clients[i].Receive(receivedBuffer);

                            BasePacket pb = new BasePacket().DeSerialize(receivedBuffer);

                            //clients[i].Send(receivedBuffer);

                            for (int j = clients.Count - 1; j >= 0; j--)
                            {
                                if (i == j)
                                    continue;

                                //Console.WriteLine(receivedBuffer);
                                Console.WriteLine($"{pb.Type} packet by {pb.player.Name}, has been sent to the others");
                                clients[j].Send(receivedBuffer);
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
