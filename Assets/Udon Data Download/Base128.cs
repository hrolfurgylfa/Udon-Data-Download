using System;
using UdonSharp;
using UnityEngine;

namespace Hroi.UdonDataDownload
{
    public class Base128 : UdonSharpBehaviour
    {
        // I know, this is really bad, but it's just here to test Decode, so I haven't optimized
        // this like the Decode function.
        public static byte[] Encode(byte[] uint8)
        {
            var numBits = uint8.Length * 8;
            var numBitsToAdd = numBits / 7;
            var bitArrLength = Util.RoundUp(numBits + numBitsToAdd, 8);

            var bitArray = new bool[bitArrLength];
            int bit = bitArrLength - 1;
            int insertCounter = 0;
            // Debug.Log(Util.ByteArrayToString(uint8));
            for (int i = uint8.Length - 1; i >= 0; i--)
            {
                bitArray[bit--] = Convert.ToBoolean(uint8[i] & 0b0000_0001);
                if (++insertCounter % 7 == 0) bitArray[bit--] = false;
                bitArray[bit--] = Convert.ToBoolean(uint8[i] & 0b0000_0010);
                if (++insertCounter % 7 == 0) bitArray[bit--] = false;
                bitArray[bit--] = Convert.ToBoolean(uint8[i] & 0b0000_0100);
                if (++insertCounter % 7 == 0) bitArray[bit--] = false;
                bitArray[bit--] = Convert.ToBoolean(uint8[i] & 0b0000_1000);
                if (++insertCounter % 7 == 0) bitArray[bit--] = false;
                bitArray[bit--] = Convert.ToBoolean(uint8[i] & 0b0001_0000);
                if (++insertCounter % 7 == 0) bitArray[bit--] = false;
                bitArray[bit--] = Convert.ToBoolean(uint8[i] & 0b0010_0000);
                if (++insertCounter % 7 == 0) bitArray[bit--] = false;
                bitArray[bit--] = Convert.ToBoolean(uint8[i] & 0b0100_0000);
                if (++insertCounter % 7 == 0) bitArray[bit--] = false;
                bitArray[bit--] = Convert.ToBoolean(uint8[i] & 0b1000_0000);
                if (++insertCounter % 7 == 0) bitArray[bit--] = false;
            }

            // var bitArrayStr = "";
            // for (int i = 0; i < bitArray.Length; i++)
            //     bitArrayStr += bitArray[i] ? "1" : "0";
            // Debug.Log(bitArrayStr);

            var bytes = new byte[bitArrLength / 8];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte)((Convert.ToInt32(bitArray[i * 8 + 0]) << 7)
                                | (Convert.ToInt32(bitArray[i * 8 + 1]) << 6)
                                | (Convert.ToInt32(bitArray[i * 8 + 2]) << 5)
                                | (Convert.ToInt32(bitArray[i * 8 + 3]) << 4)
                                | (Convert.ToInt32(bitArray[i * 8 + 4]) << 3)
                                | (Convert.ToInt32(bitArray[i * 8 + 5]) << 2)
                                | (Convert.ToInt32(bitArray[i * 8 + 6]) << 1)
                                | (Convert.ToInt32(bitArray[i * 8 + 7]) << 0));
            }
            return bytes;
        }

        public static byte[] Decode(byte[] uint7)
        {
            var uint7Str = Util.EncodeASCII(uint7);
            return DecodeStr(uint7Str);
        }

        public static byte[] DecodeStr(string uint7)
        {
            var uint8 = new byte[uint7.Length * 7 / 8];
            // Debug.Log($"uint7: {Util.ByteArrayToString(Util.DecodeASCII(uint7))}");
            // Debug.Log($"uint7.Length: {uint7.Length}");
            // Debug.Log($"uint8.Length: {uint8.Length}");

            int bitsLeft = 8;
            int uint8Pos = uint8.Length - 1;
            int i = uint7.Length - 1;
            while (true)
            {
                // Debug.Log($"State: i: {i}, uint8Pos: {uint8Pos}, bitsLeft: {bitsLeft}");

                // Different handling for the first case
                if (bitsLeft == 8)
                {
                    uint8[uint8Pos] = (byte)((uint7[i]) & 0x7F);
                    bitsLeft = 1;
                }
                else
                {
                    // Finish the current byte we're on in the 7 bit array
                    byte newInt7Mask = (byte)((~0 << bitsLeft) & 0x7F);
                    byte prevInt7Mask = (byte)(~newInt7Mask & 0x7F);
                    // Debug.Log($"& {prevInt7Mask:X2} << {8 - bitsLeft}");
                    // Debug.Log($"& {newInt7Mask:X2} >> {bitsLeft}");
                    uint8[uint8Pos--] |= (byte)((uint7[i] & prevInt7Mask) << (8 - bitsLeft));

                    // Use the rest of the data from the 7 bit array if it fits in the 8 bit array
                    if (uint8Pos < 0) break;
                    uint8[uint8Pos] = (byte)((uint7[i] & newInt7Mask) >> bitsLeft);
                    bitsLeft++;
                }

                i--;
            }

            return uint8;
        }

        private static void DebugEncodeDecode(byte[] bytes)
        {
            Debug.Log($"1:{Util.ByteArrayToString(bytes)}");
            Debug.Log($"bytes.Length: {bytes.Length}");
            var encoded = Encode(bytes);
            Debug.Log($"2:{Util.ByteArrayToString(encoded)}");
            Debug.Log($"bytes.Length: {encoded.Length}");
            var decoded = Decode(encoded);
            Debug.Log($"3:{Util.ByteArrayToString(decoded)}\n1:{Util.ByteArrayToString(bytes)}");
            Debug.Log($"bytes.Length: {decoded.Length}");
        }

        public static bool Test()
        {
            var byteArrays = new byte[][]{
                new byte[] { 0x00, 0x00, 0x00 },
                new byte[] { 0xFF, 0xFF, 0xFF },
                new byte[] { 0x0f, 0x01, 0xff, 0x1f, 0x01 },
                new byte[] { 0x0f, 0x01, 0xff, 0x1f, 0x01, 0xf3, 0x0f, 0x01, 0xff, 0x1f, 0x01 },
            };

            bool success = true;
            foreach (var byteArr in byteArrays)
            {
                if (!Util.ByteArraysEqual(Decode(Encode(byteArr)), byteArr))
                {
                    success = false;
                    Debug.LogError($"Test failed for byte array {Util.ByteArrayToString(byteArr)}.");
                    DebugEncodeDecode(byteArr);
                    break;
                }
            }

            if (success) Debug.Log("Tests Succeeded!");
            else Debug.LogError("Tests Failed! Scroll up for debug info.");
            return success;
        }
    }
}