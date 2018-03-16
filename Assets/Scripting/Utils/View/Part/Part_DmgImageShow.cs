using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Part_DmgImageShow : Part_NumImageShow  //伤害显示
{
    public Image m_Image_PreText;


    public override void Init(Sprite one)
    {
        for (int i = 0; i < m_ImageNumList.Count; i++)
        {
            m_ImageNumList[i].gameObject.SetActive(false);
        }
        m_Image_PreText.sprite = one;
        m_Image_PreText.gameObject.SetActive(true);
        m_Image_PreText.SetNativeSize();
        FreshPos();
    }

    public void Init(List<Sprite> numSprite, int num, bool isFirst10 , Sprite preSp=null)
    {
        if (preSp == null)
        {
            m_Image_PreText.gameObject.SetActive(false);
        }
        else
        {
            Init(preSp);
        }
        base.Init(numSprite, num, isFirst10);
        FreshPos();
    }


    public void FreshPos() //居中
    {
        float avrX = 0;
        int amount = 0;
        for (int i = 0; i < m_ImageNumList.Count; i++)
        {
            if (m_ImageNumList[i].gameObject.activeSelf)
            {
                avrX += m_ImageNumList[i].transform.localPosition.x;
                amount++;
            }
        }
        if (m_Image_PreText.gameObject.activeSelf)
        {
            avrX += m_Image_PreText.transform.localPosition.x;
            amount++;
        }

        avrX = avrX / (amount);

        for (int i = 0; i < m_ImageNumList.Count; i++)
        {
            m_ImageNumList[i].transform.localPosition -= new Vector3(avrX, 0, 0);
        }
        m_Image_PreText.transform.localPosition -= new Vector3(avrX, 0, 0);
    }
}
