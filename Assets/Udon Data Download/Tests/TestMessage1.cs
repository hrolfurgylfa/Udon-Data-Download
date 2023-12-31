// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Hroi.UdonDataDownload.Generated;
using NUnit.Framework.Internal;

namespace Tests
{
    public class TestMessage1
    {
        [Test]
        public void TestDecodeSimple()
        {
            int i = 0;
            var bytes = Protoscope.Run(@"
                1: 16384
                2: {""Sæl Veröld!""}
            ");
            var obj = Message1.Parse(bytes, i, bytes.Length);
            var numbers = Message1.getNumbers(obj);
            Assert.AreEqual(16384, numbers[0]);
            Assert.AreEqual(1, numbers.Length);
            Assert.AreEqual("Sæl Veröld!", Message1.getName(obj));
        }

        [Test]
        public void TestDecodeMultiple()
        {
            int i = 0;
            var bytes = Protoscope.Run(@"
                1: 2
                2: {""I was never here!""}
                1: 16384
                1: 16383
                1: 1
                2: {""Sæl Veröld!""}
                1: 23452151252155231
            ");
            var obj = Message1.Parse(bytes, i, bytes.Length);
            Assert.AreEqual(new ulong[] { 2, 16384, 16383, 1, 23452151252155231 }, Message1.getNumbers(obj));
            Assert.AreEqual("Sæl Veröld!", Message1.getName(obj));
        }
    }
}