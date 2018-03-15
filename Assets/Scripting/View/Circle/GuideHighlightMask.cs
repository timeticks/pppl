using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
    public class GuideHighlightMask : BaseImage
    {
        public RectTransform arrow;
        public Vector2 center = Vector2.zero;
        public Vector2 size = new Vector2(100, 100);

        public void DoUpdate()
        {
            // 当引导箭头位置或者大小改变后更新，注意：未处理拉伸模式
            if (arrow && center != arrow.anchoredPosition || size != arrow.sizeDelta)
            {
                this.center = arrow.anchoredPosition;
                this.size = arrow.sizeDelta;
                UpdateGeometry();
            }
        }

        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            // 点击在箭头框内部则无效，否则生效
            return !RectTransformUtility.RectangleContainsScreenPoint(arrow, sp, eventCamera);
        }


        private List<Vector3> innerVertices = new List<Vector3>();
        private List<Vector3> outterVertices = new List<Vector3>();
        readonly Vector2[] s_VertScratch = new Vector2[4];
        readonly Vector2[] s_UVScratch = new Vector2[4];
        public float xFillPercent = 0;
        public float yFillPercent = 0;
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

            xFillPercent = (1 -size.x/ (float)rect.width) * 0.5f;
            yFillPercent = (1-size.y / (float)rect.height) * 0.5f;

            s_VertScratch[1].x = xFillPercent * rect.width + center.x;
            s_VertScratch[1].y = yFillPercent * rect.height + center.y;
            s_VertScratch[2].x = rect.width - xFillPercent * rect.width + center.x;
            s_VertScratch[2].y = rect.height - yFillPercent * rect.height + center.y;

            for (int i = 0; i < 4; ++i)
            {
                s_VertScratch[i].x += rect.x ;
                s_VertScratch[i].y += rect.y ;
            }

            s_UVScratch[0] = new Vector2(outer.x, outer.y);
            s_UVScratch[1] = new Vector2(Mathf.Lerp(outer.x, outer.z, xFillPercent), Mathf.Lerp(outer.y, outer.w, yFillPercent));
            s_UVScratch[2] = new Vector2(Mathf.Lerp(outer.x, outer.z, 1 - xFillPercent), Mathf.Lerp(outer.y, outer.w, 1 - yFillPercent));
            s_UVScratch[3] = new Vector2(outer.z, outer.w);

            //给quad赋值
            toFill.Clear();
            for (int x = 0; x < 3; ++x)
            {
                int x2 = x + 1;
                for (int y = 0; y < 3; ++y)
                {
                    if (x == 1 && y == 1)
                        continue;
                    int y2 = y + 1;

                    FillChangeImage.AddQuad(toFill,
                        new Vector2(s_VertScratch[x].x, s_VertScratch[y].y),
                        new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y),
                        color,
                        new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                        new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));
                }
            }
        }

        void Update()
        {
            DoUpdate();
        }
    }
}