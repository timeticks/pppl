using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConstUtils
{
    public static string NAME_BORDER = "Border";
    public static string NAME_RIGHT_BORDER = "RightBorder";
    public static string NAME_LEFT_BORDER = "LeftBorder";
    public static string NAME_TOP_BORDER = "TopBorder";
    public static string NAME_DOWN_BORDER = "DownBorder";

    public static string TAG_UNTAGGED = "Untagged";
    public static string TAG_BALL = "Ball";
    public static string TAG_CENTER_ANCHOR = "CenterAnchor";


    public static int module_travel { get { return GameConst.GetGameConst("module_travel"); } }
    public static int module_inventory { get { return GameConst.GetGameConst("module_inventory"); } }
    public static int module_cave { get { return GameConst.GetGameConst("module_cave"); } }
    public static int module_sect { get { return GameConst.GetGameConst("module_sect"); } }
    public static int module_dungeon_map { get { return GameConst.GetGameConst("module_dungeon_map"); } }
    public static int module_make_drug { get { return GameConst.GetGameConst("module_make_drug"); } }
    public static int module_rank { get { return GameConst.GetGameConst("module_rank"); } }
    public static int module_make_equip { get { return GameConst.GetGameConst("module_make_equip"); } }
    public static int module_pet_animal { get { return GameConst.GetGameConst("module_pet_animal"); } }
    public static int module_pet_puppet { get { return GameConst.GetGameConst("module_pet_puppet"); } }
    public static int module_pvp { get { return GameConst.GetGameConst("module_pvp"); } }
    public static int module_pet_ghost { get { return GameConst.GetGameConst("module_pet_ghost"); } }
    public static int module_tower { get { return GameConst.GetGameConst("module_tower"); } }
    public static int module_activity { get { return GameConst.GetGameConst("module_activity"); } }
    public static int module_shop { get { return GameConst.GetGameConst("module_shop"); } }



    ///////////////新//////////////
    public static int produce_origin_time { get { return GameConst.GetGameConst("produce_origin_time"); } }
    public static int produce_max_time { get { return GameConst.GetGameConst("produce_max_time"); } }






    public static int id_newer_map { get { return GameConst.GetGameConst("id_newer_map"); } }


    public static int max_battle_round { get { return GameConst.GetGameConst("max_battle_round"); } }
    public static int max_battle_hand_wait { get { return GameConst.GetGameConst("max_battle_hand_wait"); } }       //手操战斗每次选技能的时间
    public static int max_battle_hand_show_time { get { return GameConst.GetGameConst("max_battle_hand_show_time"); } }   //手操战斗每大回合的最大时间

    public static int max_prestige_task_free_fresh { get { return GameConst.GetGameConst("max_prestige_task_free_fresh"); } }
    public static int max_prestige_task_free_fresh_vip { get { return GameConst.GetGameConst("max_prestige_task_free_fresh_vip"); } }
    public static int max_prestige_task_num { get { return GameConst.GetGameConst("max_prestige_task_num"); } }
    public static int max_prestige_task_num_vip { get { return GameConst.GetGameConst("max_prestige_task_num_vip"); } }

    public static int id_shopMark { get { return GameConst.GetGameConst("id_shopMark"); } }
    public static int id_shopMall { get { return GameConst.GetGameConst("id_shopMall"); } }

    public static int max_tower_floor { get { return GameConst.GetGameConst("max_tower_floor"); } }
    public static int max_tower_fail_num_vip { get { return GameConst.GetGameConst("max_tower_fail_num_vip"); } }
    public static int max_tower_fail_num { get { return GameConst.GetGameConst("max_tower_fail_num"); } }

    public static readonly int UseDiamondNum = 1;

    public static int id_idle_spell { get { return 1101000011; } }
    public static int id_normal_attack_spell { get { return 1101000010; } }

    public static int MapEventFinishStatus { get { return 9; }}  //事件完成的值

    public static int inventory_badge_event { get { return 1005000122; } } //当完成此事件，开启背包红点
    public static int InventoryBadgeItem { get { return 1203030040; } } //背包红点物品

    public const string MonsterIcon = "Npc_tou_5";


    public static int max_equip_level { get { return GameConst.GetGameConst("max_equip_level"); } }

    public static int max_level { get { return GameConst.GetGameConst("max_level"); } }
    public static int max_gold { get { return GameConst.GetGameConst("max_gold"); } }








    public static int num_certain_partner_dialogue_pct { get { return 10000; } }//伙伴某对话肯定出现的权重值

    public static int[] array_intimacy_level { get { return GameConst.GetGameConstArray("array_intimacy_level"); } }









    public static int GetNewModuleUnlockLevel(ModuleType ty)
    {
        return GameConst.GetGameConst(ty.ToString()); 
    }
}
