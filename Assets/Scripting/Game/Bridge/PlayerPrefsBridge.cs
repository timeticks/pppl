using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using LitJson;

/// <summary>
/// 此类用于控制玩家数据的增删改查
/// 玩家的数据包含玩家基础数据.城建数据,背包数据等一切游戏基础数据
/// </summary>
public partial class PlayerPrefsBridge
{
    #region 单例

    private static PlayerPrefsBridge mInstance;

    public static PlayerPrefsBridge Instance
    {
        get { return mInstance; }
    }

    #endregion

    


    private GamePlayer mPlayerData = new GamePlayer();
    public GamePlayer PlayerData { get { return mPlayerData; } }
    public GuideAccessor GuideData { get { return mGuideAccessor; } }
    private GuideAccessor mGuideAccessor = new GuideAccessor();

    public MailAccessor MailData { get { return mMailAccessor; } }
    private MailAccessor mMailAccessor = new MailAccessor();

    public ActivityAccessor ActivityData { get { return mActivityAccessor; } }
    private ActivityAccessor mActivityAccessor = new ActivityAccessor();

    public BattleRecordAccessor BattleRecordAcc { get { return mBattleRecordAccessor; } }
    private BattleRecordAccessor mBattleRecordAccessor = new BattleRecordAccessor();

    private InventoryList mSpellInventory = new InventoryList(InventoryList.InventoryType.Spells);
    private InventoryList mItemsInventory = new InventoryList(InventoryList.InventoryType.Items);
    private InventoryList mEquipsInventory = new InventoryList(InventoryList.InventoryType.Equips);
    private InventoryList mPetsInventory = new InventoryList(InventoryList.InventoryType.Pets);
    private InventoryList mRecipesInventory = new InventoryList(InventoryList.InventoryType.Recipes);
    private DungeonMapAccessor mDungeonMap = new DungeonMapAccessor();
    private MapEventAccessor mMapEventAccessor = new MapEventAccessor();
    private OldProduceAccessor mProduceAccessor = new OldProduceAccessor();//生产信息
    private ShopAccessor mShopAccessor = new ShopAccessor(); //商店限制购买问题
    private AchievementAccessor mAchievementAccessor = new AchievementAccessor();

    private PartnerAccessor mPartnerAccessor = new PartnerAccessor();
    public PartnerAccessor PartnerAcce { get { return mPartnerAccessor; } }

    public BallMapAccessor BallMapAcce { get { return mBallMapAccessor; } }
    private BallMapAccessor mBallMapAccessor = new BallMapAccessor();


    #region 初始化，消息注册
    private PlayerPrefsBridge() { }

    public static PlayerPrefsBridge CreateInstance()
    {
        mInstance = new PlayerPrefsBridge();
        mInstance.Init();
        return mInstance;
    }

    private void Init()
    {
        
        
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotTime, S2C_SnapshotTime);//时间同步

        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotPlayerAttribute, S2C_SnapshotPlayerAttribute);

        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotDungeonMap, S2C_SnapshotDungeonMap);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotSect, S2C_SnapshotSect);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotTravelBotting, S2C_SnapshotTravelBotting);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotAuxSkill, S2C_SnapshotAuxSkill);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotTravelEvent, S2C_SnapshotTravelEvent);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotTravelHistory, S2C_SnapshotTravelHistory);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapShotProduce, S2C_SnapShotProduce);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapShotProduceBotting, S2C_SnapShotProduceBotting);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotMapEvent, S2C_SnapshotMapEvent);
        
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotPlayerName, S2C_SnapshotPlayerName);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotBattleRecord, S2C_SnapshotBattleRecord);

        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotMail, S2C_SnapshotMail);

        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotPrestigeTask, S2C_SnapshotPrestigeTask);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotPrestigeTaskDetail, S2C_SnapshotPrestigeTaskDetail);

        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotShopInfo, S2C_SnapshotShopInfo);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotBuyShopInfo, S2C_SnapshotBuyShopInfo);

        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotAchieve, S2C_SnapshotAchieve);
        //GameClient.Instance.RegisterNetCodeHandler(NetCode_S.SnapshotAchieveFinish, S2C_SnapshotAchieveFinish);
    }

    public void LoadPlayer()
    {
        string savePlayer = SaveUtils.GetGameSave(GameSaveType.Player.ToString());
        //if (savePlayer.Length > 0)
        //{
        //    string tempSave = "";
        //    mBallMapAccessor = JsonMapper.ToObject<BallMapAccessor>(SaveUtils.GetGameSave(GameSaveType.BallMap.ToString()));
        //    mPlayerData = JsonMapper.ToObject<GamePlayer>(savePlayer);
        //    mItemsInventory.Read(SaveUtils.GetGameSave(GameSaveType.InvItem.ToString(), ""));

        //    tempSave = SaveUtils.GetGameSave(GameSaveType.Partner.ToString());
        //    if(tempSave.Length>0)
        //        mPartnerAccessor = JsonMapper.ToObject<PartnerAccessor>(tempSave);
        //}
        //else
        {
            CreateNewPlayer();
        }
    }

    void CreateNewPlayer()
    {
        mPlayerData = new GamePlayer();
        mPlayerData.Level = 1;
        mPlayerData.Hero = Hero.Fetcher.GetHeroCopy(1001);
        mPlayerData.name = "菜鸟";
        mPlayerData.PlayerUid = 10000 + 1;
        mPlayerData.BirthTime = AppTimer.CurTimeStampSecond;
        mSpellInventory = new InventoryList(InventoryList.InventoryType.Spells);
        mItemsInventory = new InventoryList(InventoryList.InventoryType.Items);
        addItem(105003001, 100, "");
        addItem(105003002, 100, "");
        mEquipsInventory = new InventoryList(InventoryList.InventoryType.Equips);
        mPetsInventory = new InventoryList(InventoryList.InventoryType.Pets);
        saveGame();
    }


    #endregion

    #region 服务器消息

    private void S2C_SnapshotPlayerName(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotPlayerName msg = MessageBridge.Instance.S2C_SnapshotPlayerName(ios);
        if (msg.Name == ""||msg.Name==string.Empty)
        {
            TDebug.LogError("S2C_SnapshotPlayerName错误，玩家名为空");
            return;
        }
        mPlayerData.name = msg.Name;
    }


    private void S2C_SnapshotPlayerAttribute(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotPlayerAttribute msg = MessageBridge.Instance.S2C_SnapshotPlayerAttribute(ios);
        long variation = 0;
        long oldData = 0;
        //TDebug.Log(string.Format("{0}:{1}", msg.Type.ToString(), msg.Attributes[0]));
        switch (msg.Type)
        {
            case PlayerAttribute.Gold:
            {
                oldData = mPlayerData.Gold;
                mPlayerData.Gold = msg.Attributes[0];
                variation = mPlayerData.Gold;
                if (mPlayerData.Gold - oldData>0)
                    FreshAchieveProgress(AchievementAccessor.ClientCountType.AddGold, (int)(mPlayerData.Gold - oldData));
                else
                    FreshAchieveProgress(AchievementAccessor.ClientCountType.ConsumeGold, (int)(mPlayerData.Gold - oldData));
                break;
            }
            case PlayerAttribute.Diamond:
            {
                oldData = mPlayerData.Diamond;
                mPlayerData.Diamond = msg.Attributes[0];
                variation = mPlayerData.Diamond;
                FreshAchieveProgress(AchievementAccessor.ClientCountType.ConsumeDiamond, (int)(mPlayerData.Diamond - oldData));
                break;
            }
            case PlayerAttribute.Potential:
            {
                mPlayerData.Potential = msg.Attributes[0];
                variation = mPlayerData.Potential;
                break;
            }
            case PlayerAttribute.Exp:
            {
                mPlayerData.Exp = msg.Attributes[0];
                variation = mPlayerData.Exp;
                break;
            }
            case PlayerAttribute.Level:
            {

                if (mPlayerData.Level < msg.Attributes[0])
                {
                    mPlayerData.Level = msg.Attributes[0];
                    variation = mPlayerData.Level;
                    AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.LevelUp, msg.Attributes[0].ToString()));

                    List<Travel> travelList = Travel.TravelFetcher.GetTravelList();
                    for (int i = 0, length = travelList.Count; i < length; i++)
                    {
                        if (travelList[i].Level == msg.Attributes[0]) //解锁新的挂机地点
                        {
                            AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.TravelNewSite));
                            SaveUtils.SetIntInPlayer(EvtListenerType.TravelNewSite.ToString(), 1);
                        }
                    }
                }
                else
                {
                    mPlayerData.Level = msg.Attributes[0];
                    variation = mPlayerData.Level;
                }
                AppEvtMgr.Instance.SendNotice(new EvtItemData(EvtType.CurLevel, msg.Attributes[0].ToString()));
                break;
            }
            case PlayerAttribute.IsSetName:
            {
                mPlayerData.IsSetName = msg.Attributes[0] == 1;
                break;
            }
            case PlayerAttribute.IsFinishNewerMap:
            {
                mPlayerData.IsFinishNewerMap = msg.Attributes[0] == 1;
                break;
            }
        }
        if (LobbySceneMainUIMgr.Instance != null) LobbySceneMainUIMgr.Instance.FreshTopRoleInfo();
        TDebug.Log(string.Format("同步角色属性|类型:{0},当前值:{1},增量值:{2}", msg.Type, variation, msg.Attributes[0]));
    }

    private void S2C_SnapshotTime(BinaryReader ios)//服务器时间同步
    {
        NetPacket.S2C_SnapshotTime msg = MessageBridge.Instance.S2C_SnapshotTime(ios);
        AppTimer.SetCurStamp(msg.time);
    } 


   
    #endregion


    #region 旧

    #region 战斗

    public Hero GetHeroWithProperties()
    {
        GamePlayer player = PlayerPrefsBridge.Instance.PlayerData;
        Hero hero = player.Hero.Clone();
        List<Spell> allSpellList = PlayerPrefsBridge.Instance.GetSpellAllListCopy();
        Spell[] equipSpells = PlayerPrefsBridge.Instance.GetSpellBarInfo();
        List<Spell> equipSpellList = new List<Spell>();
        for (int i = 0; i < equipSpells.Length; i++)
        {
            if (equipSpells[i] != null) equipSpellList.Add(equipSpells[i]);
        }

        Equip[] equipList = PlayerPrefsBridge.Instance.GetEquipBarInfo();
        //Pet[] petList = PlayerPrefsBridge.Instance.GetPetBarInfo();
        hero.Properties(equipList, equipSpellList, player.AddProm, null, player.Level);
        return hero;
    }

    #endregion

    #region 技能


    private int AddSpell(int spellIdx)
    {
        return AddSpell(Spell.Fetcher.GetSpellCopy(spellIdx));
    }

    /// <summary>
    /// 往背包中添加一个技能
    /// </summary>
    /// <returns>背包中的位置</returns>
    private int AddSpell(Spell spell)
    {
        if (spell == null)
        {
            TDebug.LogWarning("添加技能失败,技能为空");
            return -1;
        }

        return mSpellInventory.AddGoods(spell);
    }


    /// <summary>
    /// 获取背包中的技能
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public Spell GetSpellByIdx(int idx)
    {
        BaseObject spell = mSpellInventory.GetGoodsById(idx);
        if (spell == null)
        {
            TDebug.LogWarning("获取背包中的技能失败,技能不存在 | idx=" + idx);
        }
        return (Spell)spell;
    }


    /// <summary>
    /// 返回背包中的位置
    /// </summary>
    public int GetSpellIndexOf(int id)
    {
        return mSpellInventory.IndexOf(id);
    }
    private List<Spell> GetSpellAllList(bool isCopy)//得到所有的招式列表
    {
        List<Spell> skillList = new List<Spell>();
        int count = mSpellInventory.GetListLength();
        for (int i = 0; i < count; i++)
        {
            Spell item = (Spell)mSpellInventory.GetGoodsByPos(i); //获取已穿戴的功法信息
            if (item != null)
            {
                if (isCopy) skillList.Add(item.Clone());
                else skillList.Add(item);
            }
        }
        return skillList;
    }

    /// <summary>
    /// 获取穿戴/未穿戴的招式列表
    /// </summary>
    /// <param name="isWearList">true返回穿戴列表；false返回未穿戴</param>
    /// <param name="isCopy">是否需要深复制</param>
    /// <returns></returns>
    private List<Spell> GetSpellList(bool isWearList, bool isCopy)
    {
        List<Spell> spellAllList = GetSpellAllList(false);
        List<Spell> spellList = new List<Spell>();
        for (int i = 0; i < spellAllList.Count; i++)
        {
            bool isWeared = false;
            for (int j = 0; j < mPlayerData.SpellList.Length; j++)
            {
                if (spellAllList[i].idx == mPlayerData.SpellList[j] && mPlayerData.SpellList[j] != 0)
                {
                    isWeared = true; break;
                }
            }
            if (isWeared == isWearList)//如果isNeedWearList=true，则返回穿戴列表
            {
                if (isCopy) { spellList.Add(spellAllList[i].Clone()); }
                else { spellList.Add(spellAllList[i]); }
            }
        }
        return spellList;
    }
    /// <summary>
    /// 获取穿戴/未穿戴技能
    /// </summary>
    public List<Spell> GetSpellListCopy(bool isWearList)
    {
        return GetSpellList(isWearList, true);
    }
    /// <summary>
    /// 获取所有技能
    /// </summary>
    /// <returns></returns>
    public List<Spell> GetSpellAllListCopy()
    {
        return GetSpellAllList(true);
    }
    /// <summary>
    /// 返回技能栏信息
    /// </summary>
    /// <returns></returns>
    public Spell[] GetSpellBarInfo()
    {
        Spell[] equipSpell = new Spell[(int)Spell.PosType.Max];
        for (int i = 0; i < mPlayerData.SpellList.Length; i++)
        {
            if (mPlayerData.SpellList[i] != (int)Spell.PosType.Max)
            {
                equipSpell[i] = GetInventorySpellByPos(mPlayerData.SpellList[i]);
            }
            else
                equipSpell[i] = null;
        }
        return equipSpell;
    }
    /// <summary>
    /// 查询技能在技能装备栏中的位置
    /// </summary>
    /// <param name="spell"></param>
    /// <returns></returns>
    public int GetSpellPosInEquiped(Spell spell)
    {
        Spell[] equipSpell = GetSpellBarInfo();
        for (int i = 0; i < equipSpell.Length; i++)
        {
            if (equipSpell[i] != null && spell.idx == equipSpell[i].idx)
                return i;
        }
        TDebug.LogError(string.Format("在技能装备栏中未查询到技能{0}", spell.idx.ToString()));
        return 0;
    }
    /// <summary>
    /// 获取当前装备栏位置上的技能
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Spell GetSpellCurWear(Spell.PosType pos)
    {
        int InventoryIndex = mPlayerData.SpellList[(int)pos];
        Spell spell = GetInventorySpellByPos(InventoryIndex);
        if (spell != null)
        {
            return spell;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 根据技能在背包中的位置获取技能
    /// </summary>
    /// <param name="partIdx"></param>
    /// <returns></returns>
    public Spell GetInventorySpellByPos(int partIdx)
    {//getIventorySpellsByPos
        Spell spell = (Spell)mSpellInventory.GetGoodsByPos(partIdx);
        if (spell != null)
        {
            return spell.Clone();
        }
        return null;
    }

    public int GetSpellNumByLevel(int level) //得到1级或满级的功法数量
    {
        List<Spell> spellList = GetSpellAllList(true);
        int spellNum = 0;
        Spell spell = null;
        //for (int i = 0, length = spellList.Count; i < length; i++)
        //{
        //    spell = new Skill(spellList[i]); 
        //    if (spell == null) continue;
        //    if (spell.GetOffsetLevel() >= level)
        //        spellNum++;
        //}
        return spellNum;
    }


    #endregion

    #region 法宝


    private List<Equip> GetEquipAllList(bool isCopy)//得到所有的法宝列表
    {
        List<Equip> equipList = new List<Equip>();
        int count = mEquipsInventory.GetListLength();
        for (int i = 0; i < count; i++)
        {
            Equip item = (Equip)mEquipsInventory.GetGoodsByPos(i); //获取已穿戴的功法信息
            if (item != null)
            {
                if (isCopy) equipList.Add(item.Clone());
                else equipList.Add(item);
            }
            else
            {
                equipList.Add(null);
            }
        }
        return equipList;
    }
    public List<Equip> GetEquipAllListNoCopy()
    {
        return GetEquipAllList(false);
    }


    public Equip GetInventoryEquipByPos(int pos, bool isCopy = false)
    {//getIventoryEquipsByPos
        Equip equip = (Equip)mEquipsInventory.GetGoodsByPos(pos);
        if (equip != null && equip.idx != 0)
        {
            if (isCopy) return equip.Clone();
            else return equip;
        }
        return null;
    }
    public int GetEquipPos(Equip equip)
    {
        return mEquipsInventory.IndexOf(equip);
    }
    public Equip[] GetEquipBarInfo()
    {
        Equip[] equipEquip = new Equip[(int)Equip.EquipType.Max];
        for (int i = 0; i < mPlayerData.EquipList.Length; i++)
        {
            if (mPlayerData.EquipList[i] != (int)Equip.EquipType.None)
            {
                equipEquip[i] = GetInventoryEquipByPos(mPlayerData.EquipList[i]);
            }
            else
                equipEquip[i] = null;
        }
        return equipEquip;
    }

    public Equip RemovEquipByPos(int pos)
    {
        Equip removeEquip = (Equip)mEquipsInventory.ReplaceGoodsOnPos(pos, new Equip());
        if (removeEquip == null || removeEquip.idx == 0)
        {
            TDebug.LogWarning("移除背包中的装备失败,装备不存在 | pos=" + pos);
            return null;
        }
        return (Equip)removeEquip;
    }

    #endregion

    #region 宠物

    private Pet AddPet(Pet pet, int pos)
    {
        return null;
        //if (pet == null)
        //{
        //    TDebug.LogError("添加宠物失败，宠物为空");
        //    return null;
        //}
        //return (Pet)mPetsInventory.AddGoodsOnPos(pet,pos);
    }
    public Pet RemovPetByPos(int pos)
    {
        return null;
        //Pet removePet = (Pet)mPetsInventory.RemoveGoodsByPos(pos);
        //if (removePet == null)
        //{
        //    TDebug.LogWarning("移除背包中的宠物失败,装备不存在 | pos=" + pos);
        //    return null;
        //}
        //return (Pet)removePet;
    }

    public int GetPetIndexOf(int id)
    {
        return mPetsInventory.IndexOf(id);
    }
    private List<Pet> GetPetAllList(bool isCopy)
    {
        List<Pet> petList = new List<Pet>();
        //int count = mPetsInventory.GetListLength();
        //for (int i = 0; i < count; i++)
        //{
        //    Pet pet = (Pet)mPetsInventory.GetGoodsByPos(i);
        //    if(pet!=null)
        //    {
        //        if (isCopy) petList.Add(new Pet(pet));
        //        else petList.Add(pet);
        //    }
        //}
        return petList;
    }
    public List<Pet> GetPetAllListCopy()
    {
        return GetPetAllList(true);
    }
    public Pet GetPetByPosInventory(int index)
    {
        //Pet pet = (Pet)mPetsInventory.GetGoodsByPos(index);
        //if (pet != null)
        //{
        //    return new Pet(pet);
        //}
        return null;
    }

    public Pet[] GetPetBarInfo()
    {
        Pet[] equipPet = new Pet[(int)Pet.PetTypeEnum.Max];
        for (int i = 0; i < mPlayerData.PetList.Length; i++)
        {
            if (mPlayerData.PetList[i] != (int)Pet.PetTypeEnum.None)
            {
                equipPet[i] = GetPetByPosInventory(mPlayerData.PetList[i]);
            }
            else
                equipPet[i] = null;
        }
        return equipPet;
    }
    public int GetPetNumByType(Pet.PetTypeEnum petType, int levelMin) //得到某等级以上的某种灵兽
    {
        List<Pet> petList = GetPetAllListCopy();
        int petNum = 0;
        Pet pet = null;
        for (int i = 0, length = petList.Count; i < length; i++)
        {
            pet = petList[i];
            if (pet == null) continue;
            if (pet.Type == petType && pet.Level + pet.CurLevel >= levelMin)
                petNum++;
        }
        return petNum;
    }
    #endregion

    #region 道具

    /// <summary>
    /// 往背包中添加一个道具
    /// </summary>
    private int AddItem(Item item)
    {
        if (item == null)
        {
            TDebug.LogWarning("添加道具失败,道具为空");
            return -1;
        }
        return mItemsInventory.AddGoods(item);
    }

    /// <summary>
    /// 往背包中添加一个道具
    /// </summary>
    private Item AddItemOnPos(Item Item, int pos)
    {
        if (Item == null)
        {
            TDebug.LogWarning("添加道具失败,道具为空");
            return null;
        }

        return (Item)mItemsInventory.AddGoodsOnPos(Item, pos);
    }

    /// <summary>
    /// 往背包中添加一个
    /// </summary>
    private Item AddItemOnPos(int idx, int pos)
    {
        Item item = Item.Fetcher.GetItemCopy(idx);
        return AddItemOnPos(item, pos);
    }

    public Item GetItemByPos(int pos)
    {
        BaseObject item = mItemsInventory.GetGoodsByPos(pos);
        if (item == null)
        {
            TDebug.LogWarning("获取背包中的道具失败,道具不存在 | pos=" + pos);
            return null;
        }
        return (Item)item;
    }

    public Item RemoveItemByPos(int pos)
    {
        BaseObject item = mItemsInventory.RemoveGoodsByPos(pos);
        if (item == null)
        {
            TDebug.LogWarning("移除背包中的道具失败,道具不存在 | pos=" + pos);
            return null;
        }
        return (Item)item;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public Item GetItemByIdx(int idx)
    {
        BaseObject item = mItemsInventory.GetGoodsById(idx);
        if (item == null)
        {
            TDebug.LogWarning("获取背包中的道具失败,道具不存在 | idx=" + idx);
            return null;
        }
        return (Item)item;
    }

    public int GetItemNum(int idx)
    {
        BaseObject item = mItemsInventory.GetGoodsById(idx);
        if (item == null)
        {
            return 0;
        }
        return ((Item)item).num;
    }
    /// <summary>
    /// 返回背包中的位置
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    public int IndexOfItem(int idx)
    {
        return mItemsInventory.IndexOf(idx);
    }


    private List<Item> GetItemAllList(bool isCopy)
    {
        List<Item> itemList = new List<Item>();
        int count = mItemsInventory.GetListLength();
        Item item = null;
        for (int i = 0; i < count; i++)
        {
            item = (Item)mItemsInventory.GetGoodsByPos(i);
            if (item != null)
            {
                if (isCopy) { itemList.Add(item.Clone()); }
                else { itemList.Add(item); }
            }
        }
        return itemList;
    }


    /// <summary>
    /// 获取道具列表
    /// </summary>
    /// <param name="isCopy">是否需要深复制</param>
    private List<Item> GetItemList(bool isCopy)
    {
        List<Item> itemList = new List<Item>();
        //List<Item> itemAllList = GetItemAllList(false);
        //for (int i = 0; i < itemAllList.Count; i++)
        //{
        //    if (isCopy) { itemList.Add(new Item(itemAllList[i])); }
        //    else { itemList.Add(itemAllList[i]); }
        //}
        return itemList;
    }

    public List<Item> GetItemAllListCopy()
    {
        return GetItemAllList(true);
    }

    #endregion

    #region 秘境
    //////////////////////////////////////////秘境//////////////////////////////////////////////////
    private void S2C_SnapshotDungeonMap(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotDungeonMap msg = MessageBridge.Instance.S2C_SnapshotDungeonMap(ios);

        mDungeonMap = msg.DungeonMap;
        if (mDungeonMap.IsEntered)
        {
        }
        else
        {
        }
    }

    //保存秘境信息
    public void SetMapSave(List<NetMapSaveItem> saveList, System.Action setOverCallback, DungeonMapData.MapType mapType)
    {
        TDebug.Log("SetMapSave");
        if (saveList == null || saveList.Count == 0) return;
        bool needWaitHandler = true;
        if (saveList.Count == 1 && saveList[0].SaveType == DungeonMapAccessor.MapSaveType.RolePos) needWaitHandler = false;
        //if (mapType == MapData.MapType.NewerMap) needWaitHandler = false; //新手秘境，也不保存在服务器

        mDungeonMap.SetMapSave(saveList); //内存修改

        ServPacketHander hander;
        if (needWaitHandler)
        {
            hander = delegate(BinaryReader ios)
            {
                UIRootMgr.Instance.IsLoading = false;
                if (setOverCallback != null) setOverCallback();
                setOverCallback = null;
            };
            UIRootMgr.Instance.IsLoading = true;
        }
        else  //如果是存位置或不存服务器
        {
            hander = delegate(BinaryReader ios)
            {
                if (setOverCallback != null)
                    setOverCallback();
                setOverCallback = null;
            };
        }

        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MapSave, hander);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_MapSave(saveList, mDungeonMap.RolePos));
    }
    public void SetMapSave(DungeonMapAccessor.MapSaveType saveType, int saveId, int saveValue)
    {
        NetMapSaveItem saveItem = new NetMapSaveItem(saveType, saveId, saveValue);
        SetMapSave(new List<NetMapSaveItem>() { saveItem }, null, DungeonMapData.MapType.SectMap);
    }

    //获取某一个值，返回是否成功
    public bool GetMapSaveCondition(DungeonMapAccessor.MapSaveType saveType, int saveId, out int saveValue)
    {
        saveValue = 0;
        if (mDungeonMap == null || !mDungeonMap.IsEntered)
            return false;
        switch (saveType)
        {
            case DungeonMapAccessor.MapSaveType.Event:
                if (mDungeonMap.EventDic.ContainsKey(saveId))
                {
                    saveValue = mDungeonMap.EventDic[saveId];
                    return true;
                }
                return false;
            case DungeonMapAccessor.MapSaveType.Item:
                if (mDungeonMap.ItemDic.ContainsKey(saveId))
                {
                    saveValue = mDungeonMap.ItemDic[saveId];
                    return true;
                }
                return false;
            case DungeonMapAccessor.MapSaveType.Npc:
                if (mDungeonMap.NpcDic.ContainsKey(saveId))
                {
                    saveValue = mDungeonMap.NpcDic[saveId];
                    return true;
                }
                return false;
            case DungeonMapAccessor.MapSaveType.RolePos:
                saveValue = SaveUtils.GetIntInPlayer(PrefsSaveType.RolePos.ToString());
                return true;
        }
        return false;
    }


    public DungeonMapAccessor GetDungeonMapCopy()
    {
        if (mDungeonMap != null)
            return new DungeonMapAccessor(mDungeonMap);
        return null;
    }

    public MapEventAccessor GetMapEventAccessorCopy()
    {
        return new MapEventAccessor(mMapEventAccessor);
    }

    public List<int> GetCanRewardList()
    {
        return mMapEventAccessor.GetCanRewardEvents();
    }

    public bool IsMapEventCanReward()
    {
        foreach (var temp in mMapEventAccessor.EventStatusPool)
        {
            if (temp.Value == MapEvent.MapEventStatus.CanReaward)
                return true;
        }
        return false;
    }


    public void S2C_SnapshotMapEvent(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotMapEvent msg = MessageBridge.Instance.S2C_SnapshotMapEvent(ios);
        if (msg.MapId != 0)
            mMapEventAccessor.MapList.Add(msg.MapId); //将已同步的地图id存入，之后不再重复
        foreach (var temp in msg.EventStatusPool)
        {
            if (mMapEventAccessor.EventStatusPool.ContainsKey(temp.Key))
            {
                mMapEventAccessor.EventStatusPool[temp.Key] = temp.Value;
            }
            else
            {
                mMapEventAccessor.EventStatusPool.Add(temp.Key, temp.Value);
            }
        }

    }


    public List<Item> GetMapItemList()
    {
        List<Item> itemList = new List<Item>();
        //if (mDungeonMap == null) return itemList;
        //Item item;
        //foreach (var temp in mDungeonMap.ItemDic)
        //{
        //    item = Item.ItemFetcher.GetItemByCopy(temp.Key);
        //    item.curNum = temp.Value;
        //    itemList.Add(item);
        //}
        return itemList;
    }

    #endregion

    #region 门派
    //////////////////////////门派////////////////////////////
    private void S2C_SnapshotSect(BinaryReader ios)
    {
        TDebug.Log("S2C_SnapshotSect");
        NetPacket.S2C_SnapshotSect msg = MessageBridge.Instance.S2C_SnapshotSect(ios);
        mPlayerData.MySect = msg.SectType;
        if (LobbySceneMainUIMgr.Instance != null) LobbySceneMainUIMgr.Instance.FreshLobbyInfo();
    }


    #endregion

    #region 生产制作

    public void S2C_SnapShotProduceBotting(BinaryReader ios)
    {
        NetPacket.S2C_SnapShotProduceBotting msg = MessageBridge.Instance.S2C_SnapShotProduceBotting(ios);
        mProduceAccessor = new OldProduceAccessor(msg);
        OnSnapShotHappend(PrefsSnapShotEventType.SnapShotProduceBotting);
    }
    public void S2C_SnapShotProduce(BinaryReader ios)
    {
        NetPacket.S2C_SnapShotProduce msg = MessageBridge.Instance.S2C_SnapShotProduce(ios);
        bool isExist = false;
        for (int i = 0; i < mProduceAccessor.ProduceList.Count; i++)
        {
            if (mProduceAccessor.ProduceList[i].RecipeID == msg.ProduceItem.RecipeID)
            {
                isExist = true;
                mProduceAccessor.ProduceList[i] = msg.ProduceItem;
            }
        }
        if (!isExist)
            mProduceAccessor.ProduceList.Add(msg.ProduceItem);
        TDebug.Log("S2C_SnapShotProduce==" + (msg.ProduceItem.FinishTime - AppTimer.CurTimeStampMsSecond) + "==" + msg.ProduceItem.RecipeID + "==" + msg.ProduceItem.MyRecipeType);
        OnSnapShotHappend(PrefsSnapShotEventType.SnapShotProduce);
    }

    public ProduceItem GetProduceItem(AuxSkillLevel.SkillType type)
    {
        ProduceItem item = null;
        for (int i = 0; i < mProduceAccessor.ProduceList.Count; i++)
        {
            if ((int)type == (int)mProduceAccessor.ProduceList[i].MyRecipeType)
            {
                item = mProduceAccessor.ProduceList[i];
            }
        }
        return item;
    }
    public ProduceItem GetProduceItem(int recipeID)
    {
        ProduceItem item = null;
        for (int i = 0; i < mProduceAccessor.ProduceList.Count; i++)
        {
            if (recipeID == mProduceAccessor.ProduceList[i].RecipeID)
            {
                item = mProduceAccessor.ProduceList[i];
            }
        }
        return item;
    }
    /// <summary>
    /// 获取当前配方类型正在制作中的配方
    /// </summary>
    /// <param name="recipeID"></param>
    /// <returns></returns>
    public ProduceItem GetRecipeCurTypeProduce(int recipeID)
    {
        ProduceItem item = null;
        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(recipeID);
        for (int i = 0; i < mProduceAccessor.ProduceList.Count; i++)
        {
            if (recipe.Type == mProduceAccessor.ProduceList[i].MyRecipeType)
            {
                item = mProduceAccessor.ProduceList[i];
            }
        }
        return item;
    }

    public bool IsProduceFinish()
    {
        for (int i = 0; i < mProduceAccessor.ProduceList.Count; i++)
        {
            if (mProduceAccessor.ProduceList[i].RecipeID == 0) continue;
            if (mProduceAccessor.ProduceList[i].FinishTime <= AppTimer.CurTimeStampMsSecond)
                return true;
        }
        return false;
    }

    public OldProduceAccessor GetProduceAccessor()
    {
        return mProduceAccessor;
    }


    #endregion

    #region 邮件
    public void S2C_SnapshotMail(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotMail msg = MessageBridge.Instance.S2C_SnapshotMail(ios);
        mMailAccessor.Inited = true;
        mMailAccessor.MailAmount = msg.MailAmount;
        int removeMailIdx = msg.RemoveMail;
        if (msg.ReplaceOrAdd)
        {
            for (int i = 0; i < msg.MailList.Count; i++)
            {
                bool idxHave = false;
                for (int j = 0; j < mMailAccessor.MailList.Count; j++)
                {
                    if (mMailAccessor.MailList[j].idx == msg.MailList[i].idx)
                    {
                        mMailAccessor.MailList[j] = msg.MailList[i];
                        idxHave = true;
                        break;
                    }
                }
                if (!idxHave) mMailAccessor.MailList.Add(msg.MailList[i]);
            }

        }
        else
        {
            for (int i = 0; i < msg.MailList.Count; i++)
            {
                mMailAccessor.MailList.Add(msg.MailList[i]);
            }
        }
        if (msg.RemoveMail > 0) //移除特定邮件
        {
            for (int i = 0; i < mMailAccessor.MailList.Count; i++)
            {
                if (mMailAccessor.MailList[i].idx == removeMailIdx)
                {
                    mMailAccessor.MailList.RemoveAt(i); break;
                }
            }
        }
        //排序，未打开的在前，idx大的在前
        mMailAccessor.MailList.Sort((x, y) =>
        {
            if (!x.IsOpened && y.IsOpened) return -1;
            else if (x.IsOpened && !y.IsOpened) return 1;
            return y.idx.CompareTo(x.idx);
        });
    }

    public void OpenMailDetail(int mailIdx)
    {
        for (int i = 0; i < mMailAccessor.MailList.Count; i++)
        {
            if (mMailAccessor.MailList[i].idx == mailIdx)
            {
                mMailAccessor.MailList[i].IsOpened = true;
            }
        }
    }

    #endregion

    #region 同步信息改变监听
    private Dictionary<PrefsSnapShotEventType, Action> mSnapShotHandlers = new Dictionary<PrefsSnapShotEventType, Action>();
    private Dictionary<PrefsSnapShotEventType, Action<object>> mSnapShotObjectHandlers = new Dictionary<PrefsSnapShotEventType, Action<object>>();

    public void RegisterSnapShotHandler(PrefsSnapShotEventType ty, Action handler)
    {
        if (mSnapShotHandlers.ContainsKey(ty))
        {
            mSnapShotHandlers[ty] += handler;
        }
        else
        {
            mSnapShotHandlers.Add(ty, handler);
        }
    }

    public void RemoveSnapShotHandler(PrefsSnapShotEventType ty, Action handler)
    {
        if (mSnapShotHandlers.ContainsKey(ty))
        {
            mSnapShotHandlers[ty] -= handler;
        }
    }

    public void OnSnapShotHappend(PrefsSnapShotEventType ty)//有对应的同步事件发生
    {
        if (mSnapShotHandlers.ContainsKey(ty) && mSnapShotHandlers[ty] != null)
        {
            mSnapShotHandlers[ty]();
        }
    }
    #endregion

    #region 商店

    public void S2C_SnapshotBuyShopInfo(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotBuyShopInfo msg = MessageBridge.Instance.S2C_SnapshotBuyShopInfo(ios);
        if (mShopAccessor == null) mShopAccessor = new ShopAccessor();
        mShopAccessor.AddShopInfo(msg);
    }
    public void S2C_SnapshotShopInfo(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotShopInfo msg = MessageBridge.Instance.S2C_SnapshotShopInfo(ios);
        mShopAccessor = new ShopAccessor(msg);
    }

    public int GetShopInofDayNum(int commodityId)
    {
        if (mShopAccessor == null)
        {
            return int.MaxValue;
        }
        else
        {
            if (mShopAccessor.EveryDayList.ContainsKey(commodityId))
                return mShopAccessor.EveryDayList[commodityId];
            else
                return 0;
        }
    }

    public int GetShopInofHisNum(int commodityId)
    {
        if (mShopAccessor == null)
        {
            return int.MaxValue;
        }
        else
        {
            if (mShopAccessor.HistoryList.ContainsKey(commodityId))
                return mShopAccessor.HistoryList[commodityId];
            else
                return 0;
        }
    }


    #endregion

    #region 成就

    public void S2C_SnapshotAchieve(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotAchieve msg = MessageBridge.Instance.S2C_SnapshotAchieve(ios);
        mAchievementAccessor.InitAccessor(msg);
    }
    public void S2C_SnapshotAchieveFinish(BinaryReader ios)
    {
        NetPacket.S2C_SnapshotAchieveFinish msg = MessageBridge.Instance.S2C_SnapshotAchieveFinish(ios);
        mAchievementAccessor.FreshAccessor(msg);
    }
    public int GetAchievementInof(int achievementId)
    {
        if (mAchievementAccessor.AchievementInfo.ContainsKey(achievementId))
        {
            return mAchievementAccessor.AchievementInfo[achievementId];
        }
        return -1;
    }
    public int GetAchievementProgess(Achievement.AchieveType type)
    {
        return mAchievementAccessor.AchieveProgress[(int)type];
    }
    public List<Achieve> GetAchievementLastFinish()
    {
        List<Achieve> tempList = new List<Achieve>();
        Achievement ach;
        if (mAchievementAccessor.SecondLastFinish != 0)
        {
            ach = Achievement.AchievementFetcher.GetAchievementByCopy(mAchievementAccessor.SecondLastFinish);
            tempList.Add(new Achieve(ach, mAchievementAccessor.SecondLastFinishPos - 1, true));
        }
        if (mAchievementAccessor.LastFinish != 0)
        {
            ach = Achievement.AchievementFetcher.GetAchievementByCopy(mAchievementAccessor.LastFinish);
            tempList.Add(new Achieve(ach, mAchievementAccessor.LastFinishPos - 1, true));
        }
        return tempList;
    }
    public AchievementAccessor GetAchieveAccessor()
    {
        return new AchievementAccessor(mAchievementAccessor);
    }
    public void FreshAchieveProgress(AchievementAccessor.ClientCountType type, int num)
    {
        switch (type)
        {
            case AchievementAccessor.ClientCountType.AdvertNum:
                mAchievementAccessor.AdvertNum += num;
                break;
            case AchievementAccessor.ClientCountType.AddGold:
                mAchievementAccessor.AddGold += num;
                break;
            case AchievementAccessor.ClientCountType.ConsumeGold:
                mAchievementAccessor.ConsumeGold += num;
                break;
            case AchievementAccessor.ClientCountType.ConsumeDiamond:
                if (num < 0)
                    mAchievementAccessor.ConsumeDiamond += num;
                break;
            case AchievementAccessor.ClientCountType.LevelUpPet:
                mAchievementAccessor.LevelUpPet += num;
                break;
            case AchievementAccessor.ClientCountType.EvolvePet:
                mAchievementAccessor.EvolvePet += num;
                break;
        }
    }

    public void FreshPromAchieve()
    {
        //OldHero hero = GetHeroWithProperties();
        //for (int i  = (int)AttrType.Str; i<= (int)AttrType.Luk; i ++)
        //{
        //   if(PullPromAchieve((AttrType)i, hero.GetProm((AttrType)i)))
        //      break;
        //}
    }

    bool PullPromAchieve(AttrType promType, int value)
    {
        List<Achievement> achievementList = Achievement.AchievementFetcher.GetAchievementsByCopy(Achievement.AchieveSubType.Prom);
        Achievement achieve = null;
        for (int i = achievementList.Count - 1; i >= 0; i--)
        {
            if (achievementList[i].ConProm == (int)promType)
            {
                achieve = achievementList[i];
                break;
            }
        }
        if (achieve == null)
            return false;
        if (!mAchievementAccessor.AchievementInfo.ContainsKey(achieve.idx))
        {
            if (achieve.ConValue[0] <= value)
            {
                GameClient.Instance.RegisterNetCodeHandler(NetCode_S.PullInfo, S2C_PullInfo);
                GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullInfo(PullInfoType.PromAchieve, (int)promType));
                UIRootMgr.Instance.IsLoading = true;
                return true;
            }
            return false;
        }
        int index = mAchievementAccessor.AchievementInfo[achieve.idx];
        if (index >= achieve.Names.Length) //成就已做完
            return false;
        else if (achieve.ConValue[index] <= value)
        {
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.PullInfo, S2C_PullInfo);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullInfo(PullInfoType.PromAchieve, (int)promType));
            UIRootMgr.Instance.IsLoading = true;
            return true;
        }
        return false;
    }
    public void S2C_PullInfo(BinaryReader ios)
    {
        NetPacket.S2C_PullInfo msg = MessageBridge.Instance.S2C_PullInfo(ios);
        UIRootMgr.Instance.IsLoading = false;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.PullInfo, null);
    }
    #endregion
    #endregion



}
public enum PrefsSnapShotEventType
{
    SnapshotRole,
    SnapshotSpell,
    SnapShotEquip,
    SnapShotTravelEvent,
    SnapShotTravelBotting,
    SnapShotProduce,
    SnapShotProduceBotting,
    SnapShotRetreat,
    SnapshotBattleRecord,
    SnapshotAssem,
}