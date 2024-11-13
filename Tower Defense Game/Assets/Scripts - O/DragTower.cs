using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragTower : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // List of tower prefabs for each upgrade level
    public List<GameObject> towerPrefabs;

    // Costs for each upgrade level (for upgrading, not placing)
    public List<int> upgradeCosts;

    public int placementCost; // Cost to place the tower (same for all levels)

    private GameObject draggedTower; // The instance of the tower being dragged
    private Camera mainCamera;       // Reference to the main camera

    private int currentUpgradeLevel = 0; // Starts at base level (0)
    private int maxUpgradeLevel;

    void Start()
    {
        mainCamera = Camera.main;
        maxUpgradeLevel = towerPrefabs.Count - 1;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Instantiate the dragged tower based on the current upgrade level
        if (towerPrefabs != null && towerPrefabs.Count > 0)
        {
            draggedTower = Instantiate(towerPrefabs[currentUpgradeLevel]);
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

            if (IsValidPlacement(mousePosition, out Vector3 snapPosition))
            {
                if (Enemy.totalReward >= placementCost)
                {
                    draggedTower.transform.position = snapPosition;
                    Towers.towers.Add(draggedTower);
                    Enemy.totalReward -= placementCost; // Deduct the placement cost
                }
                else
                {
                    // Not enough resources to place the tower
                    Destroy(draggedTower);
                }
            }
            else
            {
                // Invalid placement
                Destroy(draggedTower);
            }

            draggedTower = null; // Reset the dragged tower
        }
    }

    private Vector3 GetWorldPositionFromMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10; // Adjust this to match your game's depth (camera's Z position)
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private bool IsValidPlacement(Vector3 position, out Vector3 snapPosition)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("TowerTile"))
        {
            snapPosition = hit.collider.transform.position; // Center of the tile
            return true;
        }

        snapPosition = Vector3.zero; // Default value if not valid
        return false;
    }

    // Method to handle the upgrade button click
    public void UpgradeTower()
    {
       
            int upgradeCost = upgradeCosts[currentUpgradeLevel]; // Cost for the next upgrade level

            if (Enemy.totalReward >= upgradeCost)
            {
                Enemy.totalReward -= upgradeCost; // Deduct the upgrade cost
                currentUpgradeLevel++; // Increase the upgrade level

                // Update the UI as needed (you mentioned you handle UI changes separately)
            }
            else
            {
                UnityEngine.Debug.Log("Not enough resources to upgrade the tower.");
                // Optionally, provide feedback to the player (e.g., display a message)
            }
        
        
    }
}
