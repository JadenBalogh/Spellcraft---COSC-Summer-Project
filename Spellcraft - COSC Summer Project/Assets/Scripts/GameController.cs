using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameController instance = null;
    public GameObject player;
    public GameObject[] enemies;

    private GridController gridController;

	// Ensure there is only one instance of GameController
	void Awake ()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        gridController = GetComponent<GridController>();
    }

    void Update()
    {

    }

    public void beginCombat()
    {
        GameObject.Find("MainMenu").SetActive(false);
        gridController.createGrid();
        gridController.spawnPlayer(player);
        gridController.spawnEnemies(enemies);
    }
}
