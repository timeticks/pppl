using UnityEngine;
using System.Collections;
using System.IO;

using System;
using System.Collections.Generic;

public class StartSceneMgr : SceneMgrBase
{
    public static StartSceneMgr Instance { get; private set; }

    void Awake() 
    {
        Instance = this;
        _Init(this);
    }
	

}
