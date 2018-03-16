using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Window_RoleDetailInfo : WindowBase {

    public class ViewObj
    {
        public Text BaseAtriText;
        public Text HpText;
        public Text AtkText;
        public Text MDefText;
        public Text MpText;
        public Text PDefText;
        public Text HighAttriText;
        public Text HitText;
        public Text CritText;
        public Text MissText;
        public Text DefCritText;
        public Text BrokenText;
        public Text BlockText;
        public Text CritDmgText;
        public Text DefCritDmgText;
        public Text UpDmgText;
        public Text DownDmgText;
        public Text IceDmg;
        public Text PoisonDmg;
        public Text IceDef;
        public Text PoisonDef;
        public Text YinDef;
        public Text FireDef;
        public Text YinDmg;
        public Text FireDmg;
        public Text ThurderDmg;
        public Text YangDmg;
        public Text ThurderDef;
        public Text YangDef;
        public Text TextStr;
        public Text TextCon;
        public Text TextMind;
        public Text TextVit;
        public Text TextMana;
        public Text TextLuk;
        public ViewObj(UIViewBase view)
        {
            BaseAtriText = view.GetCommon<Text>("BaseAtriText");
            HpText = view.GetCommon<Text>("HpText");
            AtkText = view.GetCommon<Text>("AtkText");
            MDefText = view.GetCommon<Text>("MDefText");
            MpText = view.GetCommon<Text>("MpText");
            PDefText = view.GetCommon<Text>("PDefText");
            HighAttriText = view.GetCommon<Text>("HighAttriText");
            HitText = view.GetCommon<Text>("HitText");
            CritText = view.GetCommon<Text>("CritText");
            MissText = view.GetCommon<Text>("MissText");
            DefCritText = view.GetCommon<Text>("DefCritText");
            BrokenText = view.GetCommon<Text>("BrokenText");
            BlockText = view.GetCommon<Text>("BlockText");
            CritDmgText = view.GetCommon<Text>("CritDmgText");
            DefCritDmgText = view.GetCommon<Text>("DefCritDmgText");
            UpDmgText = view.GetCommon<Text>("UpDmgText");
            DownDmgText = view.GetCommon<Text>("DownDmgText");
            IceDmg = view.GetCommon<Text>("IceDmg");
            PoisonDmg = view.GetCommon<Text>("PoisonDmg");
            IceDef = view.GetCommon<Text>("IceDef");
            PoisonDef = view.GetCommon<Text>("PoisonDef");
            YinDef = view.GetCommon<Text>("YinDef");
            FireDef = view.GetCommon<Text>("FireDef");
            YinDmg = view.GetCommon<Text>("YinDmg");
            FireDmg = view.GetCommon<Text>("FireDmg");
            ThurderDmg = view.GetCommon<Text>("ThurderDmg");
            YangDmg = view.GetCommon<Text>("YangDmg");
            ThurderDef = view.GetCommon<Text>("ThurderDef");
            YangDef = view.GetCommon<Text>("YangDef");
            if (TextStr == null) TextStr = view.GetCommon<Text>("TextStr");
            if (TextCon == null) TextCon = view.GetCommon<Text>("TextCon");
            if (TextMind == null) TextMind = view.GetCommon<Text>("TextMind");
            if (TextVit == null) TextVit = view.GetCommon<Text>("TextVit");
            if (TextMana == null) TextMana = view.GetCommon<Text>("TextMana");
            if (TextLuk == null) TextLuk = view.GetCommon<Text>("TextLuk");
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
        OldHero hero = null;//PlayerPrefsBridge.Instance.GetHeroWithProperties();
        Fresh();
    }

    public void Fresh()
    {
        GamePlayer player = PlayerPrefsBridge.Instance.PlayerData;
        Hero hero = PlayerPrefsBridge.Instance.GetHeroWithProperties();

        mViewObj.TextStr.text = string.Format("等级: {0}", player.Level);
        mViewObj.TextLuk.text = string.Format("经验: {0}/{1}", player.Exp , HeroLevelUp.GetCurLevelExp(player.Level));
        mViewObj.TextMana.text = string.Format("法力: {0}", "");
        mViewObj.TextVit.text = string.Format("魂力: {0}", "");
        mViewObj.TextMind.text = string.Format("神识: {0}", "");
        mViewObj.TextCon.text = string.Format("体魄: {0}", "");

        mViewObj.BaseAtriText.text = "基础属性";
        mViewObj.HpText.text = string.Format("生命: {0}/{1}", hero.hp, hero.hp);
        mViewObj.MpText.text = string.Format("魔法: {0}/{1}", hero.mp, hero.mp);
        mViewObj.AtkText.text = string.Format("攻击: {0}", hero.phyAtk);
        mViewObj.PDefText.text = string.Format("物御: {0}", hero.phyDef);
        mViewObj.MDefText.text = string.Format("法御: {0}", hero.magDef);

        mViewObj.HighAttriText.text = "高级属性"; 
        mViewObj.HitText.text = string.Format("命中: {0}%", hero.hit.ToFloat_100().ToString("f1"));
        mViewObj.MissText.text = string.Format("闪避: {0}%", hero.dodge.ToFloat_100().ToString("f1"));
        mViewObj.BrokenText.text = string.Format("破招: {0}%","");
        mViewObj.BlockText.text = string.Format("招架: {0}%", "");
        mViewObj.CritText.text = string.Format("暴击: {0}%", (hero.critPct).ToFloat_100().ToString("f1"));
        mViewObj.DefCritText.text = string.Format("抗暴: {0}%", (hero.defCrit).ToFloat_100().ToString("f1"));
        mViewObj.CritDmgText.text = string.Format("暴伤: {0}%", (hero.critDmg).ToFloat_100().ToString("f1"));
        mViewObj.DefCritDmgText.text = string.Format("韧性: {0}%", "");
        mViewObj.UpDmgText.text = string.Format("增伤: {0}%", (hero.extraDmg).ToFloat_100().ToString("f1"));
        mViewObj.DownDmgText.text = string.Format("减伤: {0}%", (hero.dmgReduce).ToFloat_100().ToString("f1"));

        //mViewObj.IceDmg.text = string.Format("冰攻: {0}%", hero.IceDmgDec.ToFloat_100().ToString("f1"));
        //mViewObj.FireDmg.text = string.Format("火攻: {0}%", hero.FireDmgInc.ToFloat_100().ToString("f1"));
        //mViewObj.ThurderDmg.text = string.Format("雷攻: {0}%", hero.ThunderDmgInc.ToFloat_100().ToString("f1"));
        //mViewObj.PoisonDmg.text = string.Format("毒攻: {0}%", hero.PoisonDmgInc.ToFloat_100().ToString("f1"));
        //mViewObj.YinDmg.text = string.Format("阴攻: {0}%", hero.YinDmgInc.ToFloat_100().ToString("f1"));
        //mViewObj.YangDmg.text = string.Format("阳攻: {0}%", hero.YangDmgInc.ToFloat_100().ToString("f1"));
        //mViewObj.IceDef.text = string.Format("冰抗: {0}%", hero.IceDmgDec.ToFloat_100().ToString("f1"));
        //mViewObj.FireDef.text = string.Format("火抗: {0}%", hero.FireDmgDec.ToFloat_100().ToString("f1"));
        //mViewObj.ThurderDef.text = string.Format("雷抗: {0}%", hero.ThunderDmgDec.ToFloat_100().ToString("f1"));
        //mViewObj.PoisonDef.text = string.Format("毒抗: {0}%", hero.PoisonDmgDec.ToFloat_100().ToString("f1"));
        //mViewObj.YinDef.text = string.Format("阴抗: {0}%", hero.YinDmgDec.ToFloat_100().ToString("f1"));
        //mViewObj.YangDef.text = string.Format("阳抗: {0}%", hero.YangDmgDec.ToFloat_100().ToString("f1"));
    }
}
