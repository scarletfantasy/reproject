using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holefilingcontroltradition : MonoBehaviour
{
    public GameObject obj;
    public anothereye cam;
    public Material mixmat;
    public Material[] mats;
    RenderTexture rt;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Camera>().SetReplacementShader(Shader.Find("reproject/objectrenderholefillingshader"), "RenderType");
        rt = new RenderTexture(400, 300, 0);
        //this.GetComponent<Camera>().targetTexture = rt;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnPreRender()
    {
        /*foreach (var r in obj.GetComponentsInChildren<MeshRenderer>())
        {
           foreach (var m in r.materials)
            {
                r.material.SetTexture("_StencilTex", cam.targetimage);
            }
            
        }*/
        foreach(var m in mats)
        {
            m.SetTexture("_StencilTex", cam.targetimage);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mixmat.SetTexture("_RayTex", source);
        Graphics.Blit(cam.targetimage, destination, mixmat);
        //Graphics.Blit(source, destination);
    }
}
