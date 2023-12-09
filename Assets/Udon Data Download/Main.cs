using UdonSharp;
using UnityEngine;
using VRC.Collections;
using VRC.SDK3.Data;
using VRC.SDK3.StringLoading;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;


namespace Hroi.UdonDataDownload
{
    public class Main : UdonSharpBehaviour
    {
        public VRCUrl url;

        void Start()
        {
            // VRCStringDownloader.LoadUrl(url, (IUdonEventReceiver)this);
            // var i = 0;
            // var bytes = new byte[] { 0x08, 0x2A, 0x10, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x21 };
            // var data = Message1.Parse(bytes, ref i, bytes.Length);
            // Debug.Log(data.ToString());

            // var dataDict = new DataDictionary();
            // dataDict.Add("Hello", "World");
            // dataDict.Add("I", "am");
            // dataDict.Add("testing", "this.");
            // Debug.Log(dataDict["I"].ToString());
            // if (true)
            // {
            //     var dataList = new DataList();
            //     dataList.Add("Hello");
            //     dataList.Add("Yellow");
            //     dataList.Add("Green");
            //     dataDict["A"] = dataList;
            //     Debug.Log(dataList[0].ToString());
            //     Debug.Log(dataDict["I"].ToString());
            //     Debug.Log(dataDict.ToString());
            //     Debug.Log(dataDict["A"].DataList[1]);
            // }
        }

        override public void OnStringLoadSuccess(IVRCStringDownload result)
        {
            Debug.Log("Done, success");

            var bytes = Base128.DecodeStr(result.Result);
            Debug.Log(Util.ByteArrayToString(bytes));
        }

        override public void OnStringLoadError(IVRCStringDownload result)
        {
            Debug.Log("Done, failure");
        }
    }
}
