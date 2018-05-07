using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerAccessor
{
    public PartnerData.SelectStep selectStepIndex = PartnerData.SelectStep.Sex; //当前已选择的步骤，
    public PartnerData.Sex selectSex = PartnerData.Sex.None;
    public PartnerData.HairColor selectHairColor = PartnerData.HairColor.None;
    public PartnerData.SkinColor selectSkinColor = PartnerData.SkinColor.None;
    public PartnerData.CharacType selectCharacType = PartnerData.CharacType.None;
    public PartnerData.HappyMemory selectHappyMemory = PartnerData.HappyMemory.None;

    public PartnerData curPartener;

    public List<PartnerData> oldPartenerList;

    //得到好感度等级
    public static int GetIntimacyLevel(int intimacyNum)
    {
        int[] intimacyArray = GameConstUtils.array_intimacy_level;
        for (int i = intimacyArray.Length-1; i >=0; i--)
        {
            if (intimacyArray[i] <= intimacyNum)
                return i;
        }
        return 0;
    }
}



public class PartnerData
{
    public int createTime;      //创建时间
    
    public int idx;
    public SkinColor skinColor;
    public HairColor hairColor;
    public HappyMemory happyMemory;
    public int intimacyNum;        //好感度

    //得到好感度等级
    public int GetIntimacyLevel()
    {
        int[] intimacyArray = GameConstUtils.array_intimacy_level;
        for (int i = intimacyArray.Length - 1; i >= 0; i--)
        {
            if (intimacyArray[i] <= intimacyNum)
                return i;
        }
        return 0;
    }


    //选择步骤
    public enum SelectStep
    {
        Sex,
        SkinColor,
        HairColor,
        Charac,
        HappyMemory,
        Max,
    }


    public enum Sex
    {
        None,
        Male,
        Female,
        Max
    }

    public enum SkinColor
    {
        None,
        White,
        Yellow,
        BlackYellow,
        Black,
        Max
    }

    public enum HairColor
    {
        None,
        Black,
        White,
        Blue,   //蓝色
        Gold,   //金黄
        Max
    }

    public enum CharacType
    {
        None,
        Cute,   //可爱
        Mature, //成熟
        Max
    }

    public enum HappyMemory   //往事回忆，后面去知乎看看
    {
        None,
        SeeStar,
        BuyFoot,
        Max
    }
}