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
    public class TestMessage2
    {
        [Test]
        public void TestDecodeSimple()
        {
            var bytes = Protoscope.Run(@"
                1: {
                    1: 15
                    2: {""Jón""}
                }
                1: {
                    1: 17
                    1: 19
                    1: 21
                    2: {""Kalli""}
                }
                2: {""/home/user""}
            ");
            var obj = Message2.Parse(bytes, 0, bytes.Length);
            var objs = Message2.getObjects(obj);
            Assert.AreEqual("Jón", User.getName(objs[0]));
            Assert.AreEqual("Kalli", User.getName(objs[1]));
            Assert.AreEqual(new ulong[] { 15 }, User.getNumbers(objs[0]));
            Assert.AreEqual(new ulong[] { 17, 19, 21 }, User.getNumbers(objs[1]));
            Assert.AreEqual("/home/user", Message2.getPath(obj));
        }
    }
}