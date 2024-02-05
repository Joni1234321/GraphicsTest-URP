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
                float4 normal : NORMAL;
                float4 color : COLOR0;
            };

            StructuredBuffer<int> _Triangles;
            StructuredBuffer<float3> _LocalPositions;
            StructuredBuffer<float3> _Normals;
            StructuredBuffer<float3> _Positions;
            uniform float4 _Color;
            
            v2f vert(uint vertexID: SV_VertexID, uint instanceID : SV_InstanceID)
            {
                v2f o;
                const float3 local = _LocalPositions[_Triangles[vertexID]];
                const float3 pos = _Positions[instanceID];
                o.pos = UnityObjectToClipPos(local + pos);
                o.color = _Color;
                o.normal = float4(_Normals[vertexID], 1);
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