using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace GamePackets
{
    public class GameBasePacket
    {
        protected MemoryStream ms;
        protected BinaryWriter bw;

        protected MemoryStream msr;
        protected BinaryReader br;

        public enum PacketType
        {
            Unknown = -1,
            None,

            Rigidbody,
            Instantiate,
            PlayerController,
            Trigger
        }
        public PacketType Type { get; private set; }

        public int objID;

        public GameBasePacket()
        {
            Type = PacketType.Unknown;
            objID = 0;
        }

        public GameBasePacket(PacketType type, int objectID)
        {
            this.Type = type;
            objID = objectID;
        }

        public virtual byte[] Serialize()
        {
            ms = new MemoryStream();
            bw = new BinaryWriter(ms);

            bw.Write((int)Type);


            return null;
        }
        public virtual GameBasePacket DeSerialize(byte[] buffer)
        {
            msr = new MemoryStream(buffer);
            br = new BinaryReader(msr);

            Type = (PacketType)br.ReadInt32();

            return this;
        }
    }
}
