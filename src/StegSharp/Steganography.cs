using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace StegSharp
{
    public class Steganography
    {
        public byte[] DecryptBitmap(Bitmap image, Bitmap original)
        {
            byte[] length = new byte[4];
            int lengthPos = 0;

            BitArray buffer = new BitArray(8);
            int bitPos = 0;

            for (int j = 0; j < 8; j++)
            {
                var imagePixel = image.GetPixel(0, j);
                var originalPixel = original.GetPixel(0, j);

                buffer[bitPos++] = decryptByte(imagePixel.A, originalPixel.A);
                buffer[bitPos++] = decryptByte(imagePixel.R, originalPixel.R);
                buffer[bitPos++] = decryptByte(imagePixel.G, originalPixel.G);
                buffer[bitPos++] = decryptByte(imagePixel.B, originalPixel.B);

                if (bitPos >= 8)
                {
                    bitPos = 0;
                    buffer.CopyTo(length, lengthPos++);
                }
            }

            return decryptBitmap(image, original, BitConverter.ToInt32(length, 0), 0, 8);
        }

        private byte[] decryptBitmap(Bitmap image, Bitmap original, int byteCount, int x, int y)
        {
            byte[] result = new byte[byteCount];
            int bytePos = 0;

            BitArray buffer = new BitArray(8);
            int bitPos = 0;

            for (int i = x; i < image.Width && bytePos < byteCount; i++)
            {
                for (int j = i == x ? y : 0; j < image.Height && bytePos < byteCount; j++)
                {
                    var imagePixel = image.GetPixel(i, j);
                    var originalPixel = original.GetPixel(i, j);

                    buffer[bitPos++] = decryptByte(imagePixel.A, originalPixel.A);
                    buffer[bitPos++] = decryptByte(imagePixel.R, originalPixel.R);
                    buffer[bitPos++] = decryptByte(imagePixel.G, originalPixel.G);
                    buffer[bitPos++] = decryptByte(imagePixel.B, originalPixel.B);

                    if (bitPos >= 8)
                    {
                        bitPos = 0;
                        buffer.CopyTo(result, bytePos++);
                    }
                }
            }
            return result;
        }

        public Bitmap EncryptBitmap(Bitmap image, byte[] data)
        {
            byte[] dataWithLength = new byte[data.Length + 4];
            BitConverter.GetBytes(data.Length).CopyTo(dataWithLength, 0);
            data.CopyTo(dataWithLength, 4);

            return encryptBitmap(image, dataWithLength);
        }

        private Bitmap encryptBitmap(Bitmap image, byte[] data)
        {
            BitArray bits = new BitArray(data);
            int bitPos = 0;

            Bitmap result = new Bitmap(image.Width, image.Height);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    var pixel = image.GetPixel(i, j);

                    var a = encryptRandomByte(pixel.A);
                    var r = encryptRandomByte(pixel.R);
                    var g = encryptRandomByte(pixel.G);
                    var b = encryptRandomByte(pixel.B);

                    if (bitPos < bits.Length)
                    {
                        a = encryptByte(pixel.A, bits.Get(bitPos++));
                        r = encryptByte(pixel.R, bits.Get(bitPos++));
                        g = encryptByte(pixel.G, bits.Get(bitPos++));
                        b = encryptByte(pixel.B, bits.Get(bitPos++));
                    }
                    result.SetPixel(i, j, Color.FromArgb(a, r, g, b));
                }
            }

            return result;
        }

        private byte setLsb(byte b, bool val)
        {
            return val ? (byte)(b | 1) : (byte)(b & 254);
        }

        private bool getLsb(byte b)
        {
            return (b & 1) == 1;
        }

        private byte encryptByte(byte b, bool k)
        {
            return setLsb(b, k ^ getLsb(b));
        }
        private bool decryptByte(byte b, byte k)
        {
            return getLsb(b) ^ getLsb(k);
        }

        private static Random rnd = new Random();
        private byte encryptRandomByte(byte b)
        {
            return setLsb(b, rnd.Next(0, 2) == 1 ? true : false);
        }
    }
}

