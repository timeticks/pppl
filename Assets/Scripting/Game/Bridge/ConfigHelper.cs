using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigHelper :  ILevelUpFetcher,  IPVEDialogueFetcher, IMapDataFetcher, 
                         ISelectDialogFetcher, IRecipeFetcher, IAuxSkillFetcher, ILobbyDialogueFetcher,ITravelFetcher,ITravelEventFetcher,ITravelSmallEventFetcher,
                         ISpellObtainFetcher, ISectFetcher, ILootFetcher, IPetFetcher, IMapEventFetcher, ICaveFetcher, ISpellLevelUpFetcher,
                         IPetLevelUpFetcher, IErrorStatusFetcher,IGameConstFetcher,IPropLevelUpFetcher,ICommodityFetcher,IShopFetcher,IPrestigeLevelFetcher,
                         IPrestigeTaskFetcher,ITowerFetcher,IAchievementFetcher,IAchieveRewardFetcher,IBookSpellFetcher,IHeadIconFetcher,
                        
    IHeroFetcher,IBuffFetcher,IAttrTableFetcher,IAttrProbFetcher,IDropQualityRateFetcher,IMonsterRateFetcher,
    IDropGradeFetcher,IBallMapFetcher,ISkillFetcher, IItemFetcher, IEquipFetcher,IMonsterPrefixFetcher,IMonsterLevelUpFetcher,
    IQualityTableFetcher, IPartnerDialogueFetcher, IPartnerFetcher, IMindTreeMapFetcher, IBallFetcher, INatureLevelUpFetcher,
    IIntimacyLevelUpFetcher,IRechargeFetcher
{
    private static readonly  ConfigHelper mInstance = new ConfigHelper();

    public Dictionary<int, Hero>             mHeroCached                        = new Dictionary<int, Hero>(12);
    public Dictionary<int, HeroLevelUp>      mHeroLevelUpCached                 = new Dictionary<int, HeroLevelUp>(12);
    public Dictionary<int, Spell>            mSkillCached                       = new Dictionary<int, Spell>(12);
    public Dictionary<int, Item> mItemCached                                    = new Dictionary<int, Item>(12);
    public Dictionary<int, Equip>            mEquipCached                       = new Dictionary<int,Equip>(12);
    public Dictionary<int, PVEDialogue>      mPVEDialogueCached                 = new Dictionary<int, PVEDialogue>(12);
    public Dictionary<int, MapData>          mMapDataCached                     = new Dictionary<int, MapData>(12);
    public Dictionary<int, Recipe>           mRecipeCached                      = new Dictionary<int, Recipe>(12);
    public Dictionary<int, AuxSkillLevel>     mAuxSkillLevelCached              = new Dictionary<int, AuxSkillLevel>(12);
    public Dictionary<int, Travel>           mTravelCached                      = new Dictionary<int, Travel>(12);
    public Dictionary<int, TravelEvent>      mTravelEventCached                 = new Dictionary<int, TravelEvent>(12);
    public Dictionary<int, TravelSmallEvent> mTravelSmallEventCached            = new Dictionary<int, TravelSmallEvent>(12);
    public Dictionary<string, LobbyDialogue> mLobbyDialogueCached               = new Dictionary<string, LobbyDialogue>(3);
    public Dictionary<Sect.SectType, Sect>   mSectCached                        = new Dictionary<Sect.SectType, Sect>(3);
    public Dictionary<int, SpellObtain>      mSpellObtainCached                 = new Dictionary<int, SpellObtain>(12);
    public Dictionary<int, Pet>             mPetCached                          = new Dictionary<int,Pet>(12);
    public Dictionary<int, MapEvent>        mMapEventCached                     = new Dictionary<int, MapEvent>(12);
    public Dictionary<int, Cave>            mCaveCached                         = new Dictionary<int, Cave>(12);
    public Dictionary<int, SpellLevelUp>    mSpellLevelUpCached                 = new Dictionary<int, SpellLevelUp>(12);
    public Dictionary<int, PetLevelUp>      mPetLevelUpCached                   = new Dictionary<int, PetLevelUp>(12);
    public Dictionary<int, ErrorStatus>     mErrorStatusCached                  = new Dictionary<int, ErrorStatus>(12);
    public Dictionary<int, PropLevelUp>      mPropLevelUpCached                 = new Dictionary<int, PropLevelUp>(12);
    public Dictionary<int, Shop>               mShopCached                      = new Dictionary<int, Shop>(12);
    public Dictionary<int, Commodity>        mCommodityCached                   = new Dictionary<int, Commodity>(12);
    public Dictionary<int, PrestigeLevel>      mPrestigeLevelCached             = new Dictionary<int, PrestigeLevel>(12);
    public Dictionary<int, PrestigeTask>       mPrestigeTaskCached              = new Dictionary<int, PrestigeTask>(12);
    public Dictionary<int, Tower>              mTowerCached                     = new Dictionary<int, Tower>(12);
    public Dictionary<int, Achievement>         mAchievementCached              = new Dictionary<int, Achievement>(12);
    public Dictionary<int, AchieveReward>   mAchieveRewardCached                = new Dictionary<int, AchieveReward>(12);
    public Dictionary<int, BookSpell>          mBookSpellCached                 = new Dictionary<int, BookSpell>(12);
    public Dictionary<int, HeadIcon>          mHeadIconCached                   = new Dictionary<int, HeadIcon>(12);
    public Dictionary<int, Buff>              mBuffCached                       = new Dictionary<int, Buff>(12);

    public Dictionary<int, DropQualityRate> mDropQualityRateCached              = new Dictionary<int, DropQualityRate>();
    public Dictionary<AttrType, AttrTable>  mAttrTableCached                    = new Dictionary<AttrType, AttrTable>();
    public Dictionary<int, AttrProb>        mAttrProbCached                     = new Dictionary<int, AttrProb>();
    public Dictionary<string, GameConst>    mGameConstCached                    = new Dictionary<string, GameConst>(12);
    public Dictionary<int, DropGrade>       mDropGradeCached                    = new Dictionary<int, DropGrade>(12);
    public Dictionary<int, BallMap>         mMapCached                          = new Dictionary<int, BallMap>(12);
    public Dictionary<int, MonsterPrefix>   mMonsterPrefixCached                = new Dictionary<int, MonsterPrefix>(12);
    public Dictionary<int, MonsterLevelUp>  mMonsterLevelUpCached               = new Dictionary<int, MonsterLevelUp>(12);
    public Dictionary<int, MonsterRate>     mMonsterRateCached                  = new Dictionary<int, MonsterRate>(12);
    public Dictionary<int, QualityTable>    mQualityTableCached                 = new Dictionary<int, QualityTable>(12);
    public Dictionary<string, List<PartnerDialogue>> mPartnerDialogueCached     = new Dictionary<string, List<PartnerDialogue>>(12);
    public Dictionary<int, Partner>         mPartnerCached                      = new Dictionary<int, Partner>(12);
    public Dictionary<int, Loot>            mLootCached                         = new Dictionary<int, Loot>(12);
    public Dictionary<string, SelectDialog>    mSelectDialogCached              = new Dictionary<string, SelectDialog>(12);
    public Dictionary<int, Ball>            mBallCached                         = new Dictionary<int, Ball>(12);
    public Dictionary<int, MindTreeMap>     mMindTreeMapCached                  = new Dictionary<int,MindTreeMap>(3);
    public Dictionary<NatureType, Dictionary<int, NatureLevelUp>> mNatureLevelUpCached = new Dictionary<NatureType, Dictionary<int, NatureLevelUp>>(3);
    public Dictionary<int, IntimacyLevelUp>     mIntimacyLevelUpCached          = new Dictionary<int,IntimacyLevelUp>(3);
    public Dictionary<int, Recharge>            mRechargeCached                 = new Dictionary<int,Recharge>(3);

    public static ConfigHelper Instance
    {
        get
        {
            return mInstance;
        }
    }

    public void Init(DataName dataName, string text)
    {
        

        if (dataName == DataName.DropQualityRate)
        {
            Dictionary<string, DropQualityRate> pool = LitJson.JsonMapper.ToObject<Dictionary<string, DropQualityRate>>(text);
            foreach (var temp in pool)
            {
                temp.Value.CheckLegal();
                mDropQualityRateCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始EquipDropQualityRate成功:{0}项", mDropQualityRateCached.Count));
        }
        else if (dataName == DataName.Item)
        {
            Dictionary<string, Item> pool = LitJson.JsonMapper.ToObject<Dictionary<string, Item>>(text);
            foreach (var temp in pool)
            {
                temp.Value.CheckLegal();
                mItemCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始Item成功:{0}项", mItemCached.Count));
        }
        else if (dataName == DataName.Spell)
        {
            Dictionary<string, Spell> pool = LitJson.JsonMapper.ToObject<Dictionary<string, Spell>>(text);
            foreach (var temp in pool)
            {
                temp.Value.CheckLegal();
                mSkillCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始Skill成功:{0}项", mSkillCached.Count));
        }
        else if (dataName == DataName.Hero)
        {
            Dictionary<string, Hero> pool = LitJson.JsonMapper.ToObject<Dictionary<string, Hero>>(text);
            foreach (var temp in pool)
            {
                temp.Value.CheckLegal();
                mHeroCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始Hero成功:{0}项", mHeroCached.Count));
        }
        else if (dataName == DataName.GameConst)
        {
            Dictionary<string, GameConst> pool = LitJson.JsonMapper.ToObject<Dictionary<string, GameConst>>(text);
            foreach (var temp in pool)
            {
                temp.Value.CheckLegal();
                mGameConstCached.Add(temp.Value.name, temp.Value);
            }
            TDebug.Log(string.Format("初始mGameConstCached成功:{0}项", mGameConstCached.Count));
        }
        else if (dataName == DataName.DropGrade)
        {
            Dictionary<string, DropGrade> pool = LitJson.JsonMapper.ToObject<Dictionary<string, DropGrade>>(text);
            foreach (var temp in pool)
            {
                temp.Value.CheckLegal();
                mDropGradeCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始mDropGradeCached成功:{0}项", mDropGradeCached.Count));
        }
        else if (dataName == DataName.BallMap)
        {
            Dictionary<string, BallMap> pool = LitJson.JsonMapper.ToObject<Dictionary<string, BallMap>>(text);
            foreach (var temp in pool)
            {
                temp.Value.CheckLegal();
                mMapCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始mMapCached成功:{0}项", mMapCached.Count));
        }
        else if (dataName == DataName.LobbyDialogue)
        {
            Dictionary<string, LobbyDialogue> pool = LitJson.JsonMapper.ToObject<Dictionary<string, LobbyDialogue>>(text);
            foreach (var temp in pool)
            {
                temp.Value.CheckLegal();
                mLobbyDialogueCached.Add(temp.Value.name, temp.Value);
            }
            TDebug.Log(string.Format("初始LobbyDialogue成功:{0}项", mLobbyDialogueCached.Count));
        }
        else if (dataName == DataName.HeroLevelUp)
        {
            Dictionary<string, HeroLevelUp> pool = LitJson.JsonMapper.ToObject<Dictionary<string, HeroLevelUp>>(text);
            foreach (var temp in pool)
            {
                mHeroLevelUpCached.Add(temp.Value.level, temp.Value);
            }
            TDebug.Log(string.Format("初始HeroLevelUp成功:{0}项", mHeroLevelUpCached.Count));
        }
        else if (dataName == DataName.QualityTable)
        {
            Dictionary<string, QualityTable> pool = LitJson.JsonMapper.ToObject<Dictionary<string, QualityTable>>(text);
            foreach (var temp in pool)
            {
                mQualityTableCached.Add(temp.Value.dropQuality, temp.Value);
            }
            TDebug.Log(string.Format("初始QualityTable成功:{0}项", mQualityTableCached.Count));
        }
        else if (dataName == DataName.PartnerDialogue)
        {
            Dictionary<string, PartnerDialogue> pool = LitJson.JsonMapper.ToObject<Dictionary<string, PartnerDialogue>>(text);
            foreach (var temp in pool)
            {
                //转换配置时，会将范围数组的长度都设为2
                if (temp.Value.intimacyRange.Length == 0) temp.Value.intimacyRange = new int[2] { 0, GameConstUtils.array_intimacy_level.Length };
                else if (temp.Value.intimacyRange.Length == 1) temp.Value.intimacyRange = new int[2] { temp.Value.intimacyRange[0], temp.Value.intimacyRange[0] };

                if (temp.Value.chatNumRange.Length == 0) temp.Value.chatNumRange = new int[2] { 0, int.MaxValue };
                else if (temp.Value.chatNumRange.Length == 1) temp.Value.chatNumRange = new int[2] { temp.Value.chatNumRange[0], temp.Value.chatNumRange[0] };

                if (temp.Value.hobbyRange.Length == 0) temp.Value.hobbyRange = new int[2] { 1, int.MaxValue };
                else if (temp.Value.hobbyRange.Length == 1) temp.Value.hobbyRange = new int[2] { temp.Value.hobbyRange[0], temp.Value.hobbyRange[0] };

                if (temp.Value.timeRange.Length == 0) temp.Value.timeRange = new int[2] { 0, 24 };
                else if (temp.Value.timeRange.Length == 1) temp.Value.timeRange = new int[2] { temp.Value.timeRange[0], temp.Value.timeRange[0] };

                if (temp.Value.characRange.Length == 0) temp.Value.characRange = new int[2] { (int)PartnerData.CharacType.None + 1, (int)PartnerData.CharacType.Max };
                else if (temp.Value.characRange.Length == 1) temp.Value.characRange = new int[2] { temp.Value.characRange[0], temp.Value.characRange[0] };

                if (temp.Value.sexRange.Length == 0) temp.Value.sexRange = new int[2] { (int)PartnerData.Sex.None + 1, (int)PartnerData.Sex.Max };
                else if (temp.Value.sexRange.Length == 1) temp.Value.sexRange = new int[2] { temp.Value.sexRange[0], temp.Value.sexRange[0] };

                for (int i = temp.Value.sexRange[0]; i <= (temp.Value.sexRange.Length > 1 ? temp.Value.sexRange[1] : temp.Value.sexRange[0]); i++)
                {
                    for (int j = temp.Value.characRange[0]; j <= (temp.Value.characRange.Length > 1 ? temp.Value.characRange[1] : temp.Value.characRange[0]); j++)
                    {
                        for (int k = temp.Value.intimacyRange[0]; k <= temp.Value.intimacyRange[1]+1; k++)
                        {
                            string groupKey = PartnerDialogue.GetGroupKey(i, j, k);
                            if (mPartnerDialogueCached.ContainsKey(groupKey))
                                mPartnerDialogueCached[groupKey].Add(temp.Value);
                            else
                            {
                                TDebug.LogInEditorF("PartnerDialogue中新的GroupKey:{0}|sex:{1}|chara:{2}|inti:{3}", groupKey,i,j,k);
                                mPartnerDialogueCached[groupKey] = new List<PartnerDialogue>() {temp.Value};
                            }
                        }
                    }
                }
            }
            TDebug.Log(string.Format("初始PartnerDialogue成功:{0}项", mQualityTableCached.Count));
        }
        else if (dataName == DataName.Partner)
        {
            Dictionary<string, Partner> pool = LitJson.JsonMapper.ToObject<Dictionary<string, Partner>>(text);
            foreach (var temp in pool)
            {
                mPartnerCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始Partner成功:{0}项", mPartnerCached.Count));
        }
        else if (dataName == DataName.Loot)
        {
            Dictionary<string, Loot> pool = LitJson.JsonMapper.ToObject<Dictionary<string, Loot>>(text);
            foreach (var temp in pool)
            {
                mLootCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始Loot成功:{0}项", mLootCached.Count));
        }
        else if (dataName == DataName.SelectDialog)
        {
            Dictionary<string, SelectDialog> pool = LitJson.JsonMapper.ToObject<Dictionary<string, SelectDialog>>(text);
            foreach (var temp in pool)
            {
                mSelectDialogCached.Add(temp.Value.name, temp.Value);
            }
            TDebug.Log(string.Format("初始SelectDialog成功:{0}项", mSelectDialogCached.Count));
        }
        else if (dataName == DataName.Ball)
        {
            Dictionary<string, Ball> pool = LitJson.JsonMapper.ToObject<Dictionary<string, Ball>>(text);
            foreach (var temp in pool)
            {
                mBallCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始Ball成功:{0}项", mBallCached.Count));
        }
        else if (dataName == DataName.MindTreeMap)
        {
            Dictionary<string, MindTreeMap> pool = LitJson.JsonMapper.ToObject<Dictionary<string, MindTreeMap>>(text);
            foreach (var temp in pool)
            {
                mMindTreeMapCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始MindTreeMap成功:{0}项", mMindTreeMapCached.Count));
        }
        else if (dataName == DataName.NatureLevelUp)
        {
            Dictionary<string, NatureLevelUp> pool = LitJson.JsonMapper.ToObject<Dictionary<string, NatureLevelUp>>(text);
            foreach (var temp in pool)
            {
                if (!mNatureLevelUpCached.ContainsKey(temp.Value.natureType))
                    mNatureLevelUpCached[temp.Value.natureType] = new Dictionary<int, NatureLevelUp>();
                mNatureLevelUpCached[temp.Value.natureType].Add(temp.Value.level, temp.Value);
            }
            TDebug.Log(string.Format("初始NatureLevelUp成功:{0}项", mNatureLevelUpCached.Count));
        }
        else if (dataName == DataName.IntimacyLevelUp)
        {
            Dictionary<string, IntimacyLevelUp> pool = LitJson.JsonMapper.ToObject<Dictionary<string, IntimacyLevelUp>>(text);
            foreach (var temp in pool)
            {
                mIntimacyLevelUpCached.Add(temp.Value.level, temp.Value);
            }
            TDebug.Log(string.Format("初始IntimacyLevelUp成功:{0}项", mIntimacyLevelUpCached.Count));
        }
        else if (dataName == DataName.Recharge)
        {
            Dictionary<string, Recharge> pool = LitJson.JsonMapper.ToObject<Dictionary<string, Recharge>>(text);
            foreach (var temp in pool)
            {
                mRechargeCached.Add(temp.Value.idx, temp.Value);
            }
            TDebug.Log(string.Format("初始Recharge成功:{0}项", mRechargeCached.Count));
        }
        //if (assets == TUtils.MDEncode("Hero"))
        //{
        //    int length = ios.ReadInt16();
        //    TDebug.LogWarning(string.Format("初始英雄配置表:{0}条", length));
        //    for (int i = 0; i < length; i++)
        //    {
        //        Hero hero = new Hero();
        //        hero.Serialize(ios);
        //        if (!mHeroCached.ContainsKey(hero.Idx))
        //            mHeroCached.Add(hero.Idx, hero);
        //    }
        //}
    }

    private ConfigHelper()
    {
        Hero.Fetcher                                = this;
        HeroLevelUp.LevelUpFetcher                  = this;
        Spell.Fetcher                               = this;
        Equip.Fetcher                               = this;
        Item.Fetcher                                = this;
        Buff.Fetcher                                = this;
        ErrorStatus.ErrorStatusFetcher              = this;
        GameConst.Fetcher                           = this;
        AttrTable.Fetcher                           = this;
        AttrProb.Fetcher                            = this;
        DropQualityRate.Fetcher                     = this;
        DropGrade.Fetcher                           = this;
        BallMap.Fetcher                             = this;
        MonsterPrefix.Fetcher                       = this;
        MonsterLevelUp.Fetcher                      = this;
        MonsterRate.Fetcher                         = this;
        LobbyDialogue.LobbyDialogueFetcher          = this;
        QualityTable.Fetcher                        = this;
        PartnerDialogue.Fetcher                     = this;
        Partner.Fetcher                             = this;
        Loot.LootFetcher                            = this;
        SelectDialog.DialogFetcher                  = this;
        Ball.Fetcher                                = this;
        MindTreeMap.MindTreeMapFetcher              = this;
        NatureLevelUp.Fetcher                       = this;
        IntimacyLevelUp.Fetcher                     = this;
        Recharge.Fetcher                            = this;
        //PVEDialogue.PVEDialogueFetcher           = this;
        //MapData.MapDataFetcher                  = this;
        //Dialog.DialogFetcher                    = this;
        //Recipe.RecipeFetcher                    = this;
        //AuxSkillLevel.AuxSkillFetcher            = this;
        //Travel.TravelFetcher                    = this;
        //TravelEvent.TravelEventFetcher           = this;
        //TravelSmallEvent.TravelSmallEventFetcher  = this;
        //SpellObtain.SpellObtainFetcher           = this;
        //Sect.SectFetcher                        = this;

        //Pet.PetFetcher                           = this;
        //MapEvent.MapEventFetcher                 = this;
        //Cave.CaveFetcher                         = this;
        //SpellLevelUp.SpellLevelUpFetcher         = this;
        //PetLevelUp.petLevelUpFetcher             = this;
        //PropLevelUp.propLevelUpFetcher           = this;
        //Shop.ShopFetcher                         = this;
        //Commodity.CommodityFetcher               = this;
        //PrestigeTask.PrestigeTaskFetcher         = this;
        //PrestigeLevel.PrestigeLevelFetcher       = this;
        //Tower.TowerFetcher                       = this;
        //Achievement.AchievementFetcher           = this;
        //AchieveReward.AchieveRewardFetcher       = this;
        //BookSpell.BookSpellFetcher               = this;
        //HeadIcon.HeadIconFetcher                 = this;
    }

    Hero IHeroFetcher.GetHeroCopy(int idx)
    {
        Hero origin = null;
        if (mHeroCached.TryGetValue(idx, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("Hero没有此Idx:{0}", idx));
        return null;
    }
    Buff IBuffFetcher.GetBuffCopy(int idx)
    {
        Buff origin = null;
        if (mBuffCached.TryGetValue(idx, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("Buff没有此Idx:{0}", idx));
        return null;
    }
    AttrTable IAttrTableFetcher.GetAttrTableCopy(AttrType attrTy)
    {
        AttrTable origin = null;
        if (attrTy.ToInt() > 100)
            attrTy = (AttrType) (attrTy.ToInt() - 100);
        if (mAttrTableCached.TryGetValue(attrTy, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("AttrTable没有此attrTy:{0}", attrTy));
        return null;
    }
    AttrProb IAttrProbFetcher.GetAttrProbCopy(AttrProb.ObjType objType , int objParam)
    {
        AttrProb origin = null;
        int key = AttrProb.GetKey(objType, objParam);
        if (mAttrProbCached.TryGetValue(key, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("AttrProb没有此key:{0}", key));
        return null;
    }
    DropQualityRate IDropQualityRateFetcher.GetDropQualityRateCopy(int idx)
    {
        DropQualityRate origin = null;
        if (mDropQualityRateCached.TryGetValue(idx, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("EquipDropQualityRate没有此idx:{0}", idx));
        return null;
    }
    DropGrade IDropGradeFetcher.GetDropGradeCopy(int idx)
    {
        DropGrade origin = null;
        if (mDropGradeCached.TryGetValue(idx, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("DropGrade没有此idx:{0}", idx));
        return null;
    }


    Spell ISkillFetcher.GetSpellCopy(int idx)
    {
        Spell origin = null;
        if (mSkillCached.TryGetValue(idx, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("Spell没有此Idx:{0}", idx));
        return null;
    }
    Equip IEquipFetcher.GetEquipCopy(int idx)
    {
        Equip origin = null;
        if (mEquipCached.TryGetValue(idx, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("Equip没有此Idx:{0}", idx));

        return null;
    }
    List<Equip> IEquipFetcher.GetEquipByLevelCopy(int level)
    {
        List<Equip> equipList = new List<Equip>();
        foreach (var temp in mEquipCached)
        {
            if (temp.Value.originLevel == level)
                equipList.Add(temp.Value.Clone());
        }
        return equipList;
    }

    Item IItemFetcher.GetItemCopy(int idx)
    {
        Item origin = null;
        if (mItemCached.TryGetValue(idx, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("Item没有此Idx:{0}", idx));
        return null;
    }

    SelectDialog ISelectDialogFetcher.GetSelectDialogByCopy(string key)
    {
        SelectDialog origin = null;
        if (mSelectDialogCached.TryGetValue(key, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("Dialog没有此Idx:{0}", key));
        return null;
    }

    BallMap IBallMapFetcher.GetBallMapCopy(int idx,bool isCopy=true)
    {
        BallMap origin = null;
        if (mMapCached.TryGetValue(idx, out origin))
        {
            if (isCopy) return origin.Clone();
            return origin;
        }
        TDebug.LogError(string.Format("Map没有此Idx:{0}", idx));
        return null;
    }

    List<BallMap> IBallMapFetcher.GetAllBallMapCopy(BallMap.MapType ty , bool isCopy=true)
    {
        List<BallMap> mapList = new List<BallMap>();
        foreach (var temp in mMapCached)
        {
            if (temp.Value.mapType != ty) continue;
            if (isCopy) mapList.Add(temp.Value.Clone());
            else mapList.Add(temp.Value);
        }
        return mapList;
    }

    MonsterPrefix IMonsterPrefixFetcher.GetMonsterPrefixCopy(int idx , bool isCopy = true)
    {
        MonsterPrefix origin = null;
        if (mMonsterPrefixCached.TryGetValue(idx, out origin))
        {
            if (isCopy)
                return origin.Clone();
            return origin;
        }
        TDebug.LogError(string.Format("MonsterPrefix没有此idx:{0}", idx));
        return null;
    }
    MonsterRate IMonsterRateFetcher.GetMonsterRateCopy(int idx, bool isCopy = true)
    {
        MonsterRate origin = null;
        if (mMonsterRateCached.TryGetValue(idx, out origin))
        {
            if (isCopy)
                return origin.Clone();
            return origin;
        }
        TDebug.LogError(string.Format("MonsterRate没有此idx:{0}", idx));
        return null;
    }
    MonsterLevelUp IMonsterLevelUpFetcher.GetMonsterLevelUpCopy(int level, bool isCopy = true)
    {
        MonsterLevelUp origin = null;
        if (mMonsterLevelUpCached.TryGetValue(level, out origin))
        {
            if (isCopy)
                return origin.Clone();
            return origin;
        }
        TDebug.LogError(string.Format("MonsterLevelUp没有此level:{0}", level));
        return null;
    }
    HeroLevelUp ILevelUpFetcher.GetLevelUpByCopy(int level)
    {
        HeroLevelUp origin = null;
        if (mHeroLevelUpCached.TryGetValue(level, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("HeroLevelUp没有此等级:{0}", level));

        return null;
    }
    /// <summary>
    /// 获取不复制的levelUp，慎用
    /// </summary>
    HeroLevelUp ILevelUpFetcher.GetLevelUpByNoCopy(int level)
    {
        HeroLevelUp origin = null;
        if (mHeroLevelUpCached.TryGetValue(level, out origin))
        {
            return origin;
        }
        TDebug.LogError(string.Format("HeroLevelUp没有此等级:{0}", level));

        return null;
    }

    LobbyDialogue ILobbyDialogueFetcher.GetLobbyDialogueByCopy(string key)
    {
        LobbyDialogue origin = null;
        if (mLobbyDialogueCached.TryGetValue(key, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogWarning(string.Format("LobbyDialogue没有此key:{0}", key));
        return null;
    }

    QualityTable IQualityTableFetcher.GetQualityTableCopy(int quality, bool isCopy = true)
    {
        QualityTable origin = null;
        if (mQualityTableCached.TryGetValue(quality, out origin))
        {
            if (isCopy)
                return origin.Clone();
            return origin;
        }
        TDebug.LogError(string.Format("QualityTable没有此品质:{0}", quality));

        return null;
    }

    private List<PartnerDialogue> mCurPartnerDialogueList = new List<PartnerDialogue>();
    private List<int> mCurPartnerDialoguePctList = new List<int>();
    PartnerDialogue IPartnerDialogueFetcher.GetPartnerDialogueCopy(PartnerData partnerData, int dayHour,int chatNum , bool isCopy = true)
    {
        Partner partner = Partner.Fetcher.GetPartnerCopy(partnerData.idx);
        string key = PartnerDialogue.GetGroupKey((int)partner.sex, (int)partner.characType, partnerData.intimacyLevel);
        List<PartnerDialogue> dialogueList;
        int hobbyType = (int) partnerData.hobbyType;
        mCurPartnerDialogueList.Clear();
        mCurPartnerDialoguePctList.Clear();
        if (mPartnerDialogueCached.TryGetValue(key, out dialogueList))
        {
            foreach (var temp in dialogueList)
            {
                //筛选时间、回忆、对话次数
                if (temp.timeRange[0] > dayHour || temp.timeRange[1] < dayHour)
                    continue;
                if (temp.hobbyRange[0] > hobbyType || temp.hobbyRange[1] < hobbyType)
                    continue;
                if (temp.chatNumRange[0] > chatNum || temp.chatNumRange[1] < chatNum)
                    continue;
                if (temp.weight == GameConstUtils.num_certain_partner_dialogue_pct)
                    return temp;
                mCurPartnerDialogueList.Add(temp);
                mCurPartnerDialoguePctList.Add(temp.weight);
            }
        }
        TDebug.LogInEditorF("待随机mCurPartnerDialogueListCount:{0}|dayHour:{1}|chatNum:{2}", mCurPartnerDialogueList.Count, dayHour, chatNum);
        if (mCurPartnerDialogueList.Count > 0)
        {
            int randIndex = RPGRandomUtility.GetIndexByPct(mCurPartnerDialoguePctList.ToArray());
            return mCurPartnerDialogueList[randIndex];
        }
        TDebug.LogError(string.Format("PartnerDialogue没有此key:{0}", key));
        return null;
    }

    Partner IPartnerFetcher.GetPartnerCopy(int idx, bool isCopy = true)
    {
        Partner origin = null;
        if (mPartnerCached.TryGetValue(idx, out origin))
        {
            return isCopy ? origin.Clone() : origin;
        }
        TDebug.LogError(string.Format("Partner没有此idx:{0}", idx));

        return null;
    }
    Partner IPartnerFetcher.GetPartnerRandomCopy(PartnerData.Sex sex, PartnerData.CharacType characTy)
    {
        Partner origin = null;
        foreach (var temp in mPartnerCached)
        {
            if (temp.Value.sex == sex && temp.Value.characType == characTy)
            {
                return temp.Value;
            }
        }

        TDebug.LogError(string.Format("Partner没有此sex:{0} | characTy:{1}", sex, characTy));

        return null;
    }


    Ball IBallFetcher.GetBallCopy(int idx, bool isCopy)
    {
        Ball origin = null;
        if (mBallCached.TryGetValue(idx, out origin))
        {
            return isCopy ? origin.Clone() : origin;
        }
        TDebug.LogError(string.Format("Ball没有此idx:{0}", idx));

        return null;
    }


    NatureLevelUp INatureLevelUpFetcher.GetNatureLevelUpCopy(NatureType natureType, int level, bool isCopy)
    {
        if (mNatureLevelUpCached.ContainsKey(natureType) && mNatureLevelUpCached[natureType].ContainsKey(level))
        {
            return isCopy ? mNatureLevelUpCached[natureType][level].Clone() : mNatureLevelUpCached[natureType][level];
        }
        TDebug.LogError(string.Format("NatureLevelUp没有此natureType:{0}|level:{1}", natureType, level));

        return null;
    }
    int INatureLevelUpFetcher.GetNatureLevelUpMax(NatureType natureType)
    {
        if (mNatureLevelUpCached.ContainsKey(natureType))
        {
            return mNatureLevelUpCached[natureType].Count-1; //包含了0级，所以减一
        }
        TDebug.LogError(string.Format("NatureLevelUp没有此key:{0}", natureType));

        return 0;
    }

    IntimacyLevelUp IIntimacyLevelUpFetcher.GetIntimacyLevelUpCopy(int level, bool isCopy)
    {
        IntimacyLevelUp origin = null;
        if (mIntimacyLevelUpCached.TryGetValue(level, out origin))
        {
            return isCopy ? origin.Clone() : origin;
        }
        TDebug.LogError(string.Format("IntimacyLevelUp没有此level:{0}", level));

        return null;
    }
    int IIntimacyLevelUpFetcher.GetIntimacyLevelUpMax()
    {
        return mIntimacyLevelUpCached.Count;
    }

    Recharge IRechargeFetcher.GetRechargeCopy(int idx, bool isCopy)
    {
        Recharge origin = null;
        if (mRechargeCached.TryGetValue(idx, out origin))
        {
            return isCopy ? origin.Clone() : origin;
        }
        TDebug.LogError(string.Format("Recharge没有此level:{0}", idx));

        return null;
    }

    List<Recharge> IRechargeFetcher.GetRechargeCopyAll()
    {
        List<Recharge> rechargeList = new List<Recharge>();
        foreach (var temp in mRechargeCached)
        {
            rechargeList.Add(temp.Value.Clone());
        }
        return rechargeList;
    }

    #region ============Old=================
    
    


    PVEDialogue IPVEDialogueFetcher.GetPVEDialogueByCopy(int idx)
    {
        PVEDialogue origin = null;
        if (mPVEDialogueCached.TryGetValue(idx, out origin))
        {
            return new PVEDialogue(origin);
        }
        TDebug.LogError(string.Format("PVEDialogue没有此Idx:{0}", idx));
        return null;
    }
    List<PVEDialogue> IPVEDialogueFetcher.GetPVEDialogueByTypeByCopy(PVELoggerType ty)
    {
        List<PVEDialogue> temp = new List<PVEDialogue>();
        foreach (var item in mPVEDialogueCached)
        {
            if (item.Value.Type == ty)
                temp.Add(new PVEDialogue(item.Value));
        }
        if (temp.Count!=0)
            return temp;
        TDebug.LogError(string.Format("PVEDialogue没有此Type:{0}", ty));
        return null;
    }



    MapData IMapDataFetcher.GetMapDataByCopy(int idx)
    {
        MapData origin = null;
        if (mMapDataCached.TryGetValue(idx, out origin))
        {
            return new MapData(origin);
        }
        TDebug.LogError(string.Format("MapData没有此Idx:{0}", idx));
        return null;
    }

    // 根据地图类型，返回地图列表。地图信息不深复制
    List<MapData> IMapDataFetcher.GetMapDataListNoCopy(MapData.MapType mapType)
    {
        List<MapData> mapList = new List<MapData>();
        foreach (var item in mMapDataCached)
        {
            if (item.Value!=null && mapType == item.Value.Type)
                mapList.Add(item.Value);
        }
        return mapList;
    }

    Recipe IRecipeFetcher.GetRecipeByCopy(int idx)
    {
        Recipe origin = null;
        if (mRecipeCached.TryGetValue(idx, out origin))
        {
            return new Recipe(origin);
        }
        TDebug.LogError(string.Format("Recipe没有此Idx:{0}", idx));
        return null;
    }
    List<Recipe> IRecipeFetcher.GetRecipeAll(AuxSkillLevel.SkillType type, int level, Sect.SectType sect)
    {
        List<Recipe> temp = new List<Recipe>();
        foreach (var item in mRecipeCached)
        {
            if ((int)item.Value.Type == (int)type && item.Value.SkillLevel == level && (item.Value.Sect == Sect.SectType.None||sect == item.Value.Sect))
            {
                temp.Add(new Recipe(item.Value));
            }
        }
        if (temp.Count != 0)
            return temp;
        TDebug.LogError(string.Format("未找到Type:{0}，Level:{1}的配方", type, level));
        return null;
    }

    AuxSkillLevel IAuxSkillFetcher.GetAuxSkilleByCopy(int level,AuxSkillLevel.SkillType type)
    {
        AuxSkillLevel origin = null;
        int key = AuxSkillLevel.GetCachedKey(level,type);
        if (mAuxSkillLevelCached.TryGetValue(key, out origin))
        {
            return new AuxSkillLevel(origin);
        }
        TDebug.LogError(string.Format("AuxSkillLevel没有此level:{0},Type:{1}", level, type));
        return null;
    }
    List<Travel> ITravelFetcher.GetTravelList()
    {
        List<Travel> temp = new List<Travel>();
        foreach (var item in mTravelCached)
        {
            temp.Add(new Travel(item.Value));
        }
        if (temp.Count != 0)
            return temp;
        TDebug.LogError("Travel表为空");
        return null;
    }
    Travel ITravelFetcher.GetTravelByCopy(int idx)
    {
        Travel origin = null;
        if (mTravelCached.TryGetValue(idx, out origin))
        {
            return new Travel(origin);
        }
        TDebug.LogError(string.Format("Travel没有此Travel:{0}", idx));
        return null;
    }
    TravelEvent ITravelEventFetcher.GetTravelEventByCopy(int id)
    {
        TravelEvent origin = null;
        if (mTravelEventCached.TryGetValue(id, out origin))
        {
            return new TravelEvent(origin);
        }
        TDebug.LogError(string.Format("TravelEvent没有此id:{0}", id));
        return null;
    }
    TravelSmallEvent ITravelSmallEventFetcher.GetTravelSmallEventByCopy(int id)
    {
        TravelSmallEvent origin = null;
        if (mTravelSmallEventCached.TryGetValue(id, out origin))
        {
            return new TravelSmallEvent(origin);
        }
        TDebug.LogError(string.Format("TravelSmallEvent没有此id:{0}", id));
        return null;
    }

    MindTreeMap IMindTreeMapFetcher.GetMindTreeMapNoCopy(int id)
    {
        MindTreeMap origin = null;
        if (mMindTreeMapCached.TryGetValue(id, out origin))
        {
            return origin;
        }
        TDebug.LogError(string.Format("MindTreeMap没有此id:{0}", id));
        return null;
    }

    

    SpellObtain ISpellObtainFetcher.GetSpellObtainByCopy(int id)
    {
        SpellObtain origin = null;
        if (mSpellObtainCached.TryGetValue(id, out origin))
        {
            return new SpellObtain(origin);
        }
        TDebug.LogError(string.Format("SpellObtain没有此id:{0}", id));
        return null;
    }

    Sect ISectFetcher.GetSectByCopy(Sect.SectType sectType)
    {
        Sect origin = null;
        if (mSectCached.TryGetValue(sectType, out origin))
        {
            return new Sect(origin);
        }
        TDebug.LogError(string.Format("Sect没有此key:{0}", sectType.ToString()));
        return null;
    }
    List<Sect> ISectFetcher.GetAllSect()
    {
        List<Sect> temp = new List<Sect>();
        foreach (var item in mSectCached)
        {
            temp.Add(new Sect(item.Value));
        }
        if (temp.Count != 0)  return temp;
        TDebug.LogError("Travel表为空");
        return null;
    }
    Loot ILootFetcher.GetLootByCopy(int id)
    {
        Loot origin = null;
        if (mLootCached.TryGetValue(id, out origin))
        {
            return origin.Clone();
        }
        TDebug.LogError(string.Format("Loot没有此id:{0}", id));
        return null;
    }
    Pet IPetFetcher.GetPetByCopy(int id)
    {
        Pet origin = null;
        if (mPetCached.TryGetValue(id, out origin))
        {
            return new Pet(origin);
        }
        TDebug.LogError(string.Format("Pet没有此id:{0}", id));
        return null;
    }

    MapEvent IMapEventFetcher.GetMapEventByCopy(int id)
    {
        MapEvent origin = null;
        if (mMapEventCached.TryGetValue(id, out origin))
        {
            return new MapEvent(origin);
        }
        TDebug.LogError(string.Format("MapEvent没有此id:{0}", id));
        return null;
    }
    Cave ICaveFetcher.GetCaveByCopy(int level)
    {
        Cave origin = null;
        if (mCaveCached.TryGetValue(level, out origin))
        {
            return new Cave(origin);
        }
        TDebug.LogError(string.Format("Cave没有此Level:{0}", level));
        return null;
    }
    int ICaveFetcher.GetMaxCaveLevel()
    {
        List<int> keyList = new List<int>(mCaveCached.Keys);
        if (keyList.Count != 0) 
           return mCaveCached[keyList[keyList.Count - 1]].Level;
        TDebug.LogError(string.Format("Cave表为空"));
        return 0;
    }
    SpellLevelUp ISpellLevelUpFetcher.GetSpellLevelUpByCopy(int level)
    {
        SpellLevelUp origin = null;
        if (mSpellLevelUpCached.TryGetValue(level, out origin))
        {
            return new SpellLevelUp(origin);
        }
        TDebug.LogError(string.Format("SpellLevelUp没有此Level:{0}", level));
        return null;
    }
    PetLevelUp IPetLevelUpFetcher.GetPetLevelUpByCopy(int level)
    {
        PetLevelUp origin = null;
        if (mPetLevelUpCached.TryGetValue(level, out origin))
        {
            return new PetLevelUp(origin);
        }
        TDebug.LogError(string.Format("PetLevelUp没有此Level:{0}", level));
        return null;
    }

    ErrorStatus IErrorStatusFetcher.GetErrorStatusByCopy(int idx)
    {
        ErrorStatus origin = null;
        if (mErrorStatusCached.TryGetValue(idx, out origin))
        {
            return new ErrorStatus(origin);
        }
        TDebug.LogError(string.Format("ErrorStatus没有此Idx:{0}", idx));
        return null;
    }

    GameConst IGameConstFetcher.GetGameConstNoCopy(string key)
    {
        GameConst origin = null;
        if (mGameConstCached.TryGetValue(key, out origin))
        {
            return origin;
        }
        TDebug.LogError(string.Format("GameConst没有此key:{0}", key));
        return null;
    }
    PropLevelUp IPropLevelUpFetcher.GetPropLevelUpByCopy(int level,PropLevelUp.PropLevelType type)
    {
        PropLevelUp origin = null;
        int key = PropLevelUp.GetCachedKey(level,type);
        if (mPropLevelUpCached.TryGetValue(key, out origin))
        {
            return new PropLevelUp(origin);
        }
        TDebug.LogError(string.Format("PropLevelUp没有此key:{0},Type:{1}", level,type));
        return null;
    }

    Shop IShopFetcher.GetShopByCopy(int idx)
    {
        Shop origin = null;
        if (mShopCached.TryGetValue(idx, out origin))
        {
            return new Shop(origin);
        }
        TDebug.LogError(string.Format("Shop没有此Idx:{0}", idx));
        return null;
    }

    Commodity ICommodityFetcher.GetCommodityByCopy(int idx)
    {
        Commodity origin = null;
        if (mCommodityCached.TryGetValue(idx, out origin))
        {
            return new Commodity(origin);
        }
        TDebug.LogError(string.Format("Commodity没有此Idx:{0}", idx));
        return null;
    }

    PrestigeLevel IPrestigeLevelFetcher.GetPrestigeLevelByCopy(int level , PrestigeLevel.PrestigeType ty)
    {
        PrestigeLevel origin = null;
        int key = PrestigeLevel.getKey(level, ty);
        if (mPrestigeLevelCached.TryGetValue(key, out origin))
        {
            return new PrestigeLevel(origin);
        }
        TDebug.LogError(string.Format("PrestigeLevel没有此key:{0}", key));
        return null;
    }
    PrestigeTask IPrestigeTaskFetcher.GetPrestigeTaskByCopy(int idx)
    {
        PrestigeTask origin = null;
        if (mPrestigeTaskCached.TryGetValue(idx, out origin))
        {
            return new PrestigeTask(origin);
        }
        TDebug.LogError(string.Format("PrestigeTask没有此Idx:{0}", idx));
        return null;
    }

    Tower ITowerFetcher.GetTowerByCopy(int floorNum)
    {
        Tower origin = null;
        if (mTowerCached.TryGetValue(floorNum, out origin))
        {
            return new Tower(origin);
        }
        TDebug.LogError(string.Format("Tower没有此floorNum:{0}", floorNum));
        return null;
    }
    List<Tower> ITowerFetcher.GetSpeRewardTowersNoCopy()
    {
        List<Tower> speList = new List<Tower>();
        foreach (var item in mTowerCached)
        {
            if (item.Value.SpeReward>0)
                speList.Add(item.Value);
        }
        return speList;
    }
    List<Achievement> IAchievementFetcher.GetAchievementsByCopy(Achievement.AchieveType type)
    {
        List<Achievement> achList = new List<Achievement>();
        foreach (var item in mAchievementCached)
        {
            if (item.Value.MyAchieveType == type)
            {
                achList.Add(item.Value);
            }
        }
        return achList;
    }
    List<Achievement> IAchievementFetcher.GetAchievementsByCopy(Achievement.AchieveSubType type)
    {
        List<Achievement> achList = new List<Achievement>();
        foreach (var item in mAchievementCached)
        {
            if (item.Value.SubType == type)
            {
                achList.Add(item.Value);
            }
        }
        return achList;
    }
    Achievement IAchievementFetcher.GetAchievementByCopy(int idx)
    {
        Achievement origin = null;
        if (mAchievementCached.TryGetValue(idx, out origin))
        {
            return new Achievement(origin);
        }
        TDebug.LogError(string.Format("Achievement没有此idx:{0}", idx));
        return null;
    }
    AchieveReward IAchieveRewardFetcher.GetAchieveRewardByCopy(int point)
    {
        AchieveReward origin = null;
        if (mAchieveRewardCached.TryGetValue(point, out origin))
        {
            return new AchieveReward(origin);
        }
        TDebug.LogError(string.Format("AchievementReward没有此point:{0}", point));
        return null;
    }
    List<AchieveReward> IAchieveRewardFetcher.GetAchieveRewardAllByCopy()
    {
        List<AchieveReward> achRewardList = new List<AchieveReward>();
        foreach (var item in mAchieveRewardCached)
        {         
            achRewardList.Add(item.Value);
        }
        return achRewardList;
    }
    BookSpell IBookSpellFetcher.GetBookSpellByCopy(int idx)
    {
        BookSpell origin = null;
        if (mBookSpellCached.TryGetValue(idx, out origin))
        {
            return new BookSpell(origin);
        }
        TDebug.LogError(string.Format("BookSpell没有此idx:{0}", idx));
        return null;
    }

    HeadIcon IHeadIconFetcher.GetHeadIconByCopy(int idx)
    {
        HeadIcon origin = null;
        if (mHeadIconCached.TryGetValue(idx, out origin))
        {
            return new HeadIcon(origin);
        }
        TDebug.LogError(string.Format("HeadIcon没有此idx:{0}", idx));
        return null;
    }
    #endregion

}
