using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;


#endif
public class CircleRectImage : Image
{
   [Range(1,2)]
    public float RadiusScale;
    [Range(1, 4)]
    public float RadiusRatio;

    private float mLastRadiusScale;
    private float mLastRadiusRatio;
    private bool mIsSetMat;

    void SetRectMat()
    {
        Shader s = Shader.Find("Unlit/CircleRect");
        Material mat = new Material(s);
        material = mat;
        material.SetFloat("_RadiusScale", RadiusScale);
        material.SetFloat("_RadiusRatio", RadiusRatio);
    }
#if UNITY_EDITOR
    [ExecuteInEditMode]
    void Update()
    {
        if (!mIsSetMat)
        {
            SetRectMat();
            mIsSetMat = true;
        }
        if (mLastRadiusScale != RadiusScale)
        {
            material.SetFloat("_RadiusScale", RadiusScale);
            mLastRadiusScale = RadiusScale;
        }
        if (mLastRadiusRatio != RadiusRatio)
        {
            material.SetFloat("_RadiusRatio", RadiusRatio);
            mLastRadiusRatio = RadiusRatio;
        }
    }
#endif

}
#if UNITY_EDITOR
[CanEditMultipleObjects()]
[CustomEditor(typeof(CircleRectImage), true)]
public class CircleRectImageEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        //CircleRectImage targetIns = (CircleRectImage)target;
        //targetIns.RadiusRatio = EditorGUILayout.FloatField("RadiusRatio ", targetIns.RadiusRatio);
        //targetIns.RadiusScale = EditorGUILayout.FloatField("RadiusScale ", targetIns.RadiusScale);
    }
}
#endif