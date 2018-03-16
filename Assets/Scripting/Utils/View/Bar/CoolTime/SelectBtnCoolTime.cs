using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectBtnCoolTime : CoolTimeBase
{
    public Button m_Btn;
    public Image m_ImageSelected;
    public Image m_IconImage;
    internal bool m_IsSeleted;
    public List<UIAnimationBaseCtrl> m_Ani;
    public Material m_HighlitMat;
    public void SwitchSelect(bool select)
    {
        m_IsSeleted = select;
        m_ImageSelected.gameObject.SetActive(m_IsSeleted);
        if (gameObject.activeSelf)
        {
            if (select) { m_Ani.ForEach(x => { m_IconImage.material = m_HighlitMat; x.DoSelfAnimation(); }); }
            else { m_Ani.ForEach(x => { m_IconImage.material = null; x.StopAnimation(); x.ResetBySaveData(); }); }
        }
    }
}
