using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    public int maxHeight = 5000;
    public int seaHeight = 3000;
    public int minHeight = 2500;
    public int numChunks = 10;
    List<Vector3> generateChunks(int numChunks) {
        Vector3 center = Vector3.zero;
        List<Vector3> chunkBoundaries = new List<Vector3>(numChunks+1);
        chunkBoundaries.Add(center);

        float theta = 365/numChunks;

        for (float rot=0; rot < 365; rot += theta) {
            Vector3 boundary = Quaternion.AngleAxis(rot, Vector3.back) * (Vector3.up * maxHeight);
            chunkBoundaries.Add(boundary);
        }

        return chunkBoundaries;
    }
    Vector3[] fillChunks(List<Vector3> chunkBoundaries) {
        return chunkBoundaries.ToArray();
    }
    int[] generateTriangles(Vector3[] vertices) {
        int[] triangles = new int[(vertices.Length-1) * 3];
        for (int i = 1; i < vertices.Length; i++) {
            int triangleIndex = (i-1)*3;
            triangles[triangleIndex] = i;
            triangles[triangleIndex+1] = 0;
            triangles[triangleIndex+2] = (i!=vertices.Length-1) ? i+1 : 1;
        }
        return triangles;
    }
    // Start is called before the first frame update
    void Start()
    {
        var mesh = new Mesh();
        List<Vector3> chunkBoundaries = generateChunks(numChunks);
        Vector3[] vectors = fillChunks(chunkBoundaries);
        mesh.vertices = vectors;
        mesh.triangles = generateTriangles(vectors);

        var meshfilter = this.GetComponent<MeshFilter>();
        meshfilter.mesh = mesh;
    }
}
