using System;
using HarmonyLib;
using UdonSharp;
using UnityEngine;
using VRC.Collections;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;


namespace Hroi.UdonDataDownload
{
    public class Message1
    {
        public static DataToken Parse(byte[] bytes, ref int i, int end)
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
                    case 1:
                        key = "numbers";
                        allowMultiple = true;
                        if (wireType != 0)
                        {
                            Debug.LogError($"Field \"{key}\" on message \"Message1\" expected to be wire type 0, was wire type {wireType}");
                            return DataError.TypeMismatch;
                        }
                        var num = ProtobufUtil.DecodeVarInt(bytes, ref i);
                        token = new DataToken(num);
                        break;
                    case 2:
                        key = "name";
                        allowMultiple = false;
                        var length = (int)ProtobufUtil.DecodeVarInt(bytes, ref i);
                        token = new DataToken(UTF8.Decode(bytes, i, i + length));
                        i += length;
                        break;
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
                numbersArr[j] = numbersDataList[j].ULong;
            obj["numbers"] = new DataToken(numbersArr);

            return obj;
        }

        public static ulong[] getNumbers(DataToken obj) =>
            (ulong[])ProtobufUtil.GetObjList(obj, "Message1", "numbers", "getNumbers");

        public static string getName(DataToken obj) =>
            ProtobufUtil.GetObjSingle(obj, "Message1", "name", "getName").String;
    }
}
