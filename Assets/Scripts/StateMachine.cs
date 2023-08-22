using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<string, State> _states = new Dictionary<string, State>();
    private string _currentStateName;
    public string InitialState
    {
        set
        {
            _currentStateName = value;
            CurrentState.OnEnter();
        }
    }
    public State CurrentState => _states[_currentStateName];
    public void Register(string stateName, State state)
    {
        state.StateMachine = this;
        _states.Add(stateName, state);
    }
    public void ChangeTo(string stateName)
    {
        CurrentState.OnExit();
        _currentStateName = stateName;
        CurrentState.OnEnter();
    }
}
