using UnityEngine;

public class PositionHelper
{
    public const int Distance = 3;
    public const float HexSize = 0.5f;

    public static Position WorldToHexPosition(Vector3 worldpostion)
    {
        var hexPostionQ = (Mathf.Sqrt(3f) / 3f * worldpostion.x - 1f / 3f * worldpostion.z) / HexSize;
        var hexPostionR = (2f / 3f * worldpostion.z) / HexSize;

        return HexHelper.AxialRound(hexPostionQ,hexPostionR);
    }

    public static Vector3 HexToWorldPosition(Position hexPostion)
    {
        var worldPositionX = HexSize * (Mathf.Sqrt(3) * hexPostion.Q + Mathf.Sqrt(3) / 2f * hexPostion.R); ;
        var worldPositionZ = HexSize * (3f / 2f * hexPostion.R); ;

        return new Vector3(worldPositionX, 0, worldPositionZ);
    }
}