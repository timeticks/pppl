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
    public PartnerData.HobbyType selectHobbyType = PartnerData.HobbyType.None;


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

    public bool HavePartner()
    {
        if (curPartener == null || curPartener.idx == 0)
            return false;
        return true;
    }

    //根据已选择的同伴信息，生成同伴
    public void InitCurPartnerBySelect()
    {
        curPartener = new PartnerData();
        curPartener.createTime = AppTimer.CurTimeStampSecond;
        curPartener.startFindTime = PlayerPrefsBridge.Instance.PlayerData.BirthTime;
        curPartener.hairColor = selectHairColor;
        curPartener.skinColor = selectSkinColor;
        curPartener.hobbyType = selectHobbyType;

        curPartener.intimacyNum = 0;
        curPartener.intimacyLevel = 1;

        //characType和sexType，可以通过idx找到
        Partner partner = Partner.Fetcher.GetPartnerRandomCopy(selectSex,
            selectCharacType);
        curPartener.partnerName = partner.name;
        curPartener.idx = partner.idx;
    }

}



public class PartnerData
{
    //characType和sex，可以通过idx找到
    public int idx;
    public int startFindTime;   //开始寻找时间
    public int createTime;      //创建时间
    public string partnerName;
    
    public SkinColor skinColor;
    public HairColor hairColor;
    public HobbyType hobbyType;     

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

    public enum HobbyType   //爱好   旅游|运动|看书|画画|音乐
    {
        None,
        Traval,
        Sport,
        Book,
        Draw,
        Music,
        Max
    }
}