using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    public int maxHeight = 5000;
    public int minHeight = 2500;
    public int numVertices = 10;
    public int octaves = 4;
    public float lacunarity = 3;
    public float persistance = 0.3f;
    public float seed;
    public int dayLength = 100;
    protected float theta;
    public Planet planet;
    protected Planet lastPlanet;
    int[] generateTriangles(Vector3[] vertices) {
        int[] triangles = new int[(vertices.Length-1) * 3];
        int triangleIndex = 0;
        for (int i = 1; i < vertices.Length; i++) {
            triangleIndex = (i-1)*3;
            triangles[triangleIndex] = (i!=vertices.Length-1) ? i+1 : 1;
            triangles[triangleIndex+1] = 0;
            triangles[triangleIndex+2] = i;
        }
        return triangles;
    }
    Mesh generateMesh() {
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
        
        return mesh;
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
    public void setHeights(Mesh mesh) {
        seed = Random.Range(0, 1000000);
        int heightRange = maxHeight - minHeight;
        Vector3[] vertices = mesh.vertices;

        float theta = 365f/numVertices;

        for (int i=1; i < vertices.Length; i++) {
            float height = perlinOctaves(i) * heightRange + minHeight;
            vertices[i] = vertices[i].normalized * height;
        }

        mesh.vertices = vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(0.5f +(vertices[i].x)/(2*maxHeight), 
                             0.5f + (vertices[i].y)/(2*maxHeight));
        }
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
    // Start is called before the first frame update
    public void CreatePlanet() {
        Mesh mesh = generateMesh();
        setHeights(mesh);
        Vector2[] colliderPath = new Vector2[mesh.vertices.Length];
        colliderPath[0] = mesh.vertices[mesh.vertices.Length-1];
        for (int i = 1; i < colliderPath.Length; i++) {
            colliderPath[i] = mesh.vertices[i];
        }

        planet.GetComponent<PolygonCollider2D>().SetPath(0, colliderPath);
        mesh.Optimize();
        planet.GetComponent<MeshFilter>().mesh = mesh;
        planet.dayLength = dayLength;
        planet.maxHeight = maxHeight;
        
        if (lastPlanet != null) {
            GameObject.Find("Camera").transform.parent = this.transform;
            Object.Destroy(lastPlanet.gameObject);
        }
        lastPlanet = Instantiate(planet, transform.position, transform.rotation);
    }
    void Start()
    {
        CreatePlanet();
    }
}
