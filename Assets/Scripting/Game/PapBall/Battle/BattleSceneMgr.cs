using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSceneMgr : MonoBehaviour {
    public static BattleSceneMgr instance
    { get; private set; }    


    void Awake()
    {
        instance = this;
    }




}
