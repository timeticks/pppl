using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVETest :MonoBehaviour
{
    /// <summary>
    /// 1、完善从传入怪物到爆出物品的流程 （装备流程差不多了）
    /// 2、道具和装备的通用显示界面
    /// 3、装备强化
    /// </summary>
    void OnGUI()
    {
        if (GUILayout.Button("开始战斗"))
        {
            InitTest();
        }
        if (GUILayout.Button("继续战斗"))
        {
            PVEJob.ResumeThread();
        }
        if (GUILayout.Button("加速"))
        {
            Time.timeScale = Time.timeScale == 1 ? 10 : 1;
        }
        //if (GUILayout.Button("1111111111111111111111111111"))
        //{
        //    UIRootMgr.Instance.OpenWindow<Window_MapChoose>(WinName.Window_MapChoose).OpenWindow();
        //}
        if (GUILayout.Button("地图创建下一个前缀怪物"))
        {
            TextAsset t = Resources.Load("PVEBot") as TextAsset;
            PVEBot bot = LitJson.JsonMapper.ToObject<PVEBot>(t.text);
            PVEHero challenger = PVEBot.GetPVEHero(bot.challengerHeroIdx, bot.challengerLevel, bot.challengerNormalSpell, bot.challengerAttackSpells, bot.challengerEquipList);
            PVEHero defier = PVEMgr.MonsterCreator(30001);

            PVEJob.Instance.Init(challenger, defier, true);
        }
        if (GUILayout.Button("选择地图"))
        {
            //1、实现选择地图后进行连续随机，且战斗胜利后进行掉落。
            //2、玩家基本信息，和掉落的装备加入背包
            //实现选择地图后进行连续随机，且战斗胜利后进行掉落
            
        }
        //if (GUILayout.Button("选择地图2"))
        //{
        //    if (PVEMgr.Instance == null)
        //    {
        //        gameObject.CheckAddComponent<PVEMgr>().Init(30002);
        //    }
        //    else PVEMgr.Instance.SwitchMap(30002);
        //}
        if (GUILayout.Button("掉落"))
        {
            DropInfo dropItem = new DropInfo();
            dropItem.monsterLevel = 1;
            dropItem.monsterQuality = 5;
            dropItem.dropQuality = 5;
            Dictionary<DropGrade.DropType, List<object>> dropMap = DropGrade.RunDrop(dropItem);
            //foreach (var temp in dropMap)
            //{
            //    for (int i = 0; i < temp.Value.Count; i++)
            //    {
            //        if (temp.Key == DropGrade.DropType.Gold)
            //            TDebug.Log(string.Format("name:{0},quality:{1}", "金币", temp.Value[i]));
            //        else
            //        {
            //            TDebug.Log(string.Format("name:{0},quality:{1}", ((Equip) (temp.Value[i])).name,
            //                ((Equip) (temp.Value[i])).curQuality));
            //            PlayerPrefsBridge.Instance.addEquip((Equip) (temp.Value[i]) , "");
            //        }
            //    }
            //}
        }
        if (GUILayout.Button("存档"))
        {
            PlayerPrefsBridge.Instance.saveGame();
            //PlayerPrefsBridge.Instance.savePlayerModule();
        }
    }

    public void InitTest()//测试战斗
    {
        TextAsset t = Resources.Load("PVEBot") as TextAsset;
        PVEBot bot = LitJson.JsonMapper.ToObject<PVEBot>(t.text);

        PVEHero challenger = PVEBot.GetPVEHero(bot.challengerHeroIdx, bot.challengerLevel, bot.challengerNormalSpell, bot.challengerAttackSpells,  bot.challengerEquipList);
        PVEHero defier = PVEBot.GetPVEHero(bot.defierHeroIdx, bot.defierLevel, bot.defierNormalSpell, bot.defierAttackSpells, bot.defierEquipList);

        PVEJob.Instance.Init(challenger, defier, true);
    }

   
}

public class HeroProperty
{
    public Hero hero;
    public int level;
    public List<Spell> skillList;
    public List<int> bornBuffList;
}

public class PVEBot
{
    public string name;
    public int uid;
    public string challengerName;
    public int challengerHeroIdx;
    public int challengerLevel;
    public int challengerNormalSpell;//如果此技能为0，则直接取hero配置的普通攻击
    public int challengerIdleSpell;
    public int[] challengerAttackSpells;
    //public int[] challengerDefSpells;
    public int[] challengerPassiveSpells;
    public int[] challengerEquipList;

    public string defierName;
    public int defierHeroIdx;
    public int defierLevel;
    public int defierNormalSpell;
    public int defierIdleSpell;
    public int[] defierAttackSpells;
    //public int[] defierDefSpells;
    public int[] defierPassiveSpells;
    public int[] defierEquipList;

    //根据配置，生成pveHero的信息
    public static PVEHero GetPVEHero(int heroIdx, int level,int commonAtk, int[] atkSkill, int[] equips , int prefixId=0)
    {
        Hero hero = Hero.Fetcher.GetHeroCopy(heroIdx);
        List<Equip> equipList = new List<Equip>();
        if (equips != null)
        {
            for (int i = 0; i < equips.Length; i++)
            {
                equipList.Add(Equip.Fetcher.GetEquipCopy(equips[i]));
            }
        }

        List<Spell> skillList = new List<Spell>();
        if (commonAtk == 0)
            commonAtk = hero.commonAtk;
        skillList.Add(Spell.Fetcher.GetSpellCopy(commonAtk));
        if (atkSkill != null)
        {
            for (int i = 0; i < atkSkill.Length; i++)
            {
                skillList.Add(Spell.Fetcher.GetSpellCopy(atkSkill[i]));
            }
        }
        
        //for (int i = 0; i < passiveSkill.Length; i++)
        //{
        //    skillList.Add(Skill.Fetcher.GetSpellCopy(passiveSkill[i]));
        //}
        List<int> birthBuff = null;;
        if (prefixId>0)
        {
            MonsterPrefix prefix = MonsterPrefix.Fetcher.GetMonsterPrefixCopy(prefixId);
            if (prefix != null && prefix.buffId > 0)
            {
                birthBuff = new List<int>();
                birthBuff.Add(prefix.buffId);
            }
        }

        hero.Properties(equipList.ToArray(), skillList, null, birthBuff, level);
        PVEHero pveHero = new PVEHero(hero, level, skillList, prefixId);
        return pveHero;
    }
}