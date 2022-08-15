using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePackets
{
    public class PlayerInfoPacket : GameBasePacket
    {
        public string clientDesignation;

        public PlayerInfoPacket()
        {
            clientDesignation = "";
        }

        //This packet should only be used once, when assigning clients. In every other packet except this one and Instantiation, objID refers to the object name.
        public PlayerInfoPacket(string objId) :
            base(PacketType.PlayerInfo, objId)
        {
            clientDesignation = objID;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(clientDesignation);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            clientDesignation = br.ReadString();

            return this;
        }
    }
}
