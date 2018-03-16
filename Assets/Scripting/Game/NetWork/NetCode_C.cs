using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NetCode_C : short
{
    
    SnapshotTime = 0,
    VerifyLogin  = 1,
    Login,
    CreateRole,    
    EnterGame,     //

    EquipSpell, //装备招式
    StudySpell,

    EquipItem,
    StudyItem,

    EnterPVE,
    EquipEquip,    //穿卸装备

    SellItem,   //出售道具
    UseItem,

    EnterDungeonMap,   //进入秘境
    CheckCondition,     //得到条件结果
    MapSave,            //秘境数据保存
    MapEnd,

    Produce,
    ProduceAccelerate,
    FinishProduce,
    AuxSkillLevelUp,

    SetSect,         //拜入宗门
    SpellObtain,     //获得技能


    TravelStart, //开始挂机
    FinishTravelEvent,
    SetPlayerName,
    PullRank,
    PullOtherRoleInfo,  

    SellEquip,
    PullInfo,
    PetLevelUp,
    EuqipPet,
    ZazenStart,
    ZazenFinish,
    RetreatStart,
    RetreatFinish,
    CaveLevelUp, 

    PullMapEvent,
    MapEventReward,    
 
    EnterPVP,
    EquipStrength,
    GiveUp,
    PetSell,
    ExpandStore,
    BattleHand,
    RemoveSaveRole,
    AuxSkillToolLevelUp,

    ReceiveMail,
    MailDetail,
    ShopInfo,
    BuyProduct,

    FreshPrestigeTask,
    StartPrestigeTask,
    FinishPrestigeTask,

    GetAchieveReward,

 
    //////////////////////////
    BuyVIP,
    GetVIPDailyAward,

    Register,
    FastRegister,     //一键注册


}
