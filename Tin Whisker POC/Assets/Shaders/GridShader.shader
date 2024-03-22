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
            ZWrite Off
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
                float2 gridUV = i.uv * _GridSpacing;
                float2 gridLines = frac(gridUV);
                
                // Determine the length of a single dash and gap
                float dashLength = _DashLength;
                float gapLength = 0.1;
                
                // Calculate the position within a single dash-gap segment
                float dashPosX = gridLines.x - floor(gridLines.x / (dashLength + gapLength)) * (dashLength + gapLength);
                float dashPosY = gridLines.y - floor(gridLines.y / (dashLength + gapLength)) * (dashLength + gapLength);

                // Determine if the current position falls within a dash or a gap
                float isDashX = (dashPosX < dashLength) ? 1.0 : 0.0;
                float isDashY = (dashPosY < dashLength) ? 1.0 : 0.0;
                
                // Adjust the thickness of the dashed lines
                float2 offset = abs(gridLines - 0.5);
                float lineThickness = 0.5 - _LineThickness;
                float2 thickness = clamp((lineThickness - offset) / _LineThickness, 0, 1);
                
                // Combine thickness for horizontal and vertical lines
                float gridLine = min(thickness.x, thickness.y);
                
                // Invert the grid line to color the lines instead of spaces between
                return _Color * (1 - gridLine) * isDashX * isDashY;
            }           
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}
