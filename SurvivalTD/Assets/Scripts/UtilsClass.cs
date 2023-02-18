using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass
{
    
    public static Mesh CombineMeshes(GameObject gameObject)
    {
        gameObject.transform.position = Vector3.zero;

        MeshFilter[] allMesheFilters = gameObject.GetComponentsInChildren<MeshFilter>();

        CombineInstance[] combineInstance = new CombineInstance[allMesheFilters.Length];

        for (int i = 0; i < allMesheFilters.Length; i++)
        {
            combineInstance[i].mesh = allMesheFilters[i].sharedMesh;
            combineInstance[i].transform = allMesheFilters[i].transform.localToWorldMatrix;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combineInstance);

        return mesh;
    }

}
