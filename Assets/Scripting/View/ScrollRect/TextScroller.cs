using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;
using System;
public class TextScroller : MonoBehaviour,IBeginDragHandler,IEndDragHandler
{
    public int MaxItemNum = 6;
    public int ItemHeight = 540;
    public int ItemWidth = 924;
    public bool TextRayCast = true;
    public GameObject TextPrefab;
    public ScrollRect MyScrollView;
    [Range(500, 10000)]
    public float MoveBottomSpd = 3000; //实际滑动速度
    public RectTransform RootTextItem;
    private List<Text> mTextList = new List<Text>();

    private List<Text> mTextCached = new List<Text>();
    private bool mStartMoveBottom = false;  //开始重置到底部
    private float mCurBottomPos;
    private int mCurUseFrame;
    private bool mCanMoveBottom = true;

    private bool mImmeDiateStartScroll = true; // 拖拽结束后，如未处于滚动窗口末尾，是否需要立即恢复滚动
    private bool mDragStopScroll = false; // 拖拽的时候时候需要停止滚动
    private Action mBeginDragAction;
    private Action mEndDragAction;
    public void AddNewTextLine(string msg, bool needClear = false)
    {
        AddText(msg, true, true, needClear);
    }

    /// <summary>
    /// 不自动换行
    /// </summary>
    public void AddText(string msg, bool needClear)
    {
        AddText(msg, false, needClear);
    }

    /// <summary>
    /// 不自动换行，
    /// 并且根据是否有换行符，判断是否可以将文本放在NewText中
    /// </summary>
    public void AddTextCheckCanNewText(string msg, bool needClear)
    {
        if (msg.Contains("\r\n") || msg.Contains("\n"))
            AddText(msg, false, true, needClear);
        else
            AddText(msg, false, false, needClear);
    }

    private void AddText(string msg, bool newLine, bool canNewText, bool needClear = false)
    {
        Text textItem = null;
        bool isNewTextItem = true;
        if (needClear)
        {
            for (int i = 0; i < mTextList.Count; i++)
            {
                Text item = mTextList[i];
                item.text = "";
                RectTransform trans = item.rectTransform;
                if (trans != null)
                {
                    trans.sizeDelta = new Vector2(trans.sizeDelta.x, 0f);
                    mTextCached.Add(item);
                    if (i == 0)
                    {
                        trans.anchoredPosition = Vector2.zero;
                    }
                    else
                    {
                        RectTransform rect = mTextList[i - 1].rectTransform;
                        trans.anchoredPosition = new Vector2(0, rect.anchoredPosition.y - rect.sizeDelta.y);
                    }
                }
               
            }
            mTextList.Clear();
            RootTextItem.anchoredPosition = Vector2.zero;
        }

        if (mTextList.Count == 0)
        {
            textItem = NewTextItem();
            mTextList.Add(textItem);
        }
        else if (mTextList[mTextList.Count - 1].rectTransform!=null && mTextList[mTextList.Count - 1].rectTransform.sizeDelta.y >= ItemHeight && canNewText)
        {
            if (mTextList.Count < MaxItemNum)
            {
                textItem = NewTextItem();
                mTextList.Add(textItem);
            }
            else
            {
                textItem = mTextList[0];
                mTextList.RemoveAt(0);
                mTextList.Add(textItem);
                MyScrollView.normalizedPosition = Vector2.zero;
                for (int i = 0; i < mTextList.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        mTextList[i].rectTransform.anchoredPosition = Vector2.zero;
                    }
                    else
                    {
                        RectTransform rect = mTextList[i - 1].rectTransform;
                        if (rect != null)
                            mTextList[i].rectTransform.anchoredPosition = new Vector2(0, rect.anchoredPosition.y - rect.sizeDelta.y);
                    }
                }
                textItem.text = "";
                textItem.GetComponent<Transform>().SetAsLastSibling();
            }
            RectTransform tempTrans = mTextList[mTextList.Count - 2].rectTransform;
            if (tempTrans!=null)
                textItem.rectTransform.anchoredPosition = new Vector2(0f, tempTrans.anchoredPosition.y - tempTrans.sizeDelta.y);
            lastTextTrans = tempTrans;
            curTextTrans = textItem.rectTransform;
            resetPos = true;
        }
        else
        {
            textItem = mTextList[mTextList.Count - 1];
            isNewTextItem = false;
        }
        if (!isNewTextItem && textItem.text.Length > 0 && newLine)
            msg = "\r\n" + msg;
        if (isNewTextItem)//如果是新文本，则将开头的换行符删掉
        {
            if (msg.StartsWith("\r\n")) msg = msg.Substring(2);
            else if (msg.StartsWith("\n")) msg = msg.Substring(1);
        }

        StringBuilder wholeStr = new StringBuilder(textItem.text);
        wholeStr.Append(msg);
        textItem.text = wholeStr.ToString();
        textItem.gameObject.SetActive(true);
        RectTransform tans = textItem.rectTransform;
        if (tans != null)
        {
            if (textItem.text == "")
                tans.sizeDelta = new Vector2(ItemWidth, 1); // 设置为0会导致文本渲染不出
            else
                tans.sizeDelta = new Vector2(ItemWidth, textItem.preferredHeight);
        }
    }
    /// <summary>
    /// 设置拖拽事件，beginDragAction 开始拖拽委托，endDragAction 结束拖拽委托，dragStopScroll 拖拽时是否需要停止滚动,immeDiateStartScroll拖拽结束后，如未处于滚动窗口末尾，是否需要立即恢复滚动
    /// </summary>
    /// <param name="beginDragAction"></param>
    /// <param name="endDragAction"></param>
    /// <param name="dragStopScroll"></param>
    public void SetDragAction(Action beginDragAction,Action endDragAction,bool dragStopScroll = true,bool immeDiateStartScroll=true)
    {
        mBeginDragAction = beginDragAction;
        mEndDragAction = endDragAction;
        mDragStopScroll = dragStopScroll;
        mImmeDiateStartScroll = immeDiateStartScroll;
    }

    private int size = 0; // text 长宽为整数
    void UpdateContentSize()
    {
        size = 0;
        foreach (Text item in mTextList)
        {
            size += (int)item.rectTransform.sizeDelta.y;
            // size += (int)item.preferredHeight;
        }
        RootTextItem.sizeDelta = new Vector2(RootTextItem.sizeDelta.x, size);
    }

    private bool resetPos = false;
    private RectTransform lastTextTrans = null;
    private RectTransform curTextTrans = null;
    void Update()
    {
        if (resetPos)
        {
            resetPos = false;
            curTextTrans.anchoredPosition = new Vector2(0f, lastTextTrans.anchoredPosition.y - lastTextTrans.sizeDelta.y);
        }

        UpdateContentSize();
        if (mStartMoveBottom && mCanMoveBottom)  //开始移到底部
        {
            mCurBottomPos = MyScrollView.normalizedPosition.y;
            mCurUseFrame++;
            if (mCurBottomPos > 0)
            {
                float pctSpd = MoveBottomSpd / MyScrollView.content.rect.height;
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
    public void MoveBottom()//将文案滑动，移到底部
    {
        mCurUseFrame = 0;
        mStartMoveBottom = true;
    }
    public void DirectToBottom(bool delay = true)
    {
        mStartMoveBottom = false;
        DirectMove();
        //if (delay)
        //{
        //    RootTextItem.gameObject.SetActive(false);
        //    Invoke("DirectMove", 0.05f);
        //}
        //else
        //    DirectMove();
    }
    void DirectMove()
    {
        UpdateContentSize();
        MyScrollView.normalizedPosition = Vector2.zero;
     //   RootTextItem.anchoredPosition = new Vector3(RootTextItem.anchoredPosition.x, RootTextItem.sizeDelta.y - MyScrollView.viewport.sizeDelta.y, 0);
       // RootTextItem.gameObject.SetActive(true);
    }
    public void ClearMsg()
    {
        for (int i = 0; i < mTextList.Count; i++)
        {
            Text item = mTextList[i];
            item.text = "";
            RectTransform trans = item.rectTransform;
            item.rectTransform.sizeDelta = new Vector2(trans.sizeDelta.x, 0f);
            mTextCached.Add(item);
            if (i == 0)
            {
                trans.anchoredPosition = Vector2.zero;
            }
            else
            {
                RectTransform rect = mTextList[i - 1].rectTransform;
                trans.anchoredPosition = new Vector2(0, rect.anchoredPosition.y - rect.sizeDelta.y);
            }
        }
        mTextList.Clear();
        RootTextItem.anchoredPosition = Vector2.zero;
    }
    public void DirectToTop()
    {
        mStartMoveBottom = false;
        UpdateContentSize();
        RootTextItem.anchoredPosition = new Vector3(RootTextItem.anchoredPosition.x, 0, 0);
    }
    Text NewTextItem()
    {
        Text temp = null;
        if (mTextCached.Count > 0)
        {
            temp = mTextCached[0];
            mTextCached.RemoveAt(0);
            temp.rectTransform.sizeDelta = new Vector2(ItemWidth, temp.rectTransform.sizeDelta.y);
            return temp;
        }
        else
        {
            GameObject go = Instantiate(TextPrefab) as GameObject;
            go.transform.SetParent(RootTextItem);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.GetComponent<RectTransform>().sizeDelta = new Vector2(ItemWidth, 0);
            temp = go.GetComponent<Text>();
            temp.raycastTarget = TextRayCast;
            return temp;
        }
    }
    public void OnBeginDrag(PointerEventData events)
    {
        if (!mDragStopScroll) return;

        mCanMoveBottom = false;
        if(mBeginDragAction!=null) mBeginDragAction();
        CancelInvoke("StartScroll");
    }
    public void OnEndDrag(PointerEventData events)
    {
        if (!mDragStopScroll) return;
        if (mImmeDiateStartScroll)
            StartScroll();
        else
        {
            if (MyScrollView.verticalScrollbar!=null&&MyScrollView.verticalScrollbar.value < 0.01f)
            {
                StartScroll();
            }
            else if (MyScrollView.horizontalScrollbar != null && MyScrollView.horizontalScrollbar.value < 0.01f)
            {
                StartScroll();
            }
            else
                Invoke("StartScroll", 0.7f);
        }     
    }

    void StartScroll()
    {
        mCanMoveBottom = true;
        if (mEndDragAction != null) mEndDragAction();
    }
}
