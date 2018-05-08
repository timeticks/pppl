using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;



public enum DataName : short
{
    Item,   //道具

    Hero,
    Spell ,
    DropQualityRate,
    GameConst,
    BallMap,
    DropGrade,
    LobbyDialogue,
    HeroLevelUp,
    QualityTable,
    PartnerDialogue,
    Partner,
    /**********文本配置表***********/
    //TextBattle,
    //ItemText,
    //GoodsText,
    //SkillText,
    //ShipText,
    //NaviText,
    //BattleText,
    //StageText,
    //CityText,
    //RemainText,
    //AreaText,
    //InvestChatText,


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
        TDebug.Log( "GameData load count===" + Dic_DataBase.Count );
    }

    void LoadDataBaseFromRes( DataName _name )
    {
        try
        {
            //进行litJson中int转long的方法，否则某些小于int.MaxValue的整数无法直接转为long   Can't assign value '0' (type System.Int32) to type System.Int64
            LitJson.JsonMapper.RegisterImporter<int, long>((int value) =>
            {
                return (long)value;
            });
            LitJson.JsonMapper.RegisterImporter<string, Eint>((string value) =>
            {
                int temp = 0;
                int.TryParse(value, out temp);
                return (int)temp;
            });

            TextAsset asset = SharedAsset.Instance.LoadAssetSyncObj_ImmediateRelease(BundleType.GameDataBundle, _name.ToString()) as TextAsset;
            ConfigHelper.Instance.Init(_name, asset.text);
            //Dic_DataBase.Add(_name, (Hashtable)MiniJson.JsonDecode(asset.text));
        }
        catch (System.Exception exc) { TDebug.LogError("读取配置表:" + _name +"===="+ exc.Message); }
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
        if (hash.ContainsKey(keyStr))
        {
            return hash[keyStr]as Hashtable;
        }
        TDebug.LogError("GetData Error：" + _name + "   key" + keyStr);
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
