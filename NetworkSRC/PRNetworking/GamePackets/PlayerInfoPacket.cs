using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackets
{
    public class PlayerInfoPacket : GameBasePacket
    {
        public string playerName;

        public Guid playerID;
        public int playerIntType;

        public PlayerInfoPacket()
        {
        }

        public PlayerInfoPacket(Guid playerID, string name) :
            base(PacketType.PlayerInfo, name)
        {
            playerName = name;
            this.playerID = playerID;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(playerName);
            bw.Write(playerID.ToString());

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            playerName = br.ReadString();
            playerID = new Guid(br.ReadString());

            return this;
        }
    }
}
