using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;


//Author : KEG
//Datum  : 25.09.2015 10:47:25
//Datei  : BasePacket.cs


namespace GCore.Networking.RPC.Protokoll
{
    [Serializable]
    public class BasePacket
    {
        private int NextID = 0;

        #region Members
        public int ID { get; private set; }
        #endregion

        #region Events
        #endregion

        #region Initialization
        public BasePacket()
        {
            this.ID = NextID++;
        }
        #endregion

        #region Finalization
        ~BasePacket()
        {

        }
        #endregion

        #region Interface

        public byte[] Serialize()
        {
            return BasePacket.Serialize(this);
        }

        public static byte[] Serialize(BasePacket packet)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, packet);
                return memoryStream.ToArray();
            }
        }

        public static BasePacket Deserialize(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return (BasePacket)binaryFormatter.Deserialize(memoryStream);
            }
        }
        #endregion

        #region Tools
        #endregion

        #region Browsable Properties
        #endregion

        #region NonBrowsable Properties
        #endregion
    }
}