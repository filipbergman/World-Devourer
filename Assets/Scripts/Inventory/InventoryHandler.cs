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
                        Debug.Log("FOUND: " + item.id);
                        InventorySlot invSlot = new InventorySlot(false, item);
                        inventory.Add(invSlot);
                    }
                }
                Destroy(itemTransform.gameObject);
            }
        }
    }

    public BlockType GetCurrentBlock()
    {
        if(inventory.Count > 0)
        {
            return inventory[0].item.blockType;
        }
        return BlockType.Air;
    }

}

struct InventorySlot
{
    public bool free;
    public Item item;
    public int amount;

    public InventorySlot(bool free, Item item)
    {
        this.free = free;
        this.item = item;
        amount = 1;
    }
}