Shader "Test/Primitives"
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

            StructuredBuffer<int> _Triangles;
            StructuredBuffer<float3> _LocalPositions;
            uniform float4 _Color;
            uniform float2 _Size;
            
            v2f vert(uint vertexID: SV_VertexID, uint instanceID : SV_InstanceID)
            {
                v2f o;
                const float3 local = _LocalPositions[_Triangles[vertexID]];
                const float x = instanceID % _Size.x;
                const float z = instanceID / _Size.x;
                const float3 pos = mul(float3(x, 0, z), 1.5f);
                o.pos = UnityObjectToClipPos(local + pos);
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