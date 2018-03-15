using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于绑定多张Sprite，并根据Name来获取
/// </summary>
public class SpritePrefab : MonoBehaviour {
    [System.Serializable]
    public class SpriteData
    {
        public string Name;
        public Sprite Sp;

        public SpriteData(string name, Sprite sp)
        {
            Name = name;
            Sp = sp;
        }
    }

    public List<SpriteData> SpriteList;
    public Material Mat;
    private Dictionary<string, Sprite> mSpriteDict;

    void Awake()
    {
        
    }

    void InitDict()
    {
        mSpriteDict = new Dictionary<string, Sprite>();
        for (int i = 0; i < SpriteList.Count; i++)
        {
            if (!mSpriteDict.ContainsKey(SpriteList[i].Name))
                mSpriteDict.Add(SpriteList[i].Name, SpriteList[i].Sp);
            else Debug.LogError("SpritePrefab中有重复的Name" + SpriteList[i].Name);
        }
    }

    public Sprite GetSprite(string keyStr)
    {
        if (mSpriteDict == null) InitDict();
        if (mSpriteDict.ContainsKey(keyStr))
        {
            return mSpriteDict[keyStr];
        }
        else
        {
            TDebug.LogErrorFormat("没有此sprite: {0}" , keyStr);
            return null;
        }
    }

    public Sprite GetMainSprite()
    {
        if (mSpriteDict.Count > 0)
        {
            foreach (var temp in mSpriteDict)
            {
                return temp.Value;
            }
        }
        return null;
    }


}
