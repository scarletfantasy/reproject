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
