using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class Window_RecipeDetial : WindowBase {

    public class ViewObj
    {
        public GameObject Part_ItemRecipe;
        public Text TextState;
        public GameObject LvUpTip;
        public NumScrollTool ProScrollRoot;
        public Text TextTitleLeft;
        public Transform RootItemRecipe;
        public GameObject PanelProduce;
        public Text TextName;
        public Text TextDes;
        public Text TextInfo0;
        public Text TextInfo1;
        public Text TextInfo2;
        public Text TextInfo3;
        public Text TextLastTime;
        public GameObject Produce;
        public GameObject UpSpeed;
        public Button BtnIncreaseNum;
        public Button BtnDecreaseNum;
        public Text TextTitleCostMater;
        public Text TextCost0;
        public Text TextCost1;
        public Text TextCost2;
        public Text TextCost3;
        public Button BtnProduce;
        public Button BtnExit;
        public Button BtnUpSpeed;
        public Text TextProduceNum;            
        public List<Text> TextCostList;
        public Text TextBtnUpSpeed;
        public Text TextLvUpCost;
        public Button BtnLvUpCost;
        public Button BtnUpAuxSkill;
        public Text TextLvDemand;
        public GameObject LvUpCostDetial;
        public Button BtnMask;
        public Text TextLvUpCostDesc;
        public Text TextType;
        public GameObject LimitMask;
        public Text TextBtnForgeProduce;
        public ViewObj(UIViewBase view)
        {
            Part_ItemRecipe = view.GetCommon<GameObject>("Part_ItemRecipe");
            TextState = view.GetCommon<Text>("TextState");
            LvUpTip = view.GetCommon<GameObject>("LvUpTip");
            ProScrollRoot = view.GetCommon<NumScrollTool>("ProScrollRoot");
            TextTitleLeft = view.GetCommon<Text>("TextTitleLeft");
            RootItemRecipe = view.GetCommon<Transform>("RootItemRecipe");
            PanelProduce = view.GetCommon<GameObject>("PanelProduce");
            TextName = view.GetCommon<Text>("TextName");
            TextDes = view.GetCommon<Text>("TextDes");
            TextInfo0 = view.GetCommon<Text>("TextInfo0");
            TextInfo1 = view.GetCommon<Text>("TextInfo1");
            TextInfo2 = view.GetCommon<Text>("TextInfo2");
            TextInfo3 = view.GetCommon<Text>("TextInfo3");
            Produce = view.GetCommon<GameObject>("Produce");
            UpSpeed = view.GetCommon<GameObject>("UpSpeed");
            BtnIncreaseNum = view.GetCommon<Button>("BtnIncreaseNum");
            BtnDecreaseNum = view.GetCommon<Button>("BtnDecreaseNum");
            TextTitleCostMater = view.GetCommon<Text>("TextTitleCostMater");
            TextCost0 = view.GetCommon<Text>("TextCost0");
            TextCost1 = view.GetCommon<Text>("TextCost1");
            TextCost2 = view.GetCommon<Text>("TextCost2");
            TextCost3 = view.GetCommon<Text>("TextCost3");
            BtnProduce = view.GetCommon<Button>("BtnProduce");
            BtnExit = view.GetCommon<Button>("BtnExit");
            BtnUpSpeed = view.GetCommon<Button>("BtnUpSpeed");
            TextProduceNum = view.GetCommon<Text>("TextProduceNum");
            TextLastTime = view.GetCommon<Text>("TextLastTime");         
            TextBtnUpSpeed = view.GetCommon<Text>("TextBtnUpSpeed");
            TextLvUpCost = view.GetCommon<Text>("TextLvUpCost");
            BtnLvUpCost = view.GetCommon<Button>("TextLvUpCost");
            BtnUpAuxSkill = view.GetCommon<Button>("BtnUpAuxSkill");
            TextLvDemand = view.GetCommon<Text>("TextLvDemand");
            LvUpCostDetial = view.GetCommon<GameObject>("LvUpCostDetial");
            BtnMask = view.GetCommon<Button>("BtnMask");
            TextLvUpCostDesc = view.GetCommon<Text>("TextLvUpCostDesc");
            TextType = view.GetCommon<Text>("TextType");
            LimitMask = view.GetCommon<GameObject>("LimitMask");
            TextBtnForgeProduce = view.GetCommon<Text>("TextBtnForgeProduce");
            TextCostList = new List<Text>();         
            TextCostList.Add(TextCost1);
            TextCostList.Add(TextCost2);
            TextCostList.Add(TextCost3);
        }
        public void ResetSiteScroll()
        {
            Vector3 tempPos = RootItemRecipe.transform.localPosition;
            RootItemRecipe.transform.localPosition = new Vector3(tempPos.x, 0, tempPos.z);
        }
    }
    //等级标签Item
    public class RecipeStateObj
    {
        public Button BtnRecipeState;
        public Text TextState;
        public Transform Mark;
        public GridLayoutGroup RootRecipeBtn;
        public GameObject Part_ItemRecipeBtn;
        public Image Part_ItemRecipe;
        public ContentParentSizeFit contentSizeFit;
        public int LevelDamand;

        public GameObject gameobject;
        public bool isOpen = false;
        public List<RecipeItemObj> ListRecipeItem = new List<RecipeItemObj>();
        public int CurChooseRecipeIndnx = 0;
        public RecipeStateObj(UIViewBase view)
        {
            BtnRecipeState = view.GetCommon<Button>("BtnRecipeState");
            TextState = view.GetCommon<Text>("TextState");
            Mark = view.GetCommon<Transform>("Mark");
            RootRecipeBtn = view.GetCommon<GridLayoutGroup>("RootRecipeBtn");
            Part_ItemRecipeBtn = view.GetCommon<GameObject>("Part_ItemRecipeBtn"); 
            Part_ItemRecipe = view.GetCommon<Image>("Part_ItemRecipe");
            contentSizeFit = Part_ItemRecipe.GetComponent<ContentParentSizeFit>();
            gameobject = view.gameObject;
        }
        /// <summary>
        /// 关闭下拉列表
        /// </summary>
        public void CloseRecipeItemList()
        {
            isOpen = false;
            Part_ItemRecipe.GetComponent<Image>().enabled = false;
            Mark.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            RootRecipeBtn.gameObject.SetActive(false);
            contentSizeFit.ResetSize();
        }
        public void OpenRecipeItemList()
        {
            isOpen = true;
            Part_ItemRecipe.GetComponent<Image>().enabled = true;
            Mark.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
            RootRecipeBtn.gameObject.SetActive(true);
            contentSizeFit.ResetSize();
        }
       
    }

    //配方Item
    public class RecipeItemObj
    {
        public Button BtnRecipe;
        public Text TextTitle;
        public Image IconItem;
        public GameObject gameobject;
        public int RecipeId;
        public int InventoryPos;
        public Sprite Bg_kuang_06;
        public Sprite Bg_kuang_04;
        public GameObject LimitMask;
        public RecipeItemObj(UIViewBase view)
        {
            BtnRecipe = view.GetCommon<Button>("BtnRecipe");
            TextTitle = view.GetCommon<Text>("TextTitle");
            IconItem = view.GetCommon<Image>("IconItem");
            Bg_kuang_06 = view.GetCommon<Sprite>("Bg_kuang_06");
            Bg_kuang_04 = view.GetCommon<Sprite>("Bg_kuang_04");
            LimitMask = view.GetCommon<GameObject>("LimitMask");
            gameobject = view.gameObject;
        }
        public void SelectItem(bool select)
        {
            BtnRecipe.image.overrideSprite = select ? Bg_kuang_06 : Bg_kuang_04;
            BtnRecipe.image.sprite = select ? Bg_kuang_06 : Bg_kuang_04;
        }
    }

    private ViewObj mViewObj;
    private AuxSkillLevel.SkillType mSkillType; // 当前生活技能
    private List<RecipeStateObj> mRecipeStatelist = new List<RecipeStateObj>();
    private List<RecipeItemObj> mRecipeItemList = new List<RecipeItemObj>();

    private int mSelectRecipeId; // 当前选中配方

    private RecipeStateObj mSelectStateBtn;//当前选中的境界；
    private long mCurFinishTime = 0; //当前制作时间
    public void OpenWindow(AuxSkillLevel.SkillType skillType )
    {
        if (mViewObj == null) { mViewObj = new ViewObj(mViewBase); }
        OpenWin();
        Init(skillType);
        RegisterNetCodeHandler(NetCode_S.Produce, S2C_Produce);
        RegisterNetCodeHandler(NetCode_S.ProduceAccelerate, S2C_ProduceAccelerate);
        RegisterNetCodeHandler(NetCode_S.FinishProduce, S2C_FinishProduce);
        RegisterNetCodeHandler(NetCode_S.AuxSkillLevelUp, S2C_AuxSkillLevelUp);
    }
    void Init(AuxSkillLevel.SkillType skillType)
    {
        mSkillType = skillType;
        if (skillType== AuxSkillLevel.SkillType.MakeDrug)
        {
            mViewObj.TextTitleLeft.text = "丹药配方";
        }
        else if (skillType == AuxSkillLevel.SkillType.Forge)
        {
            mViewObj.TextTitleLeft.text = "法宝图纸";
        }
        else if (skillType == AuxSkillLevel.SkillType.Mine)
        {
            mViewObj.TextTitleLeft.text = "矿脉地图";
        }
        else if (skillType == AuxSkillLevel.SkillType.GatherHerb)
        {
            mViewObj.TextTitleLeft.text = "药山地图";
        }
        FreshStateButtons();
        FreshProficiency();
        AutoChooseRecipe(skillType);
        mViewObj.BtnExit.SetOnClick(delegate() { CloseWin(); });
        mViewObj.ResetSiteScroll();
    }

    public void FreshBadge(bool isActive)
    {
        if (!isActive)
            BadgeTips.SetBadgeViewFalse(mViewObj.BtnUpSpeed.transform);
        else
            BadgeTips.SetBadgeView(mViewObj.BtnUpSpeed.transform);
    }

    //自动选择配方,有制作进度选择制作中的配方，没有制作进度显示当前技能等级下的第一个配方
    void AutoChooseRecipe(AuxSkillLevel.SkillType skillType)
    {
        ProduceItem item = PlayerPrefsBridge.Instance.GetProduceItem(skillType);
        if (item != null)
        {
            ChooseRecipe(item.RecipeID);
        }
        else
        {
            int curLevel = PlayerPrefsBridge.Instance.PlayerData.AuxSkillList[(int)mSkillType].Level;
            List<Recipe> showList = Recipe.GetLevelRecipes(mSkillType, curLevel,PlayerPrefsBridge.Instance.PlayerData.MySect);
            int tempId = showList[0].idx;
            ChooseRecipe(tempId);
        }
    }
   /// <summary>
   /// 通过配方id选择配方
   /// </summary>
   /// <param name="recipeId"></param>
    void ChooseRecipe(int recipeId)
    {
        int curLevel = PlayerPrefsBridge.Instance.PlayerData.AuxSkillList[(int)mSkillType].Level;
        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(recipeId);
        BtnEvt_RecipeItemClick((int)recipe.SkillLevel);
        BtnEvt_RecipeBtnClick(recipeId);     
    }
    //点击对应境界按钮
    public void BtnEvt_RecipeItemClick(int levelDemand)
    {
        GameObject go = (GameObject)SharedAsset.Instance.LoadSpritePrefabObj("IconAtlas");
        SpritePrefab commonSprite = go.GetComponent<SpritePrefab>();
        RecipeStateObj obj=null;
        for (int i = 0; i < mRecipeStatelist.Count; i++)
        {
            if (mRecipeStatelist[i].LevelDamand == levelDemand)
            {
                obj = mRecipeStatelist[i];
                break;
            }
        }
        if (obj == null) return;
        if (obj.isOpen)//关闭下拉列表
        {  
            obj.CloseRecipeItemList();
            mSelectStateBtn = null;
        }
        else
        {
            if (mSelectStateBtn != null) mSelectStateBtn.CloseRecipeItemList();//关闭上一个展开的下拉列表
            mSelectStateBtn = obj;
            List<Recipe> showList = Recipe.GetLevelRecipes(mSkillType, obj.LevelDamand, PlayerPrefsBridge.Instance.PlayerData.MySect);
            for (int i = mRecipeItemList.Count; i < showList.Count; i++)
            {
                GameObject g = Instantiate(obj.Part_ItemRecipeBtn);
                TUtility.SetParent(g.transform, obj.RootRecipeBtn.transform, false);
                RecipeItemObj item = new RecipeItemObj(g.GetComponent<UIViewBase>());              
                mRecipeItemList.Add(item);
            }
            ////重赋值
            obj.ListRecipeItem.Clear();
            for (int i = 0, length = mRecipeItemList.Count; i < length; i++)
            {
                TUtility.SetParent(mRecipeItemList[i].gameobject.transform, obj.RootRecipeBtn.transform, false);
                obj.ListRecipeItem.Add(mRecipeItemList[i]);
            }
            ///初始化
            for (int i = 0,length= showList.Count; i <length; i++)
            {
                
            }
            for (int i = showList.Count; i < obj.ListRecipeItem.Count; i++)
            {
                obj.ListRecipeItem[i].gameobject.SetActive(false);
            }
            obj.OpenRecipeItemList();
        }
    }
    //点击具体配方
    public void BtnEvt_RecipeBtnClick(int recipeId)
    {
    }


    private int batchNum = 1; //当前批量数量
    private int maxBatchNum = 10;
    //批量生产
    public void BtnEvt_InputBatchNum(int num)
    {
        batchNum += num;
        if (batchNum > maxBatchNum)batchNum = maxBatchNum;
        if (batchNum < 1) batchNum = 1;
        mViewObj.BtnIncreaseNum.enabled = batchNum != maxBatchNum;
        mViewObj.BtnDecreaseNum.enabled = batchNum != 1;
        mViewObj.TextProduceNum.text = batchNum.ToString();
        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(mSelectRecipeId);
        FreshCostItem(recipe);
    }

    public void BtnEvt_LvUpCostClick(string Des)
    {
        UIRootMgr.Instance.SetRootInTopLobby(WindowName, mViewObj.LvUpCostDetial.transform);
        mViewObj.LvUpCostDetial.SetActive(true);
        mViewObj.TextLvUpCostDesc.text = Des;
        mViewObj.BtnMask.SetOnClick(delegate() { mViewObj.LvUpCostDetial.SetActive(false); });
    }

    ///初始化配方境界标签
    void FreshStateButtons(bool reset=true )
    {
        int curLevel = PlayerPrefsBridge.Instance.PlayerData.AuxSkillList[(int)mSkillType].Level;
        for (int i = mRecipeStatelist.Count; i < curLevel; i++)
        {
            GameObject g = Instantiate(mViewObj.Part_ItemRecipe);
            TUtility.SetParent(g.transform, mViewObj.RootItemRecipe.transform, false);
            RecipeStateObj item = new RecipeStateObj(g.GetComponent<UIViewBase>());
            mRecipeStatelist.Add(item);
        }
  
        for (int i = 0; i < mRecipeStatelist.Count; i++)
        {
            RecipeStateObj recpObj = mRecipeStatelist[mRecipeStatelist.Count-i-1];
            AuxSkillLevel skillLevel = AuxSkillLevel.AuxSkillFetcher.GetAuxSkilleByCopy(i + 1, (AuxSkillLevel.SkillType)((int)mSkillType));
            recpObj.TextState.text = TUtility.GetTagStr(skillLevel.name);
            recpObj.LevelDamand = i + 1;
            int tempIndex =i+1;
            if (i >curLevel-1)
            {
                recpObj.gameobject.SetActive(false);
            }
            else
            {
                recpObj.gameobject.SetActive(true);
                recpObj.isOpen = false;
                recpObj.BtnRecipeState.SetOnClick(delegate() { BtnEvt_RecipeItemClick(tempIndex); });
            }
            if (reset)
            {
                recpObj.Mark.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                recpObj.RootRecipeBtn.gameObject.SetActive(false);
                recpObj.contentSizeFit.ResetSize();
            }
        }
      
    }
    void FreshRecipeInfo(int recipeId)
    {
        FreshBadge(false);
        BtnEvt_InputBatchNum(-10);
        mCurFinishTime = 0;
        mViewObj.PanelProduce.SetActive(true);
        ProduceItem produceItem =  PlayerPrefsBridge.Instance.GetProduceItem(recipeId);
        if (produceItem != null) //当前配方有制作进度
        {
            mViewObj.UpSpeed.SetActive(true);
            mViewObj.Produce.SetActive(false);
            long LastSitTime = produceItem.FinishTime - AppTimer.CurTimeStampMsSecond;
            if (LastSitTime > 0)//还未完成
            {
                batchNum = produceItem.Num;
                mCurFinishTime = produceItem.FinishTime;
                mViewObj.TextBtnUpSpeed.text = "加  速";
                mViewObj.BtnUpSpeed.SetOnClick(delegate() { BtnEvt_Accelerate(recipeId); });
            }
            else
            {
                LastSitTime = 0;
                mViewObj.TextBtnUpSpeed.text = "领  取";
                FreshBadge(true);
                mViewObj.BtnUpSpeed.SetOnClick(delegate() { BtnEvt_FinishProduce(recipeId); });
            }
            mViewObj.TextLastTime.text = TUtility.TimeSecondsToDayStr_LCD(LastSitTime.ToInt_1000());
        }
        else
        {
            mViewObj.UpSpeed.SetActive(false);
            mViewObj.Produce.SetActive(true);
            mViewObj.BtnProduce.SetOnClick(delegate() { BtnEvt_Produce(recipeId); });
        }

        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(recipeId);
        mViewObj.TextName.text = recipe.name;
        mViewObj.TextDes.text = "\u3000\u3000" + recipe.Desc;
        if (mSkillType == AuxSkillLevel.SkillType.MakeDrug)
        {
            mViewObj.TextType.text = "";
            mViewObj.TextInfo0.text = string.Format("丹药效果: {0}", recipe.OneStr);
            mViewObj.TextInfo1.text = string.Format("产出内容: {0}", recipe.TwoStr);
            mViewObj.TextInfo2.text = string.Format("成功概率: {0}", recipe.ThreeStr);
            mViewObj.TextTitleCostMater.text = "炼丹材料";          
            mViewObj.TextBtnForgeProduce.text = TUtility.GetTagStr("制作");
        }
        else if (mSkillType == AuxSkillLevel.SkillType.Forge)
        {
            Equip equip = (Equip)(recipe.GetTranslateObject());
            mViewObj.TextInfo0.text = string.Format("最高品质: {0}", recipe.OneStr);
            mViewObj.TextInfo1.text = string.Format("基础属性: {0}", recipe.TwoStr);
            mViewObj.TextInfo2.text = string.Format("携带等级: {0}", recipe.ThreeStr);
            mViewObj.TextTitleCostMater.text = "炼器材料";
            //mViewObj.TextType.text = TUtility.TryGetEquipTypeString(equip.EquipPos);
            mViewObj.TextBtnForgeProduce.text = TUtility.GetTagStr("制作");
        }
        else if (mSkillType == AuxSkillLevel.SkillType.Mine)
        {
            mViewObj.TextType.text = "";
            mViewObj.TextInfo0.text = string.Format("灵矿产量: {0}", recipe.OneStr);
            mViewObj.TextInfo1.text = string.Format("稀有矿石: {0}", recipe.TwoStr);
            mViewObj.TextInfo2.text = string.Format("开启方式: {0}", recipe.ThreeStr);
            mViewObj.TextTitleCostMater.text = "产出矿石";           
            mViewObj.TextBtnForgeProduce.text = TUtility.GetTagStr("开采");
        }
        else if (mSkillType == AuxSkillLevel.SkillType.GatherHerb)
        {
            mViewObj.TextType.text = "";
            mViewObj.TextInfo0.text = string.Format("灵草产量: {0}", recipe.OneStr);
            mViewObj.TextInfo1.text = string.Format("稀有灵草: {0}", recipe.TwoStr);
            mViewObj.TextInfo2.text = string.Format("开启方式: {0}", recipe.ThreeStr);
            mViewObj.TextTitleCostMater.text = "产出草药";       
            mViewObj.TextBtnForgeProduce.text = TUtility.GetTagStr("采集");
        }
    }
    void FreshCostItem(Recipe recipe)
    {
        if (recipe.Type == Recipe.RecipeType.Drug || recipe.Type == Recipe.RecipeType.Equip)
        {
            mViewObj.TextCost0.text = string.Format("灵石: {0}", recipe.Money * batchNum);
            mViewObj.TextCost0.gameObject.SetActive(true);
            Item item;
            for (int i = 0; i < mViewObj.TextCostList.Count; i++)
            {
                if (i < recipe.Science.Length && i < recipe.Number.Length)
                {
                    item = Item.Fetcher.GetItemCopy(recipe.Science[i]);
                    if (item == null) continue;
                    int costNum = recipe.Number[i] * batchNum;
                    mViewObj.TextCostList[i].text = string.Format("{0}: {1}/{2}", item.name, PlayerPrefsBridge.Instance.GetItemNum(recipe.Science[i]), costNum);
                    mViewObj.TextCostList[i].gameObject.SetActive(true);
                }
                else
                    mViewObj.TextCostList[i].gameObject.SetActive(false);
            }
            string costTime = TUtility.TimeSecondsToDayStr_LCD(recipe.Time * batchNum);
            AuxSkillLevel skill = PlayerPrefsBridge.Instance.PlayerData.AuxSkillList[(int)mSkillType];
            bool ProficiencyFull = skill.CurProficiency >= skill.Proficiency;
            mViewObj.TextInfo3.text = string.Format("消耗时间: {0}", costTime);
        }
        else if (recipe.Type == Recipe.RecipeType.GatherHerb|| recipe.Type == Recipe.RecipeType.Mine)
        {
            mViewObj.TextCost0.gameObject.SetActive(false);
            for (int i = 0; i < mViewObj.TextCostList.Count; i++)
            {
                mViewObj.TextCostList[i].gameObject.SetActive(false);
            }
            Dictionary<int, LootType> recipeLoot = Loot.GetLootsId(new List<int> { recipe.Quality0,recipe.Quality1,recipe.Quality2,recipe.Quality3 });
            List<int> keys = new List<int>(recipeLoot.Keys);
            if (keys != null && keys.Count > 0)
            {
                Item item;
                item = Item.Fetcher.GetItemCopy(keys[0]);
                mViewObj.TextCost0.text = item.name;
                mViewObj.TextCost0.gameObject.SetActive(true);

                for (int i = 0; i < keys.Count-1; i++)
                {
                    item = Item.Fetcher.GetItemCopy(keys[i+1]);
                    if (item == null) continue;
                    mViewObj.TextCostList[i].text = item.name;
                    mViewObj.TextCostList[i].gameObject.SetActive(true);                 
                }
                string costTime = TUtility.TimeSecondsToDayStr_LCD(recipe.Time * batchNum);
                AuxSkillLevel skill = PlayerPrefsBridge.Instance.PlayerData.AuxSkillList[(int)mSkillType];
                bool ProficiencyFull = skill.CurProficiency >= skill.Proficiency;
                mViewObj.TextInfo3.text = string.Format("消耗时间: {0}", costTime);
            }      
        }
      
    }
    ///刷新生活技能熟练度
    void FreshProficiency()
    {
        AuxSkillLevel skill = PlayerPrefsBridge.Instance.PlayerData.AuxSkillList[(int)mSkillType];
        if (skill != null)
        {
            mViewObj.TextState.text = skill.name;
            if (skill.CurProficiency >= skill.Proficiency)
            {
                mViewObj.BtnUpAuxSkill.SetOnClick(delegate() { BtnEvt_UpLevelAuxSkill(skill.mType); });
                mViewObj.LvUpTip.gameObject.SetActive(true);
                mViewObj.ProScrollRoot.gameObject.SetActive(false);
                HeroLevelUp lvUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(skill.State);
                mViewObj.TextLvDemand.text = string.Format("角色等级达到{0}", lvUp.name);         
                Item item = Item.Fetcher.GetItemCopy(skill.Item);
                mViewObj.TextLvUpCost.text = item.name;
                mViewObj.BtnLvUpCost.SetOnClick(delegate() { BtnEvt_LvUpCostClick(item.desc); });
            }
            else
            {
                mViewObj.LvUpTip.gameObject.SetActive(false);
                mViewObj.ProScrollRoot.gameObject.SetActive(true);
                mViewObj.ProScrollRoot.Fresh((float)skill.CurProficiency / skill.Proficiency, string.Format("{0}/{1}", skill.CurProficiency, skill.Proficiency), "熟练度");
            }

        }
    }
    private long mOffsetTime = 0;
    void FreshTime()
    {
        if (mCurFinishTime > 0)
        {
            mOffsetTime = mCurFinishTime - AppTimer.CurTimeStampMsSecond;
            if (mOffsetTime > 0)
                mViewObj.TextLastTime.text = TUtility.TimeSecondsToDayStr_LCD(mOffsetTime.ToInt_1000());
            else
            {
                FreshBadge(true);
                mCurFinishTime = 0;
                mViewObj.TextBtnUpSpeed.text = "领取";
                mViewObj.BtnUpSpeed.SetOnClick(delegate() { BtnEvt_FinishProduce(mSelectRecipeId); });
            }
        }
    }


    //TODO :生产制作加速花费暂定为1
    int costAccelerate = 1;//加速花费
    public void BtnEvt_UpLevelAuxSkill(AuxSkillLevel.SkillType type)
    {
        AuxSkillLevel skill = PlayerPrefsBridge.Instance.PlayerData.AuxSkillList[(int)type];
        int  costID = skill.Item;
        HeroLevelUp lvUp = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(skill.State);
        if (PlayerPrefsBridge.Instance.PlayerData.Level < skill.State && UIRootMgr.Instance.MessageBox.ShowStatus(string.Format("升级需要人物等级达到:{0}", lvUp.name)))
            return;
        Item item = Item.Fetcher.GetItemCopy(costID);
        if (PlayerPrefsBridge.Instance.GetItemByIdx(costID) == null && UIRootMgr.Instance.MessageBox.ShowStatus(string.Format("缺少材料:{0}", item.name)))
            return;    
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_AuxSkillLevelUp((byte)type));   
    }
    public void BtnEvt_Produce(int recipeID)
    {
    }
    public void BtnEvt_Accelerate(int recipeID)
    {
        long offsetTime = mCurFinishTime - AppTimer.CurTimeStampMsSecond;
        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(mSelectRecipeId);
        int costDiamond = Mathf.CeilToInt(offsetTime.ToInt_1000()/(float)recipe.Time) * costAccelerate;
        UIRootMgr.Instance.MessageBox.ShowInfo_HaveCancel(string.Format("是否消耗{0}仙玉立即完成？", costDiamond), delegate() { CallBack_Accelerate(recipeID, costDiamond); });
    }
    public void CallBack_Accelerate(int recipeID,int costDiamond)
    {
        if (PlayerPrefsBridge.Instance.PlayerData.Diamond < costDiamond && UIRootMgr.Instance.MessageBox.ShowStatus(ServerStatusCode.GLOBAL_WARN_CODE_ZHUAN_SHI_BU_ZU))
            return;
        ProduceItem produceItem = PlayerPrefsBridge.Instance.GetProduceItem(recipeID);
        if (produceItem != null) //当前配方有制作进度
        {
            long LastSitTime = produceItem.FinishTime - AppTimer.CurTimeStampMsSecond;
            if (LastSitTime > 0)//还未完成
            {
                UIRootMgr.Instance.IsLoading = true;
                GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_FinishProduce(true , recipeID));
            }
        }
    }
    public void BtnEvt_FinishProduce(int recipeID)
    {
        UIRootMgr.Instance.IsLoading = true;
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_FinishProduce(false ,recipeID));
    }


    public void S2C_Produce(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_Produce msg = MessageBridge.Instance.S2C_Produce(ios);
        BtnEvt_RecipeBtnClick(mSelectRecipeId);
    }
    public void S2C_ProduceAccelerate(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;      
        NetPacket.S2C_ProduceAccelerate msg = MessageBridge.Instance.S2C_ProduceAccelerate(ios);
        BtnEvt_RecipeBtnClick(mSelectRecipeId);
    }
    public void S2C_FinishProduce(BinaryReader ios) 
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_FinishProduce msg = MessageBridge.Instance.S2C_FinishProduce(ios);
        UIRootMgr.LobbyUI.ShowDropInfo(msg.TranslateList);
        UIRootMgr.LobbyUI.AppendTextNewLine(string.Format("获得{0}点熟练度",msg.Prociency));
        FreshProficiency();
        BtnEvt_RecipeBtnClick(mSelectRecipeId);
    }
    public void S2C_AuxSkillLevelUp(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        
    }

   
    bool GetCostCheck(int recipeID, out string itemName)
    {
        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(recipeID);
        Item myItem = null;
        itemName = "";
        bool costEnough = true;
        for (int i = 0; i < recipe.Science.Length; i++)
        {
            myItem = PlayerPrefsBridge.Instance.GetItemByIdx(recipe.Science[i]);
            itemName = Item.Fetcher.GetItemCopy(recipe.Science[i]).name;
            if (myItem==null||myItem.num < recipe.Number[i] * batchNum)
            {
                costEnough = false;
                break;
            }
        }
        return costEnough;
    }
    public string GetCostStr(int recipeId)
    {
        Recipe recipe = Recipe.RecipeFetcher.GetRecipeByCopy(recipeId);
        Item item;
        Item myItem;
        string str = "";
        for (int i = 0; i < recipe.Science.Length && i < recipe.Number.Length; i++)
        {
            item = Item.Fetcher.GetItemCopy(recipe.Science[i]);
            if (item == null) continue;
            myItem = PlayerPrefsBridge.Instance.GetItemByIdx(recipe.Science[i]);
            str = string.Format("{0}:{1}/{2}\r\n", item.name, myItem == null ? 0 : (int)myItem.num, recipe.Number[i]);
        }
        return str;
    }

    public void CloseWin()
    {
        base.CloseWindow();
        //UIRootMgr.Instance.OpenWindow<WindowBig_CaveChoose>(WinName.WindowBig_CaveChoose, CloseUIEvent.CloseCurrent).OpenWindow(WindowBig_CaveChoose.ChildTab.LifeSkill);
        //UIRootMgr.Instance.OpenWindow<Window_Recipe>(WinName.Window_Recipe, CloseUIEvent.None).OpenWindow();
    }


    void Update()
    {
        FreshTime();
    }
  
}
