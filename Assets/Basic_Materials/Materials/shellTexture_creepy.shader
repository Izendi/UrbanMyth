Shader "Custom/UVToColorWithURPLighting"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // Optional texture (can be omitted if not needed)
        _LayerHeight ("LayerHeight", Float) = 0.1  // Float input
        _CullFactor ("CullFactor", Float) = 0.1  // Float input
        _Shininess ("Shininess", Float) = 32.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ ADDITIONAL_LIGHTS_VERTEX ADDITIONAL_LIGHTS
            #pragma multi_compile _ MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ ADDITIONAL_LIGHT_SHADOWS
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"


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

            struct Input
            {
                float2 uv;
                float3 viewDir;
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
            float _LayerHeight;
            float _CullFactor;
            float _Shininess;

            float random(float2 st) 
            {
                return frac(sin(dot(st.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            // Fragment shader
            float4 frag(Varyings IN) : SV_Target
            {
                // Calculate the grid position for each fragment
                float2 gridPos = floor(IN.uv * 100.0);
                float result = random(gridPos);

                // Grass base color
                float3 grassColor = float3(158.0 / 62.0, 169.0 / 176.0, 46.0 / 255.0);

                // Cull some grass based on random value and _CullFactor
                if (result >= 0.025 * _CullFactor)
                {
                    // Grass is kept, apply color as usual
                }
                else
                {
                    discard;
                }

                if (_LayerHeight == 0.0f)
                {
                    grassColor = float3(0.0, 0.0, 0.0);
                    return float4(grassColor, 1.0);  // Return black
                }

                float intensity = _LayerHeight * 0.05;
                grassColor = grassColor * min(intensity, 1.0);

                

                // Combine diffuse and ambient lighting
                grassColor = grassColor;

                


                return float4(grassColor, 1.0);  // Return the final color
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}