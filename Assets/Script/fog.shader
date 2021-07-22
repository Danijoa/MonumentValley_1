// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/fog"
{
    Properties
    {
        // 오브젝트 색상 , 텍스쳐
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Texture", 2D) = "white" {}    // 텍스쳐 샘플..

        // 안개 색상, 시작, 끝 
        _FogColor("Fog Color", Color) = (0.3, 0.4, 0.7, 1.0)
        _FogStart("Fog Start", float) = 0
        _FogEnd("Fog End", float) = 0
    }
        
    SubShader
   {
        Tags{ "RenderType" = "Opaque" } // 불투명..

        CGPROGRAM
        // surface 함수 이름 : surf
        // 사용할 shader model : Lambert
        // color 계산 함수 이름 : mycolor
        // vertex 계산 함수 이름 : myvert
        #pragma surface surf Lambert finalcolor:mycolor vertex:myvert

        // Properties에서 만든 변수 메모리로 올리자
        fixed4 _Color;      // fixed4 -> rgba 벡터..
        sampler2D _MainTex;
        fixed4 _FogColor;
        half _FogStart;     // half float fixed
        half _FogEnd;

        struct Input
        {
            float2 uv_MainTex;  // _MainTex uv 좌표 저장
            half fog;           // fog 위치
        };

        // 위치 계산 + 진하기 계산(lerp 되는 정도)
        void myvert(inout appdata_full v, out Input data)
        {
            UNITY_INITIALIZE_OUTPUT(Input,data);

            //float4 pos = mul(unity_ObjectToWorld, v.vertex).xyzw;
            float4 pos = v.vertex.xyzw;

            data.fog = saturate((pos.y - _FogStart) / (_FogStart - _FogEnd));
        }

        // 색상 계산 
        void mycolor(Input IN, SurfaceOutput o, inout fixed4 color)
        {
            fixed3 fogColor = _FogColor.rgb;    // 안개 색상
            fixed3 tintColor = _Color.rgb;      // 오브젝트 색상

            #ifdef UNITY_PASS_FORWARDADD        // ..이게 정의 되어 있다면
            fogColor = 0;
            #endif
            color.rgb = lerp(color.rgb * tintColor, fogColor, IN.fog);
        }

        // 최종 색상 채우기
        void surf(Input IN, inout SurfaceOutput o)
        {
            // _MainTex로드, IN으로 들어온 uv_MainTex 사용해서 맵핑?.....
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
        }

        ENDCG
   }

   Fallback "Diffuse"
}