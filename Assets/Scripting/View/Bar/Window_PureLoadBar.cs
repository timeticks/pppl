using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window_PureLoadBar : MonoBehaviour
{
    public class ViewObj
    {
        public Scrollbar ProgressScrollbar;
        public RawImage BgImage;
        public Text DescText;
        public ViewObj(UIViewBase view)
        {
            ProgressScrollbar = view.GetCommon<Scrollbar>("ProgressScrollbar");
            BgImage = view.GetCommon<RawImage>("BgImage");
            DescText = view.GetCommon<Text>("DescText");
        }
    }
    private ViewObj mViewObj;

    public void Init()
    {
        if (mViewObj == null)
        {
            mViewObj = new ViewObj(GetComponent<UIViewBase>());
        }
    }

    public void Fresh(float pct, string content)
    {
        mViewObj.ProgressScrollbar.value = pct;
        mViewObj.DescText.text = content;
    }
}
