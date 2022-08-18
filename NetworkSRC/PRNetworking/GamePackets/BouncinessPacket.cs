using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class BouncinessPacket : GameBasePacket
    {
        public float Bounciness;

        public BouncinessPacket()
        {
            Bounciness = 0;
        }

        public BouncinessPacket(float bounciness, string objID) :
            base(PacketType.Bounciness, objID)
        {
            Bounciness = bounciness;;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(Bounciness);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            Bounciness = br.ReadSingle();

            return this;
        }
    }
}
