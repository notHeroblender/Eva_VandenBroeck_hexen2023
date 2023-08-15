using System.Collections.Generic;
using UnityEngine;

public class MoveSetHelper
{
    private readonly Board _board;
    private readonly Position _fromPosition;
    private readonly Player _player;

    public delegate bool Validator(Board board, Position fromPosition, Position toPosition);

    private List<Position> _validPositions = new List<Position>();

    public MoveSetHelper(Board board, Position fromPosition)
    {
        //if (!board.TryGetPiece(fromPosition, out var piece))
        //    Debug.Log("Was not be able to get the piece");

        _board = board;
        _fromPosition = fromPosition;
        _player = Player.Player1;
        //_player = piece.Player; //meteor null
    }
    public List<Position> CollectValidPositions()
    {
        return _validPositions;
    }

    //Right
    public MoveSetHelper RightUp(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(0, 1, maxSteps, validators);
    public MoveSetHelper Right(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(1, 0, maxSteps, validators);
    public MoveSetHelper RightDown(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(1, -1, maxSteps, validators);


    //Left
    public MoveSetHelper LeftUp(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(-1, 1, maxSteps, validators);
    public MoveSetHelper Left(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(-1, 0, maxSteps, validators);
    public MoveSetHelper LeftDown(int maxSteps = int.MaxValue, params Validator[] validators)
            => Collect(0, -1, maxSteps, validators);

    public MoveSetHelper Collect(int xOffset, int yOffset, int maxSteps = int.MaxValue, params Validator[] validators)
    {
        xOffset *= (_player == Player.Player1) ? 1 : -1;
        yOffset *= (_player == Player.Player1) ? 1 : -1;

        var nextPosition = new Position(_fromPosition.Q + xOffset, _fromPosition.R + yOffset);

        var steps = 0;
        while(steps < maxSteps && _board.IsValidPosition(nextPosition))
        {
           
            _validPositions.Add(nextPosition);
            nextPosition = new Position(nextPosition.Q + xOffset, nextPosition.R + yOffset);
            steps++;
        }
        return this;
    }
}
