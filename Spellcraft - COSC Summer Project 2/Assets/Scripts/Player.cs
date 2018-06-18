using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int maxEnergy = 1;
    public int maxMana = 1;
    public int maxHealth = 1;
    [HideInInspector]
    public Vector3 gridPosition;

    private int energy;
    private int mana;
    private int health;

	void Start ()
    {

	}
	
	void Update ()
    {
        if (GameController.instance.playersTurn)
        {
            int h = (int)Input.GetAxisRaw("Horizontal");
            int v = (int)Input.GetAxisRaw("Vertical");

            if (h != 0 || v != 0)
            {
                Debug.Log("Input pressed!");
                Move(h, v);
            }
        }
	}

    void Move(int x, int y)
    {
        energy--;
        Vector3 translation = new Vector3(x, y, 0f);
        gridPosition += translation;
        GameController.instance.gridController.SetScreenPosition(gameObject, gridPosition);
        GameController.instance.playersTurn = false;
    }
}
