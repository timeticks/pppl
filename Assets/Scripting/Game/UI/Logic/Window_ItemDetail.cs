using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Window_ItemDetail : WindowBase
{

    public class ViewObj
    {
        public Transform Root;
        public Button BtnExit;
        public Text TextDesc;
        public Text TextName;
        public Text TextPrice;
        public Button RootBtn;
        public TextButton TBtnBuy;
        public TextButton TBtnEquip;
        public TextButton TBtnSell;

        public ViewObj(UIViewBase view)
        {
            if (Root != null) return;
            Root = view.GetCommon<Transform>("Root");
            BtnExit = view.GetCommon<Button>("BtnExit");
            TextDesc = view.GetCommon<Text>("TextDesc");
            TextName = view.GetCommon<Text>("TextName");
            TextPrice = view.GetCommon<Text>("TextPrice");
            RootBtn = view.GetCommon<Button>("RootBtn");
            TBtnBuy = view.GetCommon<TextButton>("TBtnBuy");
            TBtnEquip = view.GetCommon<TextButton>("TBtnEquip");
            TBtnSell = view.GetCommon<TextButton>("TBtnSell");
        }
    }
    private ViewObj mViewObj;
    private int mCurSelectIndex;

    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        OpenWin();
        Init();

    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }
    public void Init()
    {
        mCurSelectIndex = 0;
        //mViewObj.SellBtn.SetOnClick(BtnEvt_Sell);
        //mViewObj.UseBtn.SetOnClick(BtnEvt_Use);
    }

    void OnEnable()
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        DropInfo dropItem = new DropInfo();
        dropItem.monsterLevel = 1;
        dropItem.monsterQuality = 5;
        dropItem.dropQuality = 5;
        //Equip equip = DropGrade.GetDropEquip(dropItem);
        //EquipBase baseEquip = new EquipBase(equip);
        //TDebug.Log(LitJson.JsonMapper.ToJson(baseEquip));
        Dictionary<DropGrade.DropType, List<object>> dropMap = DropGrade.RunDrop(dropItem);
        foreach (var temp in dropMap)
        {
            for (int i = 0; i < temp.Value.Count; i++)
            {
                if (temp.Key == DropGrade.DropType.Gold)
                    TDebug.Log(string.Format("name:{0},quality:{1}", "金币", temp.Value[i]));
                else
                    Show((Equip)(temp.Value[i]));
            }
        }
        
    }

    void Show(Equip equip)
    {
        mViewObj.TextName.text = equip.name;

        StringBuilder descStr = new StringBuilder();

        AttrTable attrTable;
        descStr.Append("<size=40>");
        for (int i = 0; i < equip.curMainAttrType.Length; i++)
        {
            attrTable = AttrTable.Fetcher.GetAttrTableCopy(equip.curMainAttrType[i]);
            descStr.Append(string.Format(attrTable.addStr, equip.curMainAttrVal[i]) + "\r\n");
        }
        descStr.Append("</size>");
        descStr.Append("\r\n");

        descStr.Append("<color=#54B1D0FF>");
        for (int i = 0; i < equip.curSubType.Length; i++)
        {
            attrTable = AttrTable.Fetcher.GetAttrTableCopy(equip.curSubType[i]);
            descStr.Append(string.Format(attrTable.addStr, equip.curSubVal[i]) + "\r\n");
        }
        descStr.Append("/color");

        mViewObj.TextDesc.text = descStr.ToString();
    }


    /// <summary>
    /// 根据最大叠加数刷新列表
    /// </summary>
    List<Item> SortByMaxStack(List<Item> itemList)
    {
        //itemList.Sort((x, y) => { return x.idx.CompareTo(y.idx); });
        itemList.Sort((x, y) =>
        {
            if ((x.type == Item.ItemType.Stuff && y.type == Item.ItemType.Task) || (y.type == Item.ItemType.Stuff && x.type == Item.ItemType.Task))
            {
                return ((int)x.type).CompareTo((int)y.type);
            }
            if(y.type == x.type)
                return ((int)x.idx).CompareTo(y.idx);
            return ((int)y.type).CompareTo((int)x.type);
        }); //根据类型排序 ：3、1、2

        for (int i = 0, length = itemList.Count; i < length; i++)
        {
            if (itemList[i].num > itemList[i].maxStack)
            {
                int remainNum = itemList[i].num - itemList[i].maxStack;

                itemList[i].num = itemList[i].maxStack;
                int count = Mathf.CeilToInt(remainNum / (float)itemList[i].maxStack);
                Item newItem = Item.Fetcher.GetItemCopy(itemList[i].idx);
                for (int j = 0; j < count; j++)
                {
                    int num = remainNum > itemList[i].maxStack ? itemList[i].maxStack : remainNum;
                    newItem.num = num;
                    itemList.Insert(i + 1, newItem);
                }
            }
        }
        return itemList;
    }









}
