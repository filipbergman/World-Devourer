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
    private int currentItemIndex = 0;
    public InventoryUI inventoryUI;

    void Start()
    {
        groundItems = transform.Find("/GroundItems");
        soundManager = transform.Find("/SoundManager").GetComponent<SoundManager>();
        itemHandler = transform.Find("/ItemHandler").GetComponent<ItemHandler>();
        inventoryUI = transform.Find("/InventoryUI").GetComponent<InventoryUI>();
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
                        inventoryUI.AddToInventory(item, 1);
                    }
                }
                Destroy(itemTransform.gameObject);
            }
        }
    }
}
