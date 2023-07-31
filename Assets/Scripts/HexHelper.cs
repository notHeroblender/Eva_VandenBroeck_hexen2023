using UnityEngine;

public class HexHelper
{
    public static Vector2 CubeToAxial(Vector3 cube)
    {
        var qhex = cube.x;
        var rhex = cube.y;

        return new Vector2(qhex, rhex);
    }

    public static Vector3 AxialToCube(float q, float r)
    {
        var qCube = q;
        var rCube = r;
        var sCube = -q - r;

        return new Vector3(qCube, rCube, sCube);
    }

    public static Position AxialRound(float q, float r)
    {
        Vector2 hex = CubeToAxial(CubeRound(AxialToCube(q, r)));
        return new Position((int)hex.x, (int)hex.y);
    }

    public static Vector3 CubeRound(Vector3 cube)
    {
        var q = Mathf.Round(cube.x);
        var r = Mathf.Round(cube.y);
        var s = Mathf.Round(cube.z);

        var qDiff = Mathf.Abs(q - cube.x);
        var rDiff = Mathf.Abs(r - cube.y);
        var sDiff = Mathf.Abs(s - cube.z);

        if (qDiff > rDiff && qDiff > sDiff)
            q = -r - s;
        else if (rDiff > sDiff)
            r = -q - s;
        else
            s = -q - r;

        return new Vector3(q, r, s);
    }

    public static Position AxialSubtract(Position a, Position b) => new Position(a.Q - b.Q, a.R - b.R);
    public static Position AxialAdd(Position a, Position b) => new Position(a.Q + b.Q, a.R + b.R);

    public static int AxialDistance(Position a, Position b)
    {
        return (Mathf.Abs(a.Q - b.Q) + Mathf.Abs(a.Q + a.R - b.Q - b.R) + Mathf.Abs(a.R - b.R)) / 2;
    }
}