using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private static int _deckSize = 12;
    [SerializeField] private int _handSize = 5;
    [SerializeField] private GameObject[] _cards;
    [SerializeField] private GameObject[] _cardPrefabs;

    public void SetupCards(Engine engine)
    {
        //generate deck of random cards
        for (int i = 0; i < _deckSize; i++)
        {
            GameObject card = Instantiate(_cardPrefabs[Random.Range(0, _cardPrefabs.Length)], transform);
            card.transform.gameObject.SetActive(false);
            card.GetComponent<Card>().GameEngine = engine;
            _cards[i] = card;
        }
        Debug.Log("Deck Generated");
        DeckUpdate();
    }

    public void DeckUpdate()
    {
        List<GameObject> tmp = new List<GameObject>(_cards);
       
        Vector3 startPosition = GetStartPosition(tmp, transform.position);

        for (int i = 0; i < tmp.Count; i++)
        {
            GameObject card = tmp[i];

            if (card.GetComponent<Card>().IsPlayed)
            {
                tmp.RemoveAt(i);
                card.SetActive(false);
            }
        }

        int handSize;
        if (tmp.Count >= 5)
            handSize = _handSize;
        else
            handSize = tmp.Count;

        //Can be replaced with Horizontal Layout Group
        for (int i = 0; i < handSize; i++)
        {
            GameObject card = tmp[i];
            card.SetActive(true);
            card.transform.position = startPosition;

            startPosition += new Vector3(240, 0);
        }
        _cards = tmp.ToArray();
    }

    //card placement
    private Vector3 GetStartPosition(List<GameObject> tmp, Vector3 startPosition)
    {
        if (tmp.Count >= 5)
            startPosition = transform.position + new Vector3(-480, 0, 0);
        else if (tmp.Count == 4)
            startPosition = transform.position + new Vector3(-360, 0, 0);
        else if (tmp.Count == 3)
            startPosition = transform.position + new Vector3(-240, 0, 0);
        else if (tmp.Count == 2)
            startPosition = transform.position + new Vector3(-120, 0, 0);
        else if (tmp.Count == 1)
            startPosition = transform.position;

        return startPosition;
    }
}
