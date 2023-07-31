using System;
using System.Collections.Generic;

public class PieceMovedEventArgs : EventArgs
{
    public PieceView Piece { get; }
    public Position FromPosition { get; }
    public Position ToPosition { get; }

    public PieceMovedEventArgs(PieceView piece, Position fromPosition, Position toPosition)
    {
        Piece = piece;
        FromPosition = fromPosition;
        ToPosition = toPosition;
    }
}

public class PieceTakenEventArgs : EventArgs
{
    public PieceView Piece { get; }
    public Position FromPosition { get; }

    public PieceTakenEventArgs(PieceView piece, Position fromPosition)
    {
        Piece = piece;
        FromPosition = fromPosition;
    }
}

public class PiecePlacedEventArgs : EventArgs
{
    public PieceView Piece { get; }
    public Position ToPosition { get; }

    public PiecePlacedEventArgs(PieceView piece, Position fromPosition)
    {
        Piece = piece;
        ToPosition = fromPosition;
    }
}

public class Board
{
    public event EventHandler<PieceMovedEventArgs> PieceMoved;
    public event EventHandler<PieceTakenEventArgs> PieceTaken;
    public event EventHandler<PiecePlacedEventArgs> PiecePlaced;

    private Dictionary<Position, PieceView> _pieces = new Dictionary<Position, PieceView>();
    
        
    private readonly int _distance;
        
    public Board(int distance)
    {
        _distance = distance;

    }
    public bool TryGetPiece(Position position, out PieceView piece)
            => _pieces.TryGetValue(position, out piece);

    public bool TryGetPieceAt(Position position, out PieceView piece)
        => _pieces.TryGetValue(position, out piece);

    public bool IsValidPosition(Position position)
        => (_distance >= HexHelper.AxialDistance(new Position(0,0), position));

    //places new piece on position
    public bool Place(Position position, PieceView piece)
    {
        if (piece == null)
            return false;

        if (!IsValidPosition(position))
            return false;

        if (_pieces.ContainsKey(position))
            return false;

        if (_pieces.ContainsValue(piece))
            return false;

        OnPiecePlaced(new PiecePlacedEventArgs(piece, position));

        _pieces[position] = piece;

        return true;
    }

    //changes piece position
    public bool Move(Position fromPosition, Position toPosition)
    {
        if (!IsValidPosition(toPosition))
            return false;

        if (_pieces.ContainsKey(toPosition))
            return false;

        if (!_pieces.TryGetValue(fromPosition, out var piece))
            return false;
        
        _pieces.Remove(fromPosition);
        _pieces[toPosition] = piece;

        OnPieceMoved(new PieceMovedEventArgs(piece, fromPosition, toPosition));

        return true;
    }

    //removes piece from position
    public bool Take(Position fromPosition)
    {
        if (!IsValidPosition(fromPosition))
            return false;

        if (!_pieces.ContainsKey(fromPosition))
            return false;

        if (!_pieces.TryGetValue(fromPosition, out var piece))
            return false;

        _pieces.Remove(fromPosition);

        OnPieceTaken(new PieceTakenEventArgs(piece, fromPosition));

        return true;
    }

    protected virtual void OnPieceMoved(PieceMovedEventArgs eventArgs)
    {
        var handler = PieceMoved;
        handler?.Invoke(this, eventArgs);
    }

    protected virtual void OnPieceTaken(PieceTakenEventArgs eventArgs)
    {
        var handler = PieceTaken;
        handler?.Invoke(this, eventArgs);
    }

    private void OnPiecePlaced(PiecePlacedEventArgs eventArgs)
    {
        var handler = PiecePlaced;
        handler?.Invoke(this, eventArgs);
    }
}