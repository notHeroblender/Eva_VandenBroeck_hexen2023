using System;
using System.Collections.Generic;
using UnityEngine;

public class PositionEventArgs : EventArgs
{
    public Position Position { get; }
    public PositionEventArgs(Position position)
    {
        Position = position;
    }
}

public class BoardView : MonoBehaviour
{
    public event EventHandler<PositionEventArgs> PositionClicked;

    private Dictionary<Position, PositionView> _positionViews = new Dictionary<Position, PositionView>();


    private List<Position> _activePosition = new List<Position>();

    private List<Position> _tilePositons = new List<Position>();
    public List<Position> TilePositions => _tilePositons;

    //updates which positions are active
    public List<Position> SetActivePosition
    {
        set
        {
            foreach (var position in _activePosition)
                _positionViews[position].DeActivate();

            if (value == null)
                _activePosition.Clear();
            else
                _activePosition = value;

            foreach (var position in value)
                _positionViews[position].Activate();
        }
    }

    //adds the pieces and tiles to their respective dictionaries
    private void OnEnable()
    {
        var positionViews = GetComponentsInChildren<PositionView>();
        foreach (var positionView in positionViews)
        {
            _positionViews.Add(positionView.HexPosition, positionView);
            _tilePositons.Add(positionView.HexPosition);
        }
    }

    internal void ChildClicked(PositionView positionView)
        => OnPositionClicked(new PositionEventArgs(positionView.HexPosition));

    protected virtual void OnPositionClicked(PositionEventArgs e)
    {
        var handler = PositionClicked;
        handler.Invoke(this, e);
    }
}