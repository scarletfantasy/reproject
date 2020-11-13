using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroll : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject show;
    public GameObject unshow;
    void Start()
    {
        
    }
   /* private void OnPostRender()
    {


        var render = show.GetComponent<MeshRenderer>();
        if (render)
        {
            render.enabled = false;
        }
        foreach (var r in show.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = false;
        }
        var unrender = unshow.GetComponent<MeshRenderer>();
        if (unrender)
        {
            unrender.enabled = true;
        }
        foreach (var r in unshow.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = true;
        }

    }
    private void OnPreRender()
    {
        var render = show.GetComponent<MeshRenderer>();
        if(render)
        {
            render.enabled = true;
        }
        foreach (var r in show.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = true;
        }
        var unrender = unshow.GetComponent<MeshRenderer>();
        if (unrender)
        {
            unrender.enabled = false;
        }
        foreach (var r in unshow.GetComponentsInChildren<MeshRenderer>())
        {
            r.enabled = false;
        }

    }*/
    private void OnPostRender()
    {
        
        if(show.activeSelf)
        {
            show.SetActive(false);
        }
        if(!unshow.activeSelf)
        {
            unshow.SetActive(true);
        }
        
    }
    private void OnPreRender()
    {
        if(!show.activeSelf)
        {
            show.SetActive(true);
        }
        if(unshow.activeSelf)
        {
            unshow.SetActive(false);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
