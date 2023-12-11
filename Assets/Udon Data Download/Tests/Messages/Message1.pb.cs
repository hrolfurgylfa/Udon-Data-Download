// SPDX-License-Identifier: MIT
// Code generated by UdonDataDownload. DO NOT EDIT.

using UnityEngine;
using VRC.SDK3.Data;

namespace Hroi.UdonDataDownload.Generated
{
    public class Message1
    {
        public static DataToken Parse(byte[] bytes, int i, int end)
        {
            var obj = new DataDictionary();
            obj["__type"] = "Message1";

            // Create a DataList for all multi fields.
            obj["numbers"] = new DataList();

            while (i < end)
            {
                var fieldPrefix = ProtobufUtil.DecodeVarInt(bytes, ref i);
                var wireType = (byte)(fieldPrefix & 0x7);
                var fieldNumber = fieldPrefix >> 3;

                // Parse the field
                DataToken token = new DataToken();
                bool allowMultiple;
                string key;
                switch (fieldNumber)
                {
                    case 1: {
                        key = "numbers";
                        allowMultiple = true;
                        if (wireType != 0)
                        {
                            Debug.LogError($"Field \"numbers\" on message \"Message1\" expected to be wire type 0, was wire type {wireType}");
                            return DataError.TypeMismatch;
                        }
                        
                        var num = ProtobufUtil.DecodeVarInt(bytes, ref i);
                        token = new DataToken(num);
                        
                        break;
                    }
                    case 2: {
                        key = "name";
                        allowMultiple = false;
                        if (wireType != 2)
                        {
                            Debug.LogError($"Field \"name\" on message \"Message1\" expected to be wire type 2, was wire type {wireType}");
                            return DataError.TypeMismatch;
                        }
                        var length = (int)ProtobufUtil.DecodeVarInt(bytes, ref i);
                        token = new DataToken(UTF8.Decode(bytes, i, i + length));
                        i += length;
                        break;
                    }
                    default:
                        Debug.LogError($"Unsupported field number {fieldNumber} on \"Message1\".");
                        return DataError.TypeMismatch;
                }

                // Add the field to the dict
                // TODO: Add support for packed repeated: https://protobuf.dev/programming-guides/encoding/#packed
                // TODO: Merge embedded message fields together instead of keeping the last one: https://protobuf.dev/programming-guides/encoding/#last-one-wins
                if (allowMultiple)
                    obj[key].DataList.Add(token);
                else
                    obj[key] = token;
            }

            // Copy the data from all mutli fields into regular lists.
            var numbersDataList = obj["numbers"].DataList;
            var numbersArr = new ulong[numbersDataList.Count];
            for (int j = 0; j < numbersArr.Length; j++)
                numbersArr[j] = (ulong)numbersDataList[j].ULong;
            obj["numbers"] = new DataToken(numbersArr);

            return obj;
        }

        public static ulong[] getNumbers(DataToken obj) =>
            (ulong[])ProtobufUtil.GetObj(obj, "Message1", "numbers", "getNumbers").Reference;

        public static string getName(DataToken obj) =>
            (string)ProtobufUtil.GetObj(obj, "Message1", "name", "getName").String;
    }
}