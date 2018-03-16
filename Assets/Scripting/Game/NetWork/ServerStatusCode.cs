using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStatusCode
{
    private static readonly int GLOBAL_ERROR_CODE      = 940010000;
    private static readonly int GLOBAL_WARN_CODE       = 930010000;

    private static readonly int GATEWAY_ERROR_CODE 	  = 940020000;
    private static readonly int GATEWAY_WARN_CODE      = 930020000;

    private static readonly int LOGIN_ERROR_CODE       = 940030000;
    private static readonly int LOGIN_WARN_CODE        = 930030000;

    
    private static readonly int HERO_WARN_CODE         = 930040000;
    
    private static readonly int LOBBY_ERROR_CODE   	= 940050000;
    private static readonly int LOBBY_WARN_CODE    	= 930050000;

    private static readonly int BATTLE_ERROR_CODE  	= 940060000;
    private static readonly int BATTLE_WARN_CODE   	= 930060000;
    

    ////////////通用警告信息//////////////////////////////////////////

    public static readonly int GLOBAL_WARN_CODE_WAN_JIA_SHU_JU_YI_CHANG    = GLOBAL_WARN_CODE + 1;//玩家数据异常

    public static readonly int GLOBAL_WARN_CODE_DAO_JU_WEI_YONG_YOU        = GLOBAL_WARN_CODE + 2;//尚未拥有道具
    public static readonly int GLOBAL_WARN_CODE_DAO_JU_BU_ZU               = GLOBAL_WARN_CODE + 3;//道具数量不足
    public static readonly int GLOBAL_WARN_CODE_ZHUANG_BEI_WEI_YONG_YOU    = GLOBAL_WARN_CODE + 4;//尚未拥有装备   
    public static readonly int GLOBAL_WARN_CODE_CHONG_FU_CAO_ZUO           = GLOBAL_WARN_CODE + 5;//请勿重复操作
    public static readonly int GLOBAL_WARN_CODE_PEI_ZHI_CUO_WU             = GLOBAL_WARN_CODE + 6;//配置错误
    public static readonly int GLOBAL_WARN_CODE_CAO_ZUO_SHI_BAI            = GLOBAL_WARN_CODE + 7;//操作失败
    public static readonly int GLOBAL_WARN_CODE_JIN_BI_BU_ZU               = GLOBAL_WARN_CODE + 8;//金币不足
    public static readonly int GLOBAL_WARN_CODE_ZHUAN_SHI_BU_ZU            = GLOBAL_WARN_CODE + 9;//钻石不足
    public static readonly int GLOBAL_WARN_CODE_QIAN_NENG_BU_ZU            = GLOBAL_WARN_CODE + 10;//潜能不足
    public static readonly int GLOBAL_WARN_CODE_JING_YAN_BU_ZU             = GLOBAL_WARN_CODE + 11;//经验不足   
    public static readonly int GLOBAL_WARN_CODE_DENG_JI_BU_ZU              = GLOBAL_WARN_CODE + 12;//等级不足
    public static readonly int GLOBAL_WARN_CODE_JIN_BI_YI_MAN              = GLOBAL_WARN_CODE + 13;//金币已满
    public static readonly int GLOBAL_WARN_CODE_ZHUAN_SHI_YI_MAN           = GLOBAL_WARN_CODE + 14;//钻石已满
    public static readonly int GLOBAL_WARN_CODE_QIAN_NENG_YI_MAN           = GLOBAL_WARN_CODE + 15;//潜能已满
    public static readonly int GLOBAL_WARN_CODE_JING_YAN_YI_MAN            = GLOBAL_WARN_CODE + 16;//经验已满  
    public static readonly int GLOBAL_WARN_CODE_DENG_JI_YI_MAN             = GLOBAL_WARN_CODE + 17;//等级已满
    public static readonly int GLOBAL_WARN_CODE_SHENG_WANG_BU_ZU           = GLOBAL_WARN_CODE + 18;//声望不足
    
    
    public static readonly int GLOBAL_WARN_CODE_ZONG_MEN_BU_MAN_ZU         = GLOBAL_WARN_CODE + 18;//宗门不满足

    public static readonly int GLOBAL_WARN_CODE_SHENG_HUO_JI_NENG_BU_ZU    = GLOBAL_WARN_CODE + 19;//生活技能等级不足
    public static readonly int GLOBAL_WARN_CODE_YI_JING_LING_QU            = GLOBAL_WARN_CODE + 20;//已经领取过此奖励
    public static readonly int GLOBAL_WARN_CODE_MIAN_FEI_CI_SHU_YONG_WAN   = GLOBAL_WARN_CODE + 21;//免费次数已用完
    
    //////////////网关警告信息////////////////////////////////
    public static readonly int GATEWAY_WARN_CODE_FU_WU_QI_WEI_HU           = GATEWAY_WARN_CODE + 1;//服务器正在维护


    //////////////登陆服务器警告信息////////////////////////////////
    public static readonly int LOGIN_WARN_YONG_HU_BU_CUN_ZAI               = LOGIN_WARN_CODE + 1;//用户名不存在
    public static readonly int LOGIN_WARN_MI_MA_CUO_WU                     = LOGIN_WARN_CODE + 2;//密码错误
    public static readonly int LOGIN_WARN_SHU_JU_YI_CHANG                  = LOGIN_WARN_CODE + 3;//登录数据异常
    public static readonly int LOGIN_WARN_ZHANG_HAO_CHONG_FU_ZHU_CE        = LOGIN_WARN_CODE + 4;//账号重复注册

    /////////////////大厅/////////////////////////////////////
    public static readonly int LOBBY_WARN_BEI_BAO_YI_MAN                   = LOBBY_WARN_CODE + 1;//背包已满
    public static readonly int LOBBY_WARN_BI_GUAN_SHI_JIAN_WEI_DAO         = LOBBY_WARN_CODE + 2;//闭关时间未到
    public static readonly int LOBBY_WARN_QIAN_ZHI_GONG_FA_BU_MAN_ZU       = LOBBY_WARN_CODE + 3;//前置功法不满足
    public static readonly int LOBBY_WARN_WAN_CHENG_SHI_JIAN_SHI_BAI       = LOBBY_WARN_CODE + 4;//完成事件失败
    public static readonly int LOBBY_WARN_DONG_FU_DENG_JI_SHANG_XIAN       = LOBBY_WARN_CODE + 5;//洞府等级到上限
    public static readonly int LOBBY_WARN_SHENG_HUO_JI_NENG_SHU_LIAN_DU    = LOBBY_WARN_CODE + 6;//生活技能熟练度不足
    public static readonly int LOBBY_WARN_BU_NENG_XUE_XI_JI_NENG           = LOBBY_WARN_CODE + 7;//不能学习技能
    public static readonly int LOBBY_WARN_ZHUANG_BEI_BU_NENG_CHU_SHOU      = LOBBY_WARN_CODE + 8;//此装备不能出售
    public static readonly int LOBBY_WARN_JI_NENG_BU_NENG_CHU_SHOU         = LOBBY_WARN_CODE + 9;//此道具不能出售
    public static readonly int LOBBY_WARN_ZHENG_ZAI_ZHI_ZUO                = LOBBY_WARN_CODE + 10;//有物品正在制作中
    public static readonly int LOBBY_WARN_YI_YOU_ZONG_MEN                  = LOBBY_WARN_CODE + 11;//已经拜入宗门
    public static readonly int LOBBY_WARN_YI_YOU_JI_NENG                   = LOBBY_WARN_CODE + 12;//已有此技能
    public static readonly int LOBBY_WARN_LING_SHOU_YI_MAN                 = LOBBY_WARN_CODE + 13;//灵兽已经进化到最高级
    public static readonly int LOBBY_WARN_MEI_YOU_ZONG_MEN                 = LOBBY_WARN_CODE + 14;//你还没有加入宗门
    public static readonly int LOBBY_WARN_HUO_QU_WAN_JIA_SHI_BAI           = LOBBY_WARN_CODE + 15;//获取此玩家数据失败
    public static readonly int LOBBY_WARN_LING_QU_YOU_JIAN_SHI_BAI         = LOBBY_WARN_CODE + 16;//领取邮件失败
    public static readonly int LOBBY_WARN_SHI_JIAN_WEI_DAO                 = LOBBY_WARN_CODE + 17;//时间未到
    public static readonly int LOBBY_WARN_MEI_YOU_ZHENG_ZAI_JIN_XING       = LOBBY_WARN_CODE + 18;//没有正在进行的任务
    public static readonly int LOBBY_WARN_JIN_RI_CI_SHU_YONG_WAN              = LOBBY_WARN_CODE + 19;//任务次数已用完
    public static readonly int LOBBY_WARN_YOU_ZHENG_ZAI_JIN_XING           = LOBBY_WARN_CODE + 20;//有正在进行的任务
    public static readonly int LOBBY_WARN_GOU_MAI_CI_SHU_SHANG_XIAN        = LOBBY_WARN_CODE + 21;//购买商品次数已用完
    
    
    public static readonly int HERO_WARN_MING_ZI_GUO_CHANG                 = HERO_WARN_CODE + 1;//名字过长
    public static readonly int HERO_WARN_MING_ZI_FEI_FA                    = HERO_WARN_CODE + 2;//名字非法
    public static readonly int HERO_WARN_MING_ZI_CHONG_FU                  = HERO_WARN_CODE + 3;//名字重复
    public static readonly int HERO_WARN_TOU_XIANG_BU_CUN_ZAI              = HERO_WARN_CODE + 4;//头像不存在
    public static readonly int HERO_WARN_YING_XIONG_DENG_JI_SHANG_XIAN     = HERO_WARN_CODE + 5;//英雄等级达到上限


    ///////////////////////////////////////////严重错误提示///////////////////////////////////////////
    public static readonly int GLOBAL_ERROR_CODE_ZHANG_HAO_CHONG_FU_DENG_LU   = GLOBAL_ERROR_CODE + 1;//玩家数据异常
}

