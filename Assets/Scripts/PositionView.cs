using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PositionView : MonoBehaviour, IPointerClickHandler
{
    private BoardView _parent;
    public Position HexPosition => PositionHelper.WorldToHexPosition(transform.position);

    [SerializeField] private UnityEvent OnActivate;
    [SerializeField] private UnityEvent OnDeActivate;

    private void Start()
    {
        _parent = GetComponentInParent<BoardView>();
    }
     
    public void OnPointerClick(PointerEventData eventData)
    {
        _parent.ChildClicked(this);
    }

    internal void DeActivate()
    {
        OnDeActivate?.Invoke();
    }

    internal void Activate()
    {
        OnActivate?.Invoke();
    }
    internal void Remove()
    {
        this.gameObject.SetActive(false);
    }
}
