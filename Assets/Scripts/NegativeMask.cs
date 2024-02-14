using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections;
 
public class NegativeMask : Image {

    protected override void Start()
    {
        base.Start();
        StartCoroutine(Fix());
    }
     
    /// Fix for async loading scenes
    private IEnumerator Fix()
    {
        yield return null;
        maskable = false;
        maskable = true;
    }

    public override Material materialForRendering
    {
        get
        {
            // get a copy of the base material or you going to F*** up the whole project
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
 