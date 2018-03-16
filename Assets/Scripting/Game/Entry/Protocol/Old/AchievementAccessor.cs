using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class AchievementAccessor : DescObject {


    public int SecondLastFinish;  //上一个最近完成的成就
    public int SecondLastFinishPos;
    public int LastFinish;  //最近完成的成就
    public int LastFinishPos; //当前进度

    public int ZazenTime; //打坐结束结算
    public int AddGold;
    public int ConsumeGold;
    public int ConsumeDiamond;
    public int AdvertNum; //
    public int MarketPurchase;//
    public int ProduceDrug; // 次数
    public int GetMine;
    public int GetHerb;
    public int LevelUpPet;
    public int EvolvePet; 

    

    public int[] ProduceEquip;//品质 ,绿蓝紫橙
    public int[] FinishPrestigeTask;//类型 

    public Dictionary<int,int> AchievementInfo = new Dictionary<int,int>();

    public List<int> GotAchieveAwardList = new List<int>(); // 已领取的成就奖励

    public int AchievePoint;
    //////////////////////

    public int[] AchieveProgress = new int[(int)Achievement.AchieveType.Max];

    public static bool IsInitAccessor = false;

    public AchievementAccessor() { }
    public AchievementAccessor(AchievementAccessor origin)
    {
       this.SecondLastFinish = origin.SecondLastFinish;
       this.SecondLastFinishPos = origin.SecondLastFinishPos;
       this.LastFinish = origin.LastFinish;
       this.LastFinishPos = origin.LastFinishPos; 
       this.ZazenTime = origin.ZazenTime;    
       this.AddGold = origin.AddGold;       
       this.ConsumeGold = origin.ConsumeGold;     
       this.ConsumeDiamond = origin.ConsumeDiamond; 
       this.AdvertNum = origin.AdvertNum;   
       this.MarketPurchase = origin.MarketPurchase; 
       this.ProduceDrug = origin.ProduceDrug;   
       this.GetMine = origin.GetMine;   
       this.GetHerb = origin.GetHerb;    
       this.LevelUpPet = origin.LevelUpPet;
       this.EvolvePet = origin.EvolvePet;

       this.ProduceEquip = origin.ProduceEquip;//品质
       this.FinishPrestigeTask = origin.FinishPrestigeTask;//类型
       this.AchievementInfo = origin.AchievementInfo;
       this.GotAchieveAwardList = origin.GotAchieveAwardList; // 已领取的成就奖励
       this.AchievePoint = origin.AchievePoint;
       this.AchieveProgress = origin.AchieveProgress;
    }


    public enum ClientCountType
    {
         ZazenTime, //打坐结束结算
         AddGold,
         ConsumeGold,
         ConsumeDiamond,
         AdvertNum, //
         MarketPurchase,//
         ProduceDrug, // 次数
         GetMine,
         GetHerb,
         LevelUpPet,
         EvolvePet, 
         ProduceEquip,//品质
         FinishPrestigeTask,
    }

    public void InitAccessor(NetPacket.S2C_SnapshotAchieve msg)
    {

        SecondLastFinish = msg.LastFinish0;
        SecondLastFinishPos = msg.LastFinishPos0;
        LastFinish = msg.LastFinish1;
        LastFinishPos = msg.LastFinishPos1;

        ZazenTime = msg.ZazenTime;
        AddGold = msg.AddGold;
        ConsumeGold = msg.ConsumeGold;
        ConsumeDiamond = msg.ConsumeDiamond;
        AdvertNum = msg.AdvertNum;
        MarketPurchase = msg.MarketPurchase;
        ProduceDrug = msg.ProduceDrug;
        GetMine = msg.GetMine;
        GetHerb = msg.GetHerb;
        LevelUpPet = msg.LevelUpPet;
        EvolvePet = msg.EvolvePet;
        ProduceEquip = msg.ProduceEquip;
        FinishPrestigeTask = msg.FinishPrestigeTask;
        AchievementInfo = msg.AchieveMap;

        AchievePoint = msg.AchievePoint;

        foreach (var item in AchievementInfo)
        {         
            Achievement ach = Achievement.AchievementFetcher.GetAchievementByCopy(item.Key);
            AchieveProgress[(int)Achievement.AchieveType.None] += item.Value;
            AchieveProgress[(int)ach.MyAchieveType] += item.Value;
        }
        IsInitAccessor = true;
    }

    public void FreshAccessor(NetPacket.S2C_SnapshotAchieveFinish msg)
    {
         AchievePoint = msg.AchievePoint;
         SecondLastFinish = msg.SecondLastFinish; 
         SecondLastFinishPos = msg.SecondLastFinishPos;
         LastFinish = msg.LastFinish;  
         LastFinishPos = msg.LastFinishPos;

         Achievement achievement = Achievement.AchievementFetcher.GetAchievementByCopy(LastFinish);
         int addNum = 0;
         if (AchievementInfo.ContainsKey(LastFinish))
             addNum = LastFinishPos - AchievementInfo[LastFinish];
         else
             addNum = LastFinishPos;
         //统计数量
         AchieveProgress[(int)Achievement.AchieveType.None] += addNum;
         AchieveProgress[(int)achievement.MyAchieveType] += addNum;

         if (AchievementInfo.ContainsKey(SecondLastFinish))
             AchievementInfo[SecondLastFinish] = SecondLastFinishPos;
         else
             AchievementInfo.Add(SecondLastFinish,SecondLastFinishPos);

         if (AchievementInfo.ContainsKey(LastFinish))
             AchievementInfo[LastFinish] = LastFinishPos;
         else
             AchievementInfo.Add(LastFinish, LastFinishPos);

         //成就解锁提示
         for (int i = 0,length = msg.FinishPosList.Count ; i < length; i++)
         {
             if(UIRootMgr.LobbyUI!=null)
             {
                 UIRootMgr.LobbyUI.AppendAchieveTips(new Achieve(achievement, msg.FinishPosList[i], true));
             }
          //   UIRootMgr.Instance.Window_UpTips.AddAchieveTips(new Achieve(achievement, msg.FinishPosList[i], true));
         }
    }

    public void FreshZazenTime()
    {
 
    }
    

}
