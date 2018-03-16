
public enum NetCode_S : short
{

    SnapshotTime          = 0,
    SnapshotRole          = 1,   //同步角色信息
                          
    VerifyLogin           = 2,
    Login                 = 3,
    CreateRole            = 4,
    EnterGame             = 5,
                          
    SnapshotInventory     = 6,     //登陆同步背包
    SnapshotSpells        = 7,
    SnapshotEquips        , 
    SnapshotItem          ,
    EquipSpell            , 
    StudySpell            ,
    EnterPVE              ,
                          
    EquipItem             ,
    StudyItem             ,
    EquipEquip            ,  //穿戴装备
                          
    SellItem              ,
    UseItem               ,
                          
    SnapshotDungeonMap    ,
    EnterDungeonMap       ,
    CheckCondition        ,           //检查条件
    MapSave               ,           //秘境进度保存
    MapEnd                ,           //秘境结束
                          
    SetSect               ,
    SnapshotSect          ,
    SpellObtain           ,
    SnapshotSpellObtain   ,

    TravelStart           , //开始挂机  
    SnapshotTravelBotting , //同步正在挂机信息
    SnapshotTravelEvent   ,
    FinishTravelEvent     ,
    SnapshotAuxSkill,     //同步生活技能
    SnapshotAddEquip,
    SnapshotAddRecipe,
    SnapshotExp,

    Produce,
    SnapShotProduceBotting,
    SnapShotProduce,
    ProduceAccelerate,
    FinishProduce,//领取生产物品
    AuxSkillLevelUp,

    ZazenStart,
    ZazenFinish,   
    RetreatStart,   //43
    RetreatFinish,
    SnapshotPlayerAttribute,
    CaveLevelUp,//升级洞府
    SnapshotPlayerAttributeLong,
    SellEquip,
    PullInfo,
    SnapshotTravelHistory,

    SnapshotOffLine,

    PetLevelUp,
    SnapshotPet,  //53
    EquipPet,
    SnapshotEquipPet,
    ////////////////////////

    SnapshotMapEvent,
    MapEventReward,
    
    SnapshotEnterBotting,
    SetPlayerName,
    SnapshotPlayerName,
    
    PullRank,

    BattleLog,    //手操返回
    BattleReport,  //战斗结果

    AuxSkillToolLevelUp,

    GetLoot,
    PullOtherRoleInfo,

    ////////////////////////

    ReceiveMail,
    SnapshotMail,
    MailDetail,

    ShopInfo,
    BuyProduct,

    FreshPrestigeTask,       //刷新声望任务
    StartPrestigeTask,
    FinishPrestigeTask,
    SnapshotPrestigeTask,
    SnapshotPrestigeTaskDetail,


    SnapshotShopInfo,
    SnapshotBuyShopInfo,

    

    SnapshotAchieve,
    SnapshotAchieveFinish,
    GetAchieveReward,
    /////////////////////
    BuyVIP,
    GetVIPDailyAward,
   

    SnapshotBattleRecord,

    RemoveSaveRole,

    ExpandStore,

    Register,

    FastRegister,
   
}
