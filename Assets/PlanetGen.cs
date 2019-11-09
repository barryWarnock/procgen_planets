using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    public int maxHeight = 5000;
    public int seaHeight = 3000;
    public int minHeight = 2500;
    public int numChunks = 10;
    public float frequency = 1;
    public float seed = 0;
    public float power = 1;
    public int dayLength = 100;
    List<Vector3> generateChunks(int numChunks) {
        Debug.Log("generate chunks");
        Vector3 center = Vector3.zero;
        List<Vector3> chunkBoundaries = new List<Vector3>(numChunks+1);
        chunkBoundaries.Add(center);

        float theta = 365f/numChunks;
        Debug.Log(theta);

        for (float rot=0; rot < 365; rot += theta) {
            Debug.Log(rot);
            int heightRange = maxHeight - minHeight;

            float height = Mathf.Pow(Mathf.PerlinNoise(rot*frequency, seed), power) * heightRange + minHeight;
            Vector3 boundary = Quaternion.AngleAxis(rot, Vector3.back) * (Vector3.up * height);
            chunkBoundaries.Add(boundary);
        }

        Debug.Log("generated chunks");
        return chunkBoundaries;
    }
    Vector3[] fillChunks(List<Vector3> chunkBoundaries) {
        Debug.Log("fill chunks");
        return chunkBoundaries.ToArray();
    }
    int[] generateTriangles(Vector3[] vertices) {
        Debug.Log("generate triangles");
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

    private void FixedUpdate() {
        float rotationPerSec = 365f / dayLength;
        this.transform.rotation = Quaternion.AngleAxis(rotationPerSec * Time.deltaTime, Vector3.back) * this.transform.rotation;
    }
}
