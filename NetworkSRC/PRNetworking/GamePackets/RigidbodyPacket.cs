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

        public float mass;

        public bool gravityActive;
        public bool isKinematic;

        public RigidbodyPacket()
        {
            velocity = Vector3.zero;
            mass = 1f;
            gravityActive = true;
            isKinematic = false;
        }

        public RigidbodyPacket(Rigidbody rigidbody, string objID) :
            base(PacketType.Rigidbody, objID)
        {
            mass = rigidbody.mass;
            gravityActive = rigidbody.useGravity;
            isKinematic = rigidbody.isKinematic;

            velocity = rigidbody.velocity;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(mass);
            bw.Write(gravityActive);
            bw.Write(isKinematic);
            bw.Write(velocity.x);
            bw.Write(velocity.y);
            bw.Write(velocity.z);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            mass = br.ReadInt32();
            gravityActive = br.ReadBoolean();
            isKinematic = br.ReadBoolean();
            velocity = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            return this;
        }
    }
}
