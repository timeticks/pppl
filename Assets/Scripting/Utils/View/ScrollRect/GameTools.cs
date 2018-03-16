
using UnityEngine;

public static class GameTools
{
    static public GameObject AddChild(Transform parent, GameObject prefab)
    {

        GameObject go = GameObject.Instantiate(prefab) as GameObject;

        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent, false);
            go.layer = parent.gameObject.layer;
        }
        return go;
    }
}