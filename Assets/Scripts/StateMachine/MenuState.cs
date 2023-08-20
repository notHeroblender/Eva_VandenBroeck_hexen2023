using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuState : State
{
    private Buttons _menuButton;
    public override void OnEnter()
    {
        base.OnEnter();
        var op = SceneManager.LoadSceneAsync(States.Menu, LoadSceneMode.Additive);
        op.completed += InitializeScene;
    }
    public override void OnExit()
    {
        base.OnExit();
        if (_menuButton != null)
            _menuButton.StartClicked -= OnStartClicked;
        SceneManager.UnloadSceneAsync(States.Menu);
    }
    private void InitializeScene(AsyncOperation operation)
    {
        _menuButton = FindObjectOfType<Buttons>();
        if (_menuButton != null)
            _menuButton.StartClicked += OnStartClicked;
    }
    private void OnStartClicked(object sender, EventArgs e)
    {
        StateMachine.ChangeTo(States.Game);
    }
}
