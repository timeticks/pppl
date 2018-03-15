using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Part_NumImageShow : MonoBehaviour    //Part_DmgImageShow是其子类
{
    public List<Image> m_ImageNumList;

    public virtual void Init(Sprite one)  //显示闪避或miss
    {
        for (int i = 0; i < m_ImageNumList.Count; i++)
        {
            m_ImageNumList[i].gameObject.SetActive(false);
        }
        int showIndex = 0;
        m_ImageNumList[showIndex].sprite = one;
        
        m_ImageNumList[showIndex].gameObject.SetActive(true);
        m_ImageNumList[showIndex].SetNativeSize();
    }

    public virtual void Init(List<Sprite> numSprite , int num , bool isFirst10)
    {
        if (num < 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        SetNumToSpriteList(num , m_ImageNumList, numSprite, isFirst10);
    }


    /// <summary>
    /// 根据数字，将sprite赋在imageList中。isFirstIs10是否数字的第一位都显示第11个sprite，如：-10
    /// </summary>
    /// <param name="numValue"></param>
    /// <param name="numImageList"></param>
    public static void SetNumToSpriteList(int numValue, List<Image> numImageList, List<Sprite> numSpriteList, bool isFirstIs10)
    {
        if (numSpriteList == null || numImageList == null)
        {
            TDebug.LogError("numSpriteList == null || numImageList == null");
            return;
        }
        for (int i = 0; i < numImageList.Count; i++)
        {
            numImageList[i].gameObject.SetActive(true);
        }
        int curIndex = 0;
        if (isFirstIs10 && numSpriteList.Count >= 11)//如显示伤害时，第一个都为减号
        {
            numImageList[0].sprite = numSpriteList[10]; numImageList[0].SetNativeSize();
            curIndex++;
        }
        if (numImageList.Count < curIndex + 2) numValue = Mathf.Min(numValue, 9);
        else if (numImageList.Count < curIndex + 3) numValue = Mathf.Min(numValue, 99);
        else if (numImageList.Count < curIndex + 4) numValue = Mathf.Min(numValue, 999);

        if (numValue >= 1000)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue / 1000, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 1000 / 100, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        else if (numValue >= 100)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue / 100, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        else if (numValue >= 10)
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 100 / 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
            numImageList[++curIndex].gameObject.SetActive(false);
        }
        else
        {
            numImageList[curIndex].sprite = numSpriteList[Mathf.Clamp(numValue % 10, 0, numSpriteList.Count - 1)]; numImageList[curIndex].SetNativeSize();
        }
        for (int i = curIndex + 1; i < numImageList.Count; i++)
        {
            numImageList[i].gameObject.SetActive(false);
        }
    }

}
