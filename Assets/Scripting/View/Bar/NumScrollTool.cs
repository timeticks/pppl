using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用于刷新形如：    进度条 + 名字 + 当前数值 
/// </summary>
public class NumScrollTool : MonoBehaviour {

    public Text m_NameText;
    public Text m_NumText;
    public Scrollbar m_Scroll;


    public virtual void Fresh(float scrollValue , string numStr , string name)
    {
        if (m_NameText != null) 
        {
            m_NameText.text = name;
        }
        if (m_NumText != null)
        {
            m_NumText.text = numStr;
        }
        if (m_Scroll != null)
        {
            m_Scroll.size = scrollValue;
        }
    }

}
