using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Part_UpTips : MonoBehaviour 
{
    public Text m_Text_Tips;
    public void Init(string tips , Color col)
    {
        m_Text_Tips.text = tips;
        m_Text_Tips.color = col;
        gameObject.SetActive(true);
    }
}
