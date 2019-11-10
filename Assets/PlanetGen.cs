using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    public int maxHeight = 5000;
    public int minHeight = 2500;
    public int numVertices = 10;
    public int octaves = 4;
    public float lacunarity = 2;
    public float persistance = 0.5f;
    public float seed;
    public int dayLength = 100;
    protected float theta;
    int[] generateTriangles(Vector3[] vertices) {
        int[] triangles = new int[(vertices.Length-1) * 3];
        for (int i = 1; i < vertices.Length; i++) {
            int triangleIndex = (i-1)*3;
            triangles[triangleIndex] = (i!=vertices.Length-1) ? i+1 : 0;
            triangles[triangleIndex+1] = 0;
            triangles[triangleIndex+2] = i;
        }
        return triangles;
    }
    void generateMesh() {
        var mesh = new Mesh();
        Vector3 center = Vector3.zero;
        List<Vector3> chunkBoundaries = new List<Vector3>(numVertices+1);
        chunkBoundaries.Add(center);

        theta = 365f/numVertices;

        for (float rot=0; rot < 365; rot += theta) {
            Vector3 newVertex = Quaternion.AngleAxis(rot, Vector3.back) * (Vector3.up * maxHeight);
            chunkBoundaries.Add(newVertex);
        }

        mesh.vertices = chunkBoundaries.ToArray();
        mesh.triangles = generateTriangles(mesh.vertices);
        
        var meshfilter = this.GetComponent<MeshFilter>();
        meshfilter.mesh = mesh;
    }
    float perlinCircle(float index, float frequency) {
        float angle = ((index-1) * theta) * Mathf.Deg2Rad;
        float offset = frequency + seed;
;
        float x = frequency * Mathf.Cos(angle);
        float y = frequency * Mathf.Sin(angle);
        return Mathf.PerlinNoise(x+offset, y+offset);
    }
    float perlinOctaves(float index) {
        float total = 0;
        float maxValue = 0;
        for (int octave = 0; octave < octaves; octave++) {
            float frequency = Mathf.Pow(lacunarity, octave);
            float amplitude = Mathf.Pow(persistance, octave);
            maxValue += amplitude;

            total += perlinCircle(index, frequency) * amplitude;
        }
        return total/maxValue;
    }
    public void setHeights() {
        seed = Random.Range(0, 1000000);
        int heightRange = maxHeight - minHeight;
        var meshfilter = this.GetComponent<MeshFilter>();
        Vector3[] vertices = meshfilter.mesh.vertices;

        float theta = 365f/numVertices;

        for (int i=1; i < vertices.Length; i++) {
            Debug.Log(perlinOctaves(i));
            float height = perlinOctaves(i) * heightRange + minHeight;
            vertices[i] = vertices[i].normalized * height;
        }

        meshfilter.mesh.vertices = vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(0.5f +(vertices[i].x)/(2*maxHeight), 
                             0.5f + (vertices[i].y)/(2*maxHeight));
        }
        meshfilter.mesh.uv = uvs;
        meshfilter.mesh.RecalculateNormals();
    }
    // Start is called before the first frame update
    void Start()
    {
        generateMesh();
        setHeights();
    }
    private void FixedUpdate() {
        float rotationPerSec = 365f / dayLength;
        this.transform.rotation = Quaternion.AngleAxis(rotationPerSec * Time.deltaTime, Vector3.back) * this.transform.rotation;
    }
}
