using System.Collections.Generic;
public class MoveSetCollection 
{
    public static List<List<Position>> GetValidTilesForShoot(PieceView player, Board board)
    {
        List<List<Position>> positions = new List<List<Position>>();

        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).RightUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).Right().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).RightDown().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).LeftUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).Left().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition)).LeftDown().CollectValidPositions());

        return positions;
    }

    public static List<List<Position>> GetValidTilesForCones(PieceView player , Board board)
    {
        List<List<Position>> positions = new List<List<Position>>();

        positions.Add(GetTileConeRightUp(player, board));
        positions.Add(GetTileConeRight(player, board));
        positions.Add(GetTileConeRightDown(player, board));
        positions.Add(GetTileConeLeftUp(player, board));
        positions.Add(GetTileConeLeft(player, board));
        positions.Add(GetTileConeLeftDown(player, board));

        return positions;
    }
    public static List<List<Position>> GetTileRing(Position position, Board board)
    {
        List<List<Position>> positions = new List<List<Position>>();

        positions.Add(new MoveSetHelper(board, position).CollectValidPositions());
        positions.Add(new MoveSetHelper(board, position).RightUp(1).CollectValidPositions());
        positions.Add(new MoveSetHelper(board, position).Right(1).CollectValidPositions());
        positions.Add(new MoveSetHelper(board, position).RightDown(1).CollectValidPositions());
        positions.Add(new MoveSetHelper(board, position).LeftUp(1).CollectValidPositions());
        positions.Add(new MoveSetHelper(board, position).Left(1).CollectValidPositions());
        positions.Add(new MoveSetHelper(board, position).LeftDown(1).CollectValidPositions());

        return positions;
    }
    public static List<List<Position>> GetAllTiles(Board board)
    {
        List<List<Position>> positions = new List<List<Position>>();

        positions.Add(new MoveSetHelper(board, new Position(-3,0)).RightUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, new Position(-3, 1)).RightUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, new Position(-3, 2)).RightUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, new Position(-3, 3)).RightUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, new Position(-2, 3)).RightUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, new Position(-1, 3)).RightUp().CollectValidPositions());
        positions.Add(new MoveSetHelper(board, new Position(0, 3)).RightUp().CollectValidPositions());


        return positions;
    }

    private static Position HexagonalRingOffset(int ringNumber, int sideIndex)
    {
        int dx = HexagonalDirections[sideIndex].Q * ringNumber; //q and r could be switched idk
        int dy = HexagonalDirections[sideIndex].R * ringNumber;
        return new Position(dx, dy);
    }

    private static Position HexagonalDirection(int directionIndex)
    {
        return HexagonalDirections[directionIndex];
    }

    private static readonly Position[] HexagonalDirections = new Position[]
    {
    new Position(1, 0), new Position(1, -1), new Position(0, -1),
    new Position(-1, 0), new Position(-1, 1), new Position(0, 1)
    };

    private static List<Position> GetTileConeRightUp(PieceView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .RightUp(1).LeftUp(1).Right(1).CollectValidPositions(); //RightUp
    }
    private static List<Position> GetTileConeRight(PieceView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .Right(1).RightUp(1).RightDown(1).CollectValidPositions(); //Right
    }
    private static List<Position> GetTileConeRightDown(PieceView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .RightDown(1).Right(1).LeftDown(1).CollectValidPositions(); //RightDown
    }
    private static List<Position> GetTileConeLeftUp(PieceView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .LeftUp(1).RightUp(1).Left(1).CollectValidPositions(); //LeftUp
    }
    private static List<Position> GetTileConeLeft(PieceView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .Left(1).LeftUp(1).LeftDown(1).CollectValidPositions(); //Left
    }
    private static List<Position> GetTileConeLeftDown(PieceView player, Board board)
    {
        return new MoveSetHelper(board, PositionHelper.WorldToHexPosition(player.WorldPosition))
            .LeftDown(1).Left(1).RightDown(1).CollectValidPositions(); //LeftDown
    }
}