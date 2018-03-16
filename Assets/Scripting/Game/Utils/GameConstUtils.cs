using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstUtils
{
    //public static int module_travel { get { return GameConst.GetGameConst("module_travel"); } }
    //public static int module_inventory { get { return GameConst.GetGameConst("module_inventory"); } }
    //public static int module_cave { get { return GameConst.GetGameConst("module_cave"); } }
    //public static int module_sect { get { return GameConst.GetGameConst("module_sect"); } }
    //public static int module_make_drug { get { return GameConst.GetGameConst("module_make_drug"); } }
    //public static int module_make_mine { get { return GameConst.GetGameConst("module_make_mine"); } }
    //public static int module_make_herb { get { return GameConst.GetGameConst("module_make_herb"); } }
    //public static int module_make_rune { get { return GameConst.GetGameConst("module_make_rune"); } }
    //public static int module_rank { get { return GameConst.GetGameConst("module_rank"); } }
    //public static int module_make_equip { get { return GameConst.GetGameConst("module_make_equip"); } }
    //public static int module_activity_pvp { get { return GameConst.GetGameConst("module_activity_pvp"); } }
    //public static int module_tower { get { return GameConst.GetGameConst("module_tower"); } }

    //public static int module_chat { get { return GameConst.GetGameConst("module_chat"); } } 
    //public static int module_race { get { return GameConst.GetGameConst("module_race"); } }
    //public static long time_race_min_start { get { return GameConst.GetGameConstLong("time_race_min_start"); } } // 精英争霸赛距离开服开启时间
    //public static int module_activity { get { return GameConst.GetGameConst("module_activity"); } }
    //public static int module_prestige_task { get { return GameConst.GetGameConst("module_prestige_task"); } }
    //public static int module_partner { get { return GameConst.GetGameConst("module_partner"); } }
    //public static int module_res_map { get { return GameConst.GetGameConst("module_res_map"); } }
    //public static int module_pvp_res_map { get { return GameConst.GetGameConst("module_pvp_res_map"); } }
    //public static int module_rule { get { return GameConst.GetGameConst("module_rule"); } }
    //public static int module_spell_learn { get { return GameConst.GetGameConst("module_spell_learn"); } }
    //public static int module_fairy_map { get { return GameConst.GetGameConst("module_fairy_map"); } }


    //public static int num_eight_race_daily_battle { get { return GameConst.GetGameConst("num_eight_race_daily_battle"); } }

    //public static int id_newer_map { get { return GameConst.GetGameConst("id_newer_map"); } }

    //public static int max_level { get { return GameConst.GetGameConst("max_level"); } }
    //public static int max_prestige_level { get { return GameConst.GetGameConst("max_prestige_level"); } }
    //public static int max_auxskill_level { get { return GameConst.GetGameConst("max_auxskill_level"); } }
    //public static int max_inventory_num { get { return GameConst.GetGameConst("max_inventory_num"); } }
    //public static int max_gold { get { return GameConst.GetGameConst("max_gold"); } }
    //public static int max_potential { get { return GameConst.GetGameConst("max_potential"); } }
    //public static long time_rebirth_cool { get { return GameConst.GetGameConstLong("time_rebirth_cool"); } }
    //public static int max_battle_save_count { get { return GameConst.GetGameConst("max_battle_save_count"); } }

    //public static int num_vip_give_diamond { get { return GameConst.GetGameConst("num_vip_give_diamond"); } }//买vip赠送仙玉
    //public static int num_vip_daily_diamond { get { return GameConst.GetGameConst("num_vip_daily_diamond"); } }//vip每日仙玉
    //public static int num_vip_total_diamond  // 购买vip累计赠送
    //{
    //    get { return num_vip_give_diamond + num_vip_daily_diamond * 30; }
    //}
    //public static int num_vip_income_addtion { get { return GameConst.GetGameConst("num_vip_income_addtion"); } } // vip收益加成
    //public static int num_child_travel_diamond_income_addtion { get { return GameConst.GetGameConst("num_child_travel_diamond_income_addtion"); } } // 盟友游历花费仙玉加成
    
    //public static int max_can_buy_vip { get { return GameConst.GetGameConst("max_can_buy_vip"); } }//vip每日仙玉

    //public static int max_battle_round { get { return GameConst.GetGameConst("max_battle_round"); } }
    //public static int max_battle_hand_wait { get { return GameConst.GetGameConst("max_battle_hand_wait"); } }       //手操战斗每次选技能的时间
    //public static int max_battle_hand_show_time { get { return GameConst.GetGameConst("max_battle_hand_show_time"); } }   //手操战斗每大回合的最大时间

    //public static int max_prestige_task_free_fresh { get { return GameConst.GetGameConst("max_prestige_task_free_fresh"); } }
    //public static int max_prestige_task_free_fresh_vip { get { return GameConst.GetGameConst("max_prestige_task_free_fresh_vip"); } }
    //public static int max_prestige_task_num { get { return GameConst.GetGameConst("max_prestige_task_num"); } } // 声望任务最大接取次数
    //public static int max_prestige_task_num_vip { get { return GameConst.GetGameConst("max_prestige_task_num_vip"); } }

    //public static int max_child_develop_step { get { return GameConst.GetGameConst("max_child_develop_step"); } }

    //public static int max_assem_child = 10;
    //public static int max_assem_rune = 10;

    //public static int module_format { get { return GameConst.GetGameConst("module_format"); } }

    //public static int time_breakout_race_battle_start { get { return GameConst.GetGameConst("time_breakout_race_battle_start"); } }
    //public static int max_zazen_time { get { return GameConst.GetGameConst("max_zazen_time"); } }
    //public static int id_shopMark { get { return GameConst.GetGameConst("id_shopMark"); } }
    //public static int id_shopMall { get { return GameConst.GetGameConst("id_shopMall"); } }

    //public static int num_battle_up_speed_round { get { return GameConst.GetGameConst("num_battle_up_speed_round"); } }

    //public static float num_battle_up_speed
    //{
    //    get
    //    {
    //        int val = GameConst.GetGameConst("num_battle_up_speed");
    //        return val == 0 ? 2 : val.ToFloat_10000();
    //    }
    //}

    //public static int num_check_accelerate { get { return GameConst.GetGameConst("num_check_accelerate"); } }


    //宗门比斗最大免费挑战次数
    //public static int max_sect_race_challenge_num { get { return GameConst.GetGameConst("max_sect_race_challenge_num"); } }//最大宗门比斗挑战次数
    //宗门比斗最大总挑战次数
    //public static int max_sect_race_challenge_cost_buy { get { return GameConst.GetGameConst("max_sect_race_challenge_cost_buy"); } }
    //public static int num_breakout_guess_cost { get { return GameConst.GetGameConst("num_breakout_guess_cost"); } } // 精英争霸预测出线消耗仙玉
    //public static int num_eight_guess_cost { get { return GameConst.GetGameConst("num_eight_guess_cost"); } } // 精英争霸8强预测出线消耗仙玉

    //public static int num_breakout_guess_reward { get { return GameConst.GetGameConst("num_breakout_guess_reward"); } } // 精英争霸突围赛预测奖励
    //public static int num_eight_guess_reward { get { return GameConst.GetGameConst("num_eight_guess_reward"); } } // 精英争霸8强预测奖励

    //public static int max_tower_floor { get { return GameConst.GetGameConst("max_tower_floor"); } }
    //public static int max_tower_fail_num_vip { get { return GameConst.GetGameConst("max_tower_fail_num_vip"); } }
    //public static int max_tower_fail_num { get { return GameConst.GetGameConst("max_tower_fail_num"); } }
    //public static int max_pvp_challenge_num { get { return GameConst.GetGameConst("max_pvp_challenge_num"); } } // #擂台最大挑战次数
    //public static int max_pvp_reward_win_num { get { return GameConst.GetGameConst("max_pvp_reward_win_num"); } }
    //public static int max_pvp_fresh_cost { get { return GameConst.GetGameConst("max_pvp_fresh_cost"); } }
    //public static int num_accelerate_time { get { return GameConst.GetGameConst("num_accelerate_time"); } }
    //public static int num_sect { get { return GameConst.GetGameConst("num_sect"); } }
    //public static int num_accelerate_max_diamond { get { return GameConst.GetGameConst("num_accelerate_max_diamond"); } }
    //public static int num_mail_duetime { get { return GameConst.GetGameConst("num_mail_duetime"); } }
    //public static int num_connect_out_time { get { return Mathf.Max(TUtility.ONE_SECOND, GameConst.GetGameConst("num_connect_out_time")); } }
    //public static int max_soul_proficiency {get{return GameConst.GetGameConst("max_soul_proficiency");}}

    //public static int num_rank_level_1 { get { return GameConst.GetGameConst("num_rank_level_1"); } }

    //public static int max_play_ads_num { get { return GameConst.GetGameConst("max_play_ads_num"); } }
    //public static int time_direct_reward { get { return GameConst.GetGameConst("time_direct_reward"); } }
    //public static int time_ads_min_duration { get { return GameConst.GetGameConst("time_ads_min_duration"); } }
    //public static int advert_play_flag {  //广告可播放二进制 1为可播放
    //    get
    //    {
    //        int val = GameConst.GetGameConst("advert_play_flag");
    //        return val == 0 ? int.MaxValue : val;
    //    } 
    //}
    //public static int ads_poly_pct
    //{  //广告可播放二进制 1为可播放
    //    get
    //    {
    //        int val = GameConst.GetGameConst("ads_poly_pct",-1);
    //        return val == -1 ? 5000 : val;
    //    }
    //}

    //public static int max_soulbody_num = 5; //{ get { return GameConst.GetGameConst("max_soulbody_num"); } }
    //public static bool wechat_pay_open { get { return GameConst.GetGameConst("wechat_pay_open") == 1; } }

    //public static bool passport_find_account {get { return GameConst.GetGameConst("passport_find_account") == 1; } }

    //public static bool bool_open_charge { get { return GameConst.GetGameConst("bool_open_charge")==1; } }
    //public static int num_default_score_visit { get { return GameConst.GetGameConst("num_default_score_visit"); } } // 仙侣问好再见增加积分
    //public static int num_visit_recover_time { get { return GameConst.GetGameConst("num_visit_recover_time"); } }//自动恢复一次所需时间
    //public static int max_recover_visit_num { get { return GameConst.GetGameConst("max_recover_visit_num"); } }//低于该数量以后开始自动增加拜访数
    //public static int num_min_score_visit { get { return GameConst.GetGameConst("num_min_score_visit"); } }// 随机到未解锁仙侣增加的积分

    //public static int num_cost_prestige_fresh { get { return GameConst.GetGameConst("num_cost_prestige_fresh"); } }

    //争霸赛
    //public static int num_breakout_race_daily_battle { get { return GameConst.GetGameConst("num_breakout_race_daily_battle"); } }
    //public static int[] array_sect_race_challenge_cost{ get { return GameConst.GetGameConstArray("array_sect_race_challenge_cost"); } }
    //public static int[] array_sect_race_active_hour_time{ get { return GameConst.GetGameConstArray("array_sect_race_active_hour_time"); } } // 宗门比斗可挑战时段

    //圣地
    //public static int[] array_res_map_cost { get { return GameConst.GetGameConstArray("array_res_map_cost"); } } //单人圣地进入次数的花费（第一次花费|第二次花费）
    //public static int num_res_map_enter { get { return GameConst.GetGameConst("num_res_map_enter"); } } //单人圣地进入次数
    //public static int[] array_res_map_action_cost { get { return GameConst.GetGameConstArray("array_res_map_action_cost"); } } //单人圣地行动力的花费
    //public static int num_res_map_action { get { return GameConst.GetGameConst("num_res_map_action"); } } //单人圣地行动力
    //public static int[] array_pvp_res_map_monster { get { return GameConst.GetGameConstArray("array_pvp_res_map_monster"); } }

    //public static int num_child_prom_start { get { return GameConst.GetGameConst("num_child_prom_start"); } }
    //public static int num_pvp_res_map_enter { get { return GameConst.GetGameConst("num_pvp_res_map_enter"); } } //多人圣地进入次数
    //public static int[] array_pvp_res_map_cost { get { return GameConst.GetGameConstArray("array_pvp_res_map_cost"); } } //多人圣地进入次数的花费（第一次花费|第二次花费）
    //public static int num_pvp_res_map_fail { get { return GameConst.GetGameConst("num_pvp_res_map_fail"); } } //多人圣地免费失败次数
    //public static int num_pvp_res_map_fail_cost { get { return GameConst.GetGameConst("num_pvp_res_map_fail_cost"); } } //购买失败次数的花费
    //public static int num_pvp_res_map_start_point { get { return GameConst.GetGameConst("num_pvp_res_map_start_point"); } } //购买失败次数的花费


    //public static int[] array_potential_child_develop_cost { get { return GameConst.GetGameConstArray("array_potential_child_develop_cost"); } }//后人花费潜能增加培养，没有次数上限
    //public static int[] array_diamond_child_develop_cost { get { return GameConst.GetGameConstArray("array_diamond_child_develop_cost"); } }

    //public static int num_soul_body_discard_cost { get { return GameConst.GetGameConst("num_soul_body_discard_cost"); } }

    //public static int time_pvp_res_map_start { get { return GameConst.GetGameConst("time_pvp_res_map_start"); } } //圣地开放时间，每周一零点之后多久162000000
    //public static int time_pvp_res_map_end { get { return GameConst.GetGameConst("time_pvp_res_map_end"); } } //圣地结束时间，每周一零点之后多久421200000

    //public static int time_sect_chat_cd { get { return GameConst.GetGameConst("time_sect_chat_cd"); } } // 宗门聊天冷却时间

    //public static int time_common_chat_cd { get { return GameConst.GetGameConst("time_common_chat_cd"); } } // 聊天通用最小

    //public static int[] array_exchange_gold { get { return GameConst.GetGameConstArray("array_exchange_gold"); } }

    //public static int max_partner_score = 100;

    //public static int id_idle_spell { get { return 1101000011; } }
    //public static int id_normal_attack_spell { get { return 1101000010; } }
               
    //public static int id_race_sect_rank_title { get { return GameConst.GetGameConst("id_race_sect_rank_title"); } } // #精英弟子称号          
    //public static int id_race_sect_first_title { get { return GameConst.GetGameConst("id_race_sect_first_title"); } } // #首席弟子称号id         
    //public static int id_race_eight_title { get { return GameConst.GetGameConst("id_race_eight_title"); } } // #八强称号
    //public static int id_race_champion_title { get { return GameConst.GetGameConst("id_race_champion_title"); } } // #天下第一称号

    //public static int id_sect_race_default_hero { get { return GameConst.GetGameConst("id_sect_race_default_hero"); } } 
    //public static int MapEventFinishStatus { get { return 9; }}  //事件完成的值

    //public static int inventory_badge_event { get { return 1005000122; } } //当完成此事件，开启背包红点
    //public static int InventoryBadgeItem { get { return 1203030040; } } //背包红点物品

    //public static int time_rule_pos_unlock { get { return GameConst.GetGameConst("time_rule_pos_unlock"); } }     //法则消耗时间
    //public static int time_spell_learn { get { return GameConst.GetGameConst("time_spell_learn"); } }             //技能领悟消耗时间
    //public static int time_spell_learn_vip { get { return GameConst.GetGameConst("time_spell_learn_vip"); } }     //技能领悟消耗时间，vip

    //public static int num_fairy_age_one_day { get { return GameConst.GetGameConst("num_fairy_age_one_day"); } }     //仙界的年龄，一天加多少年

    //public static int num_free_child_obtain_spell { get { return GameConst.GetGameConst("num_free_child_obtain_spell"); } }     //每日后人技能免费领悟次数
    //public static int num_diamond_child_obtain_spell_cost { get { return GameConst.GetGameConst("num_diamond_child_obtain_spell_cost"); } }     //后人领悟一次技能花费仙玉
    //public static int num_child_high_spell_level { get { return GameConst.GetGameConst("num_child_high_spell_level"); } }     //后人领悟高级技能等级
    //public static int num_diamond_discard_child_spell_cost { get { return GameConst.GetGameConst("num_diamond_discard_child_spell_cost"); } }     //遗忘后人技能花费仙玉
    //public static int num_diamond_child_travel_cost { get { return GameConst.GetGameConst("num_diamond_child_travel_cost"); } }     //盟友仙玉游历花费仙玉
    //public static int num_time_child_travel_cost { get { return GameConst.GetGameConst("num_time_child_travel_cost"); } }     //盟友游历时长
   
    //public static int time_child_travel_can_finish { get { return GameConst.GetGameConst("time_child_travel_can_finish"); } }     //盟友可结束游历时间

    //public const string MonsterIcon = "Head_00";
    //public static int time_passport_time_out { get { return GameConst.GetGameConst("time_passport_time_out"); } }     //通行证登录(秒)

}
