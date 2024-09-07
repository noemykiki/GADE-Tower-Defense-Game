using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject MapTile;
    private GameObject currentTile;
    GameObject startTile;
    GameObject endTile;
    private bool reachedX = false;
    private int currentIndex;
    private int nextIndex;

    [SerializeField] private int mapWidth; //map width
    [SerializeField] private int mapHeight; //map height

    private List<GameObject> mapTiles = new List<GameObject>(); //list of tiles on map
    private List<GameObject> pathTiles = new List<GameObject>(); //list of path tiles

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

    }

    private void moveLeft()
    {

    }

    private void moveRight()
    {

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

        int randTop = UnityEngine.Random.Range(0, mapWidth);  //random number to choose a tile at the top of the map
        int ranBottom = UnityEngine.Random.Range(0, mapWidth); //random number to choose a tile at the bottom of the map

        startTile = topTiles[randTop];
        endTile = bottomTiles[ranBottom];
        currentTile = startTile;
        while (!reachedX)
        {
            if (currentTile.transform.position.x > endTile.transform.position.x)
            {
                moveLeft();
            }
            else if (currentTile.transform.position.x < endTile.transform.position.x)
            {
                moveRight();
            }
        }

    }

}




