using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class Window_AssemInfo : WindowBase
{
   
    public class ViewObj
    {
        public Transform RootSpellItem;
        public Transform RootEquipItem;
        public Transform RootPetItem;
        public GameObject Part_ItemAssem;
              
        public ViewObj(UIViewBase view)
        {
            if (RootSpellItem == null) RootSpellItem = view.GetCommon<Transform>("RootSpellItem");
            if (RootEquipItem == null) RootEquipItem = view.GetCommon<Transform>("RootEquipItem");
            if (RootPetItem == null) RootPetItem = view.GetCommon<Transform>("RootPetItem");
            if (Part_ItemAssem == null) Part_ItemAssem = view.GetCommon<GameObject>("Part_ItemAssem");

        }
    }
    public List<AssemItemObj> EquipItemList = new List<AssemItemObj>();
    public List<AssemItemObj> PetItemList = new List<AssemItemObj>();
    public List<AssemItemObj> SpellItemList = new List<AssemItemObj>(); 
    public class AssemItemObj :SmallViewObj
    {
        public Image IconType;
        public Text TextName;
        public Text TextNull;
        public Button Btn;
        public override void  Init(UIViewBase view)
        {
            if (View != null) return;
            base.Init(view);
            IconType = view.GetCommon<Image>("IconType");
            TextName = view.GetCommon<Text>("TextName");
            TextNull = view.GetCommon<Text>("TextNull");
            Btn = view.GetCommon<Button>("Btn");
        }
    }

    private ViewObj mViewObj;
    public enum ItemType:byte
    {
        Spell,//包含功法和心法
        Equip,
        Pet,
    }

    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        FreshInfo();
    }
    public override void FreshInfo()
    {
        base.FreshInfo();
        PlayerPrefsBridge.Instance.RegisterSnapShotHandler(PrefsSnapShotEventType.SnapshotAssem, FreshAssemItem);
        FreshAssemItem();
    }
    private void FreshAssemItem()
    {
        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
        SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();

        Spell[] wearSpell = PlayerPrefsBridge.Instance.GetSpellBarInfo();
        SpellItemList = TAppUtility.Instance.AddViewInstantiate<AssemItemObj>(SpellItemList, mViewObj.Part_ItemAssem, mViewObj.RootSpellItem, wearSpell.Length);
        for (int i = 0, length = wearSpell.Length; i < length; i++)
        {
            AssemItemObj obj = SpellItemList[i];
            //obj.IconType.sprite = commonSprite.GetSprite(TUtility.TryGetSpellTypeString((OldSpell.PosType)i));           
            if (wearSpell[i] != null)
            {
                //obj.TextName.text = wearSpell[i].ColorName;
                obj.TextName.gameObject.SetActive(true);
                obj.TextNull.gameObject.SetActive(false);
                int tempId = wearSpell[i].idx;
                int tempIndex= i;
                obj.Btn.SetOnClick(delegate() { BtnEvt_Assem(ItemType.Spell, tempIndex, tempId); });
            }
            else
            {
                //obj.TextNull.text = ((OldSpell.PosType)i).GetDesc();
                obj.TextName.gameObject.SetActive(false);
                obj.TextNull.gameObject.SetActive(true);
                int tempIndex = i;
                obj.Btn.SetOnClick(delegate() { BtnEvt_Assem(ItemType.Spell, tempIndex); });
            }
                
        }

        Equip[] wearEuip = PlayerPrefsBridge.Instance.GetEquipBarInfo();
        EquipItemList = TAppUtility.Instance.AddViewInstantiate<AssemItemObj>(EquipItemList, mViewObj.Part_ItemAssem, mViewObj.RootEquipItem, wearEuip.Length);
        for (int i = 0, length = wearEuip.Length; i < length; i++)
        {
            AssemItemObj obj = EquipItemList[i];
            //obj.IconType.sprite = commonSprite.GetSprite(TUtility.TryGetEquipIcon((Equip.EquipType)i));
            if (wearEuip[i] != null)
            {
                obj.TextName.text = TUtility.GetTextByQuality(wearEuip[i].name, wearEuip[i].curQuality);
                obj.TextName.gameObject.SetActive(true);
                obj.TextNull.gameObject.SetActive(false);
            }
            else
            {
                obj.TextNull.text = TUtility.GetTagStr(((Equip.EquipType)i).GetDesc());
                obj.TextName.gameObject.SetActive(false);
                obj.TextNull.gameObject.SetActive(true);             
            }
            int tempIndex = i;
            obj.Btn.SetOnClick(delegate() { BtnEvt_Assem(ItemType.Equip, tempIndex); });
        }

        Pet[] wearPet = PlayerPrefsBridge.Instance.GetPetBarInfo();
        PetItemList = TAppUtility.Instance.AddViewInstantiate<AssemItemObj>(PetItemList, mViewObj.Part_ItemAssem, mViewObj.RootPetItem, wearPet.Length);      
        for (int i = 0, length = wearPet.Length; i < length; i++)
        {
            AssemItemObj obj = PetItemList[i];
            obj.IconType.sprite = commonSprite.GetSprite(TUtility.TryGetPetTypeString((Pet.PetTypeEnum)i));
            int tempIndex = i;
            obj.Btn.SetOnClick(delegate() { BtnEvt_Assem(ItemType.Pet, tempIndex); });
            if (wearPet[i] != null)
            {
                obj.TextName.text = wearPet[i].ColorName;
                obj.TextName.gameObject.SetActive(true);
                obj.TextNull.gameObject.SetActive(false);             
            }
            else
            {
                obj.TextNull.text = PetType(i);
                obj.TextName.gameObject.SetActive(false);
                obj.TextNull.gameObject.SetActive(true);            
            }
        }
    }

    public void BtnEvt_Assem(ItemType itemType,int index,int Idx=0)
    {  
        switch (itemType)
        {
            //case ItemType.Spell:              
            //    if(BoolSpellTypeExit(TUtility.GetSpellType((OldSpell.PosType)index)))
            //        UIRootMgr.Instance.OpenWindowWithHide<Window_AssemSpellMini>(WinName.Window_AssemSpellMini,WinName.Window_AssemInfo).OpenWindowMini((OldSpell.PosType)index,Idx);
            //    break;
            case ItemType.Equip:
                if (BoolEquipTypeExit((Equip.EquipType)index))
                    UIRootMgr.Instance.OpenWindowWithHide<Window_AssemEquipMini>(WinName.Window_AssemEquipMini, WinName.Window_AssemInfo).OpenWindowMini((Equip.EquipType)index);
                break;
            case ItemType.Pet:
                if (BoolPetTypeExit((Pet.PetTypeEnum)index))
                    UIRootMgr.Instance.OpenWindowWithHide<Window_AssemPetMini>(WinName.Window_AssemPetMini, WinName.Window_AssemInfo).OpenWindowMini((Pet.PetTypeEnum)index);
                break;
        }
    }
    bool BoolSpellTypeExit(Spell.PosType type)
    {
        //List<OldSpell> spellDataList = PlayerPrefsBridge.Instance.GetSpellAllListCopy();
        //for (int i = 0, length = spellDataList.Count; i < length; i++)
        //{
        //    if (spellDataList[i].Type == type)
        //        return true;
        //}
        //UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("你尚未拥有该类型功法", Color.black);
        return false;
    }
    bool BoolEquipTypeExit(Equip.EquipType type)
    {
        List<Equip> equipDataList = PlayerPrefsBridge.Instance.GetEquipAllListNoCopy();
        //for (int i = 0,length = equipDataList.Count; i < length; i++)
        //{
        //    if (equipDataList[i].equipPos == type)
        //        return true;
        //}
        UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("你尚未拥有该类型法宝", Color.black);
        return false;
    }
    bool BoolPetTypeExit(Pet.PetTypeEnum type)
    {
        List<Pet> petDataList = PlayerPrefsBridge.Instance.GetPetAllListCopy();
        for (int i = 0,length = petDataList.Count; i < length; i++)
        {
            if (petDataList[i].Type == type)
                return true;                
        }
        UIRootMgr.Instance.MessageBox.ShowInfo_OnlyOk("你尚未拥有该类型伙伴", Color.black);
        return false;
    }


    string PetType(int index)
    {
        switch (index)
        {
            case 0: return "灵  兽";
            case 1: return "傀  儡";
            case 2: return "英  魂";
            default: return "未 知";
        }
    }
    void OnDisable()
    {
        PlayerPrefsBridge.Instance.RemoveSnapShotHandler(PrefsSnapShotEventType.SnapshotAssem, FreshAssemItem);
    }

}

