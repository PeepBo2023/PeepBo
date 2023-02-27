using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberEffect : MonoBehaviour
{
    private Material _m;
    public Shader _s;

    // Start is called before the first frame update
    void Start()
    {
        _m = new Material(_s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _m);
    }
}
