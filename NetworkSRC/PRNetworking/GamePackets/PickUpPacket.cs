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
        public string objectOwnerID;

        public PickUpPacket()
        {
            Holding = true;
            objectOwnerID = "";

        }

        public PickUpPacket(bool holding, string objID) :
            base(PacketType.PickUp, objID)
        {
            this.Holding = holding;
            objectOwnerID = "";
        }

        public PickUpPacket(bool holding, string objOwnerId, string objID) :
            base(PacketType.PickUp, objID)
        {
            this.Holding = holding;
            objectOwnerID = objOwnerId;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(Holding);
            bw.Write(objectOwnerID);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            Holding = br.ReadBoolean();
            objectOwnerID = br.ReadString();

            return this;
        }
    }
}
