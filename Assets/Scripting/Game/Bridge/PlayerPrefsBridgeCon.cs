using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
public partial class PlayerPrefsBridge
{
    public string getName()
    {
        return PlayerData.name;
    }

    public int getIdx()
    {
        return mPlayerData.PlayerIdx;
    }

    public int getLevel()
    {
        return PlayerData.Level;
    }
    public long getExp()
    {
        return PlayerData.Exp;
    }
    public int getGold()
    {
        return PlayerData.Gold;
    }
    public int getDiamond()
    {
        return PlayerData.Diamond;
    }


    public GamePlayer getPlayer()
    {
        return mPlayerData;
    }

    #region 货币及等级
    
    public int addLevel(int num , string action)
    {
        int currentLevel = PlayerData.Level;
        PlayerData.Level = Mathf.Min(currentLevel + num, GameConstUtils.max_level);
        return PlayerData.Level;
    }

    //增加经验
    public long addExp(long num, string action = "")
    {
        if (num > long.MaxValue || num < 0)
        {
            return -1;
        }
        int level = mPlayerData.Level;
        num = calculateAddExp(num, level);
        long currExp = getExp();
        if (num == 0) return currExp;

        PlayerData.Exp += num;

        int levelUpNum = calculateLevelUp(level, PlayerData.Exp);
        if (levelUpNum > 0)
        {
            addLevel(levelUpNum, action);
        }

        TDebug.Log("addExp:" + num);
        return PlayerData.Exp;
    }
    public long consumeExp(int num, string action)
    {
        if (num > int.MaxValue || num < 0)
        {
            return -1;
        }

        long currentExp = getExp();  //不将当前级数经验扣为负

        if (num == 0) return currentExp;

        int level = getLevel();
        HeroLevelUp lastLevelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(level - 1);
        long lastLevelExp = (lastLevelUp == null ? 0 : lastLevelUp.exp);
        num = (int)Mathf.Min(currentExp - lastLevelExp, num);

        if (num > currentExp)
        {
            return -1;
        }
        long remainExp = PlayerData.Exp - num;

        TDebug.Log("consumeExp:" + num);
        return remainExp;
    }

   
    private long calculateAddExp(long addExp, int level)
    {
        HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(level);
        long currExp = getExp();

        if (levelUp.exp > currExp + addExp)
            return addExp;

        //可以一直升到下一次闭关的级数
        int nextRetreatLevel = GameConstUtils.max_level; //(level % 10 == 0) ? level : (level + 10 - level % 10);  //下一次的闭关等级
        HeroLevelUp nextRetreatLevelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(nextRetreatLevel);
        HeroLevelUp lastLevelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(nextRetreatLevel - 1);

        long minExp = nextRetreatLevelUp.exp - lastLevelUp.exp;
        long curMaxExp = nextRetreatLevelUp.exp + minExp;
        if (curMaxExp > currExp + addExp)
            return addExp;
        else
        {
            return (int)(curMaxExp - currExp);
        }
    }

    //判断是否升级
    public int calculateLevelUp(int level, long exp)
    {
        HeroLevelUp levelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(level);

        if (levelUp.exp > exp || levelUp.retreat)
            return 0;
        HeroLevelUp nextLevelUp;
        for (int i = 1; i < 100; i++)
        {
            nextLevelUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(level + i);
            if (nextLevelUp != null)
            {
                if (nextLevelUp.exp > exp || nextLevelUp.retreat)//得到当前经验最大可以升级到的级数
                {
                    return i;
                }
            }
            else
            {
                return i - 1;
            }
        }
        return 0;
    }

    

    //花费金币
    public int consumeGold(int num, string action)
    {
        if (num > int.MaxValue || num < 0)
        {
            return -1;
        }

        int currentGold = getGold();
        if (num == 0) return currentGold;

        if (num > currentGold)
        {
            //LoggerUtil.warn(getIdx(), getName(), "消费金币失败,余额不足", "action:{0},curGold:{1},num:{1}", action, currentGold, num);
            return -1;
        }

        PlayerData.Gold -= num;

        //if (getAchieveUseGold() < int.MaxValue)
        //{
        //    key = MessageFormat.format("{0}{1,number,#}", RedisIndexProperties.REDIS_PLAYER_ACHIEVE_ATOM_USE_GOLD, uid);
        //    int totalUseGold = RedisDatabaseAccessor.getInstance().atomPlayerIncrBy(key, num);
        //    onFreshAchieve(ConType.UseGold, 0, totalUseGold);
        //}
        TDebug.Log("consumeGold:" + num);

        return PlayerData.Gold;
    }

    //增加金币
    public int addGold(int num, string action)
    {
        if (num > int.MaxValue || num < 0)
        {
            //LoggerUtil.warn(uid, getName(), "添加金币失败,数据越界", "num:{0,number,#}", num);
            return -1;
        }
        int currentGold = getGold();
        if (num == 0) return currentGold;
        if (currentGold + num > GameConstUtils.max_gold) //金币超过最大值
        {
            num = GameConstUtils.max_gold - currentGold;
            //LoggerUtil.warn(uid, getName(), "金币超过了上限", "num:{0,number,#}", num);
            if (num == 0) return currentGold;
        }

        PlayerData.Gold += num;

        ////刷新成就
        //if (getAchieveAddGold() < int.MaxValue)
        //{
        //    key = MessageFormat.format("{0}{1,number,#}", RedisIndexProperties.REDIS_PLAYER_ACHIEVE_ATOM_ADD_GOLD, uid);
        //    int totalAddGold = RedisDatabaseAccessor.getInstance().atomPlayerIncrBy(key, num);
        //    onFreshAchieve(ConType.AddGold, 0, totalAddGold);
        //}
        TDebug.Log("addGold:" + num);

        return PlayerData.Gold;
    }



    //消费钻石
    public int consumeDiamond(int num, string action)
    {
        if (num > int.MaxValue || num < 0)
        {
            //LoggerUtil.warn(idx, getName(), "消费钻石失败,数量越界", "action:{0},num:{1}", action, num);
            return -1;
        }

        int diamond = getDiamond();
        if (num > diamond)
        {
            //LoggerUtil.warn(idx, getName(), "消费钻石失败,余额不足", "action:{0},num:{1},cost:{2},current:{3}", action, num, num, diamond);
            return -1;
        }

        if (num == 0) return diamond;
        PlayerData.Diamond -= num;

        //刷新成就
        //key = MessageFormat.format("{0}{1,number,#}", RedisIndexProperties.REDIS_PLAYER_ACHIEVE_ATOM_USE_DIAMOND, uid);
        //int totalUseDiamond = RedisDatabaseAccessor.getInstance().atomPlayerIncrBy(key, num);
        //onFreshAchieve(ConType.UseDiamond, 0, totalUseDiamond);

        TDebug.Log("consumeDiamond:" + num);
        return PlayerData.Diamond;
    }

    //增加钻石
    public int addDiamond(int num, string action="")
    {
        if (num > int.MaxValue || num < 0)
        {
            //LoggerUtil.warn(getIdx(), getName(), "添加钻石失败,数量越界", "num:{0,number,#},action:{1}", num, action);
            return -1;
        }
        int diamond = getDiamond();
        if (num == 0) return diamond;

        PlayerData.Diamond += num;

        TDebug.Log("addDiamond:" + num);
        //String key = MessageFormat.format("{0}{1,number,#}", RedisIndexProperties.REDIS_PLAYER_ATOM_DIAMOND, uid);
        //int remainDiamond = RedisDatabaseAccessor.getInstance().atomPlayerIncrBy(key, num);
        //sendMessage(SnapshotMessageHelper.getSnapshotPlayerAttributeMessage(PlayerAttribute.Diamond, remainDiamond));
        return diamond;
    }


    public int consumeWealth(int num, int wealthType, string action)
    {
        if (num > int.MaxValue || num < 0)
        {
            //LoggerUtil.warn(idx, getName(), "消费钻石失败,数量越界", "action:{0},num:{1}", action, num);
            return -1;
        }

        int diamond = getDiamond();
        if (num > diamond)
        {
            //LoggerUtil.warn(idx, getName(), "消费钻石失败,余额不足", "action:{0},num:{1},cost:{2},current:{3}", action, num, num, diamond);
            return -1;
        }

        if (num == 0) return diamond;
        PlayerData.Diamond -= num;

        //刷新成就
        //key = MessageFormat.format("{0}{1,number,#}", RedisIndexProperties.REDIS_PLAYER_ACHIEVE_ATOM_USE_DIAMOND, uid);
        //int totalUseDiamond = RedisDatabaseAccessor.getInstance().atomPlayerIncrBy(key, num);
        //onFreshAchieve(ConType.UseDiamond, 0, totalUseDiamond);

        TDebug.Log("consumeDiamond:" + num);
        return PlayerData.Diamond;
    }

    public int addWealth(int num, int wealthType, string action)
    {
        if (num > int.MaxValue || num < 0)
        {
            //LoggerUtil.warn(getIdx(), getName(), "添加钻石失败,数量越界", "num:{0,number,#},action:{1}", num, action);
            return -1;
        }
        int diamond = getDiamond();
        if (num == 0) return diamond;

        PlayerData.Diamond += num;

        TDebug.Log("addDiamond:" + num);
        //String key = MessageFormat.format("{0}{1,number,#}", RedisIndexProperties.REDIS_PLAYER_ATOM_DIAMOND, uid);
        //int remainDiamond = RedisDatabaseAccessor.getInstance().atomPlayerIncrBy(key, num);
        //sendMessage(SnapshotMessageHelper.getSnapshotPlayerAttributeMessage(PlayerAttribute.Diamond, remainDiamond));
        return diamond;
    }



    public int consumeLootType(LootType lootType, int id)
    {
        return 0;
    }

    public int addLootType(LootType lootType, int id, int num)
    {
        return 0;
    }


    public void onLoot(List<GoodsToDrop> lootGoodsList, string action)
    {
        if (lootGoodsList == null) return;

        GoodsToDrop dropItem;
        for (int i = 0; i < lootGoodsList.Count; i++)
        {
            dropItem = lootGoodsList[i];
            if (dropItem == null) continue;
            switch (dropItem.MyType)
            {
                case LootType.Money:
                    {
                        WealthType wealthType = (WealthType)(dropItem.GoodsIdx);
                        switch (wealthType)
                        {
                            case WealthType.Diamond:
                                {
                                    addDiamond(dropItem.Amount, action);
                                    break;
                                }
                            case WealthType.Gold:
                                {
                                    addGold(dropItem.Amount, action);
                                    break;
                                }
                            case WealthType.Potentail:
                                {
                                    //addPotential(dropItem.Amount, action);
                                    break;
                                }
                            case WealthType.Exp:
                                {
                                    addExp(dropItem.Amount, action);
                                    break;
                                }
                        }
                        break;
                    }
                case LootType.Item:
                    {
                        if (addItem(dropItem.GoodsIdx, dropItem.Amount, action) == -1)
                            lootGoodsList.RemoveAt(i--); //先移除，然后进行i--
                        break;
                    }

                case LootType.Pet:
                    {
                        //if (addPet(dropItem.GoodsIdx, action) == -1)
                        //    lootGoodsList.RemoveAt(i--); //先移除，然后进行i--
                        break;
                    }

                case LootType.Spell:
                    {
                        if (AddSpell(dropItem.GoodsIdx) == -1)
                            lootGoodsList.RemoveAt(i--); //先移除，然后进行i--
                        break;
                    }

                case LootType.Equip:
                    {
                        //if (addEquip(dropItem.GoodsIdx, action) == -1)
                        //    lootGoodsList.RemoveAt(i--); //先移除，然后进行i--
                        break;
                    }

                case LootType.Recipe:
                    {
                        //if (addRecipe(dropItem.goodsIdx, action) == -1)
                        //    lootGoodsList.RemoveAt(i--); //先移除，然后进行i--
                        break;
                    }
                case LootType.Prestige:
                    {
                        //PrestigeType ty = PrestigeType.getValueByOrdinal(dropItem.goodsIdx);
                        //addPrestige(dropItem.amount, ty, action);
                        break;
                    }

                default:
                    break;
            }
        }
    }
    


    #endregion




    #region 道具
    
    //添加道具
    public int addItem(int itemIdx, int num, string action)
    {
        int pos = addIventoryItemByIdx(itemIdx, num);

        if (pos == -1)
        {
            return -1;
        }
        if (num > int.MaxValue || num < 0)
        {
            //LoggerUtil.warn(getIdx(), getName(), "添加物品失败,数量越界", "num:{0,number,#},action:{1}", num, action);
            return -1;
        }

        Item item = mItemsInventory.GetGoodsByPos(pos) as Item;
        if (item == null)
        {
            //LoggerUtil.warn(getIdx(), getName(), "添加道具失败,玩家数据错误", "num:{0,number,#},action:{1}", num, action);
            return -1;
        }
        return pos;
    }
    public int consumeItem(int itemIdx, int num, string action)
    {
        if (itemIdx < 0) return -1;

        if (num > int.MaxValue || num < 0)
        {
            //LoggerUtil.warn(getIdx(), getName(), "消耗物品失败,数量越界", "num:{0,number,#},action:{1}", num, action);
            return -1;
        }

        int pos = mItemsInventory.IndexOf(itemIdx);
        Item item = mItemsInventory.GetGoodsByPos(pos)as Item;
        if (item == null)
        {
            //LoggerUtil.warn(getIdx(), getName(), action, "消耗道具失败,玩家数据错误");
            return -1;
        }
        if (num > item.num)
        {
            //LoggerUtil.warn(getIdx(), getName(), action, "消耗道具失败,道具数量不足");
            return -1;
        }
        
        item.num -= num;
        if (item.num == 0)
        {
            if (!mItemsInventory.RemoveGoods(item))
            {
                //LoggerUtil.warn(getIdx(), getName(), "移除道具失败", "action:{0},idx:{1}", action, item.idx);
            }
        }

        return item.num;
    }

    public int addIventoryItemByIdx(int id, int num)
    {
        int pos = mItemsInventory.IndexOf(id);
        if (pos >= 0)
        {
            BaseObject obj = mItemsInventory.GetGoodsByPos(pos);
            if (obj == null) return -1;
            Item item = (Item)obj;
            if (item.maxStack != 1)  //如果ownNum=1，则只能有一个
                item.num += num;
            else if (item.maxStack == 1)
            {
                //LoggerUtil.warn(getIdx(), getName(), "重复获得唯一道具", "action:{0},idx:{1}", LogAction.GET_ITEM, id);
                return -1;
            }
            return pos;
        }
        else
        {
            Item item = Item.Fetcher.GetItemCopy(id);
            if (item == null)
            {
                return -1;
            }
            if (item.ownNum == 1) //如果ownNum=1，则只能有一个
                num = 1;
            else
                item.num = num;
            return mItemsInventory.AddGoods(item);
        }
    }
    #endregion




    #region 技能

    public bool onEquippedSpell(int equipPos, int inventoryPos)
    {
        GamePlayer player = getPlayer();
        if (equipPos < 0 || equipPos >= player.SpellList.Length)
        {
            //LoggerUtil.warn(getIdx(), getName(), "装备技能失败,装备槽信息错误", "equipPos:{0}", equipPos);
            return false;
        }
        if (inventoryPos == -1) //卸下
        {
            int oldInventoryPos = player.SpellList[equipPos];
            Spell spell = GetInventorySpellByPos(oldInventoryPos);
            if (spell == null)
            {
                //LoggerUtil.warn(getIdx(), getName(), "装备技能失败,此位置没有技能", "pos:{0}", equipPos);
                return false;
            }

            spell.curIsEquip = false;
            player.SpellList[equipPos] = -1;
        }
        else
        { //装上
            Spell spell = GetInventorySpellByPos(inventoryPos);
            if (spell == null)
            {
                //LoggerUtil.warn(getIdx(), getName(), "待装备技能为空", "pos:{0}", inventoryPos);
                return false;
            }
            if (spell.curIsEquip)
            {
                //LoggerUtil.warn(getIdx(), getName(), "需要装备的技能已经装备上了", "pos:{0}", inventoryPos);
                return false;
            }
            if (!spell.canEquip(equipPos))
            {
                //LoggerUtil.warn(getIdx(), getName(), "装备位置与技能类型不符合", "pos:{0}|type:{1}", equipPos, spell.type);
                return false;
            }
            if (spell.Level > player.Level)
            {
                //LoggerUtil.warn(getIdx(), getName(), "装备技能失败,等级不够", "action:{0,number,#},玩家等级:{1,number,#},技能等级:{2,number,#}", LogAction.STUDY_SPELL, player.level, spell.level);
                return false;
            }
            if (player.SpellList[equipPos] >= 0)//先卸下，再装备
            {
                int oldEquipInventoryPos = player.SpellList[equipPos];
                Spell oldSpell = GetInventorySpellByPos(oldEquipInventoryPos);
                if (oldSpell != null)
                {
                    oldSpell.curIsEquip = false;
                }
            }
            spell.curIsEquip = true;
            player.SpellList[equipPos] = inventoryPos;
            //saveSpellsModule();
            //savePlayerModule();
        }
        return true;
    }
    
    #endregion


    #region 装备
    public int addEquip(Equip equip, string action)
    {
        int pos = mEquipsInventory.AddGoods(equip);
        if (pos == -1)
        {
            LoggerUtil.warn(getIdx(), getName(), "添加装备失败", "action:{0},inventoryPos:{1}", action, pos);
            return -1;
        }
        return pos;
    }

    public Equip removeEquip(int pos, string action)
    {
        if (pos < 0) return null;
        for (int i = 0; i < PlayerData.EquipList.Length; i++)
        {
            if (PlayerData.EquipList[i] == pos)
            {
                LoggerUtil.warn(getIdx(), getName(), "已穿戴的装备不能移除", "action:{0},inventoryPos:{1}", action, pos);
                return null;
            }
        }
        Equip removeEquip = GetInventoryEquipByPos(pos  , false);

        if (removeEquip != null && RemovEquipByPos(pos) ==null)
        {
            LoggerUtil.warn(getIdx(), getName(), "装备移除失败", "action:{0},inventoryPos:{1}", action, pos);
            return null;
        }

        return removeEquip;
    }


    public bool onSellEquip(List<int> posList)
    {
        int sellGold = 0;
        GamePlayer player = getPlayer();
        for (int i = 0; i < player.EquipList.Length; i++)
        {
            for (int j = posList.Count - 1; j >= 0; j--)
            {
                if (player.EquipList[i] == posList[j])
                {
                    posList.Remove(j);
                    LoggerUtil.warn(getIdx(), getName(), "已穿戴的装备不能移除", "action:{0},inventoryPos:{1}", posList);
                }
            }
        }
        List<Equip> equipList = new List<Equip>();
        for (int i = 0; i < posList.Count; i++)
        {
            Equip equip = removeEquip(posList[i], LogAction.SELL_EQUIP);
            if(equip!=null)
                equipList.Add(equip);
        }
        if (equipList != null)
        {
            Equip equip;
            for (int i = 0, length2 = equipList.Count; i < length2; i++)
            {
                equip = equipList[i];
                if (equip != null) sellGold += equip.sell;
            }
            addGold(sellGold, LogAction.SELL_EQUIP);
            //sendMessage(LobbyMessageHelper.getSellEquipMessage(equipList.size(), sellGold));
            return true;
        }
        else
        {
            LoggerUtil.warn(getIdx(), getName(), "出售装备失败", "length:{1}", posList.Count);
            return false;
            //sendMessage(MessageHelper.getWarringMessage(ServerMessageType.SellEquip, ServerStatusCode.GLOBAL_WARN_CODE_WAN_JIA_SHU_JU_YI_CHANG));
        }
    }


    public bool onEquippedEquip(int equipPos, int inventoryPos)
    {
        GamePlayer player = mPlayerData;
        if (equipPos < 0 || equipPos >= player.EquipList.Length)
            return false;

        if (inventoryPos == -1) //卸下
        {
            int oldInventoryPos = player.EquipList[equipPos];
            Equip equip = GetInventoryEquipByPos(oldInventoryPos);
            if (equip == null)
                return false;
            equip.curIsEquip = false;
            player.EquipList[equipPos] = -1;
        }
        else  //装上
        {
            Equip equip = GetInventoryEquipByPos(inventoryPos);
            if (equip == null || equip.curLevel > player.Level)
            {
                LoggerUtil.warn(getIdx(), getName(), "穿戴装备失败", "lv:{0},equipLv:{1}", player.Level, equip.curLevel);
                return false;
            }
            if ((int)equip.type != equipPos)
            {
                LoggerUtil.warn(getIdx(), getName(), "穿戴装备失败,类型不匹配", "type:{0},equipPos:{1}", equip.type, equipPos);
                return false;
            }
            if (player.EquipList[equipPos] >= 0)//先卸下，再装备
            {
                int oldEquipInventoryPos = player.EquipList[equipPos];
                Equip oldEquip = GetInventoryEquipByPos(oldEquipInventoryPos);
                if (oldEquip != null)
                {
                    oldEquip.curIsEquip = false;
                }
            }
            equip.curIsEquip = true;
            player.EquipList[equipPos] = inventoryPos;
        }
        return true;
    }





    #endregion


    public void saveGame()
    {
        saveEquipModule();
        saveItemModule();
        saveSpellModule();
        savePlayerModule();
        PlayerPrefs.Save();
    }

    public void saveItemModule()
    {
        //List<BaseObject> itemList = PlayerPrefsBridge.Instance.mItemsInventory.GetList();

        string saveStr = mItemsInventory.Save();
        //string encryptSave = itemSave.ToEncString();       //TODO: 读取存档时，需要encryptSave与encryptMd5匹配
        //string encryptMd5 = TUtils.MDEncode(encryptSave);
        //SaveUtils.SetEncryptInPlayer("Item", itemSave);
        SaveUtils.SetGameSave(GameSaveType.InvItem.ToString(), saveStr);

        //Debug.Log(mItemsInventory.Save());
    }

    public void savePlayerModule()
    {
        string saveStr = JsonMapper.ToJsonWithType<GamePlayerBase>(mPlayerData);
        SaveUtils.SetGameSave(GameSaveType.Player.ToString(), saveStr);
    }

    void saveEquipModule()
    {
        string saveStr = mEquipsInventory.Save();
        SaveUtils.SetGameSave(GameSaveType.InvEquip.ToString(), saveStr);
    }

    void saveSpellModule()
    {
        string saveStr = mSpellInventory.Save();
        SaveUtils.SetGameSave(GameSaveType.InvSpell.ToString(), saveStr);
    }




}


public enum GameSaveType
{
    InvItem,
    InvEquip,
    InvSpell,
    Player,
}



public class LoggerUtil
{
    public static void warn(int uid, string name, string msg, string detail, params object[] args)//String action
    {

        if(args.Length > 0)
        {
            detail = string.Format(detail, args);
        }

        string warn = string.Format("{0,number,#}|{1}|{2}|{3}", uid, name, msg, detail);
        TDebug.Log(warn);

    }
}