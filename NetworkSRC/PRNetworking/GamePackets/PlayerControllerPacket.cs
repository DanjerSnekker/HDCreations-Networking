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
        public Vector3 position;
        public Vector3 movement;
        public Vector3 velocity;

        public string ownershipID;

        public PlayerControllerPacket()
        {
            position = Vector3.zero;
            movement = Vector3.zero;
            velocity = Vector3.zero;

            ownershipID = "";
        }

        public PlayerControllerPacket(Vector3 pcPos, string ownerID, string objID) :
            base(PacketType.PlayerController, objID)
        {
            position = pcPos;
            movement = Vector3.zero;
            velocity = Vector3.zero;

            ownershipID = ownerID;
        }

        public PlayerControllerPacket(Vector3 move, Vector3 jumpVelocity, string ownerID, string objID) :
            base(PacketType.PlayerController, objID)
        {
            position = Vector3.zero;
            movement = move;
            velocity = jumpVelocity;

            ownershipID = ownerID;
        }

        public override byte[] Serialize()
        {
            base.Serialize();

            bw.Write(position.x);
            bw.Write(position.y);
            bw.Write(position.z);

            bw.Write(movement.x);
            bw.Write(movement.y);
            bw.Write(movement.z);

            bw.Write(velocity.x);
            bw.Write(velocity.y);
            bw.Write(velocity.z);

            bw.Write(ownershipID);

            return ms.ToArray();
        }

        public override GameBasePacket DeSerialize(byte[] buffer)
        {
            base.DeSerialize(buffer);

            position = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            movement = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            velocity = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());

            ownershipID = br.ReadString();

            return this;
        }
    }
}
