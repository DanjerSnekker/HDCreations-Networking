using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class LobbyInformationPacket : BasePacket
    {
        public Player Player;

        public string Name;
        public int RoomCode;
        public int LobbyPort;
        public Guid hostID;

        public LobbyInformationPacket()
        {
            Name = "";
            RoomCode = 0;
            LobbyPort = 0;
            hostID = Guid.Empty;
        }

        public LobbyInformationPacket(string name, int roomCode, int lobbyPort, Player player, Guid id) :
            base(PacketType.Lobby, player)
        {
            Name = name;
            RoomCode = roomCode;
            LobbyPort = lobbyPort;
            hostID = id;
        }
        public override byte[] Serialize()
        {
            base.Serialize();
            bw.Write(Name);
            bw.Write(RoomCode);
            bw.Write(LobbyPort);
            bw.Write(hostID.ToString());
            //bw.Write(player.ID.ToString());
            return ms.ToArray();
        }
        public override BasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            Name = br.ReadString();
            RoomCode = br.ReadInt32();
            LobbyPort = br.ReadInt32();
            hostID = new Guid(br.ReadString());

            //hostID = new Guid(br.ReadString());

            return this;
        }
    }
}

