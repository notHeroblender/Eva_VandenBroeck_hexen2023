using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

public class Engine
{
    private List<Position> _selectedPositions = new List<Position>();
    public List<Position> SelectedPositions => _selectedPositions;

    private Board _board;
    private PieceView _player;
    private Deck _deck;
    private PieceView[] _pieces;
    private BoardView _boardView;
    private CommandQueue _commandQueue;

    public Engine(Board board, BoardView boardView, PieceView player, Deck deck, PieceView[] pieces, CommandQueue commandQueue)
    {
        _board = board;
        _player = player;
        _deck = deck;
        _pieces = pieces;
        _boardView = boardView;
        _commandQueue = commandQueue;
    }
    
    public void CardLogic(Position position)
    {
        var cards = _deck.GetComponentsInChildren<Card>();
        foreach (Card card in cards)
        {
            if (card.IsPlayed)
            {
                //replay: 
                var cardIndex = _deck._cards.IndexOf(card.gameObject);

                if (card.Type == CardType.Move)
                {
                    //card.IsPlayed = _board.Move(PositionHelper.WorldToHexPosition(_player.WorldPosition), position);

                    //replay: execute/redo
                    var playerPos = PositionHelper.WorldToHexPosition(_player.WorldPosition);
                    _commandQueue.ReturnCommands();
                    Action execute = () =>
                    {
                        card.IsPlayed = _board.Move(playerPos, position);
                        card.IsPlayed = true;
                        UnityEngine.Debug.Log("Eva: move executed");
                        _deck.DeckUpdate(); //too much deckupdate?
                    };
                    //replay: undo
                    Action undo = () =>
                    {
                        _board.Move(position, playerPos);
                        card.IsPlayed = false;
                        _deck.ReturnCard(card, cardIndex);
                        UnityEngine.Debug.Log("Eva: move undone");
                    };
                    var command = new DelegateCommand(execute, undo);
                    _commandQueue.Execute(command);
                }
                else if (!_selectedPositions.Contains(position))
                {
                    card.IsPlayed = false;
                    return;
                }
                else if (card.Type == CardType.Slash)
                {
                    //replay: execute/redo
                    List<PieceView> takenPieces = new();
                    _commandQueue.ReturnCommands();
                    Action execute = () =>
                    {
                        foreach (Position pos in _selectedPositions)
                        {
                            UnityEngine.Debug.Log("Eva: slash started");
                            //replay: 
                            foreach (var piece in _pieces)
                            {
                                var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                                {
                                    takenPieces.Add(piece);
                                    UnityEngine.Debug.Log("Eva: pieces added");
                                }
                            }

                            _board.Take(pos);
                            UnityEngine.Debug.Log("Eva: slash executed");
                        }
                        //replay: 
                        _deck.DeckUpdate();
                    };
                    //replay: undo
                    Action undo = () =>
                    {
                        foreach (var piece in takenPieces)
                        {
                            var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                            _board.Place(piecePos, piece);
                            //
                            bool wasActive = piece.gameObject.activeSelf;
                            piece.gameObject.SetActive(wasActive);
                            //
                            //piece.gameObject.SetActive(true);
                            UnityEngine.Debug.Log("Eva: slash undone");
                        }
                        card.IsPlayed = false;
                        _deck.ReturnCard(card, cardIndex);
                    };
                    var command = new DelegateCommand(execute, undo);
                    _commandQueue.Execute(command);
                }
                else if (card.Type == CardType.Shoot)
                {
                    //
                    ExecuteShootCardLogic(card, cardIndex);
                    //
                    //replay: execute/redo
                    //List<PieceView> takenPieces = new();
                    //_commandQueue.ReturnCommands();
                    //foreach (var pos in _selectedPositions)
                    //{
                    //    UnityEngine.Debug.Log("Eva: selectedposition: " + pos);
                    //}
                    //Action execute = () =>
                    //{
                    //    foreach (Position pos in _selectedPositions)
                    //    {
                    //        foreach (var piece in _pieces)
                    //        {
                    //            var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                    //            if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                    //            {
                    //                takenPieces.Add(piece);
                    //                UnityEngine.Debug.Log("Eva: pieces added");
                    //            }
                    //        }
                    //        _board.Take(pos);
                    //        UnityEngine.Debug.Log("Eva: shoot executed");
                    //    }
                    //    _deck.DeckUpdate();
                    //};
                    //replay: undo
                    //Action undo = () =>
                    //{
                    //    foreach (var piece in takenPieces)
                    //    {
                    //        var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                    //        _board.Place(piecePos, piece);
                    //        //
                    //        bool wasActive = piece.gameObject.activeSelf;
                    //        piece.gameObject.SetActive(wasActive);
                    //        //
                    //        //piece.gameObject.SetActive(true);
                    //        UnityEngine.Debug.Log("Eva: shoot undone");
                    //    }
                    //    card.IsPlayed = false;
                    //    _deck.ReturnCard(card, cardIndex);
                    //};
                }
                else if (card.Type == CardType.ShockWave)
                {
                    //replay: execute/redo
                    List<PieceView> takenPieces = new();
                    List<PieceView> movedPieces = new();
                    List<Position> moveToPos = new();
                    _commandQueue.ReturnCommands();
                    Action execute = () =>
                    {
                        foreach (Position pos in _selectedPositions)
                        {
                            Position offset = HexHelper.AxialSubtract(pos, PositionHelper.WorldToHexPosition(_player.WorldPosition));
                            Position moveTo = HexHelper.AxialAdd(pos, offset);

                            if (_board.IsValidPosition(moveTo))
                            {
                                _board.Move(pos, moveTo);
                                foreach (var piece in _pieces)
                                {
                                    var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                    if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                                    {
                                        movedPieces.Add(piece);
                                        moveToPos.Add(moveTo);
                                        UnityEngine.Debug.Log("Eva: move to pieces added");
                                    }
                                }
                            }
                            else
                            {
                                foreach (var piece in _pieces)
                                {
                                    var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                    if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                                    {
                                        takenPieces.Add(piece);
                                        UnityEngine.Debug.Log("Eva: taken pieces added");
                                    }
                                }
                                _board.Take(pos);
                            }
                        }
                        _deck.DeckUpdate();
                    };
                    //replay: undo
                    Action undo = () =>
                    {
                        foreach (var piece in takenPieces)
                        {
                            if (piece != null)
                            {
                                var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                _board.Place(piecePos, piece);
                                piece.gameObject.SetActive(true);
                            }
                        }
                        foreach (var piece in movedPieces)
                        {
                            if (piece != null)
                            {
                                var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                                _board.Move(moveToPos[movedPieces.IndexOf(piece)], piecePos);
                                UnityEngine.Debug.Log("Eva: push undone");
                            }
                        }
                        card.IsPlayed = false;
                        _deck.ReturnCard(card, cardIndex);
                    };
                    var command = new DelegateCommand(execute, undo);
                    _commandQueue.Execute(command);
                }
            }
        }
        _deck.DeckUpdate();
    }

    //
    private void ExecuteShootCardLogic(Card card, int cardIndex)
    {
        var takenPieces = new List<PieceView>();
        var positionsToTake = new List<Position>();

        foreach (Position pos in _selectedPositions)
        {
            if (!_selectedPositions.Contains(pos))
                continue;

            foreach (var piece in _pieces)
            {
                var piecePos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                if (pos.Q == piecePos.Q && pos.R == piecePos.R && piece.gameObject.activeSelf)
                {
                    takenPieces.Add(piece);
                    positionsToTake.Add(pos);
                }
            }
        }
        // Execute logic using a single command
        _commandQueue.ReturnCommands();
        _commandQueue.Execute(new DelegateCommand(
            execute: () =>
            {
                foreach (Position posToTake in positionsToTake)
                {
                    _board.Take(posToTake);
                }
                _deck.DeckUpdate();
            },
            undo: () =>
            {
                for (int i = 0; i < positionsToTake.Count; i++)
                {
                    _board.Place(positionsToTake[i], takenPieces[i]);
                }
                card.IsPlayed = false;
                _deck.ReturnCard(card, cardIndex);
            }
        ));

        card.IsPlayed = true;
    }
    //

    public void SetHighlights(Position position, CardType type, List<Position> validPositions, List<List<Position>> validPositionGroups = null)
    {
        switch (type)
        {
            case CardType.Move:
                if (validPositions.Contains(position))
                {
                    List<Position> positions = new List<Position>();

                    positions.Add(position);
                    SetActiveTiles(positions);
                }
                break;

            case CardType.Shoot:
            case CardType.Slash:
            case CardType.ShockWave:
                if (!validPositions.Contains(position))
                {
                    SetActiveTiles(validPositions);
                }
                else
                {
                    foreach (List<Position> positions in validPositionGroups)
                    {
                        if (positions.Count == 0) continue;

                        if ((type == CardType.Shoot && positions.Contains(position)) || (type == CardType.Slash && positions[0] == position) || (type == CardType.ShockWave && positions[0] == position))
                        {
                            SetActiveTiles(positions);
                            _selectedPositions = positions;
                            break;
                        }
                    }
                }
                break;
            default:
                _selectedPositions = new List<Position>();
                break;
        }
    }
    public void SetActiveTiles(List<Position> positions)
    {
        _boardView.SetActivePosition = positions;
    }

    public List<Position> GetValidPositions(CardType card)
    {
        List<Position> positions = new List<Position>();

        if (card == CardType.Move)
        {
            foreach (var position in _boardView.TilePositions)
            {
                bool positionIsFree = true;

                foreach (var piece in _pieces)
                {
                    var pos = PositionHelper.WorldToHexPosition(piece.WorldPosition);
                    if (pos.Q == position.Q && pos.R == position.R && piece.gameObject.activeSelf)
                    {
                        positionIsFree = false;
                        break;
                    }
                }
                if (positionIsFree)
                {
                    positions.Add(position);
                }
            }
            return positions;
        }
        return null;
    }

    public List<List<Position>> GetValidPositionsGroups(CardType card)
    {
        if (card == CardType.Shoot)
        {
            return MoveSetCollection.GetValidTilesForShoot(_player, _board);
        }
        else if (card == CardType.Slash || card == CardType.ShockWave)
        {
            return MoveSetCollection.GetValidTilesForCones(_player, _board);
        }
        return null;
    }
}