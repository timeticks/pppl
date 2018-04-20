using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVEMgr:MonoBehaviour
{
    public static PVEMgr Instance { get; private set; }

    public int CurMapId;
    private int mWaitSwitchMapId;

    public void Init(int mapId)
    {
        Instance = this;
        SwitchMap(mapId);
        gameObject.CheckAddComponent<PVEJob>();
    }

    public void SwitchMap(int mapId) //切换地图
    {
        if (PVEJob.Instance != null && PVEJob.Instance.curPVEStatus != PVEJob.PVEStatus.End && PVEJob.Instance.curPVEStatus != PVEJob.PVEStatus.None)
        {
            mWaitSwitchMapId = mapId;
        }
        else
        {
            CurMapId = mapId;
            mWaitSwitchMapId = 0;
        }
    }

    private List<PVEHero> dropMonsterList = new List<PVEHero>();
    public void AddDropMonster(PVEHero monster)
    {
        dropMonsterList.Add(monster);
    }

    public void RunPVE()
    {
        if (mWaitSwitchMapId != 0)
        {
            CurMapId = mWaitSwitchMapId;
            mWaitSwitchMapId = 0;
        }

        //进行下一场战斗
        TextAsset t = Resources.Load("PVEBot") as TextAsset;
        PVEBot bot = LitJson.JsonMapper.ToObject<PVEBot>(t.text);
        List<Spell> spellList = PlayerPrefsBridge.Instance.GetSpellListCopy(true);
        spellList.Add(Spell.Fetcher.GetSpellCopy(PlayerPrefsBridge.Instance.PlayerData.Hero.commonAtk));
        PVEHero challenger = new PVEHero(PlayerPrefsBridge.Instance.GetHeroWithProperties(), PlayerPrefsBridge.Instance.PlayerData.Level, spellList);
        PVEHero defier = PVEMgr.MonsterCreator(CurMapId);

        PVEJob.Instance.Init(challenger, defier, true);
    }


    public static PVEHero MonsterCreator(int mapId) //怪物生成器，得到怪物品质
    {
        return null;
    }
}
