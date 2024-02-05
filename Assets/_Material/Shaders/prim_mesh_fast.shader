Shader "Test/Primitivies/MeshFast"
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

            uniform float4 _Color;
            uniform float2 _Size;
                
            v2f vert(appdata_base v, uint instanceID : SV_InstanceID)
            {
                v2f o;
                const float x = instanceID % _Size.x;
                const float z = instanceID / _Size.x;
                const float3 pos = mul(float3(x, 0, z), 1.5f);
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