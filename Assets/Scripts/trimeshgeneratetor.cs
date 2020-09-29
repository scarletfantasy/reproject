using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class trimeshgeneratetor : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3[] pos;
    Mesh mesh;
    private int[] triangles;
    private Vector2[] uv;
    public int width=800;
    public int height=600;
    public int n = 800 * 600;
    void Start()
    {
        n = width * height;
        mesh = GetComponent<MeshFilter>().mesh;
        pos = new Vector3[(int)(4 * n)];
        triangles = new int[(int)(6 * n )];
        uv = new Vector2[(int)(4 * n )];
        int max = (int)(6 * n );
        int count = 0;
        Vector3 up = new Vector3(0.0f, 1.0f / height, 0.0f);
        Vector3 right = new Vector3(1.0f/width, 0.0f, 0.0f);
        for (int i = 0; i < max; i += 6)
        {
            int n = i / 3 * 2;
            triangles[i] = n + 2;
            triangles[i + 1] = n + 1;
            triangles[i + 2] = n;
            triangles[i + 3] = n + 1;
            triangles[i + 4] = n + 2;
            triangles[i + 5] = n + 3;
        }
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                Vector3 curpos= new Vector3((float)i / width, (float)j / height, 0.0f);
                pos[count * 4] = curpos - up - right;
                pos[count * 4 + 1] = curpos - up + right;
                pos[count * 4 + 2] = curpos + up - right;
                pos[count * 4 + 3] = curpos + up + right;
                //pos[count] = new Vector3((float)i / 800.0f, (float)j / 600.0f, 0.0f);
                //Debug.Log(pos[count]);
                
                uv[count * 4] = curpos - up - right;
                uv[count * 4 + 1] = curpos - up + right;
                uv[count * 4 + 2] = curpos + up - right;
                uv[count * 4 + 3] = curpos + up + right;
                
                count++;
            }
        }
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = pos;
        mesh.triangles = triangles;
        mesh.uv = uv;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
