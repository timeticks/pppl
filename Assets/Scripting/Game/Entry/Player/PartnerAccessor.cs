using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerAccessor
{
    public PartnerData.SelectStep selectStepIndex = PartnerData.SelectStep.None; //当前已选择的步骤，
    public PartnerData.Sex selectSex = PartnerData.Sex.None;
    public PartnerData.HairColor selectHairColor = PartnerData.HairColor.None;
    public PartnerData.SkinColor selectSkinColor = PartnerData.SkinColor.None;
    public PartnerData.CharacType selectCharacType = PartnerData.CharacType.None;
    public PartnerData.HappyMemory selectHappyMemory = PartnerData.HappyMemory.None;
    public int selectPartnerIdx;

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

    //根据已选择的同伴信息，生成同伴
    public void InitCurPartnerBySelect()
    {
        curPartener = new PartnerData();
        curPartener.createTime = AppTimer.CurTimeStampSecond;
        curPartener.hairColor = selectHairColor;
        curPartener.skinColor = selectSkinColor;
        curPartener.intimacyNum = 0;
        curPartener.intimacyLevel = 1;
        curPartener.happyMemory = selectHappyMemory;
        curPartener.idx = selectPartnerIdx;
    }

}



public class PartnerData
{
    public int createTime;      //创建时间
    
    public int idx;
    public SkinColor skinColor;
    public HairColor hairColor;     
    public HappyMemory happyMemory; 
    public Eint intimacyLevel=0;       //好感等级
    public Eint intimacyNum=0;         //好感度


    //选择步骤
    public enum SelectStep
    {
        None,
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