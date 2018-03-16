using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayFrameByUV_RawImage : PlayFrameByUVBase
{
    public RawImage m_RawImage;
    void Awake()
    {
        if (m_RawImage == null) m_RawImage = GetComponent<RawImage>();
    }

    public override void SetTexture(Texture tex)
    {
        m_RawImage.texture = tex;
    }
    public override void SetTextureScale(Vector2 scale)
    {
        m_RawImage.uvRect = new Rect(m_RawImage.uvRect.position, scale);
    }
    public override void SetTextureOffst(Vector2 num)
    {
        m_RawImage.uvRect = new Rect(num, m_RawImage.uvRect.size);
    }
}
