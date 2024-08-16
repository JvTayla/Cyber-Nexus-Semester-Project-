using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangerScript : MonoBehaviour
{
    public Renderer MeshRenderer;
    // Start is called before the first frame update


    void Start()
    {
        MeshRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
