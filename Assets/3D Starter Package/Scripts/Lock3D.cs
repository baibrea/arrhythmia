// Unity Starter Package - Version 1
// University of Florida's Digital Worlds Institute
// Written by Logan Kemper

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using System.Collections;

namespace DigitalWorlds.StarterPackage3D
{
    /// <summary>
    /// Add to a GameObject with a trigger collider to create a lock that can only be unlocked if the player has the requisite item(s) in their inventory.
    /// </summary>
    public class Lock3D : MonoBehaviour
    {
        [Header("Lock Settings")]
        [Tooltip("Enter the tag name that should register collisions.")]
        [SerializeField] private string tagName;

        [Tooltip("The name of the item that the lock requires.")]
        [SerializeField] private string requiredItemName;

        [Tooltip("The quantity of the item required.")]
        [SerializeField] private int requiredItemCount;

        [Tooltip("Whether a button press should be required to unlock the lock. If false, it will check automatically on the trigger collision.")]
        [SerializeField] private bool requireButtonPress;

        // keyToPress now uses new Input System
        [Tooltip("The key input that the script is listening for.")]
        [SerializeField] private Key keyToPress = Key.F;

        [Tooltip("Whether the items required for the lock should be deleted when unlocking.")]
        [SerializeField] private bool deleteItemsWhenUsed;

        // Grid info so the grid can be updated when the door is unlocked
        [Header("Grid Information")]
        [SerializeField] private int gridX;
        [SerializeField] private int gridY;
        [SerializeField] private GridManager gridManager;

        // Info to check which side the player is on when trying to unlock the door, and only allow unlocking from that side

        [Header("Door Settings")]

        [SerializeField] private Facing playerEntersFrom;
        [SerializeField] private Transform doorTransform;

        [Space(20)]
        [Header("Events")]
        [SerializeField] private UnityEvent onUnlocked, onUnlockFailed;

        private bool isUnlocked = false;
        public enum Facing { North, South, East, West };
        private Inventory inventory;

        // Start() registers the door in the gridManager's doors dictionary
        private void Start()
        {
            if (gridManager == null)
            {
                Debug.LogError("Grid Manager reference not set on " + gameObject.name);
            }
            gridManager.RegisterDoor(gridX, gridY, this);
        }

        // Update() checks if the corresponding key has been pressed to unlock the door
        // If a button press is required and the player's inventory has been assigned to inventory
        private void Update()
        {
            if (Keyboard.current[keyToPress].wasPressedThisFrame && requireButtonPress && inventory != null)
            {
                CheckUnlock();
            }
        }

        // OnTriggerEnter() runs when the GameObject with a player tag interacts with the door
        // It assigns the player's inventory to inventory, and calls CheckUnlock() if no button press is required
        private void OnTriggerEnter(Collider other)
        {
            if (!string.IsNullOrEmpty(tagName) && other.CompareTag(tagName))
            {
                if (other.gameObject.TryGetComponent(out Inventory inv))
                {
                    inventory = inv;

                    if (!requireButtonPress)
                    {
                        CheckUnlock();
                    }
                }
            }
        }

        // OnTriggerExit() unassigns the player's inventory when they exit the door trigger
        private void OnTriggerExit(Collider other)
        {
            if (!string.IsNullOrEmpty(tagName) && other.CompareTag(tagName))
            {
                if (other.gameObject.TryGetComponent(out Inventory inv) && inv == inventory)
                {
                    inventory = null;
                }
            }
        }

        // CheckUnlock() checks if the required item(s) are in the inventory, then handle the unlocking
        private void CheckUnlock()
        {
            int count = inventory.GetItemCount(requiredItemName);
            if (count >= requiredItemCount)
            {
                if (deleteItemsWhenUsed)
                {
                    inventory.DeleteItemFromInventory(requiredItemName, requiredItemCount);
                }
                UnlockDoor();
                onUnlocked.Invoke();
            }
            else
            {
                onUnlockFailed.Invoke();
            }
        }

        // UnlockDoor() updates the grid to make the tile traversable, 
        // disables the collider on the door, and rotates the door
        private void UnlockDoor()
        {
            if (isUnlocked) return;
            isUnlocked = true;
            gridManager.gridObject.SetSpace(gridX, gridY, "floor");
            GetComponent<Collider>().enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
            if (doorTransform != null && playerEntersFrom != Facing.East)
            {
                doorTransform.localRotation = Quaternion.Euler(0, -90f, 0);
            }
            else if (doorTransform != null)
            {
                doorTransform.localRotation = Quaternion.Euler(0, 90f, 0);
            }

            
            // StartCoroutine(RotateDoor());
        }

        // RotateDoor() smoothly rotates the door open over a short duration
        // FIXME: Door currently collides with player during rotation, causing it to get hit and float away.
        private IEnumerator RotateDoor()
        {

            float duration = 0.1f;
            float elapsed = 0f;

            Quaternion start = transform.rotation;
            Quaternion end = start * Quaternion.Euler(0, -90f, 0);

            while (elapsed < duration)
            {
                transform.rotation = Quaternion.Slerp(start, end, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.rotation = end;
        }

        // TryUnlock() is a simple unlock method that can be called to try unlocking the door, such as GridManager.cs
        public void TryUnlock(Inventory inv, Vector2 playerDirection)
        {
            if (isUnlocked || inv == null || !isCorrectSide(playerDirection))
            {
                return;
            }
            inventory = inv;
            CheckUnlock();
        }

        // Checks if the player is on the right side of the door to unlock it, according to the playerEntersFrom variable
        private bool isCorrectSide(Vector2 playerDirection)
        {
            switch (playerEntersFrom)
            {
                case Facing.North:
                    return playerDirection.y < 0;
                case Facing.South:
                    return playerDirection.y > 0;
                case Facing.East:
                    return playerDirection.x < 0;
                case Facing.West:
                    return playerDirection.x > 0;
                default:
                    return false;
            }
        }
    }
}