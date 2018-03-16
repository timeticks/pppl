using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideAccessor
{
    public bool JustPassNewerMap//刚刚通过新手秘境
    {
        get { return mJustPassNewerMap; }
        set 
        {
            mJustPassNewerMap = value;
            //if (mJustPassNewerMap == true) { WhenLevelUp(null); }
        }
    }
    private bool mJustPassNewerMap;

    //public enum ModuleType
    //{
    //    Travel,
    //    Inventory,
    //    Cave,
    //    Sect,
    //    DungeonMap,
    //    MakeDrug,
    //    Rank,
    //    MakeEquip,
    //    PetAnimal,
    //    PetPuppet,
    //    PVP,
    //    PetGhost,
    //    Max
    //}

    //public enum GuideStatus
    //{
    //    None,
    //    Guiding,
    //    Guided,
    //}

    public void Init()
    {
        //AppEvtMgr.Instance.Register(new EvtItemData(EvtType.LevelUp), EvtListenerType.GuideModuleOpen, WhenLevelUp);
    }


}
