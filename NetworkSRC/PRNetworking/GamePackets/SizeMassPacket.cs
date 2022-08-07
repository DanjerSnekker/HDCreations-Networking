using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class SizeMassPacket : GameBasePacket
    {
        public Vector3 Scale;
        public float Mass;

        public SizeMassPacket()
        {
            Scale = Vector3.zero;
        }

        public SizeMassPacket(Vector3 scale,float mass, string objID) :
            base(PacketType.SizeMass, objID)
        {
            this.Scale = scale;
            this.Mass = mass;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(Scale.x);
            bw.Write(Scale.y);
            bw.Write(Scale.z);
            bw.Write(Mass);
            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            Scale = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Mass = br.ReadSingle();
            return this;
        }
    }
}

