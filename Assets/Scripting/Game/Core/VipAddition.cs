using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VipAddition 
{
    MAX_PRESTIGE_TASK_NUM,
	MAX_PRESTIGE_TASK_FREE_FRESH,
	MAX_TOWER_FAIL_NUM,
}

public static class VipAdditionExt
{
    public static int getValueByVip(this VipAddition ty ,  bool isVip)
    {
        switch (ty)
        {
            case VipAddition.MAX_PRESTIGE_TASK_NUM:
                return isVip ? GameConstUtils.max_prestige_task_num : GameConstUtils.max_prestige_task_num_vip;
            case VipAddition.MAX_PRESTIGE_TASK_FREE_FRESH:
                return isVip ? GameConstUtils.max_prestige_task_free_fresh : GameConstUtils.max_prestige_task_free_fresh_vip;
            case VipAddition.MAX_TOWER_FAIL_NUM:
                return isVip ? GameConstUtils.max_tower_fail_num : GameConstUtils.max_tower_fail_num_vip;
        }
        return 0;
    }

}
