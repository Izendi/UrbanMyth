using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneZeroSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        // Ensure lighting is reset
        DynamicGI.UpdateEnvironment(); // This forces Global Illumination to update
    }

    
}
