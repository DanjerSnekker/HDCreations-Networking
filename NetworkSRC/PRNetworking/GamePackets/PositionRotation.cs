using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class PositionRotation : GameBasePacket
    {
        public Vector3 Position;
        public Vector3 Rotation;

        public PositionRotation()
        {
            Position = Vector3.zero;
            Rotation = Vector3.zero;
        }

        public PositionRotation(Vector3 position, Vector3 rotation, string objID) :
            base(PacketType.PositionRotation, objID)
        { 
            this.Position = position;
            this.Rotation = rotation;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

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

            Position = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Rotation = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            return this;
        }
    }
}
