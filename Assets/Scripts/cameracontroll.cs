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
    private void OnPostRender()
    {
        
        if(show.activeInHierarchy)
        {
            show.SetActive(false);
        }
        if(!unshow.activeInHierarchy)
        {
            unshow.SetActive(true);
        }
        
    }
    private void OnPreRender()
    {
        if(!show.activeInHierarchy)
        {
            show.SetActive(true);
        }
        if(unshow.activeInHierarchy)
        {
            unshow.SetActive(false);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
