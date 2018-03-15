using System.Collections;
using UnityEngine;

public class EyeBlinkEffect : MonoBehaviour
{
    public Material _Material;
    public float WaveScale = 1.65f;
    public float LineWidth = 0.6f;

    public float PointY = 1.2f;
    public bool EnablePlay;
    public float DurationTime = 4;
    public AnimationCurve AniCur;
    public Vector2 AniValue;
    void OnEnable()
    {
        
        if (EnablePlay)
        {
            Play();
        }
    }

    public void Copy(EyeBlinkEffect origin)
    {
        AniCur = origin.AniCur;
        _Material = origin._Material;
        DurationTime = origin.DurationTime;
        AniValue = origin.AniValue;

    }

    private IEnumerator doBlinkAniCor;
    public void Play()
    {
        _Material.SetFloat("_lineWidth", LineWidth);
        _Material.SetFloat("_waveScale", WaveScale);
        PointY = AniValue.x;

        if (doBlinkAniCor != null)
            StopCoroutine(doBlinkAniCor);
        doBlinkAniCor = DoBlinkAni();
        StartCoroutine(doBlinkAniCor);
    }

    public IEnumerator DoBlinkAni()
    {
        float curTime = 0;
        while (curTime <DurationTime)
        {
            curTime += Time.deltaTime;
            PointY = Mathf.LerpUnclamped(AniValue.x, AniValue.y, AniCur.Evaluate(curTime / DurationTime));
            yield return null;
        }
        enabled = false;
    }
    

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_Material == null) return;
        _Material.SetFloat("_pointY", PointY);
        Graphics.Blit(source, destination, _Material);
    }
    
}