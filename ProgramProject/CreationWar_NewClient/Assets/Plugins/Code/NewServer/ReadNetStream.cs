using UnityEngine;
using System.Collections;
using System;

public class ReadNetStream : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        DateTime t1 = DateTime.Now;
        NetDataManager.rollreadData();
        DateTime t2 = DateTime.Now;
        TimeSpan ts1 = t2 - t1;
//        if (ts1.Milliseconds > 5)
//        {
//            KDebug.WriteLog("--------------------ReadNetStream too LONG-------------------" + ts1.Milliseconds);
//        }
        NetDataManager.update();
	}
}
