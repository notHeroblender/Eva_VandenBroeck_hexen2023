using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : State
{
    private GameObject _enemy;
    private Board _board;
    private Deck _deck;
    private BoardView _boardView;
    private Engine _engine;
    private PieceView[] _pieces;

    public PlayerState(GameObject enemy, Board board, Deck deck, BoardView boardView, Engine engine, PieceView[] pieces)
    {
        _enemy = enemy;
        _board = board;
        _deck = deck;
        _boardView = boardView;
        _engine = engine;
        _pieces = pieces;
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnExit()
    {
        base.OnExit();
    }
    void Start()
    {
        var piecesViews = FindObjectsOfType<PieceView>();

        PieceView player = null;
        foreach (var pieceView in piecesViews)
            if (pieceView.Player == Player.Player1)
            {
                player = pieceView;
                break;
            }

        var boardView = FindObjectOfType<BoardView>();
        boardView.PositionClicked += OnPositionClicked;
        _boardView = boardView;

        _engine = new Engine(_board, _boardView, player, _deck, _pieces);

        _deck.SetupCards(_engine);
    }
    private void OnPositionClicked(object sender, PositionEventArgs e)
    {
        _engine.CardLogic(e.Position);
        StateMachine.ChangeTo(States.Enemy);
    }
}
