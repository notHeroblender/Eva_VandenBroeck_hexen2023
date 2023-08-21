using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    [SerializeField] private GameLoop _gameloop;
    public void UndoButton()
    {
        _gameloop._commandQueue.Previous();
    }

    public void RedoButton()
    {
        _gameloop._commandQueue.Next();
    }
}
