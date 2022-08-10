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

        //public string playerID;
        //public int playerIntType;

        public PlayerInfoPacket()
        {
        }

        public PlayerInfoPacket(string objId) :
            base(PacketType.PlayerInfo, objId)
        {
            playerName = objID;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(playerName);
            //bw.Write(playerID);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            playerName = br.ReadString();
            //playerID = br.ReadString();

            return this;
        }
    }
}
