using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public Transform groundItems;
    public SoundManager soundManager;
    public ItemHandler itemHandler;

    public int itemPickupDistance = 1;

    private static InventorySlot[] inventory;
    private static int invSize = 42;
    private int bottomInvSize = 10;
    private int currentItemIndex = 0;
    //private static InventorySlot[] craftingSlots;

    public InventoryUI inventoryUI;

    void Start()
    {
        groundItems = transform.Find("/GroundItems");
        soundManager = transform.Find("/SoundManager").GetComponent<SoundManager>();
        itemHandler = transform.Find("/ItemHandler").GetComponent<ItemHandler>();
        inventoryUI = transform.Find("/InventoryUI").GetComponent<InventoryUI>();
        inventory = new InventorySlot[invSize];
        inventoryUI.SetCurrentSlot(currentItemIndex);
    }

    void Update()
    {
        foreach (Transform itemTransform in groundItems)
        {
            if (Vector3.Distance(itemTransform.position, transform.position) < itemPickupDistance)
            {
                soundManager.PlayPopSound();
                // TODO: do this more efficiently?
                foreach (var item in itemHandler.itemList)
                {
                    if (itemTransform.name.StartsWith(item.itemPrefab.name))
                    {
                        Debug.Log("item: " + item);

                        // Check if item already exists, if not add a new item to inventory:
                        //if (inventoryUI.Contains(item) == false)
                        //{
                        inventoryUI.AddToInventory(item, 1);
                        //}
                    }
                }
                Destroy(itemTransform.gameObject);
            }
        }
    }
}
//    public void ShowCraftedItem(Item item)
//    {
//        inventory[invSize - 1] = new InventorySlot(item, 1);
//    }

//    internal void DropAllItems()
//    {
//        inventoryUI.DropHoldingItem(currentItemIndex);
//        inventory[currentItemIndex] = null;
//    }

//    internal void DropOneItem()
//    {
//        if(inventory[currentItemIndex] != null)
//        {
//            inventoryUI.DropOneItem(currentItemIndex);
//            ChangeInventorySlotAmount(-1);
//        }
//    }

//    public bool HoldingItem()
//    {
//        return inventoryUI.HoldingItem();
//    }

//    internal void ToggleInventory()
//    {
//        inventoryUI.ToggleInventory();
//    }

//    private void AddToInventory(InventorySlot invSlot)
//    {
//        for (int i = 0; i < inventory.Length; i++)
//        {
//            if(inventory[i] == null)
//            {
//                inventory[i] = invSlot;
//                return;
//            }
//        }
//    }

//    public BlockType GetCurrentBlock()
//    {
//        if(inventory[currentItemIndex] != null)
//            return inventory[currentItemIndex].item.blockType;
//        return BlockType.Nothing;
//    }

//    public void ChangeInventorySlotAmount(int amount)
//    {
//        if(inventory[currentItemIndex].amount + amount == 0)
//        {
//            inventory[currentItemIndex] = null;
//            inventoryUI.RemoveSlot(currentItemIndex);
//            return;
//        }
//        inventory[currentItemIndex] = new InventorySlot(inventory[currentItemIndex].item, inventory[currentItemIndex].amount + amount);
//        inventoryUI.UpdateUI(inventory);
//    }

//    private bool AddItemIfItemExists(Item newItem)
//    {
//        int index = 0;
//        foreach (var slot in inventory)
//        {
//            if(slot != null && slot.item == newItem)
//            {
//                inventory[index] = new InventorySlot(inventory[index].item, inventory[index].amount + 1);
//                return true;
//            }
//            index++;
//        }
//        return false;
//    }

//    public void SetCurrentItem(int index)
//    {
//        if (index <= inventory.Length)
//        {
//            currentItemIndex = index;
//            inventoryUI.SetCurrentSlot(currentItemIndex);
//        }  
//    }
//    public void ScrollWheelChangeCurrentItem(float val)
//    {
//        if (val < 0)
//        {
//            if (--currentItemIndex == -1)
//                currentItemIndex = bottomInvSize - 1;
//        } else
//        {
//            if (++currentItemIndex == bottomInvSize)
//                currentItemIndex = 0;
//        }
//        inventoryUI.SetCurrentSlot(currentItemIndex);
//    }

//    public void UpdateInventory(InventorySlot inventorySlot, int oldIndex, int newIndex, bool removeOld)
//    {
//        //Debug.Log("Blocktype " + inventorySlot.item.blockType + " added to inventory slot " + newIndex);
//        if(removeOld == true)
//        {
//            Debug.Log("Removing blocktype " + inventory[oldIndex]?.item.blockType + " at slot " + oldIndex);
//            inventory[oldIndex] = null;
//        }
//        inventory[newIndex] = inventorySlot;
//        inventoryUI.UpdateUI(inventory);
//    }

//}

//public class InventorySlot
//{
//    //public int slotIndex;
//    public Item item;
//    public int amount { get; set; }

//    public InventorySlot(Item item, int amount)
//    {
//        this.item = item;
//        this.amount = amount;
//    }
//}