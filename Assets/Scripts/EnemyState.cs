using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class EnemyState : State
{
    public static Position PlayerSpawn = new Position(0, 0);

    public static List<Position> ValidPositions = new List<Position>();
    public override void OnEnter()
    {
        base.OnEnter();

        EnemyMoves();
        StateMachine.ChangeTo(States.Player);
    }

    private void EnemyMoves()
    {
        GameObject[] entities = GameObject.FindGameObjectsWithTag("Enemy");
        var positionViews = FindObjectsOfType<PositionView>();

        foreach (PositionView positionView in positionViews)
        {
            //add all pos except the player spawn to the list
            if (positionView.HexPosition.Q != PlayerSpawn.Q && positionView.HexPosition.R != PlayerSpawn.R)
                ValidPositions.Add(positionView.HexPosition);
        }

        //change enemy pos to new random pos and remove pos from list
        for (int i = 0; i < entities.Count(); i++)
        {
            int random = Random.Range(0, ValidPositions.Count);
            Position position = ValidPositions[random];
            entities[i].gameObject.transform.position = PositionHelper.HexToWorldPosition(position);
            ValidPositions.RemoveAt(random);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
