using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject MapTile;
    public Sprite PathTileSprite;
    public GameObject TowerTile;
    private GameObject currentTile;
    public GameObject Castle;
    public Sprite TowerTileSprite;
    private bool reachedX;
    private bool reachedY;
    private int currentIndex;
    private int nextIndex;
    public List<GameObject> castlePrefabs; // List of castle prefabs for each upgrade level
    public List<int> upgradeCosts; // List of upgrade costs for each level
    private int currentUpgradeLevel = 0; // Track current upgrade level

    [SerializeField] private int mapWidth; //map width
    [SerializeField] private int mapHeight; //map height

    public static List<GameObject> mapTiles = new List<GameObject>(); //list of tiles on map
    public static List<GameObject> pathTiles = new List<GameObject>(); //list of path tiles
    public static List<GameObject> towerTiles = new List<GameObject>(); //list of tower tiles
    public static GameObject[] startTile = new GameObject[3];
    public static GameObject endTile;

    private GameObject currentCastle; // Reference to the instantiated castle
    // Start is called before the first frame update
    void Start()
    {
        generateMap();
    }


    private List<GameObject> getTopTiles() //adds the top row of tiles to a list
    {
        List<GameObject> edgeTiles = new List<GameObject>();

        for (int i = mapWidth * (mapHeight - 1); i < mapWidth * mapHeight; i++)
        {
            edgeTiles.Add(mapTiles[i]);
        }

        return edgeTiles;
    }

    private List<GameObject> getBottomTiles() //adds bottom row of tiles to a list
    {
        List<GameObject> edgeTiles = new List<GameObject>();
        for (int i = 0; i < mapWidth; i++)
        {
            edgeTiles.Add(mapTiles[i]);
        }
        return edgeTiles;
    }


    private void moveDown()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex - mapWidth;
        currentTile = mapTiles[nextIndex];
    }

    private void moveLeft()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex - 1;
        currentTile = mapTiles[nextIndex];
    }

    private void moveRight()
    {
        pathTiles.Add(currentTile);
        currentIndex = mapTiles.IndexOf(currentTile);
        nextIndex = currentIndex + 1;
        currentTile = mapTiles[nextIndex];
    }

    private void generateMap() // generates map
   
        {
        mapTiles.Clear();
        pathTiles.Clear();
        towerTiles.Clear();

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                GameObject newTile = Instantiate(MapTile);
                mapTiles.Add(newTile);
                newTile.transform.position = new Vector2(x, y);
            }
        }

        List<GameObject> topTiles = getTopTiles();     //list of the tiles at the top of the map
        List<GameObject> bottomTiles = getBottomTiles(); // list of tiles at the bottom of the map
       

        int ranBottom = UnityEngine.Random.Range(0, mapWidth); //random number to choose a tile at the bottom of the map


        endTile = bottomTiles[ranBottom];
        currentCastle = Instantiate(Castle, endTile.transform.position, Quaternion.identity); // Store reference to the castle
        for (int i = 0; i < 3; i++)
        {
            reachedX = false;
            reachedY = false;


            int randTop = UnityEngine.Random.Range(0, mapWidth); //random number to choose a tile at the top of the map
            int ranMove = UnityEngine.Random.Range(1, mapHeight - 1);

            startTile[i] = topTiles[randTop];

            currentTile = startTile[i];

            for (int j = 0; j < ranMove; j++)
            {
                moveDown();
            }


            int loop = 0;
            while (!reachedX)
            {
                loop++;
                if (loop > 1000)
                {
                    break;
                }

                if (currentTile.transform.position.x > endTile.transform.position.x)
                {
                    moveLeft();
                }
                else if (currentTile.transform.position.x < endTile.transform.position.x)
                {
                    moveRight();
                }
                else
                {
                    reachedX = true;
                }
            }

            while (!reachedY)
            {
                if (currentTile.transform.position.y > endTile.transform.position.y)
                {
                    moveDown();
                }
                else
                {
                    reachedY = true;
                }
            }

            pathTiles.Add(endTile);
            foreach (GameObject obj in pathTiles)
            {
                SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = PathTileSprite; // Set the path sprite
                }
            }
           
        }
        
        AddTowerTiles();
    }

    private void AddTowerTiles()
    {
        int count = 0;
        int maxAttempts = 100; // Maximum number of attempts to find a valid position per placement

        while (count < 7)
        {
            bool spotFound = false;
            int attempts = 0;

            while (!spotFound && attempts < maxAttempts)
            {
                // Select a random path tile
                GameObject pathTile = pathTiles[UnityEngine.Random.Range(0, pathTiles.Count)];
                Vector2 position = pathTile.transform.position;

                // Define possible adjacent positions
                Vector2[] potentialPositions = {
                new Vector2(position.x + 1, position.y),
                new Vector2(position.x - 1, position.y),
                new Vector2(position.x, position.y + 1),
                new Vector2(position.x, position.y - 1)
            };

                foreach (Vector2 potentialPosition in potentialPositions)
                {
                    // Check if the position is within bounds and not a top tile or path tile
                    if (IsValidTowerPosition(potentialPosition))
                    {
                        GameObject towerTile = Instantiate(TowerTile);
                        towerTile.transform.position = potentialPosition;
                        SpriteRenderer spriteRenderer = towerTile.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.sprite = TowerTileSprite;  // Set the new tile sprite
                        }
                        towerTiles.Add(towerTile);
                        count++;
                        spotFound = true;
                        break; // Exit the adjacent positions loop
                    }
                }

                attempts++;

                // Log a warning if max attempts are reached without finding a valid position
                if (attempts >= maxAttempts)
                {
                    UnityEngine.Debug.LogWarning("Max attempts reached. Unable to find a spot for a new tower tile.");
                }
            }

            // Log a message if we couldn't place a tower tile after max attempts
            if (count < 5 && attempts >= maxAttempts)
            {
                UnityEngine.Debug.LogWarning("Not enough valid spots found for tower tiles. Still trying...");
            }
        }
    }

    // Helper method to check if a position is a valid tower placement
    private bool IsValidTowerPosition(Vector2 position)
    {
        // Ensure the position is within the map bounds
        if (position.x < 0 || position.x >= mapWidth || position.y < 0 || position.y >= mapHeight)
        {
            return false;
        }

        // Ensure the position is not on a top tile
        if (getTopTiles().Exists(tile => (Vector2)tile.transform.position == position))
        {
            return false;
        }

        // Ensure the position is not already occupied by a path tile or another tower tile
        if (pathTiles.Exists(tile => (Vector2)tile.transform.position == position) ||
            towerTiles.Exists(tile => (Vector2)tile.transform.position == position))
        {
            return false;
        }

        return true;
    }
    public void UpgradeCastle()
    {
        if (currentCastle != null && castlePrefabs != null && castlePrefabs.Count > currentUpgradeLevel + 1)
        {
            int nextUpgradeLevel = currentUpgradeLevel + 1;
            int upgradeCost = upgradeCosts[nextUpgradeLevel];

            if (Enemy.totalReward >= upgradeCost)
            {
                Enemy.totalReward -= upgradeCost;

                Vector3 position = currentCastle.transform.position;
                Quaternion rotation = currentCastle.transform.rotation;

                Destroy(currentCastle);

                currentCastle = Instantiate(castlePrefabs[nextUpgradeLevel], position, rotation);
                currentUpgradeLevel = nextUpgradeLevel;
            }
            else
            {
                UnityEngine.Debug.Log("Not enough resources to upgrade the castle.");
            }
        }
        else
        {
            UnityEngine.Debug.Log("Castle is already at maximum upgrade level or prefabs are not properly assigned.");
        }
    }



}