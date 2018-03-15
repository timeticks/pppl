using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 可滑动文本
/// </summary>
public class TextScrollView : MonoBehaviour
{
    public ScrollRect MyScrollView;
    public Text ContentText;       //内容文本
    public Scrollbar Scrollbar;
    [Range(500,10000)]
    public float MoveBottomSpd = 3000; //实际滑动速度
    public int MaxLineNum = 60;         //最大文本行数
    public bool OnlyChangeShowScroll;   //是否只在值改变时，显示滑动条

    private bool mStartMoveBottom = false;  //开始重置到底部
    private bool mCanMoveBottom = true;
    private float mCurBottomPos;
    private int mCurUseFrame;
    void Update()
    {
        if(mStartMoveBottom)  //开始移到底部
        {
            mCurBottomPos = MyScrollView.normalizedPosition.y;
            mCurUseFrame++;
            if(mCurBottomPos>0)
            {
                float pctSpd = MoveBottomSpd/MyScrollView.content.rect.height;
                mCurBottomPos -= Time.deltaTime * pctSpd;
            }
            if (mCurBottomPos <= 0 && mCurUseFrame > 1)//移动结束
            {
                MyScrollView.normalizedPosition = Vector2.zero;
                mStartMoveBottom = false;
                mCurUseFrame = 0;
            }
            else
            {
                MyScrollView.normalizedPosition = new Vector2(0, mCurBottomPos);
            }

        }
    }
    //public void OnBeginDrag(PointerEventData events)
    //{
    //    mCanMoveBottom = false;
    //}
    //public void OnEndDrag(PointerEventData events)
    //{
    //    mCanMoveBottom = true;
    //}
    public void AppendText(string str, bool needClear = false)
    {
        if (needClear) SetText(new StringBuilder(""));
        StringBuilder wholeStr = new StringBuilder(ContentText.text);
        wholeStr.Append(str);
        SetText(wholeStr);
    }
    public void AppendTextNewLine(string str)
    {
        if (ContentText.text.Length>0)
            str = "\r\n" + str;
        StringBuilder wholeStr = new StringBuilder(ContentText.text);
        wholeStr.Append(str);
        SetText(wholeStr);
    }
    private void SetText(StringBuilder str)
    {
        //检查是否超过最大行数
        int lineNum = GetKeyCount(str.ToString());   
        if (lineNum > MaxLineNum)
        {
            int deleteNum = lineNum - MaxLineNum;
            str = new StringBuilder(RemoveHeadLine(str.ToString(), deleteNum));
        }
        ContentText.text = str.ToString();
    }

    /// <summary>
    /// 从开始移除字符串的指定行数
    /// </summary>
    public static string RemoveHeadLine(string str, int removeLineNum)
    {
        int removeIndex = 0;
        for (int i = removeIndex; i < removeLineNum; i++)
        {
            if (removeIndex == -1) return str;
            removeIndex = str.IndexOf("\r\n", removeIndex + 1);
        }
        return str.Substring(removeIndex);
    }



    public void MoveBottom()//将文案滑动，移到底部
    {
        mCurUseFrame = 0;
        mStartMoveBottom = true;
    }


    public const char enterStr = '\r';
    public static int GetKeyCount(string str1) //得到换行符数量
    {
        int count = 0;
        int length = str1.Length - 1;
        for (int k = 0; k < length; k++)
        {
            if (str1[k].Equals(enterStr))
            {
                if (str1[k + 1].Equals('\n'))
                    count++;
            }
        }
        return count;
    }

    //public string aaaaaaaaaaaa;
    //void OnGUI()
    //{
    //    if (GUILayout.Button("aaaaaaaaaaaaaaa"))
    //    {
    //        AppendTextNewLine(aaaaaaaaaaaa+"\r\n"+aaaaaaaaaaaa);
    //        MoveBottom();
    //    }

    //}

}
