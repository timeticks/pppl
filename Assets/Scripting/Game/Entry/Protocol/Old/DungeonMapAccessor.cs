using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DungeonMapAccessor :DescObject
{

	public bool IsEntered;
    public int MapIdx;
    public short RolePos;

    public Dictionary<int, Eint> ItemDic = new Dictionary<int, Eint>();
    public Dictionary<int, Eint> NpcDic = new Dictionary<int, Eint>();
    public Dictionary<int, Eint> EventDic = new Dictionary<int, Eint>();
  
    public List<int> EnterClosePos = new List<int>();

    public enum MapSaveType : byte
    {
        None,
        RolePos,
        Item,
        Event,
        Npc,
        EnterClose,
    }

    public DungeonMapAccessor() { }

    public DungeonMapAccessor(DungeonMapAccessor origin)
    {
        IsEntered = origin.IsEntered;
        MapIdx = origin.MapIdx;
        RolePos = origin.RolePos;
        ItemDic = origin.ItemDic;
        NpcDic = origin.NpcDic;
        EventDic = origin.EventDic;

        EnterClosePos = new List<int>(origin.EnterClosePos);
    }

    public void SetMapSave(List<NetMapSaveItem> saveList)
    {
        for (int i = 0; i < saveList.Count; i++)
        {
            SetMapSave(saveList[i].SaveType, saveList[i].SaveId, saveList[i].SaveValue);
        }
    }

    //保存秘境信息
    public void SetMapSave(DungeonMapAccessor.MapSaveType saveType, int saveId, int saveValue)
    {
        IsEntered = true;
        switch (saveType)
        {
            case DungeonMapAccessor.MapSaveType.Event:
                if (EventDic.ContainsKey(saveId))
                {
                    EventDic[saveId] = saveValue;
                    return;
                }
                else
                {
                    EventDic.Add(saveId, Mathf.Max(0, saveValue));
                }
                break;
            case DungeonMapAccessor.MapSaveType.Item:
                if (ItemDic.ContainsKey(saveId))
                {
                    ItemDic[saveId] += saveValue;
                    return;
                }
                else
                {
                    ItemDic.Add(saveId, Mathf.Max(0, saveValue));
                }
                break;
            case DungeonMapAccessor.MapSaveType.Npc:
                if (NpcDic.ContainsKey(saveId))
                {
                    NpcDic[saveId] = saveValue;
                    return;
                }
                else
                {
                    NpcDic.Add(saveId, Mathf.Max(0, saveValue));
                }
                break;
            case DungeonMapAccessor.MapSaveType.EnterClose:
                for (int i = 0, length = EnterClosePos.Count; i < length; i++)
                {
                    if (EnterClosePos[i] == saveValue)
                    {
                        return;
                    }
                }
                EnterClosePos.Add(saveValue);
                break;
            case DungeonMapAccessor.MapSaveType.RolePos:
                SaveUtils.SetIntInPlayer(PrefsSaveType.RolePos.ToString(), saveValue);
                break;
        }

    }


    public void ReadFrom(BinaryReader ios)
    {
        IsEntered = ios.ReadBoolean();
        if (IsEntered)
        {
            RolePos = ios.ReadInt16();
            MapIdx = ios.ReadInt32();

            int length = ios.ReadByte();
            ItemDic = new Dictionary<int, Eint>();
            for (int i = 0; i < length; i++)
            {
                ItemDic.Add(ios.ReadInt32(), ios.ReadInt16());
            }
            
            length = ios.ReadByte();
            NpcDic = new Dictionary<int, Eint>();
            for (int i = 0; i < length; i++)
            {
                NpcDic.Add(ios.ReadInt32() , ios.ReadByte());
            }
            
            length = ios.ReadByte();
            EventDic = new Dictionary<int, Eint>();
            for (int i = 0; i < length; i++)
            {
                EventDic.Add(ios.ReadInt32() , ios.ReadByte());
            }
            
            length = ios.ReadByte();
            for (int i = 0; i < length; i++)
            {
                EnterClosePos.Add(ios.ReadByte());
            }
        }
    }


    public List<int> GetFinishEvent()
    {
        List<int> finishList = new List<int>();
        foreach (var temp in EventDic)
        {
            if (temp.Value >= GameConstUtils.MapEventFinishStatus)
            {
                finishList.Add(temp.Key);
            }
        }
        return finishList;
    }
}


public enum NpcStatus : byte
{
    Disable,
    Enable,
    Dead
}
