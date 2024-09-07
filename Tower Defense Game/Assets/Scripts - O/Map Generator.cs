using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject MapTile;
    public Sprite PathTileSprite;
    private GameObject currentTile;
    private bool reachedX ;
    private bool reachedY ;  
    private int currentIndex;
    private int nextIndex;

    [SerializeField] private int mapWidth; //map width
    [SerializeField] private int mapHeight; //map height

    public static List<GameObject> mapTiles = new List<GameObject>(); //list of tiles on map
    public static List<GameObject> pathTiles = new List<GameObject>(); //list of path tiles
    public static GameObject startTile;
    public static GameObject endTile;
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
        for (int i = 0; i < 3; i++)
        {
            reachedX = false;
            reachedY = false;

     
            int randTop = UnityEngine.Random.Range(0, mapWidth);  //random number to choose a tile at the top of the map
            int ranMove = UnityEngine.Random.Range(1, mapHeight - 1);

            startTile = topTiles[randTop];

            currentTile = startTile;

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
                    spriteRenderer.sprite = PathTileSprite;  // Set the path sprite
                }
            }
        }
    }

}




