Shader "Custom/GridShader" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _GridSpacing ("Grid Spacing", Float) = 1
        _LineThickness ("Line Thickness", Range(0.001, 0.1)) = 0.01
        _DashLength ("Dash Length", Float) = 0.05
    }
    SubShader {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType"="Opaque" }
        LOD 200
        
        Pass {
            ZWrite On
            ZTest Always // Always render the grid regardless of depth
            
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off // Disable backface culling to render both sides of the geometry
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1; // Pass world position to fragment shader
            };
            
            sampler2D _MainTex;
            fixed4 _Color;
            float _GridSpacing;
            float _LineThickness;
            float _DashLength;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = v.vertex; // Store world position
                o.uv = v.uv;
                return o;
            }
            
            
            fixed4 frag (v2f i) : SV_Target {
                // Calculate world space position from the interpolated vertex position
                float3 worldPos = i.worldPos.xyz / i.worldPos.w;
                
                // Calculate distance from camera
                float distanceToCamera = distance(worldPos, _WorldSpaceCameraPos);
            
                // Calculate fade factor based on distance
                float fadeFactor = saturate(1.0 - distanceToCamera / _FadeDistance);
            
                // Calculate grid UV coordinates
                float2 gridUV = i.uv * _GridSpacing;
            
                // Calculate grid lines
                float2 gridLines = frac(gridUV);
                
                // Determine if the current position falls within a dash or a gap
                float dashPosX = gridLines.x - floor(gridLines.x / (_DashLength + _GapLength)) * (_DashLength + _GapLength);
                float dashPosY = gridLines.y - floor(gridLines.y / (_DashLength + _GapLength)) * (_DashLength + _GapLength);
                float isDashX = (dashPosX < _DashLength) ? 1.0 : 0.0;
                float isDashY = (dashPosY < _DashLength) ? 1.0 : 0.0;
            
                // Adjust the thickness of the dashed lines
                float2 offset = abs(gridLines - 0.5);
                float lineThickness = 0.5 - _LineThickness;
                float2 thickness = clamp((lineThickness - offset) / _LineThickness, 0, 1);
                float gridLine = min(thickness.x, thickness.y);
            
                // Invert the grid line to color the lines instead of spaces between
                return _Color * (1 - gridLine) * isDashX * isDashY * fadeFactor;
            }        
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}
