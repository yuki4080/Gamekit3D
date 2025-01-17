void LerpOneTo_float(float b, float t, out float Out)
{
    float oneMinusT = 1 - t;
    Out = oneMinusT + b * t;
}
