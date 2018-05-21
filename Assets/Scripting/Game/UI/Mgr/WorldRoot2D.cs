using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRoot2D : MonoBehaviour
{
    public class ViewObj
    {
        public Transform BallRoot;
        public GameObject Part_StarSprite;
        public GameObject Bg;
        public ViewObj(UIViewBase view)
        {
            if (BallRoot != null) return;
            BallRoot = view.GetCommon<Transform>("BallRoot");
            Part_StarSprite = view.GetCommon<GameObject>("Part_StarSprite");
            Bg = view.GetCommon<GameObject>("Bg");
        }
    }
    private ViewObj mViewObj;

    public void Init()
    {
        if (mViewObj == null)
        {
            mViewObj = new ViewObj(GetComponent<UIViewBase>());
        }
    }
}
 