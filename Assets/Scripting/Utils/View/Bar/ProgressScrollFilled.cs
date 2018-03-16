using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ProgressScrollFilled : MonoBehaviour
{
    public Text m_NameText;
    public Text m_NumText;
    public Image m_Handle;

    public void Fresh(float scrollValue, string numStr, string name)
    {
        if (m_NameText != null)
        {
            m_NameText.text = name;
        }
        if (m_NumText != null)
        {
            m_NumText.text = numStr;
        }
        if (m_Handle != null)
        {
#if UNITY_EDITOR
            if(m_Handle.type!= Image.Type.Filled)
                TDebug.LogError("this image`s type must be filled!!");
#endif
            m_Handle.fillAmount = scrollValue;
        }
    }
}
