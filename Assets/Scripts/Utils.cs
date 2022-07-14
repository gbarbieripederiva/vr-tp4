public class Utils
{
    // Maps x from an interval [a, b] to [c, d]
    public static float mapNumber(float x, float a, float b, float c, float d)
    {
        return (x - a) * (d - c) / (b - a) + c;
    }
}
