using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GamePackets
{
    public class PlayerControllerPacket : GameBasePacket
    {
        public int ownerID { get; set; }

        public bool isGrounded;
        public Vector3 velocity;

        public PlayerControllerPacket()
        {
            isGrounded = true;
            velocity = Vector3.zero;
        }

        public PlayerControllerPacket(string objID, int ownershipID, bool grounded, Vector3 moveVelocity) :
            base(PacketType.PlayerController, objID)
        {
            ownerID = ownershipID;
            isGrounded = grounded;
            velocity = moveVelocity;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(ownerID);
            bw.Write(isGrounded);
            bw.Write(velocity.x);
            bw.Write(velocity.y);
            bw.Write(velocity.z);
            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            ownerID = br.ReadInt32();
            isGrounded = br.ReadBoolean();
            velocity = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            return this;
        }
    }
}
