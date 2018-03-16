
using LitJson;
using System.Collections.Generic;


public class InventoryList
{
    private List<BaseObject>            mAllInventory;
    private int                         mCapacity;
    private InventoryType               mType;

    public InventoryList(InventoryType type)
    {
        this.mType = type;
        mAllInventory = new List<BaseObject>();
    }
     
    public enum InventoryType {
        Spells,
        Items,
        Equips,
        Pets,
        Recipes,
        Books, //功法
    }

    public List<BaseObject> GetList()
    {
        return mAllInventory;
    }
    
    public int IndexOf(int id)
    {
        if (id == 0) return -1;

        BaseObject obj;
        for (int i = 0; i < mAllInventory.Count; i++)
        {
            obj = (BaseObject)mAllInventory[i];
            if (obj != null && obj.idx == id)
                return i;
        }
        return -1;
    }

    public int IndexOf(BaseObject obj)
    {
        if (obj == null) return -1;
        BaseObject tempObj;
        for (int i = 0; i < mAllInventory.Count; i++)
        {
            tempObj = mAllInventory[i];
            if (tempObj != null && obj == tempObj)
                return i;
        }
        return -1;
    }

    public BaseObject GetGoodsById(int id)
    {
        if (id == 0) return null;

        BaseObject obj;
        for(int i = 0; i < mAllInventory.Count; i++){
            obj = (BaseObject)mAllInventory[i];
             if (obj != null && obj.idx == id)
                return obj;
        }
        return null;
    }


    public bool RemoveGoods(BaseObject obj)
    {
        return mAllInventory.Remove(obj);
    }


    public BaseObject RemoveGoodsByIdx(int idx)
    {
        BaseObject gameObject = (BaseObject)GetGoodsById(idx);
        if (gameObject != null)
            RemoveGoods(gameObject);
        return gameObject;
    }

    public BaseObject RemoveGoodsByPos(int pos)
    {
        BaseObject gameObject = GetGoodsByPos(pos);
        if (gameObject != null)
            RemoveGoods(gameObject);
        return gameObject;
    }


    public BaseObject GetGoodsByPos(int pos)
    {
        if (pos < 0 || pos >= mAllInventory.Count) return null;

        BaseObject obj = mAllInventory[pos];
        if(obj != null)
        {
            return obj;
        }
        //LogListener.Log("背包中不存在pos=[" + pos + "]的物品");
        return null;
    }



    public int AddGoods(BaseObject newGoods)
    {
        int pos = GetNextFreePos(newGoods);
        if (pos >= 0) {
            AddGoodsOnPos(newGoods, pos);
            return pos;
        } else {
            //LogListener.LogError("物品表" + this.mType + "内没有足够空间添加:" + newGoods);
            return -1;
        }
    }


    /** 根据ID移除背包内指定数量的物品 */
    //public BaseObject SetSpecifiedAmountGoodsById(int idx, int amount)
    //{
    //    
    //    for (int i = 0; i < mAllInventory.Count; i++)
    //    {
    //        BaseObject goods = mAllInventory[i];
    //        if (goods != null)
    //        {
    //            if (goods.Idx == idx)
    //            {
    //               if (mAllInventory[i] is Goods)
    //                {
    //                    if (amount == 0)
    //                    {
    //                        mAllInventory.Remove(goods);
    //                    }
    //                    else
    //                    {
    //                        ((Goods)mAllInventory[i]).Count = amount;
    //                    }
    //                    return goods;
    //                }
    //            }
    //        }
    //    }
    //
    //    if(amount > 0)
    //    {
    //        Goods g = Goods.GoodsFetcher.GetGoodsCopy(idx);
    //        g.Count = amount;
    //        mAllInventory.Add(g);
    //        return g;
    //    }
    //   
    //
    //    //LogListener.LogError("物品栏内物品出错，id为：" + idx + "的物品，并没有：" + amount + "个");
    //    return null;
    //}
  
        
    /** 根据ID移除背包内指定数量的物品 */
	//public void RemoveSpecifiedAmountGoodsById(int idx, int amount) {
    //    int tempAmount = amount;
    //    for (int i = 0; i < mAllInventory.Count && tempAmount > 0; i++)
    //    {
    //        BaseObject goods = mAllInventory[i];
    //    	if (goods != null) {
    //    		if (goods.Idx == idx) 
    //            {
    //                  if (mAllInventory[i] is Hero)
    //                  {
    //                      mAllInventory.Remove(goods);
    //                  }
    //                  else if (mAllInventory[i] is Goods)
    //                  {
    //                      if(((Goods)mAllInventory[i]).Count <= amount)
    //                      {
    //                          mAllInventory.Remove(goods);
    //                      }else
    //                      {
    //                          ((Goods)mAllInventory[i]).Count -= amount;
    //                      }
    //                       
    //                  }
    //    		}
    //    	}
    //    }
    //    
    //    
    //    if (tempAmount > 0) {
    //    	//LogListener.LogError("物品栏内物品出错，id为：" + idx + "的物品，并没有：" + amount + "个");
    //    }
	//}

	public BaseObject AddGoodsOnPos(BaseObject newGoods, int pos)
    {
	    if (pos < 0)
	    {
            TDebug.LogError(string.Format("位置越界，背包长度[{0}]|Pos[{1}]", mAllInventory.Count, pos));
            return null;
	    }
        if (pos < mAllInventory.Count &&  mAllInventory[pos] == null)
        {
            mAllInventory.Insert(pos, newGoods);
        }
        else if (pos < mAllInventory.Count && mAllInventory[pos] != null)
        {
            mAllInventory[pos] = newGoods;
        }
        else if (pos <= mAllInventory.Count)
        {
            mAllInventory.Insert(pos, newGoods);
        }
        else
        {
            TDebug.LogError(string.Format("位置越界，背包长度[{0}]|Pos[{1}]", mAllInventory.Count, pos));
        }
	    return mAllInventory[pos];
    }

    public BaseObject ReplaceGoodsOnPos(int pos, BaseObject goods)
    {
        if(pos>=0 && pos<mAllInventory.Count)
        {
            BaseObject obj = mAllInventory[pos];
            mAllInventory[pos] = goods;
            return obj;
        }
        return null; 
    }



    private int GetNextFreePos(BaseObject goodsToAdd)
    {
        BaseObject obj = null;
        for (int i = 0; i < mAllInventory.Count; i++) {
            obj = (BaseObject)mAllInventory[i];
            if (mType == InventoryType.Equips)
            {
                if (obj == null || obj.idx == 0)
                    return i;
            }
            else
            {
                if (obj != null && (obj.idx == goodsToAdd.idx || obj.idx == 0))
                    return i;
            }
        }
        return IsUsedLength();
    }
     
    
    public int IsUsedLength() {
        int length = 0;
        for (int i = 0; i < mAllInventory.Count; i++)
            if (mAllInventory[i] != null)
                length++;
        return length;
    }

    public int GetListLength()
    {
        return mAllInventory.Count;
    }


    /**
     * 玩家的背包长度数量，计算了最大叠加数
     * @return
     */
    public int getInventoryCount()
    {
        if (mType == InventoryType.Items)
        {
            int length = 0;
            Item item;
            for (int i = 0; i < mAllInventory.Count; i++)
            {
                if (null == mAllInventory[i]) continue;
                item = (Item)mAllInventory[i];
                if (item.maxStack >= item.num) { length++; }
                else  //大于最大叠加数
                {
                    length = length + item.num / item.maxStack + 1;
                }
            }
            return length;
        }
        else
        {
            return IsUsedLength();
        }
    }

    public string Save()
    {
        string tempSave = "";
        switch (mType)
        {
            case InventoryType.Equips:
            {
                tempSave = JsonMapper.ToJsonWithType<EquipBase>(mAllInventory);
                break;
            }
            case InventoryType.Items:
            {
                tempSave = JsonMapper.ToJsonWithType<ItemBase>(mAllInventory);
                break;
            }
            case InventoryType.Spells:
            {
                tempSave = JsonMapper.ToJsonWithType<SkillBase>(mAllInventory);
                break;
            }
        }
        return tempSave;
    }

    public void Read(string tempSave)
    {
        mAllInventory.Clear();
        switch (mType)
        {
            case InventoryType.Equips:
                {
                    List<EquipBase> tempList = JsonMapper.ToObject<List<EquipBase>>(tempSave);
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (tempList[i].idx == 0)
                        {
                            mAllInventory.Add(new Equip());
                            continue;
                        }
                        Equip temp = Equip.Fetcher.GetEquipCopy(tempList[i].idx);
                        if (temp == null) continue;
                        temp.CopyBy(tempList[i]);
                        mAllInventory.Add(temp);
                    }
                    break;
                }
            case InventoryType.Items:
                {
                    List<ItemBase> tempList = JsonMapper.ToObject<List<ItemBase>>(tempSave);
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (tempList[i].idx == 0)
                        {
                            mAllInventory.Add(new Item());
                            continue;
                        }
                        Item temp = Item.Fetcher.GetItemCopy(tempList[i].idx);
                        if (temp == null) continue;
                        temp.CopyBy(tempList[i]);
                        mAllInventory.Add(temp);
                    }
                    break;
                }
            case InventoryType.Spells:
                {
                    List<SkillBase> tempList = JsonMapper.ToObject<List<SkillBase>>(tempSave);
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        Spell temp = Spell.Fetcher.GetSpellCopy(tempList[i].idx);
                        if (temp == null) continue;
                        temp.CopyBy(tempList[i]);
                        mAllInventory.Add(temp);
                    }
                    break;
                }
        }
    }
}
