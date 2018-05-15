using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameAssetsPool : MonoBehaviour 
{
    public static GameAssetsPool Instance{get;private set;}

    public Dictionary<string, List<DestroySelf>> EffectDict = new Dictionary<string, List<DestroySelf>>();

    internal List<DestroySelf> EffectList = new List<DestroySelf>();
    internal List<MonoBehaviour> OtherList = new List<MonoBehaviour>();


    void Awake()
    {
        if (Instance == null) Instance = this;
    }
    void Start() { }

    
    public void ClearAll()//转场景时，清空list，释放内存资源
    {
    }



    public DestroySelf GetEffect(string eName)
    {
        eName = eName.ToLower();
        if (EffectDict.ContainsKey(eName))
        {
            foreach (var temp in EffectDict[eName])
            {
                if (temp != null && !temp.gameObject.activeSelf)
                {
                    return temp;
                }
            }
            
        }
        return CreateEffect(eName);
    }
    DestroySelf CreateEffect(string eName)
    {
        TLoader loader = SharedAsset.Instance.LoadAssetSync(BundleType.EffectBundle.BundleDictName() , eName);
        if (loader == null || loader.ResultObject == null) return null;
        Object asset = loader.ResultObjTo<Object>();
        GameObject obj = Instantiate(asset) as GameObject;
        DestroySelf des = obj.GetComponent<DestroySelf>();
        if (des == null)
        {
            TDebug.LogError(string.Format("{0}:此特效没有绑定DestroySelf", eName));
            des = obj.CheckAddComponent<DestroySelf>();
        }
        des.Init(eName, loader.Release);
        if (!EffectDict.ContainsKey(eName))
            EffectDict[eName] = new List<DestroySelf>();
        EffectDict[eName].Add(des);
        return des;
    }


}
