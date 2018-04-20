using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Window_MapChoose : WindowBase
{
    public class ViewObj
    {
        public GameObject Part_MapChoosePoint;
        public Transform ItemRoot;
        public Text TextTtile;
        public CanvasRenderer ImageMapLine;
        public Text TextMapName;
        public Text TextMapDesc;
        public TextButton TBtnEnter;
        public ViewObj(UIViewBase view)
        {
            if (Part_MapChoosePoint != null) return;
            Part_MapChoosePoint = view.GetCommon<GameObject>("Part_MapChoosePoint");
            ItemRoot = view.GetCommon<Transform>("ItemRoot");
            TextTtile = view.GetCommon<Text>("TextTtile");
            ImageMapLine = view.GetCommon<CanvasRenderer>("ImageMapLine");
            TextMapName = view.GetCommon<Text>("TextMapName");
            TextMapDesc = view.GetCommon<Text>("TextMapDesc");
            TBtnEnter = view.GetCommon<TextButton>("TBtnEnter");
        }
    }

    public List<TextButton> mPointObjList = new List<TextButton>();


    private ViewObj mViewObj;
    private int mCurSelectMap;

    public void OpenWindow()
    {
        if (mViewObj == null) mViewObj = new ViewObj(GetComponent<UIViewBase>());
        OpenWin();
        Init();
    }
    public override void CloseWindow(CloseActionType actionType = CloseActionType.None)
    {
        base.CloseWindow(actionType);
    }

    private List<Vector3[]> mLineList = new List<Vector3[]>();
    public void Init()
    {
    }

    void BtnEvt_SelectMap(int mapIdx)
    {
    }


    void BtnEvt_EnterMap()
    {
        if (mCurSelectMap != PVEMgr.Instance.CurMapId)
            PVEMgr.Instance.SwitchMap(mCurSelectMap);
        CloseWindow();
    }


    Mesh FreshLine(List<Vector3[]> lineList, float width)
    {
        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();
        int i = 0;
        for (i = 0; i < lineList.Count; i++)
        {
            Vector2 start = lineList[i][0];
            Vector2 end = lineList[i][1];
            Vector2 dir = (end - start).normalized;
            start = start + dir*15; //缩短直线
            end = end - dir*15;
            Vector2 vdir = new Vector2(-dir.y, dir.x).normalized * width;
            Vector2 p0 = start - vdir;
            Vector2 p1 = start + vdir;
            Vector2 p2 = end - vdir;
            Vector2 p3 = end + vdir;
            verts.Add(p0); verts.Add(p1); verts.Add(p2); verts.Add(p3);
            //uvs.Add(new Vector2(0,0));uvs.Add(new Vector2(1,0));uvs.Add(new Vector2(0,1));uvs.Add(new Vector2(1,1));
            triangles.Add(0 + i * 4); triangles.Add(2 + i * 4); triangles.Add(3 + i * 4); triangles.Add(0 + i * 4); triangles.Add(3 + i * 4); triangles.Add(1 + i * 4);
        }
        //verts = new List<Vector3>();
        //triangles = new List<int>();
        //i = 0;
        //verts.Add(new Vector3(100, 0, 0)); verts.Add(new Vector3(0, 0, 0)); verts.Add(new Vector3(100, 100, 0)); verts.Add(new Vector3(0, 100, 0));
        //triangles.Add(0 + i * 4); triangles.Add(2 + i * 4); triangles.Add(3 + i * 4); triangles.Add(0 + i * 4); triangles.Add(3 + i * 4); triangles.Add(1 + i * 4);

        mesh.vertices = verts.ToArray();
        //mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        return mesh;
    }

    void Update() //刷新六维信息和六边形
    {
        if (mLineList.Count > 0)
        {
            mViewObj.ImageMapLine.SetMesh(FreshLine(mLineList, 2));
            mViewObj.ImageMapLine.SetColor(new Color(0.9f, 0.7f, 0f, 0.7f));
        }
    }

}
