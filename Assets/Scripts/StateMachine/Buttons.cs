using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    public EventHandler StartClicked;
    [SerializeField] private Button _startButton;

    public void Play() => OnStartClicked(EventArgs.Empty);
    public void OnStartClicked(EventArgs e)
    {
        var handler = StartClicked;
        handler?.Invoke(this, e);
        //SceneManager.LoadSceneAsync(States.Game, LoadSceneMode.Additive);
        _startButton.gameObject.SetActive(false);
    }
}
