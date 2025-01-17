void blend_rnm_float(float3 n1, float3 n2, out float3 Out)
{
    n1.z += 1;
    n2.xy = -n2.xy;
    Out = n1 * dot(n1, n2) / n1.z - n2;
}
