using System.Collections;
using System.Collections.Generic;
using Tsorgy;
using UnityEngine;

/**
 * Created by Catyuki on 2017/2/3.
 */
public class GameUtils
{
    public static System.Random rand = new System.Random();

    //根据万分比概率数组，得到index
    public static int GetRandomIndex(Eint[] prob)
    {
        long total = 0;
        for (int i = 0; i < prob.Length; i++)
        {
            total += prob[i];
        }

        long randomPoint = (long)(rand.NextDouble() * total);
        for (int i = 0; i < prob.Length; i++)
        {
            if (randomPoint < prob[i])
                return i;
            else
                randomPoint -= prob[i];
        }
        return prob.Length - 1;
    }
    public static int GetRandomIndex(int[] prob) 
    {
        long total = 0;
        for (int i = 0; i < prob.Length; i++)
        {
            total += prob[i];
        }

        long randomPoint = (long)(rand.NextDouble() * total);
        for (int i = 0; i < prob.Length; i++) {
            if (randomPoint < prob[i])
                return i;
            else
                randomPoint -= prob[i];
        }
        return prob.Length - 1;
    }

    public static int GetRandom(int min , int max)
    {
        return rand.Next(min, max);
    }

    /// <summary>
    /// 将总数根据每个元素代表的值，得出每个元素所需要的数量
    /// 如amount=1000   indexValue={10,110}
    /// 此时要凑够1000,需要最少元素为9个110，1个10。那么就返回{1,9}
    /// 值越大的数在后面
    /// </summary>
    public static int[] GetIndexNumByValue(int amount, int[] indexValue)//将总数根据每个元素代表的值，得出元素数量最少下，每个元素所需要的数量
    {

        int[] indexNum = new int[indexValue.Length];//长度与indexValue相同,全为零
        for (int i = indexValue.Length - 1; i >= 0; i--)
        {
            if (indexValue[i] <= 0) continue;
            int getNum = amount / indexValue[i];
            if (getNum >= 1)
            {
                amount -= getNum * indexValue[i];
                indexNum[i] = getNum;
            }
        }
        return indexNum;
    }
    /// <summary>
    /// 传入万分比
    /// </summary>
    public static bool isTrue(int prop)
    {
        return prop > rand.Next(0, 10000);
    }


    //根据限制条件，在范围内随机
    public static int GetRandomByLimit( float num , int minPct , int maxPct , int minAdd,int maxAdd)
    {
        num = num * GameUtils.GetRandom(minPct, maxPct).ToFloat_10000();
        num += GameUtils.GetRandom(minAdd, maxAdd);
        num = Mathf.Clamp(num, 1, int.MaxValue);
        return (int)num;
    }

    //得到掉落的金币数
    public static int GetDropGold(int level, int quality)
    {
        float num = Mathf.Sqrt(level) * (1 + level * 0.2f) * Mathf.Sqrt(quality);
        num = GetRandomByLimit(num, 7000, 12000, -4, 4);
        return (int)num;
    }


    #region 战斗公式
    
    #endregion

    /**
         * 验证玩家名字是否合法
         * @param name  验证名字是否合法
         * @return      验证否通过
         */
    public static bool validatePlayerName(string name)
    {
        //string regex = "([a-z]|[A-Z]|[0-9]|[\\u4e00-\\u9fa5])+";
        //Pattern pattern = Pattern.compile(regex);
        //return pattern.matcher(name).matches();
        return true;
    }

    /**
     * 计算命中率
     * @param hit   攻方Hit
     * @param dodge 守方Dodge
     * @return
     */
    public static int hitProp(int hit, int dodge)
    {
        float hitProp = Mathf.Abs((float)(hit - dodge) / (float)10000);
        hitProp = Mathf.Max(hitProp, 0.3f);//最低命中率30%
        hitProp = Mathf.Min(hitProp, 1f);//最高命中率100%
        return Mathf.RoundToInt(hitProp * 10000);
    }

    
    /**
     * 计算招架概率
     * @param broken    攻方broken
     * @param block     守方block
     * @return=
     */
    public static int brokenProp(int broken, int block)
    {
        float brokenProp = (float)(broken - block) / 10000f;
        brokenProp = Mathf.Max(brokenProp, 0.3f);//最低招架率30%
        brokenProp = Mathf.Min(brokenProp, 1f);//最大招架率100%
        return Mathf.RoundToInt(brokenProp * 10000);
    }


    /**
     * 计算属性系数
     * @param typeDmgInc    对应类型伤害增伤
     * @param typeDmgDec    对应类型伤害减伤
     * @return
     */
    public static float attributeProp(int typeDmgInc, int typeDmgDec)
    {
        float attributeProp = 1f + (float)typeDmgInc / 10000f - (float)typeDmgDec / 10000f;
        attributeProp = Mathf.Max(attributeProp, 0.2f);//最低系数0.2
        return attributeProp;
    }



    /**
     * 计算暴击率
     * @param crit          攻方crit
     * @param resilience    守方resilience
     * @return
     */
    public static int critProp(int crit, int resilience)
    {
        float critProp = (float)(crit - resilience) / (float)10000;
        critProp = Mathf.Max(critProp, 0);//最低暴击率0%
        critProp = Mathf.Min(critProp, 1f);//最高暴击率100%
        return Mathf.RoundToInt(critProp * 10000);
    }

    /**
     * 计算暴击倍率
     * @param critRate 攻方critRate
     * @param tough 守方tough
     * @return
     */
    public static float critRateProp(float critRate, int tough)
    {
        float critProp = 1.5f + (critRate - (float)tough) / 10000;
        critProp = Mathf.Max(critProp, 1.5f);//最低暴击倍率1.5
        critProp = Mathf.Min(critProp, 3);//最高暴击倍率3
        return critProp;
    }

    /**
     * 物理减免系数
     * @param defPhysical 物理防御
     * @return
     */
    public static float defReductionCoefficient(int def)
    {
        double b = 0.001d * (double)(def + 1000);
        double reduction = 0.8 - Mathf.Pow(0.8f, (float)b);
        return (float)reduction;
    }



    /**
     * 计算力量系数
     * @param str   力量值
     * @param con   体魄
     * @return
     */
    public static float strCoeff(int str, int con)
    {
        float coeff = (float)str * 1.1f - (float)con;
        if (coeff >= 0)
            return coeff / (coeff + 100);
        return coeff / (-coeff + 100);
    }


    /**
     * 计算法伤系数
     * @param mana  攻方法力值
     * @param vit   守方活力值
     * @return
     */
    public static float manaCoeff(int mana, int vit)
    {
        float coeff = (float)mana * 1.1f - (float)vit;
        if (coeff >= 0)
            return coeff / (coeff + 100);
        return coeff / (-coeff + 100);
    }


    /**
     * 基础物理攻击力
     * @return
     */
    public static float baseBaseDamage(int attack, float defReduction, int dmgInc, int dmgDec)
    {
        return (float)attack * (1f - defReduction)  * (1f + ((float)dmgInc - (float)dmgDec) / 10000f);
    }
    /// <summary>
    /// 得到额外伤害
    /// </summary>
    public static float getExtraDamage(int extraDmg, int dmgInc, int dmgDec)
    {
        return (float)extraDmg * (1f + (dmgInc - dmgDec) / 10000f);
    }

    /**
     * 技能伤害系数
     * @return
     */
    public static float spellDamageCoeff(int min, int max)
    {
        return (float)rand.Next(min, max) / 10000f;
    }


    public static int GetPercentZazenAddition(int cave,int luk,int effect)
    {
       return(cave + luk*100+ effect);
    }

    public static int GetZazenExpIncome(int autoVal, int baseVal, int percentVal, int effectMisc, int luk)
    {
        int caveExp = autoVal + baseVal;
        float heroExp = (float)percentVal / 10000f + (float)effectMisc / 10000f;
        float lukMisc = (float)luk / 100f;
        return Mathf.RoundToInt((float)caveExp * (1 + heroExp + lukMisc));

    }

    public static int getPrestigeMoneyReward(int num, int level)
    {
        int state = level / 10;
        if (state == 0) return num;
        return (int)(num * (Mathf.Pow(state, 1f + state / 60f)));
    }
}
