using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Window_SellNumChoose : WindowBase
{
    public class ViewObj
    {
        public Button MaskBtn;
        public Text NameText;
        public Text PriceText;
        public Text SellNumText;
        public Button SubNumBtn;
        public Button AddNumBtn;
        public Slider NumSlider;
        public Button OkBtn;

        public ViewObj(UIViewBase view)
        {
            MaskBtn = view.GetCommon<Button>("MaskBtn");
            NameText = view.GetCommon<Text>("NameText");
            PriceText = view.GetCommon<Text>("PriceText");
            SellNumText = view.GetCommon<Text>("SellNumText");
            SubNumBtn = view.GetCommon<Button>("SubNumBtn");
            AddNumBtn = view.GetCommon<Button>("AddNumBtn");
            NumSlider = view.GetCommon<Slider>("NumSlider");
            OkBtn = view.GetCommon<Button>("OkBtn");

        }
    }

    private ViewObj mViewObj;
    private System.Action<int> mChooseOver;
    private int mMaxNum;
    private int mCurNum;
    public  void OpenWindow(int idx , int maxNum, System.Action<int> chooseOver)
    {
        if (mViewObj == null) mViewObj = new ViewObj(mViewBase);
        mChooseOver = chooseOver;
        OpenWin();
        Init(idx, maxNum);
        FreshNum(1);
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    void Init(int idx, int maxNum)
    {
        mMaxNum = maxNum;
        Item item = Item.Fetcher.GetItemCopy(idx);
        if (item == null) return;
        mViewObj.NameText.text = item.name;
        mViewObj.PriceText.text = string.Format("价格:{0}", item.sell);

        mViewObj.OkBtn.SetOnClick(BtnEvt_Ok);
        mViewObj.AddNumBtn.SetOnClick(delegate() { BtnEvt_AddNum(1); });
        mViewObj.SubNumBtn.SetOnClick(delegate() { BtnEvt_AddNum(-1); });

        mViewObj.MaskBtn.SetOnClick(delegate() { CloseWindow(); });
        Slider.SliderEvent sliderListener = new Slider.SliderEvent();
        sliderListener.RemoveAllListeners();
        sliderListener.AddListener(BtnEvt_SliderChange);
        mViewObj.NumSlider.onValueChanged = sliderListener;
    }

    void FreshNum(int curNum)
    {
        mCurNum = curNum;
        mViewObj.NumSlider.value = curNum/(float)mMaxNum;
        mViewObj.SellNumText.text = string.Format("出售数量:{0}", mCurNum);
    }
    public void BtnEvt_SliderChange(float val)
    {
        FreshNum(Mathf.RoundToInt(mMaxNum*val));
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
