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
        foreach (Transform child in UISlotList[index].itemTransform.transform)
        {
            Destroy(child.gameObject);
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
        Debug.Log("CLICK: " + slotTransform);
        if(slotTransform.childCount > 0)
        {
            if (holdingItem) // Replace item being held
            {
                // TODO:

                UpdateUIInventory(slotTransform);
            } else
            {
                holdingItem = true;
                oldParent = slotTransform;

                Transform itemTransform = slotTransform.GetChild(0).transform;
                holdingObjectTransform = itemTransform;
                itemTransform.SetParent(transform);
                for (int i = 0; i < UISlotList.Count; i++)
                {
                    if (UISlotList[i].itemTransform == slotTransform)
                    {
                        oldIndex = i;
                        holdingInventorySlot = UISlotList[i].inventorySlot;

                    }
                }
                StartCoroutine("ItemFollowMouse", itemTransform);
            }
            
        } 
        else if(holdingItem) 
        {
            // Put item in slotTransform
            Destroy(holdingObjectTransform.gameObject);
            holdingObjectTransform = null;
            holdingItem = false;
            UpdateUIInventory(slotTransform);
        }
        
    }

    private void UpdateUIInventory(Transform slotTransform)
    {
        for (int i = 0; i < UISlotList.Count; i++)
        {
            if(slotTransform == UISlotList[i].itemTransform)
            {
                int newIndex = i;
                FindObjectOfType<InventoryHandler>().UpdateInventory(holdingInventorySlot, oldIndex, newIndex);
                holdingInventorySlot = null;
            }
        }
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