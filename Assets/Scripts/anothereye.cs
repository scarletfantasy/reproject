using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anothereye : MonoBehaviour
{
    // Start is called before the first frame update
    Camera camera;
    public maincamera mymaincamera;
    RenderTexture depth;
    RenderTexture color;
    RenderTexture sobel;
    public RenderTexture targetimage;
    public Material mat;
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    private void OnPreRender()
    {
        depth = mymaincamera.depth;
        color = mymaincamera.color;
        sobel = mymaincamera.sobeldepth;
        mat.SetMatrix("maininvp", (mymaincamera.camera.projectionMatrix * mymaincamera.camera.worldToCameraMatrix).inverse);
        mat.SetTexture("_depth",depth);
        mat.SetTexture("_color", color);
        mat.SetTexture("_sobel", sobel);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if(!targetimage)
        {
            targetimage = RenderTexture.GetTemporary(source.width, source.height);
        }
        Graphics.Blit(source, targetimage);
        Graphics.Blit(source, destination);
    }
}
