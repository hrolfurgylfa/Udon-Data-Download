// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

namespace Hroi.UdonDataDownload
{
    public class ProtobufUtil : UdonSharpBehaviour
    {
        public static ulong DecodeVarInt(byte[] bytes, ref int byteIndex)
        {
            ulong returnInt = 0;
            int shift = 0;
            while (true)
            {
                // Exit if we are getting stuck too long
                if (shift > 63) // 9 * 7 = 63
                {
                    Debug.LogError($"Stuck too long on varint decode!");
                    return 0;
                }

                // Add to the uint and flip from little to big endian at the same time
                var data = (ulong)bytes[byteIndex] & 0x7F;
                returnInt |= data << shift;

                // Exit if this byte didn't have the continuation mark
                if ((bytes[byteIndex] & 0x80) == 0) break;

                // Increment the counters
                shift += 7;
                byteIndex++;
            };

            // We didn't increment the index for the non-continuation byte, so do that here
            byteIndex++;

            return returnInt;
        }

        private static DataToken GetObjError(string err)
        {
            Debug.LogError(err);
            return new DataToken(DataError.TypeMismatch, err);
        }

        public static DataToken GetObj(DataToken obj, string msgName, string key, string methodName)
        {
            string errEnd = " Are you sure this object came from {msgName}.parse?";

            if (obj.TokenType != TokenType.DataDictionary)
                return GetObjError($"{methodName} called on object that isn't a DataDictionary." + errEnd);

            if (!obj.DataDictionary.ContainsKey("__type"))
                return GetObjError($"{methodName} called on object that doesn't have a __type." + errEnd);

            if (obj.DataDictionary["__type"] != msgName)
                return GetObjError($"{methodName} called on object of type {obj.DataDictionary["__type"]} instead of {msgName}." + errEnd);

            return obj.DataDictionary[key];
        }
    }
}