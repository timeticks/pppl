using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IMapFetcher
{
    Map GetMapCopy(int idx,bool isCopy=true);
    List<Map> GetAllMapCopy(bool isCopy=true);
}
public class Map : BaseObject
{
    private static IMapFetcher mFetcher;

    public static IMapFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public enum MapType
    {
        Normal,
    }

    public MapType mapType;
    public string icon;
    public string desc;
    public int[] pos;  //x、y
    public Eint fitLevel=0;
    public Eint explore=0;  //所需探索值
    public Eint ranLevel=0; //随机等级
    public Eint[] loot;   //用于显示可掉落
    public bool defaultHide;
    public Eint openLevel=0;
    public Eint enterItem=0; //进入消耗道具
    public Eint mapQuality=0;//地图品质

    public Eint[] find;    //打通之后开通的地图
    public Eint[] monster;
    public int headMap;     //前置地图
    public int[] monsterPrefix;    //怪物可随机的前缀
    public int[] prefixProp;
    public Eint boss=0;

    public Map Clone()
    {
        return this.MemberwiseClone() as Map;
    }

    public Vector2 getPos()
    {
        return new Vector2(pos[0], pos[1]);
    }
}
