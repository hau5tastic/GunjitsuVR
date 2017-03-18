Shader "Custom/NoiseVisualizer" {
 Properties 
     {
         _MainTex ("Base (RGB)", 2D) = "white" {}
         samplerPerlinPerm2D("Perlin Perm", 2D) = "white" {}
         samplerPerlinGrad2D("Perlin Grad", 2D) = "white" {}
     }
 
     SubShader 
     {
         Tags { "RenderType"="Opaque" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf Lambert vertex:vert
 
         sampler2D _MainTex;
         uniform sampler2D samplerPerlinPerm2D;
         uniform sampler2D samplerPerlinGrad2D;
 
         struct Input 
         {
             float2 uv_MainTex;
             float3 vert;
         };
 
         void vert(inout appdata_full v, out Input o)
         {
             UNITY_INITIALIZE_OUTPUT(Input,o);
             o.vert = v.vertex.xyz;
             v.vertex.y += 1;
         }
 
         float perlinNoise(float2 p, float seed)
         {
             // Calculate 2D integer coordinates i and fraction p.
             float2 i = floor(p);
             float2 f = p - i;
 
             // Get weights from the coordinate fraction
             float2 w = f * f * f * (f * (f * 6 - 15) + 10);
             float4 w4 = float4(1, w.x, w.y, w.x * w.y);
 
             // Get the four randomly permutated indices from the noise lattice nearest to
             // p and offset these numbers with the seed number.
             float4 perm = tex2D(samplerPerlinPerm2D, i / 256) + seed;
 
             // Permutate the four offseted indices again and get the 2D gradient for each
             // of the four permutated coordinates-seed pairs.
             float4 g1 = tex2D(samplerPerlinGrad2D, perm.xy) * 2 - 1;
             float4 g2 = tex2D(samplerPerlinGrad2D, perm.zw) * 2 - 1;
 
             // Evaluate the four lattice gradients at p
             float a = dot(g1.xy, f);
             float b = dot(g2.xy, f + float2(-1,  0));
             float c = dot(g1.zw, f + float2( 0, -1));
             float d = dot(g2.zw, f + float2(-1, -1));
 
             // Bi-linearly blend between the gradients, using w4 as blend factors.
             float4 grads = float4(a, b - a, c - a, a - b - c + d);
             float n = dot(grads, w4);
 
             // Return the noise value, roughly normalized in the range [-1, 1]
             return n * 1.5;
         }
 
         void surf (Input IN, inout SurfaceOutput o) 
         {
             half4 c = tex2D (_MainTex, IN.uv_MainTex);
             float n = pow(perlinNoise(IN.vert.xz, 5), 2);
             o.Albedo = float3(n, n, n);//c.rgb;
             o.Alpha = c.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
}
