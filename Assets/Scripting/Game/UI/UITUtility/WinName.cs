using UnityEngine;
using System.Collections;
public enum WinName
{
    None=-1,
    UIRoot = 0,
   
    AreaUIRoot = 10,
    Window_Lua = 20,


    Window_BallBattle = 110000,    //战斗界面  
    Window_CreatePartner,          //创建伙伴
    Window_NatureLevelUp,
    Window_Store,
    ////////////////////////////////////////////////////////////////////旧//////////////////////////////////////////////////////////////////
    //放于WindowRoot层...需要设置对应值，以防在删除的时候造成对应值改变
    Window_Login           = 1000,
    Window_CreateRole      = 1001,
    Window_ChooseRole      = 1002, //选择角色界面
    Window_CreatName       = 1003,
    Window_Passport        = 1004,  //通行证登录界面
    Window_ExitGame        = 1010, //退出游戏界面
    Window_Guide           = 1020, //新手引导界面
    Window_ServerNotice    = 1030, //公告
    Window_SpecMessageBox  = 1040, //特殊通知窗口
    Window_ResetPassword   = 1050,
    Window_ChooseServer    = 1051,

    WindowBig_RoleInfo          = 10020,
    Window_RoleMainInfo         = 10021,
    Window_RoleDetailInfo       = 10022,
    Window_ItemInventory        = 10023,    //道具背包
    Window_SellNumChoose        = 10024,
    Window_UseNumChoose         = 10025,

    Window_BattleResult         = 10030,
                                
    Window_ExploreStage         = 10040,

    WindowBig_WorldChoose       = 10050,
    Window_SiteHang             = 10051,    //地点挂机
    Window_ChooseWarDungeon     = 10053,    //试炼秘境  
    Window_SectList             = 10054,    //宗门列表
    Window_HangUpDetial         = 10055,    //挂机详情
    Window_HangEventDetial      = 10560,     //挂机事件详情
    Window_HangSiteEvents       = 10570,     //地区事件列表
    Window_HangInfo             = 10580,     

    WindowBig_Sect              = 10590,


    
    WindowBig_Assem             = 10060,    //装配  
    Window_AssemInfo            = 10061,    
    Window_BattleRecord         = 10062,    // 战绩查询
    //Window_AssemBook          = 10062,    //功法
    Window_AssemSpell           = 10063,    //招式
    Window_AssemEquip           = 10064,    //法宝
    //Window_AssemDetial        = 10065,    //装卸界面
    Window_AssemPet             = 10066,    //宠物装配界面 
    Window_AssemSpellMini       = 10067,
    Window_AssemEquipMini       = 10068,
    Window_AssemPetMini         = 10069,
    Window_PetLevelUp           = 10070,

    Window_SectMap              = 10080,    
    Window_SectArchit           = 10081,
    Window_PrestigeStore        = 10082,    //声望商店
    Window_VisitOtherSect       = 10083,   
    Window_SectNotice           = 10084,                                     
    WindowBig_SectActivity      = 10085,


    Window_Rule                 = 10090,    //法则


    WindowBig_Setting           = 10100,
    Window_HeadSetting          = 10101,
    Window_Mail                 = 10102, //邮箱
    Window_System               = 10103,
    Window_BindPhone            = 10104,

    WindowBig_Activity          = 10200,
    Window_PrestigeTask         = 10201,  //声望任务
    Window_DailyActivity        = 10210,  //每日活动
    Window_Tower                = 10220,  //爬塔
    Window_Arena                = 10221, // 竞技场

    //Window_PetDetial            = 10067,    //宠物详情
    //Window_SpellDetial          = 10068,    //功法详情
    //Window_EquipDetial          = 10069,    // 法宝详情
    //Window_SpellLearn           = 10070,    
                                            
    WindowBig_CaveChoose        = 100800,   //洞府
    Window_CaveInfo             = 100801,   //洞府信息
    Window_Zazen                = 100802,   //打坐
    Window_ZazenDetial          = 100803,   
    Window_Retreat              = 100804,   //闭关
    Window_RetreatDetial        = 100805,   
    Window_RetreatResult        = 100806,   //闭关结果
    //Window_SufferDetial         = 100806,   //渡劫
    Window_MedicineUse          = 100807,   //使用药品
    //Window_IncomeResult         = 100808,   //收益结算
    Window_Recipe               = 100809,   //生活技能
    Window_RecipeDetial         = 100810,   //制作界面
    Window_LevelLimit           = 100811,   // 服务器等级限制
    Window_CaveLevelUp          = 100812,
    Window_PropLevelUp          = 100813,
    
    WindowBig_Honor            = 100900,   //荣誉
    Window_Ranking             = 100901,   //排行
    //WindowBig_OtherRoleInfo     = 100902,                
    Window_OtherRoleInfo        = 100903,
    //Window_OtherAssemInfo       = 100904,
    Window_Achieve              = 100904,  //成就
    Window_AchieveReward         = 100905,
    
                            
    Window_PrepareSpell        = 110010,    //预设技能
                                            
    Window_DungeonMap          = 111000,    //秘境地图
    Window_NpcInteract         = 111001,    //秘境npc交互面板
    Window_MapEventList        = 111002,
    Window_MapEndShow          = 111003,    //秘境结束
    Window_DungeonMapEvent     = 111004,    //秘境地图任务
    Window_ObtainSpell         = 111005,    //技能宗门获取
    Window_ResourceMap         = 111010,    //资源圣地地图    
    Window_ResMapList          = 111015,    //资源圣地选择界面
    Window_PVPResourceMap      = 111020,    //宗门资源抢夺地图
    Window_PVPResMapInfo       = 111025,    //宗门资源抢夺活动信息
    Window_SpellLearn          = 111030,    //领悟技能界面
    Window_FairyMapPlayer      = 111035,    //仙界道友界面
    Window_FullScreenDialog    = 111040,    //全屏文本显示

    WindowBig_Store            = 112000,
    Window_Shop               = 112001,

    Window_VIP                = 113000,
    Window_Charge             = 113001,
    Window_BuyNumChoose        = 113002,
    Window_ExchangeGold         = 113003,
    Window_AndroidPay         = 113004,

    Window_Partner             = 114000,
    Window_Relationship        = 114001,
    Window_VisitPartner        = 114002,

    WindowBig_Race             = 115000,
    Window_RaceSect            = 115001,
    Window_RaceResult          = 115002,
    Window_RaceBreakout        = 115003,
    Window_RaceChampion        = 115004,
    Window_RaceEight           = 115005,
    Window_RaceGuess           = 115006,

    Window_Child               = 116000,
    Window_Soul                = 116001,
    Window_Format              = 116002,
    Window_SoulDevelop          = 116003,
    Window_ChildCompare         = 116004,
    Window_AssemRune            = 116005,
    Window_AssemChild           = 116006,
    Window_FormatLevelUp        = 116007,
    Window_FormationSoul        = 116008,
    Window_SoulBodyList         = 116009,
    Window_ChildInfo            = 116010,
    Window_ChildTravel          = 116011,


    WindowBig_Festivity         = 117000,
	Window_DailyLogin           =  117001,
	Window_SearchMap            =  117002,
    Window_ChildWorldBoss       = 117003,
    Window_FestivityPartner     = 117004,   
   
    Window_OtherChildInfo       = 118000,
    
	Window_AvatarStageOne 		=119000,
	Window_AvatarDetial	     = 119001,
    Window_AvatarRacePoint      = 119002,
    Window_AvatarRaceMatch      = 119003,
    Window_AvatarRaceReward     = 119004,

    Window_AvatarChampionWall   =119005,
    Window_AvatarRaceEight      = 119006,

    Window_Nimbus               =120001,


    Window_ChildTower           = 121000,
    Window_ChildTowerRank        = 121001,
    Window_ChildTowerList        = 121002,

    WindowLua_SiteHang = 11111,


    //放于TopWindowRoot层
    Window_Chat = 30010,

    Max = 999999
}


