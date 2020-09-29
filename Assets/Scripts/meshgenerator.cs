using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class meshgenerator : MonoBehaviour
{
    // Start is called before the first frame update
    Mesh mesh;
    private Vector3[] pos;
    private int[] indices;
    private Vector2[] uv;
    void Start()
    {
       /*generate special mesh for render caustic light photon*/
        mesh = GetComponent<MeshFilter>().mesh;
        int num = 800 * 600;
        pos = new Vector3[num];
        indices = new int[num];
        uv = new Vector2[num];
        int count = 0;
        for (int i=0;i<800;++i)
        {
            for(int j=0;j<600;++j)
            {
                pos[count] = new Vector3((float)i / 800.0f, (float)j / 600.0f, 0.0f);
                //Debug.Log(pos[count]);
                indices[count] = count;
                uv[count] = new Vector2((float)i / 800.0f, (float)j / 600.0f);
                count++;
            }
        }
        mesh.indexFormat = IndexFormat.UInt32;
        mesh.vertices = pos;
        mesh.uv = uv;
        mesh.SetIndices(indices, MeshTopology.Points, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
