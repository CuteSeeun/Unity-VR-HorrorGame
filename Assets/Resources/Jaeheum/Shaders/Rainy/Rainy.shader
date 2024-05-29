Shader "Unlit/Rainy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // �ؽ��� �Ӽ�
        _Size("Size", float) = 1              // ����� ũ��
        _T("Time", float) = 1                 // �ð�
        _Distortion("Distortion", range(-5, 5)) = 1 // �ְ� ���� 
        _Blur("Blur", range(0, 1)) = 1        // �帲 ����
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" } // ���� Ÿ���� ����
        LOD 100
        //GrabTexture�� ����Ͽ� ���� ĸ���ؼ� ����� ���̱� ������,
        // �� ���̴��� ����Ǵ� ������Ʈ�� ĸ���� ����� ���� �ʵ���(=���� ���߿� ������ �ǵ���)
        // Queue�� Transparent�� ����
        // �̰� �ϴϱ� �������ܼ� �ϴ� ���Ƴ���;;
        // Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
        // GrabPass{"_GrabTexture"}
            
        Pass
        {
            CGPROGRAM 
            
            #pragma vertex vert // ���ؽ� ���̴��� 
            #pragma fragment frag // �����׸�Ʈ ���̴� �Լ� ����
            #pragma multi_compile_fog // �Ȱ� ����� ���� ���ù�
            // #define ���ù� => ��ũ�θ� �����ϴµ� ���
            // S��� ��ũ�θ� �����Ͽ� smoothstep()�Լ��� �����ϰ� ȣ���� �� �ִ�.
            #define S(a, b, t) smoothstep(a, b, t)
            #include "UnityCG.cginc" // ����Ƽ�� ����� ���̴� �Լ� ���

            struct appdata // ���ؽ� �Է� ����ü
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f // ���ؽ� ��� ����ü
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            // �ؽ��� ���÷� �� �Ӽ� ���� ���𹮵�
            sampler2D _MainTex;
            //sampler2D _GrabTexture;
            float4 _MainTex_ST;
            float _Size;
            float _T;
            float _Distortion;
            float _Blur;

            v2f vert (appdata v) // ���ؽ� ���̴� �Լ� ���� 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //���ؽ� ��ǥ ���
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // �ؽ��� ��ǥ ���
                UNITY_TRANSFER_FOG(o, o.vertex); // �Ȱ� ���
                return o;
            }

            float N21(float2 p) // ������ 
            {
                p =frac(p*float2(123.34, 345.45));
                p += dot(p, p + 34.345);
                return frac(p.x *p.y);
            }

            // ������� ���� �ø��� ���� ���̾� ���� : ���̾� �ϳ��� ����� ȿ�� UV��Ʈ
            float3 Layer(float2 UV, float t)  
            {
                float2 aspect = float2(2,1); // 2x1�� ũ��� Ÿ�ϸ� ����
                float2 uv = UV * _Size * aspect; // �ؽ��� ��ǥ ���
                uv.y += t *.25;

                float2 gv = frac(uv) - 0.5; 
                float2 id = floor(uv);
                float n = N21(id); // �ؽ��� ������ ���� sin�� ��ȭ ����
                t += n*6.2831; // sin�׷����� 2pi�ֱ��̹Ƿ� �ֱ⺰ ���� �ݺ�

                // X��ǥ ���
                float w = UV.y * 10;
                float x = (n - .5) * .8;
                x += (.4-abs(x)) * sin(3*w) * pow(sin(w), 6) * 0.45;

                // y��ǥ ���
                // ������ ���� ������ �ö� ���� ���� �׷���
                float y = -sin(t+sin(t+sin(t)*0.5)) * 0.45;
                // ����� �ϴ��� �� �� �ε巯�� Ÿ������ ��Ÿ���� ����
                // ���� -x�� ���ִ� ����: x��ǥ�� �̵��ϴ��� ���¸� ������ �� �ִ�.
                y -= (gv.x-x) * (gv.x-x);

                // ����� ��ġ ���
                // gv: ���η� �� Ÿ��, gv/ aspect: ���׶� ��
                float2 dropPos = (gv - float2(x, y)) / aspect;
                float drop = S(.05, .03, length(dropPos)); // ����� ���� 

                // ���� ��ġ ���
                float2 trailPos = (gv - float2(x, t*.25)) / aspect;
                trailPos.y = (frac(trailPos.y * 8)-0.5) / 8; // ����� ������ y�������� 8�� Ÿ�ϸ�
                float trail = S(.03, .01, length(trailPos)); // ����� ���� �׷��ֱ�

                float fogTrail = S(-.05, .05, dropPos.y);
                fogTrail *= S(.5, y, gv.y);
                trail *= fogTrail;

                fogTrail *= S(.05, .04, abs(dropPos.x)); // ����� ���� �� �ڱ� �����

                //col += fogTrail * .5;
                //col += trail;
                //col += drop;

                // Drop + Trail ��� ���� ��� == ȿ�� ���� ���
                float2 offs = drop * dropPos + trail * trailPos; 
                //if(gv.x > 0.48 || gv.y > 0.49) col = float4(1,0,0,1);

                return float3(offs, fogTrail);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = fmod(_Time.y + _T, 7200); // 2�ð����� �ݺ��ǵ���

                float4 col = 0; 

                //�������� �� �ø��� 
                float3 drops = Layer(i.uv, t);
                drops += Layer(i.uv * 1.23 + 7.54, t);
                drops += Layer(i.uv * 1.35 + 1.54, t);
                drops += Layer(i.uv * 1.57 - 7.54, t);

                float blur = _Blur * 7 * (1 - drops.z);
                col = tex2Dlod(_MainTex, float4(i.uv + drops.xy * _Distortion, 0, blur));
                return col;
            }
            ENDCG
        }
    }
}
