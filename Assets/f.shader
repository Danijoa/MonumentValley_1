// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/f"
{
    Properties
   {
        // ������Ʈ ���� , �ؽ���
        _Color("Color", Color) = (1,1,1,1)   
        _MainTex("Texture", 2D) = "white" {}    // �ؽ��� ����..

        // �Ȱ� ����, ����, �� 
        _FogColor("Fog Color", Color) = (0.3, 0.4, 0.7, 1.0)
        _FogStart("Fog Start", float) = 0
        _FogEnd("Fog End", float) = 0
   }
        
    SubShader
   {
        Tags{ "RenderType" = "Opaque" } // ������..

        CGPROGRAM
        // surface �Լ� �̸� : surf
        // ����� shader model : Lambert
        // color ��� �Լ� �̸� : mycolor
        // vertex ��� �Լ� �̸� : myvert
        #pragma surface surf Lambert finalcolor:mycolor vertex:myvert

        // Properties���� ���� ���� �޸𸮷� �ø���
        fixed4 _Color;      // fixed4 -> rgba ����..
        sampler2D _MainTex;  
        fixed4 _FogColor;
        half _FogStart;     // half float fixed
        half _FogEnd;

        struct Input
        {
            float2 uv_MainTex;  // _MainTex uv ��ǥ ����
            half fog;           // fog ��ġ
        };

        // ��ġ ��� + ���ϱ� ���(lerp �Ǵ� ����)
        void myvert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input,data);

            //float4 pos = mul(unity_ObjectToWorld, v.vertex).xyzw;
            float4 pos = v.vertex.xyzw;
            
            data.fog = saturate((_FogStart - pos.y) / (_FogStart - _FogEnd));
        }

        // ���� ��� 
        void mycolor(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            fixed3 fogColor = _FogColor.rgb;    // �Ȱ� ����
            fixed3 tintColor = _Color.rgb;      // ������Ʈ ����

            #ifdef UNITY_PASS_FORWARDADD        // ..�̰� ���� �Ǿ� �ִٸ�
            fogColor = 0;
            #endif
            color.rgb = lerp(color.rgb * tintColor, fogColor, IN.fog);
        }

        // ���� ���� ä���
        void surf(Input IN, inout SurfaceOutput o)
        {
            // _MainTex�ε�, IN���� ���� uv_MainTex ����ؼ� ����?.....
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }

        ENDCG
   }

   Fallback "Diffuse"
}