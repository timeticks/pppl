using UnityEngine;
using System.Collections;


public delegate void TriggerDelegate(Collider col);
public delegate void CollisionDelegate(Collision col);
public class TColObj : MonoBehaviour 
{
    internal TriggerDelegate triggerEnterDel;
    internal TriggerDelegate triggerStayDel;
    internal TriggerDelegate triggerExitDel;

    void OnTriggerEnter(Collider col)
    {
        if (triggerEnterDel != null)
            triggerEnterDel(col);
    }
    void OnTriggerStay(Collider col)
    {
        if (triggerStayDel != null)
            triggerStayDel(col);
    }
    void OnTriggerExit(Collider col)
    {
        if (triggerExitDel != null)
            triggerExitDel(col);
    }
}
