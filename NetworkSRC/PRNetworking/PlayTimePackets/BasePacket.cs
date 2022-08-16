using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayTimePackets
{
    public class BasePacket
    {
        protected MemoryStream ms;
        protected BinaryWriter bw;

        protected MemoryStream msr;
        protected BinaryReader br;

        public enum PacketType
        {
            Unknown = -1,
            None,

            Message,
            Lobby,
            CreateLobby,
            DisplayLobby,
            CreatePlayer,
            LobbyName,
            JoinRequest,
            KickRequest,
            StartGame,
            LeaveLobby,
            PlayerShutDown,

            Rigidbody,
            Instantiate,
            PlayerController,
            Trigger
        }
        public PacketType Type { get; private set; }

        public Player player { get; private set; }

        public BasePacket()
        {
            Type = PacketType.Unknown;
            player = null;
        }

        public BasePacket(PacketType type, Player player)
        {
            this.Type = type;
            player = null;
        }

        public virtual byte[] Serialize()
        {
            ms = new MemoryStream();
            bw = new BinaryWriter(ms);

            bw.Write((int)Type);

            return null;
        }

        public virtual BasePacket DeSerialize(byte[] buffer)
        {
            msr = new MemoryStream(buffer);
            br = new BinaryReader(msr);

            Type = (PacketType)br.ReadInt32();

            return this;
        }
    }
}
