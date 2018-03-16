using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NetMapSaveItem :DescObject
{
    public DungeonMapAccessor.MapSaveType SaveType;
    public int SaveId;
    public int SaveValue;

    public NetMapSaveItem() { }

    public NetMapSaveItem(DungeonMapAccessor.MapSaveType saveType, int saveId, int saveValue)
    {
        SaveType = saveType;
        SaveId = saveId;
        SaveValue = saveValue;
    }

    public NetMapSaveItem(NetMapSaveItem origin)
    {
        
    }

}