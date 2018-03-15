using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 冷却转圈
/// </summary>
public class CoolTimeBase : MonoBehaviour 
{
    public Image m_CoolImage;
    public Text m_CoolText;
    private int m_cur;
    private int m_max;
    private bool m_setBlack;
    public bool Fresh(int cur , int max)//返回是否冷却完毕
    {
        if (m_setBlack) return false;
        m_cur = cur;
        m_max = max;
        if (max == 0) { max = 1; cur = 0; }
        m_CoolImage.fillAmount = (float)cur / (float)max;
        m_CoolImage.gameObject.SetActive(cur > 0);
        m_CoolText.gameObject.SetActive(cur > 0);
        m_CoolText.text = cur.ToString();
        return cur <= 0;
    }
    public void SetBlack(bool setBlack)
    {
        m_setBlack = setBlack;
        if (setBlack)
        {
            m_CoolImage.gameObject.SetActive(true);
            m_CoolImage.fillAmount = 1;
            m_CoolText.gameObject.SetActive(false);
        }
        else { Fresh(m_cur, m_max); }
    }
}
