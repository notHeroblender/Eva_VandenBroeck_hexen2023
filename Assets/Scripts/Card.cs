using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Engine GameEngine;
    private GameObject _copy;

    [SerializeField] private List<Position> _validPositions = new List<Position>();
    private List<List<Position>> _validPostionGroups = new List<List<Position>>();

    [SerializeField] private CardType _type;
    public CardType Type => _type;
    public bool IsPlayed = false;


    //when holding a card, make a copy of it and move that to where the mouse is
    public void OnBeginDrag(PointerEventData eventData)
    {
        _copy = Instantiate(transform.gameObject, transform.parent);

        _validPositions = new List<Position>();
        _validPostionGroups = new List<List<Position>>();

        if (CardType.Move == Type)
            _validPositions = GameEngine.GetValidPositions(Type);   
        else if(CardType.Shoot == Type || CardType.Slash == Type || CardType.ShockWave == Type )
        {
            _validPostionGroups = GameEngine.GetValidPositionsGroups(Type);
            ValidGroupsToValidPositions();
        }
        else if (CardType.Meteor == Type) // Handle Meteor card type
        {
            _validPositions = GameEngine.GetValidPositions(Type);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        _copy.transform.position = eventData.position;

        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100) && hit.collider.tag == "Tile")
        {
            PositionView positionView = hit.transform.gameObject.GetComponent<PositionView>();

            GameEngine.SetHighlights(positionView.HexPosition, Type, _validPositions, _validPostionGroups);
        }
        else
            GameEngine.SetActiveTiles(new List<Position>());
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100) && hit.collider.tag == "Tile")
        {
            PositionView positionView = hit.transform.gameObject.GetComponent<PositionView>();
            Destroy(_copy);
            IsPlayed = true;
            positionView.OnPointerClick(eventData);
        }
        else
            Destroy(_copy);

        GameEngine.SetActiveTiles(new List<Position>());
    }

    private void ValidGroupsToValidPositions()
    {
        foreach (List<Position> positions in _validPostionGroups)
        {
            foreach (Position position in positions)
            {
                _validPositions.Add(position);
            }
        }
    }
}