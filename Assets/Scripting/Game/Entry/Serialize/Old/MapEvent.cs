using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public interface IMapEventFetcher
{
    MapEvent GetMapEventByCopy(int idx);
}

public class MapEvent : DescObject
{

    private static IMapEventFetcher mMapEventInter;


    public static IMapEventFetcher MapEventFetcher
    {
        get { return mMapEventInter; }
        set
        {
            if (mMapEventInter == null)
                mMapEventInter = value;
        }
    }

    private string mDesc;
    public enum MapEventType
    {
    	None,
        Quest,   //支线任务
        Repeat,  //重复事件
        End,     //结局
    }
    private MapEventType mEventType = MapEventType.None;
    private int mLootID;
    private string mEndDesc;

    public bool IsFirstFinish;

    public enum MapEventStatus
    {
        None,
        CanReaward,
        Finish,
    }

    public MapEventStatus EventStatus;

    public MapEvent():base()
    {
        
    }


    public MapEvent(MapEvent origin) : base(origin)
    {
        mDesc = origin.mDesc;
        mEventType = origin.mEventType;
        mLootID = origin.mLootID;
        mEndDesc = origin.mEndDesc;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        mEventType = (MapEventType)ios.ReadByte();
        mLootID = ios.ReadInt32();
        mDesc = NetUtils.ReadUTF(ios);
        mEndDesc = NetUtils.ReadUTF(ios);
    }

    /// <summary>
    /// 根据事件id，得到此事件对应的mapId
    /// </summary>
    public static List<int> GetMapsByEvents(List<int> eventList , MapData.MapType mapType)
    {
        List<int> mapList = new List<int>();
        //List<MapData> mapDataList = MapData.MapDataFetcher.GetMapDataListNoCopy(mapType);
        //Dictionary<int, bool> eventPool = new Dictionary<int, bool>();
        //for (int i = 0; i < eventList.Count; i++) //将事件添加到字典中，方便查询
        //{
        //    if (!eventPool.ContainsKey(eventList[i]))
        //        eventPool.Add(eventList[i], false);
        //}

        //for (int i = 0; i < mapDataList.Count; i++)
        //{
        //    bool isFindOver = false;
        //    for (int j = 0; j < mapDataList[i].Ending.Length; j++)
        //    {
        //        if (eventPool.ContainsKey(mapDataList[i].Ending[j])) //如果此地图中有对应事件，添加地图id
        //        {
        //            mapList.Add(mapDataList[i].idx);
        //            isFindOver = true;
        //            break;
        //        }
        //    }
        //    if (isFindOver) continue;
        //    for (int j = 0; j < mapDataList[i].Quest.Length; j++)
        //    {
        //        if (eventPool.ContainsKey(mapDataList[i].Quest[j]))
        //        {
        //            mapList.Add(mapDataList[i].idx);
        //            break;
        //        }
        //    }
        //}
        return mapList;
    }

    public string Desc
    {
        get { return mDesc; }
    }

    public MapEventType MyEventType
    {
        get { return mEventType; }
    }

    public int LootId
    {
        get { return mLootID; }
    }

    public string EndDesc
    {
        get { return mEndDesc; }
    }
}