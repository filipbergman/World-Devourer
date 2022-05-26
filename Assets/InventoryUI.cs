using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<GameObject> UISlotList = new List<GameObject>();
    public GameObject UIItemPrefab;
    public GameObject amountTextPrefab;



    // Called when block is picked up from ground(ev. chests)
    internal void UpdateUI(List<InventorySlot> inventory)
    {
        for (int i = 0; i < UISlotList.Count; i++)
        {
            if (UISlotList[i].transform.childCount == 0 && inventory.Count > i)
            {
                GameObject itemObject = Instantiate(UIItemPrefab, UISlotList[i].transform);
                Instantiate(amountTextPrefab, UISlotList[i].transform);
                itemObject.GetComponent<RawImage>().texture = inventory[i].item.blockImage;
                UISlotList[i].GetComponentInChildren<Text>().text = inventory[i].amount.ToString();
            } else if(inventory.Count > i)
            {
                UISlotList[i].GetComponentInChildren<Text>().text = inventory[i].amount.ToString();
            }
        }
    }
    public void RemoveSlot(List<InventorySlot> inventory, int index)
    {
        foreach (Transform child in UISlotList[index].transform)
        {
            Destroy(child.gameObject);
        }
    }
}
