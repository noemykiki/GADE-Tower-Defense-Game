using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{

    public GameObject basicTowerObject;
    private GameObject dummyPlacement;
    public Camera cam;

    public Vector2 getMousePosition()
    {
        return cam.ScreenToWorldPoint(Input.mousePosition);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
