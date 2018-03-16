using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapEventAccessor :DescObject
{
    public List<int> MapList = new List<int>(); //已经同步过事件状态的地图id
    public Dictionary<int, MapEvent.MapEventStatus> EventStatusPool = new Dictionary<int, MapEvent.MapEventStatus>();

    public MapEventAccessor() { }

    public MapEventAccessor(MapEventAccessor origin)
    {
        this.MapList = new List<int>();
        this.MapList.AddRange(origin.MapList);
        this.EventStatusPool = new Dictionary<int, MapEvent.MapEventStatus>();
        foreach (var temp in origin.EventStatusPool)
        {
            EventStatusPool.Add(temp.Key, temp.Value);
        }
    }

    public Dictionary<int, MapEvent.MapEventStatus> GetStatusByMapId(int mapId)
    {
        MapData mapData = MapData.MapDataFetcher.GetMapDataByCopy(mapId);
        Dictionary<int, MapEvent.MapEventStatus> events = new Dictionary<int, MapEvent.MapEventStatus>();
        if (mapData == null)
            return events;

        int eventId = 0;
        for (int i = 0; i < mapData.Quest.Length; i++)
        { //支线
            eventId = mapData.Quest[i];
            if (EventStatusPool.ContainsKey(eventId))
            {
                events.Add(eventId, EventStatusPool[eventId]);
            }
        }
        for (int i = 0; i < mapData.Ending.Length; i++)
        { //结局
            eventId = mapData.Ending[i];
            if (EventStatusPool.ContainsKey(eventId))
            {
                events.Add(eventId, EventStatusPool[eventId]);
            }
        }
        return events;
    }

    public List<int> GetCanRewardEvents() //得到所有可领奖的事件id
    {
        List<int> canRewardList = new List<int>();
        foreach (var temp in EventStatusPool)
        {
            if (temp.Value == MapEvent.MapEventStatus.CanReaward)
            {
                canRewardList.Add(temp.Key);
            }
        }
        return canRewardList;
    }

    public void ReadFrom(BinaryReader ios)
    {
        //IsEntered = ios.ReadBoolean();
        //if (IsEntered)
        //{
        //    RolePos = ios.ReadInt16();
        //    MapIdx = ios.ReadInt32();

        //}
    }

    public bool IsMapExsit(int mapId)
    {
        for (int i = 0; i < MapList.Count; i++)
        {
            if(MapList[i] == mapId)
                return true;
        }
        return false;
    }
}

