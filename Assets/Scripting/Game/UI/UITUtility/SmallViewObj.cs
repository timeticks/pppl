using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmallViewObj
{
    public UIViewBase View;

    public SmallViewObj()
    {
    }

    public virtual void Init(UIViewBase view)
    {
        View = view;
    }
}


public class ItemIndexObj : ItemIndex
{
    public virtual void Init(UIViewBase view)
    {
        View = view;
    }
}