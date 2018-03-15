using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class FillChangeImage : BaseImage {

    [Tooltip("圆形或扇形填充比例")]
    [Range(0, 0.5f)]
    public float XFillPercent = 0.1f;
    [Range(0, 0.5f)]
    public float YFillPercent = 0.1f;
    [Tooltip("是否填充圆形")]
    public bool m_FillCenter = false;

    private List<Vector3> innerVertices = new List<Vector3>();
    private List<Vector3> outterVertices = new List<Vector3>();
    static readonly Vector2[] s_VertScratch = new Vector2[4];
    static readonly Vector2[] s_UVScratch = new Vector2[4];
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        Vector4 outer, inner, padding, border;

        if (overrideSprite != null)
        {
            outer = UnityEngine.Sprites.DataUtility.GetOuterUV(overrideSprite);
            inner = UnityEngine.Sprites.DataUtility.GetInnerUV(overrideSprite);
            padding = UnityEngine.Sprites.DataUtility.GetPadding(overrideSprite);
            border = overrideSprite.border;
        }
        else
        {
            outer = Vector4.zero;
            inner = Vector4.zero;
            padding = Vector4.zero;
            border = Vector4.zero;
        }

        Rect rect = GetPixelAdjustedRect();
        padding = padding / pixelsPerUnit;

        s_VertScratch[0] = new Vector2(padding.x, padding.y);
        s_VertScratch[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);

        s_VertScratch[1].x = XFillPercent * rect.width;//border.x;
        s_VertScratch[1].y = YFillPercent * rect.height; //border.y;
        s_VertScratch[2].x = rect.width - XFillPercent * rect.width;
        s_VertScratch[2].y = rect.height - YFillPercent * rect.height;

        for (int i = 0; i < 4; ++i)
        {
            s_VertScratch[i].x += rect.x;
            s_VertScratch[i].y += rect.y;
        }

        s_UVScratch[0] = new Vector2(outer.x, outer.y);
        s_UVScratch[1] = new Vector2(Mathf.Lerp(outer.x, outer.z, XFillPercent), Mathf.Lerp(outer.y, outer.w, YFillPercent));
        s_UVScratch[2] = new Vector2(Mathf.Lerp(outer.x, outer.z, 1 - XFillPercent), Mathf.Lerp(outer.y, outer.w, 1 - YFillPercent));
        s_UVScratch[3] = new Vector2(outer.z, outer.w);

        toFill.Clear();

        for (int x = 0; x < 3; ++x)
        {
            int x2 = x + 1;

            for (int y = 0; y < 3; ++y)
            {
                if (!m_FillCenter && x == 1 && y == 1)
                    continue;

                int y2 = y + 1;

                AddQuad(toFill,
                    new Vector2(s_VertScratch[x].x, s_VertScratch[y].y),
                    new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y),
                    color,
                    new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                    new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));
            }
        }
    }

    public static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin, Vector2 uvMax)
    {
        int startIndex = vertexHelper.currentVertCount;

        vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
        vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
        vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
        vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

        vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
    }

    public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
    {
        return true;
        //Sprite sprite = overrideSprite;
        //if (sprite == null)
        //Vector2 local;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPoint, eventCamera, out local);
        //return Contains(local, outterVertices, innerVertices);
    }

    //private bool Contains(Vector2 p, List<Vector3> outterVertices, List<Vector3> innerVertices)
    //{
    //    var crossNumber = 0;
    //    RayCrossing(p, innerVertices, ref crossNumber);//检测内环
    //    RayCrossing(p, outterVertices, ref crossNumber);//检测外环
    //    return (crossNumber & 1) == 1;
    //}

    ///// <summary>
    ///// 使用RayCrossing算法判断点击点是否在封闭多边形里
    ///// </summary>
    ///// <param name="p"></param>
    ///// <param name="vertices"></param>
    ///// <param name="crossNumber"></param>
    //private void RayCrossing(Vector2 p, List<Vector3> vertices, ref int crossNumber)
    //{
    //    for (int i = 0, count = vertices.Count; i < count; i++)
    //    {
    //        var v1 = vertices[i];
    //        var v2 = vertices[(i + 1) % count];

    //        //点击点水平线必须与两顶点线段相交
    //        if (((v1.y <= p.y) && (v2.y > p.y))
    //            || ((v1.y > p.y) && (v2.y <= p.y)))
    //        {
    //            //只考虑点击点右侧方向，点击点水平线与线段相交，且交点x > 点击点x，则crossNumber+1
    //            if (p.x < v1.x + (p.y - v1.y) / (v2.y - v1.y) * (v2.x - v1.x))
    //            {
    //                crossNumber += 1;
    //            }
    //        }
    //    }
    //}


}
