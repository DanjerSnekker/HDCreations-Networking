using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class ClientReadyPacket : GameBasePacket
    {
        public bool isReady { get; set; }
        
        public ClientReadyPacket()
        {
            isReady = false;
        }

        public ClientReadyPacket(bool clientStatus, string objID) :
            base(PacketType.ClientReady, objID)
        {
            isReady = clientStatus;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(isReady);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            isReady = br.ReadBoolean();

            return this;
        }
    }
}
