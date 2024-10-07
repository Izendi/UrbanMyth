using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disableReticle : MonoBehaviour
{
    public void disable(ref Canvas reticleCanvas)
    {
        reticleCanvas.gameObject.SetActive(false);
    }

    public void enable(ref Canvas reticleCanvas)
    {
        reticleCanvas.gameObject.SetActive(true);
    }
}
