using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class Window_Mail : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_MailItem;
        public Transform MailItemRoot;
        public Transform MailDetailRoot;
        public Text MailNameText;
        public Text MailDescText;
        public Text LootText;
        public Button ReciveMailBtn;
        public Button CancelBtn;
        public Text ReciveMailText;
        public Button CloseBtn;
        public Scrollbar ScrollbarVertical;
        public ViewObj(UIViewBase view)
        {
            if (Part_MailItem == null) Part_MailItem = view.GetCommon<GameObject>("Part_MailItem");
            if (MailItemRoot == null) MailItemRoot = view.GetCommon<Transform>("MailItemRoot");
            if (MailDetailRoot == null) MailDetailRoot = view.GetCommon<Transform>("MailDetailRoot");
            if (MailNameText == null) MailNameText = view.GetCommon<Text>("MailNameText");
            if (MailDescText == null) MailDescText = view.GetCommon<Text>("MailDescText");
            if (LootText == null) LootText = view.GetCommon<Text>("LootText");
            if (ReciveMailBtn == null) ReciveMailBtn = view.GetCommon<Button>("ReciveMailBtn");
            if (CancelBtn == null) CancelBtn = view.GetCommon<Button>("CancelBtn");
            if (ReciveMailText == null) ReciveMailText = view.GetCommon<Text>("ReciveMailText");
            if (CloseBtn == null) CloseBtn = view.GetCommon<Button>("CloseBtn");
            if (ScrollbarVertical == null) ScrollbarVertical = view.GetCommon<Scrollbar>("ScrollbarVertical");

        }
    }
    public class MailItemObj : SmallViewObj
    {
        public Button ItemBtn;
        public Text TextTitleEvent;
        public Image LootImage;
        public Text TextLeftTime;
        public override void Init(UIViewBase view)
        {
            base.Init(view);
            if (ItemBtn == null) ItemBtn = view.GetCommon<Button>("ItemBtn");
            if (TextTitleEvent == null) TextTitleEvent = view.GetCommon<Text>("TextTitleEvent");
            if (TextLeftTime == null) TextLeftTime = view.GetCommon<Text>("TextLeftTime");
            if (LootImage == null) LootImage = view.GetCommon<Image>("LootImage");
        }
    }
    private List<MailItemObj> mMailItemList;

    private ViewObj mViewObj;
    private int mCurMailIdx = 0;
    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        OpenWin();
        if (!PlayerPrefsBridge.Instance.MailData.Inited)//如果邮件信息还没有
        {
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.PullInfo, S2C_PullInfo);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullInfo(PullInfoType.MailList, 0));
            UIRootMgr.Instance.IsLoading = true;
        }
        else
        {
            Fresh();
        }
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    public void S2C_PullInfo(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.PullInfo, null);
        Fresh();
    }

    public void UpdateNextMail(float val)//当滑动到底部时，添加下一页的邮件
    {
        if (val==0 && PlayerPrefsBridge.Instance.MailData.MailList.Count != PlayerPrefsBridge.Instance.MailData.MailAmount)
        {
            int curAmount = PlayerPrefsBridge.Instance.MailData.MailList.Count;
            GameClient.Instance.RegisterNetCodeHandler(NetCode_S.PullInfo, S2C_PullInfo);
            GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_PullInfo(PullInfoType.MailList, curAmount));
            UIRootMgr.Instance.IsLoading = true;
        }
    }

    public void Fresh()
    {
        mViewObj.MailDetailRoot.gameObject.SetActive(false);
        mCurMailIdx = 0;
        List<Mail> mailList = PlayerPrefsBridge.Instance.MailData.MailList;

        if (mMailItemList == null) mMailItemList = new List<MailItemObj>();
        mMailItemList = TAppUtility.Instance.AddViewInstantiate<MailItemObj>(mMailItemList, mViewObj.Part_MailItem,
            mViewObj.MailItemRoot, mailList.Count);

        //刷新邮件列表
        for (int i = 0; i < mailList.Count; i++)
        {
            mMailItemList[i].TextTitleEvent.text = mailList[i].name+string.Format("({0})",mailList[i].IsOpened?"已读":"未读");
            mMailItemList[i].LootImage.gameObject.SetActive(mailList[i].GoodsListLength>0);
            mMailItemList[i].TextLeftTime.text = TUtility.GetStringTime2((mailList[i].LifeTime - TimeUtils.CurrentTimeMillis)/1000);
            int mailIdx = mailList[i].idx;
            mMailItemList[i].ItemBtn.SetOnClick(delegate() { BtnEvt_OpenMailDetail(mailIdx); });
        }

        mViewObj.CancelBtn.SetOnClick(BtnEvt_CloseMailDetail);
        mViewObj.ReciveMailBtn.SetOnClick(BtnEvt_ReceiveMail);
        mViewObj.CloseBtn.SetOnClick(delegate() { CloseWindow(); });
        mViewObj.ScrollbarVertical.onValueChanged.RemoveAllListeners();
        mViewObj.ScrollbarVertical.onValueChanged.AddListener(UpdateNextMail);
    }
    public void BtnEvt_OpenMailDetail(int mailIdx)//打开邮件
    {
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MailDetail, S2C_MailDetail);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_MailDetail(mailIdx));
        UIRootMgr.Instance.IsLoading = true;
    }
    public void BtnEvt_CloseMailDetail()//关闭邮件详情
    {
        mCurMailIdx = 0;
        mViewObj.MailDetailRoot.gameObject.SetActive(false);
    }

    void S2C_MailDetail(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.MailDetail, null);
        NetPacket.S2C_MailDetail msg = MessageBridge.Instance.S2C_MailDetail(ios);
        PlayerPrefsBridge.Instance.OpenMailDetail(msg.DetailMail.idx); //将此邮件设为打开
        Fresh();//刷新邮件列表

        //显示打开的邮件信息
        mCurMailIdx = msg.DetailMail.idx;
        List<Mail> mailList = PlayerPrefsBridge.Instance.MailData.MailList;
        Mail mail=null;
        for (int i = 0; i < mailList.Count; i++)
        {
            if (mailList[i].idx == mCurMailIdx) { mail = mailList[i]; break; }
        }
        if (mail == null)
        {
            TDebug.LogError(string.Format("没有找到对应邮件:{0}",mCurMailIdx));
            return;
        }
        mViewObj.MailDetailRoot.gameObject.SetActive(true);
        mViewObj.MailDescText.text = msg.DetailMail.Context;
        mViewObj.MailNameText.text = msg.DetailMail.name;
        mViewObj.ReciveMailText.text = mail.GoodsListLength > 0 ? "领取并删除" : "删除";
        StringBuilder lootStr = new StringBuilder();
        for (int i = 0; i < msg.DetailMail.GoodsList.Length; i++)
        {
            lootStr.Append(string.Format("{0}\n", msg.DetailMail.GoodsList[i].GetString()));
        }
        mViewObj.LootText.text = lootStr.ToString();
    }

    
    public void BtnEvt_ReceiveMail()//领取邮件
    {
        if (mCurMailIdx == 0) return;
        GameClient.Instance.RegisterNetCodeHandler(NetCode_S.ReceiveMail, S2C_ReceiveMail);
        GameClient.Instance.SendMessage(MessageBridge.Instance.C2S_ReceiveMail(mCurMailIdx));
        UIRootMgr.Instance.IsLoading = true;
    }

    void S2C_ReceiveMail(BinaryReader ios)
    {
        UIRootMgr.Instance.IsLoading = false;
        NetPacket.S2C_ReceiveMail msg = MessageBridge.Instance.S2C_ReceiveMail(ios);
        LobbySceneMainUIMgr.Instance.ShowDropInfo(msg.GoodsList, "你领取了邮件");
        mViewObj.MailDetailRoot.gameObject.SetActive(false);
        Fresh();
    }
}
