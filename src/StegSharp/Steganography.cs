using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace StegSharp
{
    public class Steganography
    {
        public Bitmap Encode(Bitmap source, byte[] data)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            int bitPos = 0;
            var bitArray = new BitArray(encodeDataWithLength(data));

            for (int x = 0; x < source.Width; x++)
            {
                for (int y = 0; y < source.Height; y++)
                {
                    var sourcePixel = source.GetPixel(x, y);
                    byte a = sourcePixel.A;
                    byte r = sourcePixel.R;
                    byte g = sourcePixel.G;
                    byte b = sourcePixel.B;
                    if (bitPos < bitArray.Length)
                    {
                        a = encryptByte(a, bitArray.Get(bitPos++));
                        r = encryptByte(r, bitArray.Get(bitPos++));
                        g = encryptByte(g, bitArray.Get(bitPos++));
                        b = encryptByte(b, bitArray.Get(bitPos++));
                    }
                    result.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }

            return result;
        }

        private byte encryptByte(byte b, bool bit)
        {
            return setLSB(b, bit ^ getLSB(b));
        }
        private bool decryptByte(byte encrypted, byte original)
        {
            return getLSB(encrypted) ^ getLSB(original);
        }

        private byte[] encodeDataWithLength(byte[] data)
        {
            byte[] length = BitConverter.GetBytes(data.Length);
            byte[] result = new byte[data.Length + length.Length];
            int i;
            for (i = 0; i < 4; i++)
                result[i] = length[i];
            for (; i < result.Length; i++)
                result[i] = data[i - 4];
            return result;
        }

        public byte[] Decode(Bitmap encrypted, Bitmap original)
        {
            List<byte> result = new List<byte>();
            BitArray bits = new BitArray(8);
            int bitCounter = 0;

            for (int x = 0; x < encrypted.Width; x++)
            {
                for (int y = 0; y < encrypted.Height; y++)
                {
                    Color encryptedPixel = encrypted.GetPixel(x, y);
                    Color originalPixel = original.GetPixel(x, y);
                    if (bitCounter == 8)
                    {
                        bitCounter = 0;
                        byte[] arr = new byte[1];
                        bits.CopyTo(arr, 0);
                        result.Add(arr[0]);
                    }
                    bits.Set(bitCounter++, decryptByte(encryptedPixel.A, originalPixel.A));
                    bits.Set(bitCounter++, decryptByte(encryptedPixel.R, originalPixel.R));
                    bits.Set(bitCounter++, decryptByte(encryptedPixel.G, originalPixel.G));
                    bits.Set(bitCounter++, decryptByte(encryptedPixel.B, originalPixel.B));
                }
            }
            byte[] lengthB = new byte[4];
            for (int i = 0; i < 4; i++)
                lengthB[i] = result[i];
            int length = BitConverter.ToInt32(lengthB, 0);
            return result.GetRange(4, length).ToArray();
        }

        private byte setLSB(byte b, bool val)
        {
            return val ? (byte)(b | 1) : (byte)(b & 254);
        }

        private bool getLSB(byte b)
        {
            return (b & 1) == 1;
        }
    }
}