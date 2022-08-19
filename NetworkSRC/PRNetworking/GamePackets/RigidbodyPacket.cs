using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class RigidbodyPacket : GameBasePacket
    {
        public Vector3 velocity;

        public float Mass;

        public bool GravityActive;
        public bool isKinematic;

        public RigidbodyPacket()
        {
            Mass = 1f; 
            GravityActive = true;
            isKinematic = false;
        }

        public RigidbodyPacket(float mass, bool gravity, bool kinematic, string objID) :
            base(PacketType.Rigidbody, objID)
        {
            Mass = mass;
            GravityActive = gravity;
            isKinematic = kinematic;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(Mass);
            bw.Write(GravityActive);
            bw.Write(isKinematic);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            Mass = br.ReadInt32();
            GravityActive = br.ReadBoolean();
            isKinematic = br.ReadBoolean();
            
            return this;
        }
    }
}
