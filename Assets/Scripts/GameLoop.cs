using System;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    private Board _board;
    private Deck _deck;
    private BoardView _boardView;
    private Engine _engine;
    private PieceView[] _pieces;

    private Action _cardLogicCompletedCallback;

    private StateMachine _stateMachine;

    void Start()
    {
        SpawnHelper.SpawnEnemies(_enemy, 8);

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

        _stateMachine = gameObject.AddComponent<StateMachine>();
        _stateMachine.Register(States.Enemy, gameObject.AddComponent<EnemyState>());
        _stateMachine.Register(States.Player, new PlayerState(_enemy, _board, _deck, _boardView, _engine, _pieces, _cardLogicCompletedCallback));
        _stateMachine.InitialState = States.Player;
    }

    private void OnPositionClicked(object sender, PositionEventArgs e)
    {
        _engine.CardLogic(e.Position);
    }
}
