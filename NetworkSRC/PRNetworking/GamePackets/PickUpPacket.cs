using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class PickUpPacket : GameBasePacket
    {
        public bool Holding;

        public PickUpPacket()
        {
            Holding = true;
        }

        public PickUpPacket(bool holding, string objID) :
            base(PacketType.PickUp, objID)
        {
            this.Holding = holding;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(Holding);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            Holding = br.ReadBoolean();

            return this;
        }
    }
}
