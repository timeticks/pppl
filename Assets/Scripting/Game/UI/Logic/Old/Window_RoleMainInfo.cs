using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Window_RoleMainInfo : WindowBase {

    public class ViewObj
    {
        public Mesh Cylinder01;
        public RawImage HexagonImage;
        public NumScrollTool ExpScrollRoot;
        public NumScrollTool HpScrollRoot;
        public NumScrollTool MpScrollRoot;
        public Text RoleTitleText;
        public Text TextSect;
        public Text TextAge;
        public Text TextState;
        public Text StrText;
        public Text LukText;
        public Text ManaText;
        public Text VitText;
        public Text MindText;
        public Text ConText;
        public Text TextGold;
        public Text TextStone;
        public Text TextLevel;
        public Text TextPotential;
        public ViewObj(UIViewBase view)
        {
            Cylinder01 = view.GetCommon<Mesh>("Cylinder01");
            HexagonImage =view.GetCommon<RawImage>("HexagonImage");
            ExpScrollRoot = view.GetCommon<NumScrollTool>("ExpScrollRoot");
            HpScrollRoot = view.GetCommon<NumScrollTool>("HpScrollRoot");
            MpScrollRoot = view.GetCommon<NumScrollTool>("MpScrollRoot");
            RoleTitleText = view.GetCommon<Text>("RoleTitleText");
            TextSect = view.GetCommon<Text>("TextSect");
            TextAge = view.GetCommon<Text>("TextAge");
            TextState = view.GetCommon<Text>("TextState");
            StrText = view.GetCommon<Text>("StrText");
            LukText = view.GetCommon<Text>("LukText");
            ManaText = view.GetCommon<Text>("ManaText");
            VitText = view.GetCommon<Text>("VitText");
            MindText = view.GetCommon<Text>("MindText");
            ConText = view.GetCommon<Text>("ConText");
            TextGold = view.GetCommon<Text>("TextGold");
            TextStone = view.GetCommon<Text>("TextStone");
            TextLevel = view.GetCommon<Text>("TextLevel");
            TextPotential = view.GetCommon<Text>("TextPotential");
        }
    }
    private ViewObj mViewObj;
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        Init();
    }

    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    public void Init()
    {
        GamePlayer player = PlayerPrefsBridge.Instance.PlayerData;
        OldHero hero = null;//PlayerPrefsBridge.Instance.GetHeroWithProperties();
       
        mViewObj.RoleTitleText.text = "称号: 【无名小辈】";

        string sectName = "无";
        if (player.MySect != Sect.SectType.None)
        {
            Sect sect =Sect.SectFetcher.GetSectByCopy(player.MySect);
            if (sect != null) sectName = sect.name;
        }
        long curTime = AppTimer.CurTimeStampMsSecond;
        long birthTime = PlayerPrefsBridge.Instance.PlayerData.BirthTime;
        long LoginTime = (curTime - birthTime) / 1000 / 24 / 60 / 60;

        mViewObj.TextSect.text = string.Format("宗门:{0}" , sectName);
        mViewObj.TextAge.text = string.Format("年龄: {0}岁", 16 + LoginTime);
        HeroLevelUp level = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(player.Level);
        mViewObj.TextState.text = string.Format("境界: {0}", level.name);
        mViewObj.TextLevel.text = string.Format("等级: {0}级", player.Level);
        long curExp = 0;
        long LeveUpExp = 0;
        if (player.Level == 1)
        {
            curExp = player.Exp;
            LeveUpExp = level.exp;
        }
        else
        {
            HeroLevelUp lastLevel = HeroLevelUp.LevelUpFetcher.GetLevelUpByCopy(player.Level - 1);
            curExp = player.Exp - lastLevel.exp;
            LeveUpExp = level.exp - lastLevel.exp;
        }

        mViewObj.ExpScrollRoot.Fresh(
            (float)(curExp / (double)LeveUpExp),
            string.Format("{0}/{1}", curExp, LeveUpExp),
            "经验: ");
        mViewObj.HpScrollRoot.Fresh(
            hero.Hp / (float)hero.Hp,
            string.Format("{0}/{1}", hero.Hp, hero.Hp),
            "生命: ");
        mViewObj.MpScrollRoot.Fresh(
            hero.Mp / (float)hero.Mp,
            string.Format("{0}/{1}", hero.Mp, hero.Mp),
            "灵力: ");

        mMainProm[0] = hero.Str;
        mMainProm[1] = hero.Luk;
        mMainProm[2] = hero.Mana;
        mMainProm[3] = hero.Vit;
        mMainProm[4] = hero.Mind;
        mMainProm[5] = hero.Con;
        mViewObj.StrText.text = string.Format("力量{0}", hero.Str);
        mViewObj.LukText.text = string.Format("机缘{0}", hero.Luk);
        mViewObj.ManaText.text = string.Format("法力{0}", hero.Mana);
        mViewObj.VitText.text = string.Format("魂力{0}", hero.Vit);
        mViewObj.MindText.text = string.Format("神识{0}", hero.Mind);
        mViewObj.ConText.text = string.Format("体魄{0}", hero.Con);
        mViewObj.TextStone.text = string.Format("仙玉  {0}", player.Diamond);
        mViewObj.TextGold.text = string.Format("灵石  {0}", player.Gold);
        mViewObj.TextPotential.text = string.Format("潜能  {0}",player.Potential);
        float maxNum = 0;
        for (int i = 0, length = mMainProm.Length; i < length; i++)
        {
            maxNum = Mathf.Max(mMainProm[i], maxNum);
        }
        maxNum = Mathf.Max(maxNum, 100);
        for (int i = 0, length = mMainProm.Length; i < length; i++)
        {
            mMainProm[i] = Mathf.Max(0.15f, mMainProm[i]/maxNum);
        }
        mMainPctList.Clear();
        mMainPctList = new List<float>(mMainProm);
        mMainPctList.Add(0);
    }

    #region 六维属性

    private float[] mMainProm = new float[6];
    public List<float> mMainPctList = new List<float>();
    private CanvasRenderer mHexgeonCanRender;
    void Update() //刷新六维信息和六边形
    {
        if (mMainPctList.Count > 0)
        {
            mViewObj.HexagonImage.gameObject.SetActive(true);
            //0 5 4
            //1 2 3
            if (mHexgeonCanRender == null)
            {
                mHexgeonCanRender = mViewObj.HexagonImage.GetComponent<CanvasRenderer>();
            }
            TAppUtility.FreshHeaxgonVertex(mMainPctList, mViewObj.Cylinder01);
            mHexgeonCanRender.SetMesh(mViewObj.Cylinder01);
            mHexgeonCanRender.SetColor(new Color(0.9f, 0.7f, 0.4f, 0.7f));
        }
    }
   
    #endregion
    
}
