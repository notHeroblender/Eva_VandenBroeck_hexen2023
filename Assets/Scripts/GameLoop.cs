using System;
using Unity.VisualScripting;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private StateMachine _stateMachine;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private int _enemyAmount = 8;

    void Start()
    {
        _stateMachine = gameObject.AddComponent<StateMachine>();
        _stateMachine.Register(States.Menu, gameObject.AddComponent<MenuState>());
        _stateMachine.Register(States.Game, new GameState(_enemy, _enemyAmount));
        _stateMachine.InitialState = States.Menu;
    }
}
