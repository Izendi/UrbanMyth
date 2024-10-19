Shader "Custom/UVToColorWithLighting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Optional texture
        _LayerHeight ("LayerHeight", Float) = 0.1  // Float input
        _CullFactor ("CullFactor", Float) = 0.1  // Float input
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"  // For lighting functions

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;  // UV coordinates from the mesh
                float3 normalOS : NORMAL;  // Normal vector for lighting
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;  // Homogeneous clip space position
                float2 uv : TEXCOORD0;  // UV coordinates passed to the fragment shader
                float3 worldPos : TEXCOORD1;  // World position for lighting calculations
                float3 normalWS : TEXCOORD2;  // World-space normal for lighting
            };

            // Vertex shader
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);  // Transform to clip space
                OUT.uv = IN.uv;  // Pass UV coordinates to the fragment shader

                // Calculate world position and normal for lighting
                OUT.worldPos = TransformObjectToWorld(IN.positionOS).xyz;
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normalOS));

                return OUT;
            }

            // Declare uniforms
            float _LayerHeight;  // Declare _LayerHeight here
            float _CullFactor;  // Declare _CullFactor here

            // Random function
            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            // Fragment shader with lighting
            float4 frag(Varyings IN) : SV_Target
            {
                // Calculate the grid position for each fragment
                float2 gridPos = floor(IN.uv * 100.0);
                float result = random(gridPos);

                // Use UV coordinates as the base grass color
                float3 grassColor = float3(34.0 / 255.0, 139.0 / 255.0, 34.0 / 255.0);

                // Cull some grass based on random value and _CullFactor
                if (result >= 0.025 * _CullFactor)
                {
                    // Grass is kept, apply color as usual
                }
                else
                {
                    discard;  // Cull this fragment
                }

                // Lighting calculations
                // Get the normal and world position for lighting
                float3 normal = normalize(IN.normalWS);  // Ensure the normal is normalized
                float3 worldPos = IN.worldPos;

                // Get the lighting data for the fragment
                Light light = GetMainLight();  // URP built-in function to get main directional or point light

                // Calculate the direction from the fragment to the light
                float3 lightDir = normalize(light.position - worldPos);

                // Calculate the diffuse lighting (Lambert's cosine law)
                float NdotL = max(dot(normal, lightDir), 0.0);
                float3 diffuse = light.color.rgb * NdotL;  // Diffuse contribution

                // Optional: Add a simple ambient light to avoid complete darkness
                float3 ambient = float3(0.2, 0.2, 0.2);  // Simple ambient light

                // Apply lighting to the grass color
                grassColor = grassColor * (ambient + diffuse);

                // Handle special case where _LayerHeight is 0 (make grass black)
                if (_LayerHeight == 0.0f)
                {
                    grassColor = float3(0.0, 0.0, 0.0);
                    return float4(grassColor, 1.0);  // Return black with full opacity
                }

                // Adjust intensity based on _LayerHeight
                float intensity = _LayerHeight * 0.05;
                grassColor = grassColor * min(intensity, 1.0);

                // Return the final color with full opacity
                return float4(grassColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}