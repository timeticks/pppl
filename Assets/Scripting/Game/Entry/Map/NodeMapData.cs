using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonMapData:MapData
{
    public static Vector2 ItemSize = new Vector2(180, 180);    //标准大小
    public static Vector2 Detla = new Vector2(180, -180);
    public static Vector2 StartPos;

    public MindTreeMapCtrl TreeData;

    public int Width
    {
        get { if (Size.Length > 0) return Size[0];
            return 0;
        }
    }

    public int Height
    {
        get
        {
            if (Size.Length > 1) return Size[1];
            return 0;
        }
    }

    public bool GetWalkable(XyCoordRef xy)
    {
        int nodeIndex = GetNodeIndex(xy);
        long temp = (Walkable | (1 << nodeIndex));
        return (temp == Walkable);
    }


    public string GetTerrainName(int nodeIndex)
    {
        if (nodeIndex >= 0 && nodeIndex < TerrainName.Length)
        {
            return TerrainName[nodeIndex];
        }
        return "";
    }
    public string GetTerrainType(int nodeIndex)
    {
        if (nodeIndex >= 0 && nodeIndex < TerrainType.Length)
        {
            return TerrainType[nodeIndex];
        }
        return "";
    }

    public DungeonMapData(int mapIdx):base(MapData.MapDataFetcher.GetMapDataByCopy(mapIdx))
    {
        MapData map = MapData.MapDataFetcher.GetMapDataByCopy(mapIdx);
        if (map == null)
        {
            TDebug.LogError(string.Format("地图配置有错 ,idx={0}", mapIdx));
            return;
        }

        StartPos = new Vector2((-Width+1) * ItemSize.x * 0.5f, (Height-1) * ItemSize.y * 0.5f);
        Detla = new Vector2(ItemSize.x, -ItemSize.y);
    }



    public List<XyCoordRef> GetNearNode(XyCoordRef xy)
    {
        List<XyCoordRef> nearList = new List<XyCoordRef>()
        {
            new XyCoordRef(xy.m_X - 1, xy.m_Y),
            new XyCoordRef(xy.m_X + 1, xy.m_Y),
            new XyCoordRef(xy.m_X, xy.m_Y - 1),
            new XyCoordRef(xy.m_X, xy.m_Y + 1)
        };
        return nearList;
    }


    public Vector2 GetPos(int x, int y)//得到世界位置
    {
        return StartPos + new Vector2(ItemSize.x * x, -ItemSize.y * y);
    }
    public XyCoordRef GetXyByPos(Vector2 pos)//根据位置，得到最符合的坐标
    {
        XyCoordRef xy = new XyCoordRef();
        Vector2 detlaPos = pos - StartPos;
        float x = detlaPos.x / Detla.x;
        float y = detlaPos.y / Detla.y;
        if (x < 0 || y < 0) { return null; }  //不在范围内
        x -= 0.5f;
        y -= 0.5f;
        xy = new XyCoordRef(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
        if (xy.m_Y > (Height - 1) || xy.m_X > (Width - 1)) return null;
        return xy;
    }

    public XyCoordRef GetNodeXyByIndex(int _index)
    {
        XyCoordRef xy = new XyCoordRef();
        xy.m_X = _index % Width;
        xy.m_Y = _index/Width;
        return xy;
    }

    public int GetNodeIndex(int x, int y)
    {
        return y * Width + x;
    }
    public int GetNodeIndex(XyCoordRef xy)
    {
        if (xy == null) return 0;
        return xy.m_Y * Width + xy.m_X;
    }
     
}