// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Hroi.UdonDataDownload;

namespace Tests
{
    public class TestUTF8
    {
        public struct TestValue
        {
            public string str;
            public byte[] bytes;

            public TestValue(string str, byte[] bytes)
            {
                this.str = str;
                this.bytes = bytes;
            }
        }

        static readonly TestValue[] testValues = new TestValue[] {
            new TestValue("Hello World!", new byte[] {0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x21}),// ASCII regular
            new TestValue("(-){|}~", new byte[] {0x28, 0x2D, 0x29, 0x7B, 0x7C, 0x7D, 0x7E}), // ASCII symbols
            new TestValue("€ƒÄ×ÿ¾", new byte[] {0xE2, 0x82, 0xAC, 0xC6, 0x92, 0xC3, 0x84, 0xC3, 0x97, 0xC3, 0xBF, 0xC2, 0xBE}), // ASCII 128-255 (2 byte UTF-8)
            new TestValue("¡Æ߿ظ", new byte[] {0xC2, 0xA1, 0xC3, 0x86, 0xDF, 0xBF, 0xD8, 0xB8}), // Other 2 byte UTF-8
            new TestValue("돧࢐�", new byte[] {0xEB, 0x8F, 0xA7, 0xE0, 0xA2, 0x90, 0xEF, 0xBF, 0xBD}), // 3 byte UTF-8
            new TestValue("𒀀🧾", new byte[] {0xF0, 0x92, 0x80, 0x80, 0xF0, 0x9F, 0xA7, 0xBE}), // 4 byte UTF-8
            new TestValue("𝅘𝅥𝅱🂹", new byte[] {0xF0, 0x9D, 0x85, 0xA3, 0xF0, 0x9F, 0x82, 0xB9}), // More 4 byte UTF-8
        };

        [Test]
        public void TestValues([ValueSource("testValues")] TestValue testValue)
        {
            var str = UTF8.Decode(testValue.bytes, 0, testValue.bytes.Length);
            Assert.AreEqual(testValue.str, str);
        }

        [Test]
        public void TestRangeSelector()
        {
            var bytes = new byte[] { 0x20, 0x20, 0xF0, 0x9D, 0x85, 0xA3, 0xF0, 0x9F, 0x82, 0xB9, 0x20, 0x20, 0x20 };
            var str = UTF8.Decode(bytes, 2, bytes.Length - 3);
            Assert.AreEqual("𝅘𝅥𝅱🂹", str);
        }
    }
}
