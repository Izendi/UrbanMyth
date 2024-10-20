using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerHeightSetter : MonoBehaviour
{
    public float layerHeight = 1.0f;  // This value will be unique for each object
    public float CullFactor = 1.0f;

    public int maxLights = 8;

    Vector4[] lightPositions;
    Vector4[] lightColors;

    private Renderer rend;
    private MaterialPropertyBlock propBlock;

    private void Awake()
    {
        // Get the Renderer component of the GameObject
        rend = GetComponent<Renderer>();

        // Create a new MaterialPropertyBlock
        propBlock = new MaterialPropertyBlock();

        lightPositions = new Vector4[maxLights];
        lightColors = new Vector4[maxLights];

    }

    void Start()
    {

        // Set the initial layer height for this object
        SetLayerHeight();
        SetCullFactor();
    }
    void SetCullFactor()
    {
        // Get the current property block from the renderer
        rend.GetPropertyBlock(propBlock);

        // Set the _LayerHeight value in the property block
        propBlock.SetFloat("_CullFactor", CullFactor);

        // Apply the property block to the renderer
        rend.SetPropertyBlock(propBlock);
    }

    // This function updates the Material Property Block with the custom _LayerHeight
    void SetLayerHeight()
    {
        // Get the current property block from the renderer
        rend.GetPropertyBlock(propBlock);

        // Set the _LayerHeight value in the property block
        propBlock.SetFloat("_LayerHeight", layerHeight);

        // Apply the property block to the renderer
        rend.SetPropertyBlock(propBlock);
    }

    void Update()
    {
        // Find all active lights in the scene
        Light[] lights = FindObjectsOfType<Light>();
        int lightCount = Mathf.Min(lights.Length, maxLights);

        for (int i = 0; i < lightCount; i++)
        {
            Light light = lights[i];
            lightPositions[i] = light.transform.position;
            lightColors[i] = light.color * light.intensity;
        }

        // Zero out any unused light slots
        for (int i = lightCount; i < maxLights; i++)
        {
            lightPositions[i] = Vector4.zero;
            lightColors[i] = Vector4.zero;
        }

        // Set the properties on the material
        propBlock.SetInt("_CustomLightCount", lightCount);
        propBlock.SetVectorArray("_CustomLightPositions", lightPositions);
        propBlock.SetVectorArray("_CustomLightColors", lightColors);

        // Apply the property block to the renderer
        rend.SetPropertyBlock(propBlock);
    }

}
