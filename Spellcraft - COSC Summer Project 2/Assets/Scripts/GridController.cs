using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    [Serializable]
    public class Range
    {
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
    public GameObject combatUIPrefab;
    public GameObject debugPrefab;
    public GameObject backgroundPrefab;
    public Sprite[] backgroundSprites;
    public GameObject borderPrefab;
    public Sprite[] borderSprites;
    public GameObject obstaclePrefab;
    public Sprite[] obstacleSprites;
    public Range obstacleCount;

    private GameObject combatUI;
    private float tileScale;
    private Vector3 tileOffset;
    private RectTransform gridHolder;
    private List<Vector3> availableGridPositions = new List<Vector3>();
    private List<Vector3> enemySpawnPositions = new List<Vector3>();

    // Initialize the parent object of the grid tiles and calculate the proper UI scaling
    void InitializeGridHolder()
    {
        gridHolder = (RectTransform) combatUI.transform.Find("GridPanel");

        // Scale UI with screen dimensions and center the grid in its window 
        float hScale = combatUI.GetComponent<RectTransform>().rect.width * (gridHolder.anchorMax.x - gridHolder.anchorMin.x) / (columns + 2);
        float vScale = combatUI.GetComponent<RectTransform>().rect.height * (gridHolder.anchorMax.y - gridHolder.anchorMin.y) / (rows + 2);
        // Scale to the smaller of vertical or horizontal dimensions
        tileScale = hScale > vScale ? vScale : hScale;
        tileOffset = new Vector3(tileScale * ((columns / 2.0f) - 0.5f), tileScale * ((rows / 2.0f) - 0.5f), 0f);
    }

    // Initialize all background and border tiles
    void InitializeBackground()
    {
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                // Border tiles
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    GameObject tile = Instantiate(borderPrefab, Vector3.zero, Quaternion.identity, gridHolder) as GameObject;
                    SetScreenPosition(tile, new Vector3(x, y, 0f));
                    tile.GetComponent<RectTransform>().sizeDelta = new Vector2(tileScale, tileScale);
                    tile.GetComponent<Image>().sprite = borderSprites[UnityEngine.Random.Range(0, borderSprites.Length)];
                }
                // Background tiles
                else
                {
                    GameObject tile = Instantiate(backgroundPrefab, Vector3.zero, Quaternion.identity, gridHolder) as GameObject;
                    SetScreenPosition(tile, new Vector3(x, y, 0f));
                    tile.GetComponent<RectTransform>().sizeDelta = new Vector2(tileScale, tileScale);
                    tile.GetComponent<Image>().sprite = backgroundSprites[UnityEngine.Random.Range(0, backgroundSprites.Length)];

                    // Debug tile numbering
                    GameObject debug = Instantiate(debugPrefab, Vector3.zero, Quaternion.identity, gridHolder) as GameObject;
                    SetScreenPosition(debug, new Vector3(x, y, 0f));
                    debug.GetComponent<Text>().text = "[" + x + "," + y + "]";
                }
            }
        }
    }

    // Initialize the list of all possible grid object spawn positions
    void InitializeGridPositions()
    {
        availableGridPositions.Clear();

        for (int x = 0; x < columns; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                availableGridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    // Initialize the list of all possible enemy spawn positions
    void InitializeEnemySpawnPositions()
    {
        enemySpawnPositions.Clear();

        for (int x = 0; x < columns; x++)
        {
            enemySpawnPositions.Add(new Vector3(x, rows - 1, 0f));
            Debug.Log(rows - 1);
        }
    }
    
    Vector3 GetRandomGridPosition()
    {
        int randIndex = UnityEngine.Random.Range(0, availableGridPositions.Count);
        Vector3 position = availableGridPositions[randIndex];
        availableGridPositions.RemoveAt(randIndex);
        return position;
    }
    
    Vector3 GetRandomEnemySpawnPosition()
    {
        int randIndex = UnityEngine.Random.Range(0, enemySpawnPositions.Count);
        Vector3 position = enemySpawnPositions[randIndex];
        enemySpawnPositions.RemoveAt(randIndex);
        return position;
    }

    // Place a random selection of objects on the grid anywhere but the top and bottom rows
    void PlaceObjectsAtRandom(GameObject prefab, Sprite[] sprites, Range count)
    {
        int numObjects = UnityEngine.Random.Range(count.minimum, count.maximum);

        for (int i = 0; i < numObjects; i++)
        {
            Vector3 randPosition = GetRandomGridPosition();
            GameObject tile = Instantiate(prefab, Vector3.zero, Quaternion.identity, gridHolder) as GameObject;
            SetScreenPosition(tile, randPosition);
            tile.GetComponent<RectTransform>().sizeDelta = new Vector2(tileScale, tileScale);
            tile.GetComponent<Image>().sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        }
    }

    // Place enemies at random locations along the top row of the grid
    void PlaceEnemiesAtRandom(GameObject[] enemies)
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 spawnPosition = GetRandomEnemySpawnPosition();
            GameObject enemy = Instantiate(enemies[i], Vector3.zero, Quaternion.identity, gridHolder) as GameObject;
            enemy.GetComponent<Enemy>().gridPosition = spawnPosition;
            SetScreenPosition(enemy, spawnPosition);
            enemy.GetComponent<RectTransform>().sizeDelta = new Vector2(tileScale, tileScale);
        }
    }

    // Scales a grid position based on screen dimensions and the offset of the grid in the UI
    Vector3 ScaleToScreenSize(Vector3 gridPosition)
    {
        Vector3 result = Vector3.zero;
        result.x = gridPosition.x * tileScale;
        result.y = gridPosition.y * tileScale;
        result.z = gridPosition.z;
        result -= tileOffset;
        return result;
    }

    public void SetScreenPosition(GameObject target, Vector3 gridPosition)
    {
        target.GetComponent<RectTransform>().anchoredPosition = ScaleToScreenSize(gridPosition);
    }

    public void LoadCombatUI()
    {
        combatUI = Instantiate(combatUIPrefab, Vector3.zero, Quaternion.identity) as GameObject;
    }

    // Initialize all static parts of the grid
    public void CreateGrid()
    {
        InitializeGridHolder();
        InitializeBackground();
        InitializeGridPositions();
        PlaceObjectsAtRandom(obstaclePrefab, obstacleSprites, obstacleCount);
    }

    public void SpawnEnemies(GameObject[] enemies)
    {
        InitializeEnemySpawnPositions();
        PlaceEnemiesAtRandom(enemies);
    }
    
    public void SpawnPlayer(GameObject player)
    {
        GameObject playerInstance = Instantiate(player, Vector3.zero, Quaternion.identity, gridHolder) as GameObject;
        Vector3 gridPosition = new Vector3(columns / 2, 0f, 0f);
        playerInstance.GetComponent<Player>().gridPosition = gridPosition;
        SetScreenPosition(playerInstance, gridPosition);
        playerInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(tileScale, tileScale);
    }
}
