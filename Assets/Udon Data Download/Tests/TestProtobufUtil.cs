// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Hroi.UdonDataDownload;
using NUnit.Framework.Internal;

namespace Tests
{
    public class TestProtobufUtil
    {
        public struct TestValue
        {
            public ulong num;
            public byte[] bytes;

            public TestValue(ulong num, byte[] bytes)
            {
                this.num = num;
                this.bytes = bytes;
            }
        }

        static readonly TestValue[] testValues = new TestValue[] {
            new TestValue(0, new byte[] {0x80, 0x80, 0x80, 0x80, 0x00}),
            new TestValue(0, new byte[] {0x00}),
            new TestValue(42, new byte[] {0x2A}),
            new TestValue(150, new byte[] {0x96, 0x01}),
            new TestValue(16383, new byte[] {0xFF, 0x7F}),
            new TestValue(16384, new byte[] {0x80, 0x80, 0x01}),

            new TestValue(1346754332465, new byte[] {0xB1, 0x8E, 0xC1, 0x86, 0x99, 0x27}),
        };

        [Test]
        public void TestDecodeVarIntSuccess([ValueSource("testValues")] TestValue testValue)
        {
            int i = 0;
            var res = ProtobufUtil.DecodeVarInt(testValue.bytes, ref i);
            Assert.AreEqual(testValue.num, res);
            Assert.AreEqual(testValue.bytes.Length, i);
        }
    }
}
