using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// index以0开始
/// </summary>
public class PlayFrameBySprites : MonoBehaviour
{
    public Image m_MyImage;
    public bool m_PlayOnEnable = true;
    public float m_EnableDelay;
    public List<Sprite> m_SpriteList;
    public PlayFrameType m_PlayType;
    public PlayFrameEndType m_EndType;
    public float m_PlaySpd=0.1f;   //每一帧的间隔




    void OnEnable()
    {
        if (m_PlayOnEnable)
        {
            Init();
        }
    }

    void Init()
    {
        if (m_MyImage == null) GetComponent<Image>();
        Play();
    }

    public void Play()
    {
        StartCoroutine("AnimStart");
    }

    public void Stop()
    {
        StopCoroutine("AnimStart");
    }

    IEnumerator AnimStart()
    {
        if (m_EnableDelay > 0.001f) {
            m_MyImage.enabled = false;
            yield return new WaitForSeconds(m_EnableDelay);
            m_MyImage.enabled = true;
        }

        int index = 0;
        int startIndex = 0;
        int endIndex = m_SpriteList.Count - 1;

        switch (m_PlayType)
        {
            case PlayFrameType.Repeat:
                while (true)
                {
                    m_MyImage.overrideSprite = m_SpriteList[index];
                    if (index == endIndex) index = startIndex;
                    else index++;
                    //m_indexNow = m_indexNow % m_SpriteList.Count;
                    yield return new WaitForSeconds(m_PlaySpd);
                }
            case PlayFrameType.PingPong:
                int direction = 1;
                while (true)
                {
                    m_MyImage.overrideSprite = m_SpriteList[index];
                    index += direction;
                    //逆向
                    if (index >= endIndex || index == startIndex)
                    {
                        direction = -direction;
                        index += direction;
                    }
                    yield return new WaitForSeconds(m_PlaySpd);
                }
            default:
                for (; index <= endIndex; index++)
                {
                    m_MyImage.overrideSprite = m_SpriteList[index];
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
[CustomEditor(typeof(PlayFrameBySprites), true)]
public class PlayFrameBySPritesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        if (GUILayout.Button("绑定Sprites (先选择序列帧的文件夹)"))
        {
            PlayFrameBySprites targetIns = (PlayFrameBySprites)target;

            Object[] selection = Selection.GetFiltered(typeof(Texture2D), SelectionMode.DeepAssets);
            List<string> resourcesAssets = new List<string>();
            for (int i = 0; i < selection.Length; i++)
            {
                resourcesAssets.Add(AssetDatabase.GetAssetPath(selection[i]));
            }
            resourcesAssets.Sort((x, y) => { return x.CompareTo(y); });

            List<Sprite> sprites = new List<Sprite>();
            for (int i = 0; i < selection.Length; i++)
            {
                try
                {
                    Sprite sp = AssetDatabase.LoadAssetAtPath<Sprite>(resourcesAssets[i]);
                    sprites.Add(sp);
                }
                catch { TDebug.LogError("Not Sprite " + selection[i].name); }
            }
            TDebug.Log("obj amount: " + selection.Length + "     sprite amount: " + sprites.Count);
            targetIns.m_SpriteList = sprites;
        }
    }
}

#endif


