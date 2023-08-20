using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Buttons : MonoBehaviour
{
    [SerializeField] private Deck _deck;
    [SerializeField] private Button _startButton;

    public void StartButton()
    {
        _deck.GameObject().SetActive(true);
        _startButton.gameObject.SetActive(false);
        Console.WriteLine("Game started");
    }
}
