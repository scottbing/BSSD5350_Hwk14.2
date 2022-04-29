using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class ProceduralTerrain : MonoBehaviour
{
    [SerializeField]
    int xSquares = 30;
    [SerializeField]
    int zSquares = 30;

    Vector3[] vertices;
    Mesh mesh;
    int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        //he says the width and depth are a vertex count not a square count 
        //his code is wrong when he adds 1, but if you do want to change it 
        //so the depth is number of squares created not numebr of vertices,
        //uncomment the next 2 lines
        
        mesh= new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateGeometry();
        UpdateMesh();
    }

    // Update is called once per frame
    void Update()
    {
        CreateGeometry();
        UpdateMesh();
    }

    void CreateGeometry() {
        //if you want the numberinput to be the number of squares
        // not vertices, leave the +1 in the next tow lines.
        int xWidth = xSquares + 1;
        int zDepth = zSquares + 1;
        int verticeCounter = 0;

        vertices = new Vector3[(xWidth) * (zDepth)];
        for (int z = 0; z < zDepth; z++)
        {
             for (int x = 0; x < xWidth; x++)
            {
                //vertices[verticeCounter] = new Vector3(x, 1.0f, z);  
                float yVal = 1.0f;
                //float perlinnoiseY = Mathf.PerlinNoise(x * .99f, z* .99f) * 1.1f;
                // float t = Time.timeSinceLevelLoad;
                // yval = Mathf.Sin(x + t * 0.5f) + 0.5f;
                yVal = Mathf.PerlinNoise(x*0.05f, z*0.05f) * 15f;

                vertices[verticeCounter] = new Vector3(x, yVal, z);
                verticeCounter++;
            }
        }
        //triangles can only be defined after
        // vertices since they point to vertices
        triangles = new int[(xWidth-1) * (zDepth-1) * 6];
        int vertexcounter = 0; 
        int trianglecount = 0; 
        for (int z = 0; z < zDepth-1; z++)
        {
            for (int x = 0; x < xWidth-1; x++)
            {
                triangles[0 + trianglecount] = vertexcounter + 0; 
                triangles[1 + trianglecount] = vertexcounter + xSquares + 1; 
                triangles[2 + trianglecount] = vertexcounter + 1; 
                triangles[3 + trianglecount] = vertexcounter + 1; 
                triangles[4 + trianglecount] = vertexcounter + xSquares + 1; 
                triangles[5 + trianglecount] = vertexcounter + xSquares + 2; 
                vertexcounter++;
                trianglecount += 6;
            }
            vertexcounter++;
        }
    }

    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    /*
    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .5f);
        }
    }
    */
}
