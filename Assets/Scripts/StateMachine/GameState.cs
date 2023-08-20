using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : State
{
    private GameObject _enemy;
    private int _enemyAmount;
    private Board _board;
    private Deck _deck;
    private BoardView _boardView;
    private Engine _engine;
    private PieceView[] _pieces;

    public GameState(GameObject enemy, int enemyAmount)
    {
        _enemy = enemy;
        _enemyAmount = enemyAmount;
    }

    public override  void OnEnter()
    {
        base.OnEnter();
        var op = SceneManager.LoadSceneAsync(States.Game, LoadSceneMode.Additive);
        op.completed += InitializeScene;
    }
    public override void OnExit()
    {
        base.OnExit();
        if (_boardView != null)
            _boardView.PositionClicked -= OnPositionClicked;
        SceneManager.UnloadSceneAsync(States.Game);
    }

    private void InitializeScene(AsyncOperation operation) //void start() from GameLoop
    {
        SpawnHelper.SpawnEnemies(_enemy, _enemyAmount);

        _deck = FindObjectOfType<Deck>();

        _board = new Board(PositionHelper.Distance);
        _board.PieceMoved += (s, e)
             => e.Piece.MoveTo(PositionHelper.HexToWorldPosition(e.ToPosition));

        _board.PieceTaken += (s, e)
            => e.Piece.Taken();

        _board.PiecePlaced += (s, e)
           => e.Piece.Placed(PositionHelper.HexToWorldPosition(e.ToPosition));

        var piecesViews = FindObjectsOfType<PieceView>();

        foreach (var pieceView in piecesViews)
            _board.Place(PositionHelper.WorldToHexPosition(pieceView.WorldPosition), pieceView);

        PieceView player = null;
        foreach (var pieceView in piecesViews)
            if (pieceView.Player == Player.Player1)
            {
                player = pieceView;
                break;
            }
        _pieces = piecesViews;

        var boardView = FindObjectOfType<BoardView>();
        boardView.PositionClicked += OnPositionClicked;
        _boardView = boardView;

        _engine = new Engine(_board, _boardView, player, _deck, _pieces);

        _deck.SetupCards(_engine);
        _deck.GameObject().SetActive(true);
    }

    private void OnPositionClicked(object sender, PositionEventArgs e)
    {
        _engine.CardLogic(e.Position);
    }
}
