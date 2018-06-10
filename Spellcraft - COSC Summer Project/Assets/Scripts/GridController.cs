using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {

    [Serializable]
    public class Range {
        public int minimum;
        public int maximum;

        public Range(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int rows = 8;
    public int columns = 8;
    public GameObject backgroundPrefab;
    public Sprite[] backgroundSprites;
    public GameObject borderPrefab;
    public Sprite[] borderSprites;
    public GameObject obstaclePrefab;
    public Sprite[] obstacleSprites;
    public Range obstacleCount;

    private Transform gridHolder;
    private List<Vector3> availableGridPositions = new List<Vector3>();
    private List<Vector3> enemySpawnPositions = new List<Vector3>();

    void initializeBackground()
    {
        gridHolder = new GameObject("Grid").transform;

        // Initialize all background and border tiles
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    GameObject tile = Instantiate(borderPrefab, new Vector3(x, y, 0f), Quaternion.identity, gridHolder) as GameObject;
                    tile.GetComponent<SpriteRenderer>().sprite = borderSprites[UnityEngine.Random.Range(0, borderSprites.Length)];
                }
                else
                {
                    GameObject tile = Instantiate(backgroundPrefab, new Vector3(x, y, 0f), Quaternion.identity, gridHolder) as GameObject;
                    tile.GetComponent<SpriteRenderer>().sprite = backgroundSprites[UnityEngine.Random.Range(0, backgroundSprites.Length)];
                }
            }
        }
    }

    void initializeGridPositions()
    {
        availableGridPositions.Clear();

        // Initialize the list of all possible environment spawn positions
        for (int x = 0; x < columns; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                availableGridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void initializeEnemySpawnPositions()
    {
        enemySpawnPositions.Clear();

        // Initialize the list of all possible enemy spawn positions
        for (int x = 1; x < columns; x++)
        {
            enemySpawnPositions.Add(new Vector3(x, rows - 1, 0f));
        }
    }

    void centerCamera()
    {
        Camera.main.orthographicSize = (rows / 2.0f) + 1;
        Camera.main.transform.position = new Vector3((columns / 2.0f) - 0.5f, (rows / 2.0f) - 0.5f, -10f);
    }

    Vector3 getRandomGridPosition()
    {
        int randIndex = UnityEngine.Random.Range(0, availableGridPositions.Count);
        Vector3 position = availableGridPositions[randIndex];
        availableGridPositions.RemoveAt(randIndex);
        return position;
    }
    
    Vector3 getRandomEnemySpawnPosition()
    {
        int randIndex = UnityEngine.Random.Range(0, enemySpawnPositions.Count);
        Vector3 position = enemySpawnPositions[randIndex];
        enemySpawnPositions.RemoveAt(randIndex);
        return position;
    }

    void placeObjectsAtRandom(GameObject prefab, Sprite[] sprites, Range count)
    {
        int numObjects = UnityEngine.Random.Range(count.minimum, count.maximum);

        // Place a random selection of objects on the grid anywhere but the top and bottom rows
        for (int i = 0; i < numObjects; i++)
        {
            Vector3 randPosition = getRandomGridPosition();
            GameObject tile = Instantiate(prefab, randPosition, Quaternion.identity, gridHolder) as GameObject;
            tile.GetComponent<SpriteRenderer>().sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        }
    }

    void placeEnemiesAtRandom(GameObject[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 spawnPosition = getRandomEnemySpawnPosition();
            GameObject enemy = Instantiate(enemies[i], spawnPosition, Quaternion.identity) as GameObject;
        }
    }

    public void createGrid()
    {
        centerCamera();
        initializeBackground();
        initializeGridPositions();
        placeObjectsAtRandom(obstaclePrefab, obstacleSprites, obstacleCount);
    }

    public void spawnEnemies(GameObject[] enemies)
    {
        initializeEnemySpawnPositions();
        placeEnemiesAtRandom(enemies);
    }

    public void spawnPlayer(GameObject player)
    {
        GameObject playerInstance = Instantiate(player, new Vector3(columns/2, 0f, 0f), Quaternion.identity) as GameObject;
    }
}
