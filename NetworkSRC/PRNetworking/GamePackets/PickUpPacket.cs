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
        public Vector3 Position;
        public Vector3 Rotation;

        public PickUpPacket()
        {
            Holding = true;
            Position = Vector3.zero;
            Rotation = Vector3.zero;
        }

        public PickUpPacket(bool holding,Vector3 position, Vector3 rotation, string objID) :
            base(PacketType.PickUp, objID)
        {
            this.Holding = holding;
            this.Position = position;
            this.Rotation = rotation;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(Holding);
            
            bw.Write(Position.x);
            bw.Write(Position.y);
            bw.Write(Position.z);

            bw.Write(Rotation.x);
            bw.Write(Rotation.y);
            bw.Write(Rotation.z);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            Holding = br.ReadBoolean();
            
            Position = new Vector3(br.ReadSingle(), br.ReadSingle(),br.ReadSingle());
            Rotation = new Vector3(br.ReadSingle(), br.ReadSingle(),br.ReadSingle());

            return this;
        }
    }
}
