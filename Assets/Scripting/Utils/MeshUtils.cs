using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshUtils 
{
    public static Mesh GetMesh(int len, int weith)
    {
        float angle = 360.0f / len;
        Vector3 dir = Vector3.zero;
        Vector3 forward = Vector3.forward;
        Vector3[] vertices = new Vector3[len];
        for (int i = 0; i < vertices.Length; i++)
        {
            var rot = Quaternion.Euler(0, angle * i, 0);
            dir = rot * forward;
            vertices[i] = dir * weith;
        }
        int[] triangles = new int[(vertices.Length - 2) * 3];
        for (int i = 0; i < triangles.Length; i++)
        {
            if (i % 3 == 0)
            {
                triangles[i] = vertices.Length - 1;

            }
            else
            {
                int z = (i - 1) / 3;
                int y = (i - 1) % 3;
                int f = z + y;
                triangles[i] = f;
            }
        }
        Vector2[] uv = new Vector2[vertices.Length];
        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2(vertices[i].x, vertices[i].z);
        }
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }



}
