using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TAppUtility : MonoBehaviour {
    public static TAppUtility Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 摄像机RT生成
    /// </summary>
    public void RTextureInit(out Camera cam, out GameObject prefabObj, GameObject prefab, Vector3 camWorldPos, Vector3 prefabWorldPos, RenderTexture aimRT)
    {
        GameObject go = new GameObject("RTCam");
        cam = go.CheckAddComponent<Camera>();
        cam.depth = -10;
        cam.renderingPath = RenderingPath.UsePlayerSettings;
        cam.depthTextureMode = DepthTextureMode.None;
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.Depth;
        cam.backgroundColor = new Color(0, 0, 0, 0);
        cam.orthographicSize = 5;
        go.transform.position = camWorldPos;
        cam.targetTexture = aimRT;
        cam.useOcclusionCulling = true;
        prefabObj = (null == prefab) ? null : Instantiate(prefab, prefabWorldPos, Quaternion.identity);
    }

    private static List<Vector3> hexagonVertexMax = null;
    public static void FreshHeaxgonVertex(List<float> pctList , Mesh mesh) //根据
    {
        if (null == hexagonVertexMax)
        {
            hexagonVertexMax = new List<Vector3>();
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                hexagonVertexMax.Add(mesh.vertices[i]);
            }
        }
        if (pctList.Count != hexagonVertexMax.Count) { TDebug.LogError("数量不一致"); }
        List<Vector3> newVertex = new List<Vector3>();
        for (int i = 0; i < hexagonVertexMax.Count; i++)
        {
            newVertex.Add(Vector3.Lerp(Vector3.zero, hexagonVertexMax[i], pctList[i]));
        }
        mesh.vertices = newVertex.ToArray();
    }
    

    /// <summary>
    /// 检查curList数量，如果不足needNum，则新建生成。如果多余，则隐藏多余的
    /// </summary>
    public List<T> AddViewInstantiate<T>(List<T> curList, GameObject prefabObj, Transform parentTrans, int needNum, bool isRotateZero = true)where T :SmallViewObj, new()
    {
        if (null == curList)
        {
            TDebug.LogError("需要在外部对list进行初始化！！！");
            curList = new List<T>();
        }
        for (int i = curList.Count; i < needNum; i++)
        {
            GameObject g = Instantiate(prefabObj) as GameObject;
            TUtility.SetParent(g.transform, parentTrans, false, isRotateZero);
            UIViewBase p = g.GetComponent<UIViewBase>();
            T t = new T();
            t.Init(p);
            curList.Add(t);
        }
        for (int i = 0; i < curList.Count; i++)
        {
            if (curList[i]!= null &&curList[i].View!=null&&curList[i].View.gameObject.activeSelf != (i < needNum))
                curList[i].View.gameObject.SetActive(i < needNum);
        }
        return curList;
    }

    // <summary>
    /// 检查curList数量，如果不足needNum，则新建生成。如果多余，则隐藏多余的
    /// </summary>
    public List<T> AddMonoInstantiate<T>(List<T> curList, GameObject prefabObj, Transform parentTrans, int needNum, bool isRotateZero = true) where T : MonoBehaviour
    {
        if (null == curList)
        {
            TDebug.LogError("需要在外部对list进行初始化！！！");
            curList = new List<T>();
        }
        for (int i = curList.Count; i < needNum; i++)
        {
            GameObject g = Instantiate(prefabObj) as GameObject;
            TUtility.SetParent(g.transform, parentTrans, false, isRotateZero);
            T t = g.GetComponent<T>();
            curList.Add(t);
        }
        for (int i = 0; i < curList.Count; i++)
        {
            if (curList[i] != null && curList[i].gameObject.activeSelf != (i < needNum))
                curList[i].gameObject.SetActive(i < needNum);
        }
        return curList;
    }

    /// <summary>
    /// 从列表中，获得某个active为false的物体
    /// 如果没有的话，则生成
    /// </summary>
    public T GetEnableObj<T>(List<T> curList, GameObject prefabObj, Transform parentTrans , bool startActive=true)where T:MonoBehaviour
    {
        T curText = null;
        for (int i = 0; i < curList.Count; i++)
        {
            if (!curList[i].gameObject.activeSelf) curText = curList[i];
        }
        if (curText == null)
        {
            GameObject g = Instantiate(prefabObj, parentTrans, false);
            TUtility.SetParent(g.transform, parentTrans, false);
            g.transform.localPosition = Vector3.zero;
            curText = g.GetComponent<T>();
            curList.Add(curText);
        }
        return curText;
    }

    public GameObject InstantiateObj(Object obj)
    {
        return Instantiate(obj)as GameObject;
    }

}
