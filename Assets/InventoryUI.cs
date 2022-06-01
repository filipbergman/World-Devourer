using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> UISlotParentList = new List<Transform>();
    private List<SlotPair> UISlotList = new List<SlotPair>();
    public GameObject backpack;
    public GameObject UISlotPrefab;

    private Transform oldParent;
    private Transform holdingObjectTransform;
    private InventorySlot holdingInventorySlot;
    private int oldIndex;
    private bool holdingItem = false;
    private bool replacedItem = false;

    private void Start()
    {
        foreach (Transform parent in UISlotParentList)
        {
            foreach (Transform child in parent)
            {
                UISlotList.Add(new SlotPair(child, null));
            }
        }
    }

    // Called when block is picked up from ground(ev. chests)
    internal void UpdateUI(InventorySlot[] inventory)
    {
        for (int i = 0; i < UISlotList.Count; i++)
        {
            if (UISlotList[i].itemTransform.childCount == 0 && inventory[i] != null)
            {
                GameObject imageAndTextObject = Instantiate(UISlotPrefab, UISlotList[i].itemTransform.transform);
                imageAndTextObject.GetComponentInChildren<RawImage>().texture = inventory[i].item.blockImage;
                imageAndTextObject.GetComponentInChildren<RawImage>().raycastTarget = false;
                UISlotList[i].itemTransform.GetComponentInChildren<Text>().text = inventory[i].amount.ToString();
                UISlotList[i].inventorySlot = inventory[i];
            }
            else if(inventory[i] != null)
            {
                UISlotList[i].inventorySlot = inventory[i];
                UISlotList[i].itemTransform.GetComponentInChildren<Text>().text = inventory[i].amount.ToString();
            }
        }
    }
    public void RemoveSlot(int index)
    {
        foreach (Transform child in UISlotList[index].itemTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public bool HoldingItem()
    {
        return holdingItem;
    }

    internal void DropOneItem(int currentItemIndex)
    {
        // Drop item on ground: // TODO: make sure it does not drop under the ground
        GameObject prefab = UISlotList[currentItemIndex].inventorySlot.item.itemPrefab;
        Transform itemPool = transform.Find("/GroundItems");
        Transform characterTransform = FindObjectOfType<InventoryHandler>().transform;
        Transform cameraTransform = FindObjectOfType<Camera>().transform;
        Vector3 spawnPosition = characterTransform.transform.position +
                                        (2 * cameraTransform.forward.normalized) +
                                        new Vector3(0, 2, 0);
        
        GameObject dropBlock = Instantiate(prefab, itemPool);
        dropBlock.transform.position = spawnPosition;
        dropBlock.GetComponent<Rigidbody>().AddForce(300 * cameraTransform.forward.normalized);
    }

    public void DropHoldingItem(int currentItemIndex)
    {
        Destroy(holdingObjectTransform.gameObject);
        holdingInventorySlot = null;
        holdingItem = false;
        // Drop item on ground: // TODO: make sure it does not drop under the ground
        GameObject prefab = UISlotList[currentItemIndex].inventorySlot.item.itemPrefab;
        Transform itemPool = transform.Find("/GroundItems");
        Transform characterTransform = FindObjectOfType<InventoryHandler>().transform;
        Transform cameraTransform = FindObjectOfType<Camera>().transform;
        Vector3 spawnPosition = characterTransform.transform.position +
                                        (2 * cameraTransform.forward.normalized) +
                                        new Vector3(0, 2, 0);
        for (int i = 0; i < UISlotList[currentItemIndex].inventorySlot.amount; i++)
        {
            GameObject dropBlock = Instantiate(prefab, itemPool);
            dropBlock.transform.position = spawnPosition;
            dropBlock.GetComponent<Rigidbody>().AddForce(300 * cameraTransform.forward.normalized);
        }
    }

    public void SetCurrentSlot(int index)
    {
        UISlotList[index].itemTransform.GetComponent<RawImage>().color = new Color32(120, 120, 120, 255);
        for (int i = 0; i < UISlotList.Count; i++)
        {
            if (i != index)
                UISlotList[i].itemTransform.GetComponent<RawImage>().color = new Color32(90, 90, 90, 255);
        }
    }

    internal void ToggleInventory()
    {
        backpack.SetActive(!backpack.activeSelf);
        if(holdingItem)
        {
            holdingObjectTransform.SetParent(oldParent);
            holdingObjectTransform.localPosition = Vector3.zero;
            holdingObjectTransform.SetAsFirstSibling();
            holdingItem = false;
            holdingObjectTransform = null;
        }
    }

    public void HandleSlotClick(Transform slotTransform)
    {
        if(slotTransform.childCount > 0)
        {
            if (holdingItem) // Replace item being held
            {
                StopAllCoroutines();
                Destroy(holdingObjectTransform.gameObject);
                
                // Picking up new item from slot
                Transform newItemTransform = slotTransform.GetChild(0).transform;
                holdingObjectTransform = newItemTransform;
                newItemTransform.SetParent(transform);
                StartCoroutine("ItemFollowMouse", newItemTransform);
                InventorySlot tempSlot = UISlotList[int.Parse(slotTransform.name) - 1].inventorySlot;

                // Placing old item in slot
                UpdateUIInventory(slotTransform);

                // Getting block from clicked slot.
                oldIndex = int.Parse(slotTransform.name) - 1;
                holdingInventorySlot = tempSlot;
                replacedItem = true;
            }
            else // holding no item: Clicked on slot with item
            {
                holdingItem = true;
                oldParent = slotTransform;
                Transform itemTransform = slotTransform.GetChild(0).transform;
                holdingObjectTransform = itemTransform;
                itemTransform.SetParent(transform);
                StartCoroutine("ItemFollowMouse", itemTransform);
                oldIndex = int.Parse(slotTransform.name) - 1;
                holdingInventorySlot = UISlotList[oldIndex].inventorySlot;
            }
        } 
        else if(holdingItem) // holding item: Clicked on slot with no item
        {
            Destroy(holdingObjectTransform.gameObject);
            holdingObjectTransform = null;
            holdingItem = false;
            UpdateUIInventory(slotTransform);
            replacedItem = false;
        }
        
    }

    private void UpdateUIInventory(Transform slotTransform)
    {
        int newIndex = int.Parse(slotTransform.name) - 1;
        FindObjectOfType<InventoryHandler>().UpdateInventory(holdingInventorySlot, oldIndex, newIndex, !replacedItem);
        holdingInventorySlot = null;
    }

    IEnumerator ItemFollowMouse(Transform itemTransform)
    {
        while(backpack.activeSelf && holdingItem)
        {
            itemTransform.position = Input.mousePosition; 
            yield return null;
        }
    }
}

public class SlotPair
{
    public Transform itemTransform { get; set; }
    public InventorySlot inventorySlot { get; set; }

    public SlotPair(Transform itemTransform, InventorySlot inventorySlot)
    {
        this.itemTransform = itemTransform;
        this.inventorySlot = inventorySlot;
    }
}