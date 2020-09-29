using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maincamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera camera;
    Material mat;
    Material sobelmat;
    public RenderTexture depth;
    public RenderTexture color;
    public RenderTexture sobeldepth;
    void Start()
    {
        camera = GetComponent<Camera>();
        camera.depthTextureMode = DepthTextureMode.Depth;
        Shader getdepth = Shader.Find("ssf/getdepth");
        mat = new Material(getdepth);
        Shader sobeldepth = Shader.Find("Unlit/sobel");
        sobelmat = new Material(sobeldepth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(!depth)
        {
            depth = RenderTexture.GetTemporary(source.width, source.height);
        }
        if (!color)
        {
            color = RenderTexture.GetTemporary(source.width, source.height);
        }
        if(!sobeldepth)
        {
            sobeldepth= RenderTexture.GetTemporary(source.width, source.height);
        }
        Graphics.Blit(source, depth, mat);
        Graphics.Blit(depth, sobeldepth, sobelmat);
        Graphics.Blit(source, color);
    }
}
