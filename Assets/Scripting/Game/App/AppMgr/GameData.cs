using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;



public enum DataName : short
{
    Global = 0,

    Remain = 50,
    Stage,

    Item = 60,   //道具

    City = 100,
    CityBase,
    CityLevelRight,
    CityData,//以前用的，港口初始化还在占用

    GoodsBase = 110,    //贸易物品
    GoodsBuyRate,
    GoodsSoldRate,

    GamblePoker = 120,//贸易打牌
    GamblePoints,
    Dialogue,

    ShipData = 201,
    ShipEquipData = 202,

    NaviData = 301,
    NaviEquipData = 302,

    MonsterData,

    MapArea = 401,
    ExploreData = 402,//老
    TerrainData = 403,

    PassiveSkill,
    SkillData = 501,
    BuffData = 502,

    AiData = 601,
    ChatData = 602,
    //Lua,

    /**********文本配置表***********/
    textBattle,
    ItemText,
    GoodsText,
    SkillText,
    ShipText,
    NaviText,
    BattleText,
    StageText,
    CityText,
    RemainText,
    AreaText,
    InvestChatText,
    ServerTipsText = 9999
}


public class GameData : MonoBehaviour
{
    public static GameData Instance{get;private set;}
    private Dictionary<DataName, Hashtable> Dic_DataBase = new Dictionary<DataName, Hashtable>( );
    void Awake( )
    {
        Instance = this;
    }


    /// <summary>
    /// 读取所有配置表
    /// </summary>
    public void LoadAllDataBase( )
    {
        System.Type _dataName = typeof( DataName );
        System.Array Arry_name = System.Enum.GetValues( _dataName );
        for( int i = 0; i < Arry_name.LongLength; i++ )
        {
            LoadDataBaseFromRes( ( DataName )Arry_name.GetValue( i ) );
        }
        TDebug.LogFormat( "GameData load count==={0}" , Dic_DataBase.Count );
    }

    void LoadDataBaseFromRes( DataName _name )
    {
        try
        {
            TextAsset asset = SharedAsset.Instance.LoadAssetSyncObj_ImmediateRelease(BundleType.GameDataBundle, _name.ToString()) as TextAsset;
            Dic_DataBase.Add(_name, (Hashtable)MiniJson.JsonDecode(asset.text));
        }
        catch (System.Exception exc) { TDebug.LogErrorFormat("读取配置表:{0}===={1}" , _name , exc.Message); }
    }

    

    /// <summary>
    /// [id.ToString()] 索引需要用字符串
    /// </summary>
    public Hashtable GetData( DataName _name )
    {
        if (!GameData.Instance.Dic_DataBase.ContainsKey(_name))
        {
            LoadDataBaseFromRes( _name );
        }
        return GameData.Instance.Dic_DataBase[_name];
    }

    public Hashtable GetData(DataName _name, string keyStr)
    {
        Hashtable hash = GetData(_name);
        if (_name == DataName.NaviData) { if (!hash.ContainsKey(keyStr)) hash = GetData(DataName.MonsterData); }
        if (hash.ContainsKey(keyStr))
        {
            return hash[keyStr]as Hashtable;
        }
        TDebug.LogErrorFormat("GetData Error：{0}    key{1}" , _name.ToString() , keyStr);
        return null;
    }

    /// <summary>
    /// 获取形如  text0 text1  text2  text3的末尾数字不同的key列表
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="mainKey"></param>
    /// <returns></returns>
    public List<string> GetSameKey(DataName _name, string mainKey)
    {
        Hashtable hash = GetData(_name);
        int curIndex = 0;
        List<string> sameKey = new List<string>();
        if (hash.ContainsKey(mainKey))
        {
            sameKey.Add(mainKey);
        }
        while (hash.ContainsKey(mainKey + curIndex))  
        {
            sameKey.Add(mainKey + curIndex);
            curIndex ++;
        }
        return sameKey;
    }




    
}
