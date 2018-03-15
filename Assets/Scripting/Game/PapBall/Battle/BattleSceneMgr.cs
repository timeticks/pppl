using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSceneMgr : MonoBehaviour {
    public static BattleSceneMgr instance
    { get; private set; }

    public BallGroupMgr m_BallGroupMgr;
    


    void Awake()
    {
        instance = this;
    }




}
