using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragTower : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject towerPrefab; // The tower prefab to be dragged
    private GameObject draggedTower; // The instance of the tower being dragged
    private Camera mainCamera; // Reference to the main camera
    private Vector3 snapPosition; // The position where the tower should snap

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Instantiate the dragged tower and set it as a child of the canvas
        if (towerPrefab != null)
        {
            draggedTower = Instantiate(towerPrefab);
            draggedTower.SetActive(true); // Ensure it's active
            draggedTower.transform.position = GetWorldPositionFromMouse();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedTower != null)
        {
            draggedTower.transform.position = GetWorldPositionFromMouse();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
       
            if (draggedTower != null)
            {
                Vector3 mousePosition = GetWorldPositionFromMouse();

                if (IsValidPlacement(mousePosition, out Vector3 snapPosition) && Enemy.totalReward>=50)
                {
                    draggedTower.transform.position = snapPosition;
                    Towers.towers.Add(draggedTower);
                    Enemy.totalReward = Enemy.totalReward - 50;// Add to the list after placing
            }
                else
                {
                    Destroy(draggedTower);
                }
            }
        
    }

    private Vector3 GetWorldPositionFromMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10; // Adjust this to match your game's depth (camera z position)
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }
    private bool IsValidPlacement(Vector3 position, out Vector3 snapPosition)
        {
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("TowerTile"))
                {
                    snapPosition = hit.collider.transform.position; // Center of the tile
                    return true;
                }
            }

            snapPosition = Vector3.zero; // Default value if not valid
            return false;
        }
}
