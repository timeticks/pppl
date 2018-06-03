using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public interface IAchievementFetcher
{
    Achievement GetAchievementByCopy(int idx);
    List<Achievement> GetAchievementsByCopy(Achievement.AchieveType achieveType);
    List<Achievement> GetAchievementsByCopy(Achievement.AchieveSubType achieveSubType);
 
}
public class Achievement : DescObject
{
    private static IAchievementFetcher mFetcher;
    public static IAchievementFetcher AchievementFetcher
    {
        get { return mFetcher; }
        set
        {
            if (mFetcher == null)
                mFetcher = value;
        }
    }

    public static int[] AchieveCout;
    public enum AchieveType
    {
        [EnumDesc("综合")]
    	None, //综合
        [EnumDesc("成长")]
    	Growth, //成长
        [EnumDesc("宗门")]
    	Sect, // 宗门
        [EnumDesc("技能")]
    	Spell, // 技能
        [EnumDesc("伙伴")]
    	Pet,   // 伙伴
        [EnumDesc("阅历")]
    	AuxSkill, // 生产技能
        [EnumDesc("活动")]
    	Activity, // 活动
    	Max,
    }
    public enum AchieveSubType
    {
        None,
        [EnumDesc("修为")]
        Level, 			//成长
        [EnumDesc("属性")]
        Prom,  			//属性
        [EnumDesc("其他")]
        Other, 		   //其他
        [EnumDesc("本宗门")]
        SelfSect, 		//本宗门
        [EnumDesc("天工宗")]
        TianGongZong,  	//天工宗
        [EnumDesc("百草门")]
        BaiCaoMen,  	   //百草门
        [EnumDesc("天机阁")]
        TianJiGe, 		//天机阁
        [EnumDesc("功法")]
        Spell, 			//功法
        [EnumDesc("炼器")]
        Forge,  		//炼器
        [EnumDesc("炼丹")]
        MakeDrug, 		//炼丹
        [EnumDesc("灵兽")]
        Animal,  		//灵兽
        [EnumDesc("傀儡")]
        Puppet,  		//傀儡
        [EnumDesc("英魂")]
        Ghost,  		//英魂
        [EnumDesc("养成")]
        Develop, 		//养成
        [EnumDesc("游历")]
        Travel, 		//游历
        [EnumDesc("剧情")]
        Plot,  			//剧情
        [EnumDesc("仙魔录")]
        Tower, 			//爬塔
        [EnumDesc("声望")]
        Prestige, 		//声望
        Max,
    }
    public enum ConType
    {
    	None,
    	Level, 					//成长
    	Prom, 					//属性
    	CaveLevel, 				//洞府等级
    	ZazenTime,  			    //累计打坐时间
    	ObtainGold, 			    //累计获得灵石
        UseGold,                 //累计消耗灵石
    	UseDiamond,			    //累计消耗仙玉
    	AdvertNum, 				//累计收看广告次数
    	MarketPurchase,  		    //黑市购买商品次数
    	JoinSect, 				//加入宗门
    	PrestigeLevel,  		    //宗门声望
    	SpellNum,  				//拥有功法数量
    	AuxSkillLevel,  	    	//生活技能等级
    	AuxSkillToolLevel, 		//生活技能工具等级
    	RecipeNum, 				//累计获得配方数量
    	ProduceEquipNum,  		//累计制造法宝数量
    	ProduceDrugNum, 		    //累计炼丹次数
    	ProduceMine,   			//累计挖矿次数
    	GatherHerb,    			//累计采草次数
    	ObtainPet,     			//累计获得灵兽数量
    	ObtainPuppet,  			//累计获得傀儡数量
    	ObtainGhost, 			    //累计获得英魂数量
    	UpLevelPet, 			    //累计喂养伙伴次数
    	EvolvePet, 				//累计进化伙伴次数
    	FinishTravelEvent, 		//完成游历点所有事件
    	FinishPlotEvent, 		    //完成秘籍所有事件
    	TowerFloor, 			    //仙魔录层数
        PrestigeTaskNum, 		    //声望任务完成次数
    	Max,
    }  



    private string[] mNames;
    private int[] mPoints;
    private string[] mDescs;   
    public AchieveType mType = AchieveType.None;
    public AchieveSubType mSubType = AchieveSubType.None;
    public ConType mConType = ConType.None; //条件类型
    public int mConparam; //条件参数
    public int[] mConValue = new int[0];
    public int mOrder;
    public Achievement() : base() { }

    public Achievement(Achievement origin) : base(origin)
    {
        this.mPoints = origin.mPoints;
        this.mDescs = origin.mDescs;
        this.mNames = origin.mNames;
        this.mType = origin.mType;
        this.mSubType = origin.mSubType;
        this.mConType = origin.mConType;
        this.mConparam = origin.mConparam;
        this.mConValue = origin.mConValue;
        this.mOrder = origin.mOrder;
    }
    public override void Serialize(BinaryReader ios)
    {
        base.Serialize(ios);

        this.mType = (AchieveType)ios.ReadByte();
        this.mSubType = (AchieveSubType)ios.ReadByte();
        this.mConType = (ConType)ios.ReadByte();
        this.mConparam = ios.ReadByte();

        this.mOrder = ios.ReadInt16();

        int length = ios.ReadByte();
        this.mConValue = new int[length];
        for (int i = 0; i <length; i++)
        {
            this.mConValue[i] = ios.ReadInt32();
        }

        length = ios.ReadByte();
        this.mPoints = new int[length];
        for (int i = 0; i < length; i++)
        {
            this.mPoints[i] = ios.ReadByte();
        }

        length = ios.ReadByte();
        this.mNames = new string[length];
        for (int i = 0; i < length; i++)
        {
            this.mNames[i] = NetUtils.ReadUTF(ios);
        }

        length = ios.ReadByte();
        this.mDescs = new string[length];
        for (int i = 0; i < length; i++)
        {
            this.mDescs[i] = NetUtils.ReadUTF(ios);
        }
    }
    public static List<Achieve> GetAchievementsByCopy(List<Achievement> achievementList, AchieveSubType subType)
    {
        List<Achieve> tempList =new List<Achieve>();
        Achievement achievement;
        for (int i = 0,length = achievementList.Count; i < length; i++)
        {
            achievement = achievementList[i];
            if (achievement.SubType == subType)
            {
                int progerss = PlayerPrefsBridge.Instance.GetAchievementInof(achievement.idx);
                for (int j = 0,count = achievement.Names.Length; j < count; j++)
                {
                    tempList.Add(new Achieve(achievement,j,progerss-1>=j));
                }
            }
        }
        if(tempList.Count==0)
            return null;
        return tempList;
    }




    public int[] Point
    {
        get { return mPoints; }
    }
    public string[] Desc
    {
        get { return mDescs; }
    }
    public string[] Names
    {
        get { return mNames; }
    }
    public AchieveType MyAchieveType
    {
        get { return mType; }
    }
    public AchieveSubType SubType
    {
        get { return mSubType; }
    }
    public int Order
    {
        get { return mOrder; }
    }
    public ConType MyConType
    {
        get { return mConType; }
    }
    public int[] ConValue
    {
        get { return mConValue; }
    }
    public int ConProm
    {
        get { return mConparam; }
    }
}

public class Achieve
{
    public string Name;
    public string Desc;
    public int   Point;
    public bool IsGot;
    public int Order;
    public Achieve(Achievement achievement,int index,bool isGot) 
    {
        this.Name = achievement.Names[index];
        this.Desc = isGot ? achievement.Desc[index] : achievement.Desc[index] + GetProgress(achievement, index, isGot);
        this.Point = achievement.Point[index];
        this.IsGot = isGot;
        this.Order = achievement.Order * 100 + index;
    }
    public Achieve(Achieve origin)
    {
        this.Name = origin.Name;
        this.Desc = origin.Desc;
        this.Point = origin.Point;
        this.IsGot = origin.IsGot;
        this.Order = origin.Order;
    }

    string GetProgress(Achievement achievement,int index,bool isGot)
    {
        string str = "";
        switch (achievement.MyConType)
        {
            case Achievement.ConType.Level:
                str = string.Format("({0}/{1})",PlayerPrefsBridge.Instance.PlayerData.Level,achievement.ConValue[index]);
                break;
            case Achievement.ConType.Prom:
                //str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.PlayerData.GetPromTypeNum((AttrType)achievement.ConProm), achievement.ConValue[index]);
                break; 			
            case Achievement.ConType.ZazenTime:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().ZazenTime/3600, achievement.ConValue[index]);
                break;  		
            case Achievement.ConType.ObtainGold:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().AddGold, achievement.ConValue[index]);
                break; 		
            case Achievement.ConType.UseGold:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().ConsumeGold, achievement.ConValue[index]);
                break;         
            case Achievement.ConType.UseDiamond:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().ConsumeDiamond, achievement.ConValue[index]);
                break;		
            case Achievement.ConType.AdvertNum:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().AdvertNum, achievement.ConValue[index]);
                break; 		
            case Achievement.ConType.MarketPurchase:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().MarketPurchase, achievement.ConValue[index]);
                break;  	
            case Achievement.ConType.JoinSect:
                str = isGot?"": "(0/1)";
                break; 		
            case Achievement.ConType.PrestigeLevel:
                //str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.PlayerData.GetPrestige((PrestigeLevel.PrestigeType)achievement.ConProm).Level, achievement.ConValue[index]);              
                break;  	
            case Achievement.ConType.SpellNum:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetSpellNumByLevel(achievement.ConProm), achievement.ConValue[index]);
                break;  		
            case Achievement.ConType.AuxSkillLevel:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.PlayerData.AuxSkillList[achievement.ConProm].Level, achievement.ConValue[index]);              
                break;  	
            case Achievement.ConType.AuxSkillToolLevel:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.PlayerData.PropLevelList[achievement.ConProm].Level, achievement.ConValue[index]);           
                break;
            case Achievement.ConType.ProduceEquipNum:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().ProduceEquip[achievement.ConProm], achievement.ConValue[index]);
                break;  
            case Achievement.ConType.ProduceDrugNum:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().ProduceDrug, achievement.ConValue[index]);
                break; 	
            case Achievement.ConType.ProduceMine:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().GetMine, achievement.ConValue[index]);
                break;   	
            case Achievement.ConType.GatherHerb:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().GetHerb, achievement.ConValue[index]);
                break;    	
            case Achievement.ConType.ObtainPet:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetPetNumByType(Pet.PetTypeEnum.Animal,achievement.ConProm), achievement.ConValue[index]);              
                break;     	
            case Achievement.ConType.ObtainPuppet:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetPetNumByType(Pet.PetTypeEnum.Puppet, achievement.ConProm), achievement.ConValue[index]);
                break;  	
            case Achievement.ConType.ObtainGhost:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetPetNumByType(Pet.PetTypeEnum.Ghost, achievement.ConProm), achievement.ConValue[index]);
                break; 		
            case Achievement.ConType.UpLevelPet:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().LevelUpPet, achievement.ConValue[index]);
                break; 		
            case Achievement.ConType.EvolvePet:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().EvolvePet, achievement.ConValue[index]);
                break; 		
            case Achievement.ConType.FinishTravelEvent:
                str = isGot ? "" : "(0/1)";
                break;
            case Achievement.ConType.FinishPlotEvent:
                str = isGot ? "" : "(0/1)";
                break; 	
            case Achievement.ConType.TowerFloor:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.ActivityData.TowerFloorIndex, achievement.ConValue[index]);
                break; 		
            case Achievement.ConType.PrestigeTaskNum:
                str = string.Format("({0}/{1})", PlayerPrefsBridge.Instance.GetAchieveAccessor().FinishPrestigeTask[achievement.ConProm], achievement.ConValue[index]);
                break; 	
        }
        return str;
    }
}
