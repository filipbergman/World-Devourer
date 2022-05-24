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

    void Start()
    {
        groundItems = transform.Find("/GroundItems");
        soundManager = transform.Find("/SoundManager").GetComponent<SoundManager>();
        itemHandler = transform.Find("/ItemHandler").GetComponent<ItemHandler>();
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
                            InventorySlot invSlot = new InventorySlot(false, item, 1);
                            inventory.Add(invSlot);
                        }
                    }
                }
                Destroy(itemTransform.gameObject);
            }
        }

        // TODO: update inventory UI after list change?
    }

    public BlockType GetCurrentBlock()
    {
        if(inventory.Count > 0)
        {
            BlockType block = inventory[0].item.blockType;
            ChangeInventorySlotAmount(0, -1);
            return block;
        }
        return BlockType.Nothing;
    }

    private void ChangeInventorySlotAmount(int slotIndex, int amount)
    {
        if(inventory[slotIndex].amount + amount == 0)
        {
            inventory.Remove(inventory[slotIndex]);
            //inventory[slotIndex] = new InventorySlot(true, null, 0);
            return;
        }
        inventory[slotIndex] = new InventorySlot(false, inventory[slotIndex].item, inventory[slotIndex].amount + amount);
        Debug.Log("Amount after change: " + inventory[slotIndex].amount);
    }

    private bool AddItemIfItemExists(Item newItem)
    {
        int index = 0;
        foreach (var slot in inventory)
        {
            if(slot.item == newItem)
            {
                inventory[index] = new InventorySlot(false, inventory[index].item, inventory[index].amount + 1);
                Debug.Log("Added amount to same slot, new amount: " + inventory[index].amount);
                return true;
            }
            index++;
        }
        return false;
    }

}

struct InventorySlot
{
    public bool free;
    public Item item;
    public int amount { get; set; }

    public InventorySlot(bool free, Item item, int amount)
    {
        this.free = free;
        this.item = item;
        this.amount = amount;
    }


}