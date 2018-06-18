using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;
    public float turnDelay;
    public float moveDelay;
    [HideInInspector]
    public GridController gridController;
    [HideInInspector]
    public bool playersTurn = true;

    private bool enemiesMoving = false;
    private List<Enemy> enemies;

    // Ensure there is only one instance of GameController
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        gridController = GetComponent<GridController>();
        enemies = new List<Enemy>();
        playersTurn = true;
    }

    void Update()
    {
        if (playersTurn || enemiesMoving)
            return;
        
        StartCoroutine(MoveEnemies());
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        Debug.Log("Enemies moving.");
        yield return new WaitForSeconds(turnDelay);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Move();
            yield return new WaitForSeconds(moveDelay);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    // Check which scene was loaded and initialize it
    public void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetSceneByBuildIndex(level).name.Equals("Combat"))
        {
            gridController.LoadCombatUI();
            gridController.CreateGrid();
            gridController.SpawnPlayer(playerPrefab);
            gridController.SpawnEnemies(enemyPrefabs);
        }
    }
    
    public void LoadCombatScene()
    {
        SceneManager.LoadScene("Combat");
    }
}
