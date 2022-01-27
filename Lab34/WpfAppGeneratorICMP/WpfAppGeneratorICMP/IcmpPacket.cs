using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppGeneratorICMP
{
    class IcmpPacket
    {
        public UInt16 Type { get; set; } 
        public UInt16 Code { get; set; }
        public UInt16 CheckSum { get; set; } 
        public int Size { get; set; } 
        public byte[] Message { get; set; } = new byte[1024]; 

        public IcmpPacket() { }


        /// <summary>
        /// Конструктор, принимающий пришедшее сообщение
        /// </summary>
        public IcmpPacket(byte[] data, int size)
        {
            Type = data[20];
            Code = data[21]; 
            CheckSum = BitConverter.ToUInt16(data, 22); 
            Size = size - 24; 
            Buffer.BlockCopy(data, 24, Message, 0, Size);
        }


        /// <summary>
        /// Помещаем все данные в байтовый массив
        /// </summary>
        public byte[] getData()
        {
            byte[] icmpData = new byte[Size + 8];
            // Тип [0-7 бит]
            Buffer.BlockCopy(BitConverter.GetBytes(Type), 0, icmpData, 0, 2);
            // Код [8-15 бит]
            Buffer.BlockCopy(BitConverter.GetBytes(Code), 0, icmpData, 1, 2);
            // Контрольная сумма [16-31 бит]
            Buffer.BlockCopy(BitConverter.GetBytes(CheckSum), 0, icmpData, 2, 2);
            // Сообщение [32-size бит]
            Buffer.BlockCopy(Message, 0, icmpData, 4, Size);
            return icmpData;
        }

        /// <summary>
        /// вычисление контрольной суммы
        /// </summary>
        public UInt16 getChecksum()
        {
            UInt32 crc = 0;
            byte[] buffer = getData();
            int sizePacket = buffer.Count();
            int index = 0;

            while (index < sizePacket-1)
            {
                crc += Convert.ToUInt32(BitConverter.ToUInt16(buffer, index));
                index += sizeof(short);
            }
            byte[] bhelper = new byte[2];
            bhelper[0]= buffer[sizePacket - 1];
            crc += Convert.ToUInt32(BitConverter.ToUInt16(bhelper, 0));
            crc = (crc >> 16) + (crc & 0xffff);
            crc += (crc >> 16);

            return (UInt16)(~crc);
        }

    }
}
