using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Window_BuyNumChoose : WindowBase
{
    public class ViewObj
    {
        public Text NumText;
        public Button MaskBtn;
        public Button SubNumBtn;
        public Button AddNumBtn;
        public TextButton OkTBtn;
        public TextButton CancelTBtn;
        public Slider NumSlider;
        public ViewObj(UIViewBase view)
        {
            if (NumText != null) return;
            NumText = view.GetCommon<Text>("NumText");
            MaskBtn = view.GetCommon<Button>("MaskBtn");
            SubNumBtn = view.GetCommon<Button>("SubNumBtn");
            AddNumBtn = view.GetCommon<Button>("AddNumBtn");
            OkTBtn = view.GetCommon<TextButton>("OkTBtn");
            CancelTBtn = view.GetCommon<TextButton>("CancelTBtn");
            NumSlider = view.GetCommon<Slider>("NumSlider");
        }
    }

    private ViewObj mViewObj;
    private System.Action<int> mChooseOver;
    private int mMaxNum;
    private int mCurNum;
    private int mCurIdx;
    public  void OpenWindow(int commodityIdx , int maxNum, System.Action<int> chooseOver)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        mChooseOver = chooseOver;
        OpenWin();
        Init(commodityIdx, maxNum);
        FreshNum(1);
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    void Init(int commodityIdx, int maxNum)
    {
        mMaxNum = maxNum;
        mCurIdx = commodityIdx;

        mViewObj.OkTBtn.TextBtn.text = LangMgr.GetText("确 认");
        mViewObj.CancelTBtn.TextBtn.text = LangMgr.GetText("取 消");
        mViewObj.OkTBtn.SetOnClick(BtnEvt_Ok);
        mViewObj.CancelTBtn.SetOnAduioClick(delegate() { CloseWindow(); });
        mViewObj.MaskBtn.SetOnClick(delegate() { CloseWindow(); });

        mViewObj.AddNumBtn.SetOnClick(delegate() { BtnEvt_AddNum(1); });
        mViewObj.SubNumBtn.SetOnClick(delegate() { BtnEvt_AddNum(-1); });

        Slider.SliderEvent sliderListener = new Slider.SliderEvent();
        sliderListener.RemoveAllListeners();
        sliderListener.AddListener(BtnEvt_SliderChange);
        mViewObj.NumSlider.onValueChanged = sliderListener;
    }

    void FreshNum(int curNum)
    {
        mCurNum = curNum;
        mViewObj.NumSlider.value = curNum/(float)mMaxNum;
        Commodity commodity = Commodity.CommodityFetcher.GetCommodityByCopy(mCurIdx, false);
        if (commodity == null) return;
        GoodsToDrop needGoods = new GoodsToDrop(commodity.sellId, commodity.number * curNum, commodity.sellType);
        mViewObj.NumText.text = LangMgr.GetText("是否花费{0}购买\n{1}个{2}？", needGoods.GetString(), curNum, commodity.name);
    }
    public void BtnEvt_SliderChange(float val)
    {
        FreshNum(Mathf.Clamp(Mathf.RoundToInt(mMaxNum*val), 1, mMaxNum));
    }

    public void BtnEvt_AddNum(int num)
    {
        int curNum = mCurNum + num;
        curNum = Mathf.Clamp(curNum, 1, mMaxNum);
        FreshNum(curNum);
    }

    public void BtnEvt_Ok()
    {
        CloseWindow();
        if (mChooseOver != null)
        {
            mChooseOver(mCurNum);
        }
    }

}
