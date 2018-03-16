using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayFrameByUV_MeshVertix : PlayFrameByUVBase
{
    public MeshRenderer m_MeshRen;

    private MeshFilter m_MeshFiler;
    private Vector2 m_offset=Vector2.zero;
    private bool m_scaleFlag=true;   //镜像标志，true为1
    
    internal BetterList<Vector2> m_uvList = new BetterList<Vector2>();
    internal BetterList<Vector2> m_tempUVList = new BetterList<Vector2>();
    void Awake()
    {
        if (m_MeshRen == null)
        {
            m_MeshRen = GetComponent<MeshRenderer>();
        }
        m_CurTex = m_MeshRen.sharedMaterial.mainTexture;
    }

    public override void SetScaleByDir(int actionDir, DirNumType dirNumType)
    {
        switch (dirNumType)
        {
            case DirNumType.Two:
                m_scaleFlag = actionDir != (int)ActionTwoDir.Right;
                break;
            case DirNumType.Four:
                m_scaleFlag = actionDir != (int)ActionTwoDir.Right;
                break;
            case DirNumType.Six:
                m_scaleFlag = !(actionDir == (int)ActionSixDir.Right || actionDir == (int)ActionSixDir.RightBack || actionDir == (int)ActionSixDir.RightFront);
                break;
            case DirNumType.Eight:
                m_scaleFlag = !(actionDir == (int)ActionEightDir.Right || actionDir == (int)ActionEightDir.RightBack || actionDir == (int)ActionEightDir.RightFront);
                break;
        }
        if (m_uvList.size!=0)
            SetTextureOffst(m_offset);
    }

    public override void SetTexture(Texture tex)
    {
        if (m_MeshRen.material.mainTexture != tex)
        {
            m_MeshRen.material.SetTexture("_MainTex", tex);
        }
        
    }

    public override void SetTextureScale(Vector2 scale)
    {
        if (m_MeshFiler == null) m_MeshFiler = GetComponent<MeshFilter>();

        m_uvList.Clear();
        m_uvList.Add(new Vector2(0, 0f));
        m_uvList.Add(new Vector2(1f * scale.x, 1f * scale.y));
        m_uvList.Add(new Vector2(1f * scale.x, 0));
        m_uvList.Add(new Vector2(0f, 1f * scale.y));
        //m_MeshFiler.mesh.uv = m_uvList.ToArray();
        SetTextureOffst(m_offset);
    }
    public override void SetTextureOffst(Vector2 num)
    {
        if (m_MeshFiler == null) m_MeshFiler = GetComponent<MeshFilter>();
        m_tempUVList.Clear();
        if (m_uvList == null) SetTextureScale(Vector2.one);
        for (int i = 0; i < m_uvList.size; i++)  //添加到暂存列表
        {
            m_tempUVList.Add(m_uvList[i]);
        }
        
        m_offset = num;
        for (int i = 0; i < m_tempUVList.size;i++)  //设置超出1的循环
        {
            Vector2 temp = m_tempUVList[i] + num;
            m_tempUVList[i] += num;
            if (Mathf.Abs(temp.x) > 1.0001f){ temp.x = temp.x % 1f; }
            if (Mathf.Abs(temp.y) > 1.0001f){ temp.y = temp.y % 1f; }
            m_tempUVList[i] = temp;
        }
        if (m_tempUVList[0].y > m_tempUVList[1].y) //如果y轴相反，进行反置
        {
            if (m_tempUVList[0].y.Equals(1))
            {
                m_tempUVList[0] = new Vector2(m_tempUVList[0].x, 0);
                m_tempUVList[2] = new Vector2(m_tempUVList[2].x, 0);
            }
            else if (m_tempUVList[1].y.Equals(0))
            {
                m_tempUVList[1] = new Vector2(m_tempUVList[1].x, 1);
                m_tempUVList[3] = new Vector2(m_tempUVList[3].x, 1);
            }
        }
        if (!m_scaleFlag)
        {
            Vector2 temp1 = m_tempUVList[0];
            Vector2 temp2 = m_tempUVList[1];
            m_tempUVList[0] = m_tempUVList[2];
            m_tempUVList[1] = m_tempUVList[3];
            m_tempUVList[2] = temp1;
            m_tempUVList[3] = temp2;
        }
        m_MeshFiler.mesh.uv = m_tempUVList.ToArray();
    }
}
