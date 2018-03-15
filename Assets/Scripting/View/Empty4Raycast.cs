using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    /// <summary>
    /// 只在逻辑上响应Raycast但是不参与渲染绘制的组件
    /// </summary>
    public class Empty4Raycast : MaskableGraphic 
    {
        protected Empty4Raycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}