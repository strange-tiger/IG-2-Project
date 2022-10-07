// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/RayShader"
{
    Properties
    {
        _BeginColor("Begin Color", Color) = (1,1,1,1)
        _EndColor("End Color", Color) = (1,1,1,1)
    }

        SubShader
    {
        Tags
        {
            "Queue" = "Background"
            "IgnoreProjector" = "True"
        }

        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha // 알파블렌딩을 위해서 필요함.

        ZWrite Off

        Pass
        {
            CGPROGRAM
        
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            #pragma target 3.0

            fixed4 _BeginColor;
            fixed4 _EndColor;

            struct v2f // vertex to fragment
            {
                float4 pos : SV_POSITION;  // 3D 오브젝트의 월드 좌표
                float4 col : COLOR;  // 3D 오브젝트의 색깔
            };

            // 렌더링 파이프라인 순서
            // 1. 버텍스 로드 : fbx 같은 파일로부터 3D 오브젝트의 버텍스를 GPU로 로드함.
            // 2. 버텍스 셰이더 : 버텍스를 가지고 조작함. 보통 월드 변환, 뷰 변환, 컬링 등등이 일어남.
            // 3. 레스터라이즈 : 월드 변환 된 버텍스의 정보를 바탕으로 모니터의 픽셀과 대응시킴
            // 4. 픽셀 셰이더(프레그먼트 셰이더) : 실제 색깔을 정함. 조명 연산 등이 적용 됨.

            // 버텍스 셰이더
            v2f vert(appdata_full v)
            {
                // 결과값을 위한 변수 생성
                v2f o;

                // 월드 변환
                o.pos = UnityObjectToClipPos(v.vertex);

                // 컬러 정하기
                _EndColor.a = 0;

                // 선형 보간으로 실제 색깔을 정함.
                // 보간을 위한 값으로는 텍스쳐 좌표의 x값을 이용(0.0 ~ 1.0)
                // 따라서 점점 옅어지게 됨.
                o.col = lerp(_BeginColor, _EndColor, v.texcoord.x);

                return o;
            }


            // 프래그먼트 셰이더
            float4 frag(v2f i) : COLOR
            {
                // 버텍스 셰이더에서 이미 색깔을 정했기 때문에 바로 출력하면 됨
                return i.col;
            }

            ENDCG
        }
    }
}
