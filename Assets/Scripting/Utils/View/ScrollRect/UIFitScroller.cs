using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
public interface IFitScrollWindow
{
    void FreshFitScrollItem(int index);
    void Reset();
}

public class UIFitScroller : MonoBehaviour
{
    public enum Arrangement { Horizontal, Vertical, }
    public Arrangement _movement = Arrangement.Horizontal;
    //Item之间的距离
    [Range(0, 20)]
    public int cellPadiding = 5;
    //Item的宽高
    public int cellWidth = 500;
    public int CellHeightMax = 100;
    public int CellHeightMin = 100;
    //默认加载的Item个数比可显示个数大1个
    public int viewCount = 6;
    private int mViewCount = 0;
    // public GameObject itemPrefab;
    public RectTransform _content;

    private int _index = -1;
    public List<FitItemIndex> _itemList = new List<FitItemIndex>();
    private int _dataCount;

    public Queue<FitItemIndex> _unUsedQueue = new Queue<FitItemIndex>();  //将未显示出来的Item存入未使用队列里面，等待需要使用的时候直接取出

    private List<FitItemIndex> _tempItemlist;
    private List<FitItemIndex> _poolItemList = new List<FitItemIndex>(); //回收的list
    public ScrollRect ScrollView;
    public Scrollbar Scrollbar;
    public bool AutoHide;
    public IFitScrollWindow MyWindow;

    private List<float> SizeList = new List<float>();

    public void Init(IFitScrollWindow win, int dataCount)
    {
        MyWindow = win;
        DataCount = dataCount;
        ClearItemList();
        _itemList.Clear();
        _unUsedQueue.Clear();
        SizeList.Clear();
        _index = -1;
        if (_dataCount < viewCount)
            mViewCount = _dataCount;
        else
            mViewCount = viewCount;
        if (_dataCount < viewCount - 1 && AutoHide)
        {
            ScrollView.horizontalScrollbar = null;
            ScrollView.verticalScrollbar = null;
            if (Scrollbar != null) Scrollbar.gameObject.SetActive(false);
        }
        else
        {
            if (Scrollbar != null && !Scrollbar.gameObject.activeInHierarchy)
            {
                Scrollbar.value = 1;
                Scrollbar.gameObject.SetActive(true);
                if (_movement == Arrangement.Horizontal)
                    ScrollView.horizontalScrollbar = Scrollbar;
                else
                    ScrollView.verticalScrollbar = Scrollbar;
            }
        }
        for (int i = 0; i < dataCount; i++)
        {
            SizeList.Add(CellHeightMin);
        }
        OnValueChange(Vector2.zero);
    }
    void ClearItemList()
    {
        if (_itemList != null)
        {
            for (int i = _itemList.Count - 1; i >= 0; i--)
            {
                _itemList[i].SetFalse();
                _poolItemList.Add(_itemList[i]);
                _itemList.RemoveAt(i);
            }
            for (int i = 0; i < _unUsedQueue.Count; i++)
            {
                FitItemIndex itemTemp = _unUsedQueue.Dequeue();
                itemTemp.SetFalse();
                _poolItemList.Add(itemTemp);
            }
        }
    }
    public UIViewBase GetNewObj(Transform par, GameObject prefab)
    {
        if (_poolItemList.Count > 0)
        {
            UIViewBase temp = _poolItemList[0].View;
            _poolItemList.RemoveAt(0);
            //temp.gameObject.SetActive(true);
            return temp;

        }
        return GameTools.AddChild(par, prefab).GetComponent<UIViewBase>();
    }

    //public void TurnIndexItem(int index)
    //{
    //    index = Mathf.Clamp(index, 0, DataCount - (mViewCount - 1));
    //    if (DataCount < viewCount) index = 0;
    //    ScrollView.StopMovement();
    //    switch (_movement)
    //    {
    //        case Arrangement.Horizontal:
    //            _content.anchoredPosition = new Vector2(cellWidth * index + cellPadiding * index, _content.anchoredPosition.y);
    //            break;
    //        case Arrangement.Vertical:
    //            _content.anchoredPosition = new Vector2(_content.anchoredPosition.x, cellHeight * index + cellPadiding * index);
    //            break;
    //    }
    //    OnValueChange(Vector2.zero);
    //}

    public void OnValueChange(Vector2 pos)
    {
        int index = GetPosIndex();
        index = Mathf.Max(0, index);
        if (_index != index)
        {
            _index = index;
            FitItemIndex _item;
            for (int i = _itemList.Count; i > 0; i--)
            {
                _item = _itemList[i - 1];
                if (_item.Index < index || (_item.Index >= index + mViewCount))
                {
                    //GameObject.Destroy(item.gameObject);
                    _itemList.Remove(_item);
                    _item.SetFalse();
                    _unUsedQueue.Enqueue(_item);
                }
            }
            for (int i = _index; i < _index + mViewCount; i++)
            {
                if (i < 0) continue;
                if (i > _dataCount - 1) continue;
                bool isOk = false;
                foreach (FitItemIndex item in _itemList)
                {
                    if (item.Index == i) isOk = true;
                }
                if (isOk) continue;
                MyWindow.FreshFitScrollItem(i);
            }
        }
    }

    ///// <summary>
    ///// 提供给外部的方法，添加指定位置的Item
    ///// </summary>
    //public void AddItem(int index)
    //{
    //    if (index > _dataCount)
    //    {
    //        Debug.LogError("添加错误:" + index);
    //        return;
    //    }
    //    AddItemIntoPanel(index);
    //    DataCount += 1;
    //}

    ///// <summary>
    ///// 提供给外部的方法，删除指定位置的Item
    ///// </summary>
    //public void DelItem(int index)
    //{
    //    if (index < 0 || index > _dataCount - 1)
    //    {
    //        Debug.LogError("删除错误:" + index);
    //        return;
    //    }
    //    DelItemFromPanel(index);
    //    DataCount -= 1;
    //}

    //private void AddItemIntoPanel(int index)
    //{
    //    for (int i = 0; i < _itemList.Count; i++)
    //    {
    //        ItemIndex item = _itemList[i];
    //        if (item.Index >= index) item.Index += 1;
    //    }
    //    MyWindow.FreshFitScrollItem(index);
    //}

    //private void DelItemFromPanel(int index)
    //{
    //    int maxIndex = -1;
    //    int minIndex = int.MaxValue;
    //    for (int i = _itemList.Count; i > 0; i--)
    //    {
    //        ItemIndex item = _itemList[i - 1];
    //        if (item.Index == index)
    //        {
    //            GameObject.Destroy(item.View.gameObject);
    //            _itemList.Remove(item);
    //        }
    //        if (item.Index > maxIndex)
    //        {
    //            maxIndex = item.Index;
    //        }
    //        if (item.Index < minIndex)
    //        {
    //            minIndex = item.Index;
    //        }
    //        if (item.Index > index)
    //        {
    //            item.Index -= 1;
    //        }
    //    }
    //    if (maxIndex < DataCount - 1)
    //    {
    //        MyWindow.FreshFitScrollItem(maxIndex);
    //    }
    //}


    private int GetPosIndex()
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                return Mathf.FloorToInt(_content.anchoredPosition.x / -(cellWidth + cellPadiding));
            case Arrangement.Vertical:
            {
                return Mathf.FloorToInt(_content.anchoredPosition.y / (CellHeightMin + cellPadiding));
            }

        }
        return 0;
    }

    public Vector3 GetPosition(int index)
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                return new Vector3(index * (cellWidth + cellPadiding), 0f, 0f);
            case Arrangement.Vertical:
            {
                float height = 0;
                for (int i = 0; i < index; i++)
                {
                    height += SizeList[i];
                    height += cellPadiding;
                }
                return new Vector3(0f, height*-1, 0f);
            }
        }
        return Vector3.zero;
    }

    public int DataCount
    {
        get { return _dataCount; }
        set
        {
            _dataCount = value;
            UpdateTotalWidth();
        }
    }

    private void UpdateTotalWidth()
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                _content.sizeDelta = new Vector2(cellWidth * _dataCount + cellPadiding * _dataCount, _content.sizeDelta.y);
                break;
            case Arrangement.Vertical:
            {
                float height = 0;
                for (int i = 0,length =SizeList.Count ; i < length; i++)
                {
                    height += SizeList[i];
                    height += cellPadiding;
                }
                height -= cellPadiding;
                _content.sizeDelta = new Vector2(_content.sizeDelta.x, height);
                break;
            }
        }
    }

    public void UpdateItemHeight(int index, bool maxHeight)
    {
        SizeList[index] = maxHeight ? CellHeightMax : CellHeightMin;
        UpdateTotalWidth();
        OnValueChange(_content.anchoredPosition);
    }
}
