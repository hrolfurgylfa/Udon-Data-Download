// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace Hroi.UdonDataDownload
{
    public class UTF8 : UdonSharpBehaviour
    {
        public static string Decode(byte[] bytes, int start, int end)
        {
            // This is probably too long, but it is always at least big enough
            var charArr = new char[bytes.Length];
            var charArrLen = 0;

            // Source: https://stackoverflow.com/questions/73758747/looking-for-the-description-of-the-algorithm-to-convert-utf8-to-utf16
            for (int i = start; i < end;)
            {
                if ((bytes[i] & 0x80) == 0x0)
                {
                    // 1 byte
                    charArr[charArrLen] = (char)(bytes[i] & 0x7F);
                    i += 1;
                }
                else if ((bytes[i] & 0xE0) == 0xC0)
                {
                    // 2 bytes
                    charArr[charArrLen] =
                        (char)(((ushort)(bytes[i] & 0x1F) << 6)
                        | (ushort)(bytes[i + 1] & 0x3F));
                    i += 2;
                }
                else if ((bytes[i] & 0xF0) == 0xE0)
                {
                    // 3 bytes
                    charArr[charArrLen] =
                        (char)(((ushort)(bytes[i] & 0x0F) << 12)
                        | ((ushort)(bytes[i + 1] & 0x3F) << 6)
                        | (ushort)(bytes[i + 2] & 0x3F));
                    i += 3;
                }
                else if ((bytes[i] & 0xF8) == 0xF0)
                {
                    // 4 bytes
                    var CP = ((ushort)(bytes[i] & 0x07) << 18)
                         | ((ushort)(bytes[i + 1] & 0x3F) << 12)
                         | ((ushort)(bytes[i + 2] & 0x3F) << 6)
                         | (ushort)(bytes[i + 3] & 0x3F);
                    CP -= 0x10000;
                    var highSurrogate = (char)(0xD800 + (ushort)((CP >> 10) & 0x3FF));
                    var lowSurrogate = (char)(0xDC00 + (ushort)(CP & 0x3FF));
                    charArr[charArrLen] = highSurrogate;
                    charArr[++charArrLen] = lowSurrogate;
                    i += 4;
                }
                else
                {
                    // error
                    Debug.LogError($"Unsupported byte found in UTF-8 string at position {i}: {bytes[i]:X2}!");
                    i += 1;
                }

                charArrLen++;
            }

            return new string(charArr, 0, charArrLen);
        }

        public void Test()
        {

        }
    }
}