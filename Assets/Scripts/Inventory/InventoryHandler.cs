using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    public Transform groundItems;
    public SoundManager soundManager;
    public ItemHandler itemHandler;

    public int itemPickupDistance = 2;

    private static List<InventorySlot> inventory;

    public InventoryUI inventoryUI;

    void Start()
    {
        groundItems = transform.Find("/GroundItems");
        soundManager = transform.Find("/SoundManager").GetComponent<SoundManager>();
        itemHandler = transform.Find("/ItemHandler").GetComponent<ItemHandler>();
        inventoryUI = transform.Find("/InventoryUI").GetComponent<InventoryUI>();
        inventory = new List<InventorySlot>();
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
                        // Check if item already exists, if not add a new item to inventory:
                        if(AddItemIfItemExists(item) == false)
                        {
                            InventorySlot invSlot = new InventorySlot(item, 1);
                            inventory.Add(invSlot);
                        }
                    }
                }
                Destroy(itemTransform.gameObject);
                inventoryUI.UpdateUI(inventory);
            }
        }
    }

    public BlockType GetCurrentBlock()
    {
        if(inventory.Count > 0)
        {
            //if(inventory[0].free == false)
            //{
            BlockType block = inventory[0].item.blockType;
            return block;
            //}
        }
        return BlockType.Nothing;
    }

    public void ChangeInventorySlotAmount(int slotIndex, int amount)
    {
        if(inventory[slotIndex].amount + amount == 0)
        {
            //inventory[slotIndex].free = true;
            inventory.Remove(inventory[slotIndex]);
            inventoryUI.RemoveSlot(inventory, slotIndex);
            return;
        }
        inventory[slotIndex] = new InventorySlot(inventory[slotIndex].item, inventory[slotIndex].amount + amount);
        inventoryUI.UpdateUI(inventory);
    }

    private bool AddItemIfItemExists(Item newItem)
    {
        int index = 0;
        foreach (var slot in inventory)
        {
            if(slot.item == newItem)
            {
                inventory[index] = new InventorySlot(inventory[index].item, inventory[index].amount + 1);
                return true;
            }
            index++;
        }
        return false;
    }

}

public class InventorySlot
{
    //public bool free { get; set; }
    public Item item;
    public int amount { get; set; }

    public InventorySlot(Item item, int amount)
    {
        //this.free = free;
        this.item = item;
        this.amount = amount;
    }
}