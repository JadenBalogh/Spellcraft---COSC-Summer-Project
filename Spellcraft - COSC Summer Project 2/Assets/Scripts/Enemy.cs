using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[HideInInspector]
    public Vector3 gridPosition;

    private GameObject player;

	void Start ()
    {
        GameController.instance.AddEnemy(this);
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	void Update ()
    {
		
	}
    
    public void Move()
    {
        Vector3 playerPos = player.GetComponent<Player>().gridPosition;
        int xDist = (int)Mathf.Abs(playerPos.x - gridPosition.x);
        int yDist = (int)Mathf.Abs(playerPos.y - gridPosition.y);
        Vector3 translation = Vector3.zero;
        if (xDist > yDist)
        {
            translation.x = playerPos.x > gridPosition.x ? 1 : -1;
        }
        else
        {
            translation.y = playerPos.y > gridPosition.y ? 1 : -1;
        }
        gridPosition += translation;

        GameController.instance.gridController.SetScreenPosition(gameObject, gridPosition);
    }
}
