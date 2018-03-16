using System.IO;
using System.Collections.Generic;
public interface IMapDataFetcher
{
    MapData GetMapDataByCopy(int idx);
    List<MapData> GetMapDataListNoCopy(MapData.MapType mapType);
}

public class MapData : DescObject
{

    private static IMapDataFetcher mMapDataInter;


    public static IMapDataFetcher MapDataFetcher
    {
        get { return mMapDataInter; }
        set
        {
            if (mMapDataInter == null)
                mMapDataInter = value;
        }
    }

    private string mDesc;

    public enum ConditionType:byte
    {
    	None,
    	Item,
    	Spell,
    	Sect,
    	Event,
    	Att,
    	Random,
    }


    public enum MapType : byte
    {
        None,
        SingleMap,   //单人秘境
        MutiMap,     //多人秘境
        SectMap,     //宗门地图
        NewerMap,    //新手秘境
    }
    private MapType mType;

    private short mOpenLevel;

    private byte[] mSize;
    private string[] mTerrainType;
    private string[] mTerrainName;
    private long mWalkable;
    private int[][] mNpcList;
    private short mEntry;
    private int mMapID;
    private int[] mQuest;
    private int[] mEnding;
    private int mOrder;
    private string mIcon;

    public enum LimitType : byte
    {
        None,
        Level,
        Item,
        Times,  //次数
    }
    private LimitType mLimitType;
    private int mLimitMisc;

    //地图需要保存的数据
    public enum MapSaveType : byte
    {
        None,
        RolePos,
        Item,
        Event,
        Npc
    }

    public MapData() : base()
    {
        
    }
    public MapData(MapData origin) : base(origin)
    {
        mDesc = origin.mDesc;
        mIcon = origin.mIcon;
        mType = origin.mType;
        mOpenLevel = origin.mOpenLevel;
        mOrder = origin.mOrder;
        mSize = origin.mSize;
        mTerrainType = origin.mTerrainType;
        mTerrainName = origin.mTerrainName;
        mWalkable = origin.mWalkable;
        mNpcList = origin.mNpcList;
        mEntry = origin.mEntry;
        mMapID = origin.mMapID;
        mQuest = origin.mQuest;
        mEnding = origin.mEnding;
        mLimitType = origin.mLimitType;
        mLimitMisc = origin.mLimitMisc;
    }

    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);
        mType    = (MapType)ios.ReadByte();
        mLimitType  = (LimitType)ios.ReadByte();

        mOpenLevel  = ios.ReadInt16();
        mEntry      = ios.ReadInt16();
        mOrder      = ios.ReadInt16();

        mMapID      = ios.ReadInt32();
        mLimitMisc = ios.ReadInt32();
        mWalkable   = ios.ReadInt64();

        mDesc = NetUtils.ReadUTF(ios);
        mIcon = NetUtils.ReadUTF(ios);

        byte length = ios.ReadByte();
        mSize = new byte[length];
        for (int i = 0; i < length; i++)
        {
            mSize[i] = ios.ReadByte();
        }

        length = ios.ReadByte();
        mTerrainType = new string[length];
        for (int i = 0; i < length; i++)
        {
            mTerrainType[i] = NetUtils.ReadUTF(ios);
        }

        length = ios.ReadByte();
        mTerrainName = new string[length];
        for (int i = 0; i < length; i++)
        {
            mTerrainName[i] = NetUtils.ReadUTF(ios);
        }

        length = ios.ReadByte();
        mQuest = new int[length];
        for (int i = 0; i < length; i++)
        {
            mQuest[i] = ios.ReadInt32();
        }

        length = ios.ReadByte();
        mEnding = new int[length];
        for (int i = 0; i < length; i++)
        {
            mEnding[i] = ios.ReadInt32();
        }

        //length = ios.ReadByte();
        //mNpcList = new int[length][];
        //for (int i = 0; i < length; i++)
        //{
        //    int subLength = ios.ReadByte();
        //    mNpcList[i] = new int[subLength];
        //    for (int j = 0; j < subLength; j++)
        //    {
        //        mNpcList[i][j] = ios.ReadInt32();
        //    }
        //}

        if (mSize.Length == 2)
        {
            if (mType == MapType.SingleMap || mType == MapType.NewerMap)
            {
                int amount = mSize[0]*mSize[1];
                if (mTerrainName.Length == amount && mTerrainType.Length == amount)
                {
                    return;
                }
            }
            else
            {
                if (mTerrainName.Length == mTerrainType.Length)
                {
                    return;
                }
            }
        }
        TDebug.Log(string.Format("[{0}]地图配置有错误，length[{1}] ,name[{2}] ,type[{3}]", idx, mSize[0]*mSize[1], mTerrainName.Length, mTerrainType.Length));
    }





    public string Desc
    {
        get { return mDesc; }
    }

    public MapType Type
    {
        get { return mType; }
    }

    public short OpenLevel
    {
        get { return mOpenLevel; }
    }

    public byte[] Size
    {
        get { return mSize; }
    }

    public string[] TerrainType
    {
        get { return mTerrainType; }
    }

    public long Walkable
    {
        get { return mWalkable; }
    }

    public string[] TerrainName
    {
        get { return mTerrainName; }
    }

    public int[][] NpcList
    {
        get { return mNpcList; }
    }

    public short Entry
    {
        get { return mEntry; }
    }

    public int MapId
    {
        get { return mMapID; }
    }
    public int Order
    {
        get { return mOrder; }
    }

    public int[] Quest
    {
        get { return mQuest; }
    }

    public int[] Ending
    {
        get { return mEnding; }
    }

    public LimitType MyLimitType
    {
        get { return mLimitType; }
    }

    public int LimitMisc
    {
        get { return mLimitMisc; }
    }

    public string Icon
    {
        get { return mIcon; }
    }

}
