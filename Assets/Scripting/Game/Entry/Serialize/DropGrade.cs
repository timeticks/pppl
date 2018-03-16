using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropGradeFetcher
{
    DropGrade GetDropGradeCopy(int idx);
}
public class DropGrade : BaseObject
{
    private static IDropGradeFetcher mFetcher;

    public static IDropGradeFetcher Fetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }
    public enum DropType
    {
        None,   //顺序不能更改，DropGrade中填写的概率就是此顺序的
        Gold,
        
        Item,
        StrengthenItem,
        Equip,

        Diamond,
        Exp,

    }


    public int dropQuality;
    public int[] gradeProb; //结合grade，随机出此次掉落的个数和品质
    public int[] grade;     //掉落的品质
    public int[] typeProb;

    public DropGrade Clone()
    {
        return this.MemberwiseClone() as DropGrade;
    }

    //计算掉落品质与掉落数量
    public static List<DropInfo> GetDropInfoList(DropInfo dropInfo)
    {
        DropGrade drop = DropGrade.Fetcher.GetDropGradeCopy(dropInfo.monsterQuality);

        List<DropInfo> dropList = new List<DropInfo>();

        //随机最终的掉落品质  
        DropGrade tempDrop;
        for (int i = 0; i < drop.gradeProb.Length; i++)
        {
            if (GameUtils.isTrue(drop.gradeProb[i])) //如果此随机值为真，则增加一个此品质的掉落
            {
                tempDrop = DropGrade.Fetcher.GetDropGradeCopy(drop.grade[i]);
                DropType ty = (DropType)GameUtils.GetRandomIndex(tempDrop.typeProb);
                if (ty == DropType.None) continue;
                DropInfo dropItem = dropInfo.Clone();
                dropItem.dropType = ty;
                dropItem.dropQuality = tempDrop.grade[GameUtils.GetRandomIndex(tempDrop.gradeProb)];
                dropList.Add(dropItem);
            }
        }

        //将多次的金币掉落移除掉
        for (int i = 0; i < dropList.Count; i++)
        {
            if (dropList[i].dropType == DropType.Gold)
            {
                
            }
        }
        return dropList;
    }



    public static Dictionary<DropType, List<object>> RunDrop(DropInfo dropInfo)
    {
        List<DropInfo> dropInfoList = GetDropInfoList(dropInfo);
        return GetDropObjects(dropInfoList);
    }


    public static Dictionary<DropType, List<object>> GetDropObjects(List<DropInfo> dropInfoList)
    {
        Dictionary<DropType, List<object>> dict = new Dictionary<DropType, List<object>>();
        DropInfo dropInfo;
        for (int i = 0; i < dropInfoList.Count; i++)
        {
            DropType dropTy = dropInfoList[i].dropType;
            dropInfo = dropInfoList[i];
            object dropObj = null;
            switch (dropTy)
            {
                case DropType.Gold:
                {
                    dropObj = GameUtils.GetDropGold(dropInfo.monsterLevel, dropInfo.dropQuality);
                    break;
                }
                case DropType.Item:
                {
                    break;
                }
                case DropType.StrengthenItem:
                {
                    break;
                }
                case DropType.Equip:
                {
                    dropObj = GetDropEquip(dropInfoList[i]);
                    break;
                }
            }
            if (dropObj == null) continue;
            if (dict.ContainsKey(dropTy)) //添加掉落的东西
            {
                if (dropTy == DropType.Gold) //如果是金币，将所有金币相加
                {
                    int goldNum = (int)dict[dropTy][0] + (int)dropObj;
                    dict[dropTy][0] = goldNum;
                }
                else 
                    dict[dropTy].Add(dropObj);
            }
            else
            {
                dict.Add(dropTy, new List<object>() {dropObj});
            }
        }
        return dict;
    }

    public static Equip GetDropEquip(DropInfo dropInfo)
    {
        Equip dropEquip = null;

        //得到随机的品质
        DropQualityRate qualityRate =
           DropQualityRate.Fetcher.GetDropQualityRateCopy(dropInfo.dropQuality);
        int quality = GameUtils.GetRandomIndex(qualityRate.qualityProb);
        
        //在等级范围内随机一个装备id
        if (GameUtils.isTrue(1000)) //小几率增加怪物等级，即得到更高级装备
            dropInfo.monsterLevel += 5;
        int equipLevel = Mathf.Clamp(dropInfo.monsterLevel - dropInfo.monsterLevel % 10 , 1 , GameConstUtils.max_equip_level);
        List<Equip> equipList = Equip.Fetcher.GetEquipByLevelCopy(equipLevel);
        if (equipList.Count == 0)
        {
            TDebug.LogError(string.Format("没有此等级的装备,level:{0}",equipLevel));
            return null;
        }
        int[] equipProb = new int[equipList.Count];
        for (int i = 0; i < equipList.Count; i++)
        {
            if (quality >= equipList[i].qualityRange[0])  //掉落品质要大于等于装备的最小品质
                equipProb[i] = equipList[i].dropProb;
        }
        int equipIndex = GameUtils.GetRandomIndex(equipProb);
        dropEquip = equipList[equipIndex].Clone();

        //对爆出的装备附上品质，品质在装备的品质范围内
        quality = Mathf.Clamp(quality, dropEquip.qualityRange[0], dropEquip.qualityRange[1]);
        dropEquip.curQuality = quality;

        //随机等级
        dropEquip.curLevel = GameUtils.GetRandomByLimit(dropInfo.monsterLevel, 10000, 10000, -2, 3);

        //对爆出的装备进行属性随机
        if (quality > 0)
        {
            List<AttrType> attrTypeList = new List<AttrType>();
            int[] attrProb = AttrProb.GetProbList(AttrProb.ObjType.Equip, dropEquip.type.ToInt(), quality);
            //品质为几，就有几个附加属性
            for (int i = 0; i < quality; i++)
            {
                int attrIndex = GameUtils.GetRandomIndex(attrProb);
                attrProb[attrIndex] = attrProb[attrIndex] / 100;  //如果随机过的属性，概率下降100倍
                AttrType getAttr = AttrProb.GetAttrTypeByIndex(attrIndex);
                //TDebug.Log(string.Format("{0}|{1}", getAttr, LitJson.JsonMapper.ToJson(attrProb)));
                attrTypeList.Add(getAttr);
            }

            QualityTable qualityTable = QualityTable.Fetcher.GetQualityTableCopy(dropEquip.curQuality);

            //随机出属性的数值
            dropEquip.curSubType = attrTypeList.ToArray();
            dropEquip.curSubVal = new Eint[attrTypeList.Count];
            for (int i = 0; i < dropEquip.curSubType.Length; i++)
            {
                int attrValue = AttrTable.GetAttrMarkValue(dropEquip.curSubType[i], dropEquip.curLevel);
                attrValue = (int) (attrValue*dropEquip.subAttrRatio.ToFloat_10000()* qualityTable.subCoeff.ToFloat_10000());
                attrValue = GameUtils.GetRandomByLimit(attrValue, 9000, 11000, -2, 3);
                dropEquip.curSubVal[i] = attrValue;
            }

            //主属性随机
            dropEquip.curMainAttrType = dropEquip.mainAttrType.Clone()as AttrType[];
            dropEquip.curMainAttrVal = dropEquip.mainAttrVal.Clone() as Eint[];
            for (int i = 0; i < dropEquip.curMainAttrVal.Length; i++)
            {
                dropEquip.curMainAttrVal[i] =
                    (int)((int)dropEquip.curMainAttrVal[i] * qualityTable.mainCoeff.ToFloat_10000());

                dropEquip.curMainAttrVal[i] = GameUtils.GetRandomByLimit(dropEquip.curMainAttrVal[i], 9000, 11000, -2, 3);
            }
        }
        

        return dropEquip;
    }
}

public class DropInfo  //掉落项
{
    public int monsterQuality;  //怪物或宝箱品质
    public int monsterLevel;    //怪物或宝箱等级

    public int dropQuality;
    public int dropPoint;   
    public DropGrade.DropType dropType;

    public DropInfo Clone()
    {
        return this.MemberwiseClone()as DropInfo;
    }
}