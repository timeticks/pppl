using UnityEngine;
using System.Collections;

public class ShockCtrl : MonoBehaviour 
{
    Vector3 m_lastPos;
    Quaternion m_lastRot;
    bool m_isShaking = false;
    float m_shakeStrength = 0.1f;
    float m_posShake = 2f;
    float m_shakeTime = 0.4f;

    private IEnumerator shakeCameraCor;
    public void Init()
    {
        if (shakeCameraCor != null)
            StopCoroutine(shakeCameraCor);
        shakeCameraCor = ShakeCamera();
        StartCoroutine(shakeCameraCor);
    }

    public static ShockCtrl DoShock(Transform shockObj, float shakeStrength, float posShake, float shakeTime)
    {
        ShockCtrl shock = shockObj.GetComponent<ShockCtrl>();
        if (shock == null)
            shock = shockObj.gameObject.AddComponent<ShockCtrl>();
        shock.Init(shakeStrength, posShake, shakeTime);
        return shock;
    }
    public static ShockCtrl DoCamShock(Camera cam)
    {
        ShockCtrl shock = cam.GetComponent<ShockCtrl>();
        if (shock == null)
            shock = cam.gameObject.AddComponent<ShockCtrl>();
        float ratio = cam.orthographicSize / 3.5f;
        ratio = Mathf.Sqrt(ratio);
        shock.Init(0.012f * ratio, 0.2f * ratio, 0.3f * ratio);
        return shock;
    }


    public void Init(float shakeStrength, float posShake, float shakeTime)
    {
        m_shakeStrength = shakeStrength;
        m_posShake = posShake;
        m_shakeTime = shakeTime;
        if (m_isShaking)
        {
            transform.position = m_lastPos;
            transform.rotation = m_lastRot;
        }
        if (shakeCameraCor != null)
            StopCoroutine(shakeCameraCor);
        shakeCameraCor = ShakeCamera();
        StartCoroutine(shakeCameraCor);
    }


    public IEnumerator ShakeCamera()
    {
        m_isShaking = true;
        float shake_intensity = m_shakeTime;
        Vector3 orgPosition = transform.position;
        m_lastPos = orgPosition;
        Quaternion originRotation = transform.rotation;
        m_lastRot = originRotation;
        while (shake_intensity > 0)
        {
            transform.position = orgPosition + Random.insideUnitSphere * m_shakeStrength * m_posShake;
            transform.rotation = new Quaternion(
            originRotation.x + Random.Range(-m_shakeStrength, m_shakeStrength) * m_shakeStrength,
            originRotation.y + Random.Range(-m_shakeStrength, m_shakeStrength) * m_shakeStrength,
            originRotation.z + Random.Range(-m_shakeStrength, m_shakeStrength) * m_shakeStrength,
            originRotation.w + Random.Range(-m_shakeStrength, m_shakeStrength) * m_shakeStrength);
            shake_intensity -= Time.deltaTime;
            yield return null;
        }
        transform.position = orgPosition;
        transform.rotation = originRotation;
        m_isShaking = false;
    }
}
