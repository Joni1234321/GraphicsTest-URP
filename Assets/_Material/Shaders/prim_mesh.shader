Shader "Test/Primitivies/Mesh"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR0;
            };

            StructuredBuffer<float3> _Positions;
            uniform float4 _Color;
            
            v2f vert(appdata_base v, uint instanceID : SV_InstanceID)
            {
                v2f o;
                const float3 pos = _Positions[instanceID];
                o.pos = UnityObjectToClipPos(v.vertex + pos);
                o.color = _Color;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                return i.color;
            }
            ENDCG
        }
    }
}