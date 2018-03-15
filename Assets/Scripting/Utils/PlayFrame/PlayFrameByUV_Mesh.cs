using UnityEngine;
using System.Collections;

public class PlayFrameByUV_Mesh : PlayFrameByUVBase
{
    public MeshRenderer m_MeshRen;

    void Awake()
    {
        if (m_MeshRen == null) m_MeshRen = GetComponent<MeshRenderer>();
    }

    public override void SetTexture(Texture tex)
    {
        m_MeshRen.material.SetTexture("_MainTex", tex);
    }
    public override void SetTextureScale(Vector2 scale)
    {
        m_MeshRen.material.SetTextureScale("_MainTex", scale);
    }
    public override void SetTextureOffst(Vector2 num)
    {
        m_MeshRen.material.SetTextureOffset("_MainTex", num);
    }
}

