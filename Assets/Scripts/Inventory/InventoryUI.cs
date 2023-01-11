using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> UISlotParentList = new List<Transform>();
    private List<InventorySlot> inventorySlots = new List<InventorySlot>();
    public GameObject backpack;
    public GameObject UISlotPrefab;

    public CraftingUI craftingUI;
    public Transform craftedTransform;

    private Transform holdingObjectTransform;
    private InventorySlot holdingInventorySlot;
    private int oldIndex;
    private bool holdingItem = false;

    private Transform cameraTransform;
    private Transform itemPool;

    private int bottomInvSize = 10;
    private int currentItemIndex = 0;

    private void Start()
    {
        cameraTransform = FindObjectOfType<Camera>().transform;
        itemPool = transform.Find("/GroundItems");

        foreach (Transform parent in UISlotParentList)
        {
            foreach (Transform child in parent)
            {
                inventorySlots.Add(new InventorySlot(child, null));
            }
        }
    }

    private void UpdateUI(Item item, int amount, int invIndex, bool destroyChild = true)
    {
        Debug.Log("UpdateUI: item; " + item.blockType + " amount: " + amount + " index: " + invIndex);
        
        if(amount == 0) 
        {
            InventorySlot tempSlot = new InventorySlot(inventorySlots[invIndex].itemTransform, item, inventorySlots[invIndex].amount);
            inventorySlots[invIndex].item = null;
            if (destroyChild)
                Destroy(inventorySlots[invIndex].itemTransform?.GetChild(0).gameObject);
            holdingInventorySlot = tempSlot;
            return;
        }
        inventorySlots[invIndex].item = item;
        inventorySlots[invIndex].amount = amount;

        if (inventorySlots[invIndex].itemTransform.childCount == 0) // Checks if an item is already in the slot
        {
            GameObject imageAndTextObject = Instantiate(UISlotPrefab, inventorySlots[invIndex].itemTransform);
            imageAndTextObject.GetComponentInChildren<RawImage>().texture = inventorySlots[invIndex].item.blockImage;
            inventorySlots[invIndex].itemTransform.GetComponentInChildren<Text>().text = inventorySlots[invIndex].amount.ToString();
        } else
        {
            inventorySlots[invIndex].itemTransform.GetComponentInChildren<Text>().text = inventorySlots[invIndex].amount.ToString();
        }
    }

    public void PlaceBlock()
    {
        UpdateUI(inventorySlots[currentItemIndex].item, inventorySlots[currentItemIndex].amount - 1, currentItemIndex);
    }

    public void HandleSlotClick(Transform slotTransform)
    {
        // TODO: Click multiple times on crafting slot to stack same item.
        if (slotTransform.childCount > 0)
        {
            oldIndex = int.Parse(slotTransform.name);
            if (holdingItem) // Replace item being held
            {
                StopAllCoroutines();
                Destroy(holdingObjectTransform.gameObject);

                // Adding more of the same item:
                if (holdingInventorySlot.item.id == inventorySlots[int.Parse(slotTransform.name)].item.id)
                {
                    UpdateUI(holdingInventorySlot.item,
                        holdingInventorySlot.amount + inventorySlots[int.Parse(slotTransform.name)].amount,
                        int.Parse(slotTransform.name));
                    holdingItem = false;
                }
                else
                {
                    // Picking up new item from slot
                    holdingObjectTransform = slotTransform.GetChild(0).transform;
                    holdingObjectTransform.SetParent(transform);
                    StartCoroutine("ItemFollowMouse", holdingObjectTransform);
                    InventorySlot tempSlot = new InventorySlot(slotTransform, inventorySlots[oldIndex].item, inventorySlots[oldIndex].amount);

                    // Placing old item in slot
                    UpdateUI(holdingInventorySlot.item, holdingInventorySlot.amount, int.Parse(slotTransform.name));
                    // Getting block from clicked slot.
                    holdingInventorySlot = tempSlot;
                }
            }
            else // holding no item: Clicked on slot with item
            {
                holdingItem = true;
                Transform itemTransform = slotTransform.GetChild(0).transform;
                holdingObjectTransform = itemTransform;
                itemTransform.SetParent(transform);
                StartCoroutine("ItemFollowMouse", itemTransform);

                holdingInventorySlot = inventorySlots[oldIndex];
                // Will remove data where the item was before:
                UpdateUI(holdingInventorySlot.item, 0, oldIndex, false);
            }
        } 
        else if(holdingItem) // holding item: Clicked on slot with no item
        {
            Destroy(holdingObjectTransform.gameObject);
            holdingObjectTransform = null;
            holdingItem = false;
            Debug.Log("Add item to slot: " + holdingInventorySlot.item.blockType);
            UpdateUI(holdingInventorySlot.item, holdingInventorySlot.amount, int.Parse(slotTransform.name));
        }
        CheckCraftingSlots();
    }

    private void CheckCraftingSlots()
    {
        foreach(InventorySlot slot in inventorySlots.GetRange(inventorySlots.Count - 5, 4))
        {
            if(slot.item != null)
            {
                craftingUI.CheckRecipes(inventorySlots.GetRange(inventorySlots.Count - 5, 4));
                return;
            }
        }
        
    }

    // Handles the actual crafting of a new item
    public void HandleCraftedClick(Transform slotTransform)
    {
        int index = inventorySlots.Count - 5;
        foreach (InventorySlot invSlot in inventorySlots.GetRange(inventorySlots.Count - 5, 4))
        {
            if (invSlot.item != null)
                UpdateUI(invSlot.item, invSlot.amount - 1, index);
            index++;
        }
        HandleSlotClick(slotTransform);
    }

    public bool HoldingItem()
    {
        return holdingItem;
    }

    internal void DropOneItem()
    {
        // Drop item on ground: // TODO: make sure it does not drop under the ground
        if (inventorySlots[currentItemIndex].item != null)
        {
            GameObject prefab = inventorySlots[currentItemIndex].item.itemPrefab;
            Transform characterTransform = FindObjectOfType<InventoryHandler>().transform;
            Vector3 spawnPosition = characterTransform.transform.position +
                                            (2 * cameraTransform.forward.normalized) +
                                            new Vector3(0, 2, 0);

            ThrowItem(prefab, spawnPosition);
            UpdateUI(inventorySlots[currentItemIndex].item, inventorySlots[currentItemIndex].amount - 1, currentItemIndex);
        }
    }

    public void DropHoldingItem()
    {
        // Drop item on ground: // TODO: make sure it does not drop under the ground
        GameObject prefab = holdingInventorySlot.item.itemPrefab;
        Destroy(holdingObjectTransform.gameObject);
        
        holdingItem = false;
        Transform characterTransform = FindObjectOfType<InventoryHandler>().transform;
        Vector3 spawnPosition = characterTransform.transform.position +
                                        (2 * cameraTransform.forward.normalized) +
                                        new Vector3(0, 2, 0);
        for (int i = 0; i < holdingInventorySlot.amount; i++)
        {
            ThrowItem(prefab, spawnPosition);
        }
    }

    private void ThrowItem(GameObject prefab, Vector3 spawnPosition)
    {
        GameObject dropBlock = Instantiate(prefab, itemPool);
        dropBlock.transform.position = spawnPosition;
        dropBlock.GetComponent<Rigidbody>().AddForce(300 * cameraTransform.forward.normalized);
    }

    public void SetCurrentSlot(int index)
    {
        //Debug.Log("SetCurrentSlot, Index: " + index);
        inventorySlots[index].itemTransform.GetComponent<RawImage>().color = new Color32(120, 120, 120, 255);
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i != index)
                inventorySlots[i].itemTransform.GetComponent<RawImage>().color = new Color32(90, 90, 90, 255);
        }
    }

    internal void ToggleInventory()
    {
        Debug.Log("ToggleInventory");
        backpack.SetActive(!backpack.activeSelf);
    }

    public void SpawnCraftedItem(BlockType blockType)
    {
        GameObject imageAndTextObject = Instantiate(UISlotPrefab, craftedTransform);
        Item item = FindObjectOfType<ItemHandler>().BlockTypeToItem(blockType);
        imageAndTextObject.GetComponentInChildren<RawImage>().texture = item.blockImage;
        imageAndTextObject.GetComponentInChildren<Text>().text = "1";
        UpdateUI(item, 1, inventorySlots.Count - 1);
    }

    public void DespawnCraftedItem()
    {
        if(inventorySlots[inventorySlots.Count - 1].item != null)
            UpdateUI(inventorySlots[inventorySlots.Count - 1].item, 0, inventorySlots.Count - 1);
    }

    IEnumerator ItemFollowMouse(Transform itemTransform)
    {
        while(backpack.activeSelf && holdingItem)
        {
            itemTransform.position = Input.mousePosition; 
            yield return null;
        }
    }

    // ----------------------------------------------------------------------------
    // InventoryHandler
    // ----------------------------------------------------------------------------
    public void AddToInventory(Item item, int amount)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if(inventorySlots[i].item != null && inventorySlots[i].item.id == item.id)
            {
                //Debug.Log("old: " + inventorySlots[i].item.id + " new: " + item.id);
                UpdateUI(item, inventorySlots[i].amount + amount, i);
                return;
            }
        }
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].item == null)
            {
                UpdateUI(item, amount, i);
                return;
            }
        }
    }

    public BlockType GetCurrentBlock()
    {
        if (inventorySlots[currentItemIndex].item != null)
            return inventorySlots[currentItemIndex].item.blockType;
        return BlockType.Nothing;
    }

    public void SetCurrentItem(int index)
    {
        if (index <= inventorySlots.Count)
        {
            currentItemIndex = index;
            SetCurrentSlot(currentItemIndex);
        }
    }

    public void ScrollWheelChangeCurrentItem(float val)
    {
        if (val < 0)
        {
            if (--currentItemIndex == -1)
                currentItemIndex = bottomInvSize - 1;
        }
        else
        {
            if (++currentItemIndex == bottomInvSize)
                currentItemIndex = 0;
        }
        SetCurrentSlot(currentItemIndex);
    }
}

public class InventorySlot
{
    public Transform itemTransform { get; set; }
    public Item item { get; set; }
    public int amount { get; set; }

    public InventorySlot(Transform itemTransform, Item item, int amount = 0)
    {
        this.itemTransform = itemTransform;
        this.item = item;
        this.amount = amount;
    }
}