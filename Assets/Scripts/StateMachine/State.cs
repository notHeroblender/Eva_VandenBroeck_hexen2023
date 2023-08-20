using Unity.VisualScripting;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public StateMachine StateMachine { get; set; }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }

}
