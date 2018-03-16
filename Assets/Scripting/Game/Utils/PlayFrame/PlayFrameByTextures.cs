using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PlayFrameByTextures : MonoBehaviour {
    public bool m_PlayEnable;
    public MeshRenderer m_MyRender;
    public List<Texture2D> m_SpriteList;
    public PlayFrameType m_PlayType;
    public PlayFrameEndType m_EndType;
    [Range(0, 1f)]
    public float m_PlaySpd=0.08f;   //每一帧的间隔

    public int m_StartIndex;
    public int m_EndIndex;

    void OnEnable()
    {
        if (m_PlayEnable)
            Init();
    }

    void Init()
    {
        if (m_MyRender == null) GetComponent<MeshRenderer>();
        Play();
    }

    private IEnumerator animStartCor;
    public void Play()
    {
        if (animStartCor != null)
            StopCoroutine(animStartCor);
        animStartCor = AnimStart();
        StartCoroutine(animStartCor);
    }

    public void Stop()
    {
        if (animStartCor != null)
            StopCoroutine(animStartCor);
    }

    public IEnumerator AnimStart()
    {
        int index = m_StartIndex;

        switch (m_PlayType)
        {
            case PlayFrameType.Repeat:
                while (true)
                {
                    m_MyRender.material.mainTexture = m_SpriteList[index];
                    if (index == m_EndIndex) index = m_StartIndex;
                    else index++;
                    //m_indexNow = m_indexNow % m_SpriteList.Count;
                    yield return new WaitForSeconds(m_PlaySpd);
                }
            case PlayFrameType.PingPong:
                int direction = 1;
                while (true)
                {
                    m_MyRender.material.mainTexture = m_SpriteList[index];
                    index += direction;
                    //逆向
                    if (index == m_EndIndex || index == m_StartIndex) direction = -direction;
                    yield return new WaitForSeconds(m_PlaySpd);
                }
            default:
                for (; index <= m_EndIndex; index++)
                {
                    m_MyRender.material.mainTexture = m_SpriteList[index];
                    yield return new WaitForSeconds(m_PlaySpd);
                }
                switch (m_EndType)
                {
                    case PlayFrameEndType.Disable:
                        gameObject.SetActive(false);
                        break;
                    case PlayFrameEndType.Destroy:
                        Destroy(gameObject);
                        break;
                    default:
                        break;
                }
                yield break;
        }
    }
}





#if UNITY_EDITOR

[CanEditMultipleObjects()]
[CustomEditor(typeof(PlayFrameByTextures), true)]
public class PlayFrameByTexturesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if (GUILayout.Button("绑定Sprites (先选择序列帧的文件夹)"))
        {
            PlayFrameByTextures targetIns = (PlayFrameByTextures)target;

            Object[] selection = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
            List<string> resourcesAssets = new List<string>();
            for (int i = 0; i < selection.Length; i++)
            {
                resourcesAssets.Add(AssetDatabase.GetAssetPath(selection[i]));
            }
            resourcesAssets.Sort((x, y) => { return x.CompareTo(y); });

            List<Texture2D> sprites = new List<Texture2D>();
            for (int i = 0; i < selection.Length; i++)
            {
                try
                {
                    Texture2D sp = AssetDatabase.LoadAssetAtPath<Texture2D>(resourcesAssets[i]);
                    sprites.Add(sp);
                }
                catch { TDebug.LogErrorFormat("Not Sprite {0}" , selection[i].name); }
            }
            TDebug.LogFormat("obj amount: {0}     sprite amount: {1}" , selection.Length , sprites.Count);
            targetIns.m_SpriteList = sprites;
        }
    }
}

#endif


