using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAttrProbFetcher
{
    AttrProb GetAttrProbCopy(AttrProb.ObjType objTy, int objParam);
}
public class AttrProb : BaseObject
{
    private static IAttrProbFetcher mFetcher;
    public static IAttrProbFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public enum ObjType
    {
        None,
        Equip,
        Stone,
        Spec,
        Other
    }

    public ObjType objType;
    public int objParam;

    public int[] allProb; //偶数下标是数值（0-2-4），奇数下标是概率

    public AttrProb Clone()
    {
        return this.MemberwiseClone() as AttrProb;
    }




    /// <summary>
    /// 根据下标，得到对应的属性类型
    /// </summary>
    public static AttrType GetAttrTypeByIndex(int probIndex)
    {
        return AttrTable.GetAttrType((AttrType)(probIndex / 2), probIndex % 2 == 1);
    }


    //根据品质，得到各个属性的概率
    public static int[] GetProbList(AttrProb.ObjType objTy, int objPar, int quality)
    {
        AttrProb attrProb = AttrProb.Fetcher.GetAttrProbCopy(objTy, objPar);
        int[] probList  = (int[])attrProb.allProb.Clone();
        for (int i = 0; i < probList.Length; i+=2)
        {
            AttrType attrType = (AttrType)(i / 2);
            AttrTable attrTable = AttrTable.Fetcher.GetAttrTableCopy(attrType);
            //所有属性，都要乘以各个属性中此品质的概率
            if (probList[i] > 0)
            probList[i] = ((int)probList[i] * (int)attrTable.valProb[quality]);
            if (probList[i + 1]>0)
            probList[i + 1] = ((int)probList[i + 1] * (int)attrTable.valProb[quality]);
        }
        return probList;
    }

    public static int GetKey(AttrProb.ObjType objType, int objParam)
    {
        return ((int) objType)*10000 + objParam;
    }
}
