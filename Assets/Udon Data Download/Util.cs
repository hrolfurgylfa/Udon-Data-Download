// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using UdonSharp;

namespace Hroi.UdonDataDownload
{
    public class Util : UdonSharpBehaviour
    {
        public static bool ByteArraysEqual(byte[] arr1, byte[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            for (int i = 0; i < arr1.Length; i++)
                if (arr1[i] != arr2[i])
                    return false;

            return true;
        }

        public static string ByteArrayToString(byte[] bytes)
        {
            string result = "";
            foreach (byte b in bytes)
                result += b.ToString("X2");
            return result;
        }

        public static int RoundUp(int numToRound, int multiple)
        {
            if (multiple == 0)
                return numToRound;

            int remainder = numToRound % multiple;
            if (remainder == 0)
                return numToRound;

            return numToRound + multiple - remainder;
        }

        public static string EncodeASCII(byte[] bytes)
        {
            var charArr = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
                charArr[i] = (char)bytes[i];
            return new string(charArr);
        }

        public static byte[] DecodeASCII(string str)
        {
            var bytes = new byte[str.Length];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)str[i];
            return bytes;
        }
    }
}