using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Transform> UISlotParentList = new List<Transform>();
    private List<GameObject> UISlotList = new List<GameObject>();
    public GameObject UIItemPrefab;
    public GameObject amountTextPrefab;
    public GameObject backpack;

    private void Start()
    {
        foreach (Transform parent in UISlotParentList)
        {
            foreach (Transform child in parent)
            {
                UISlotList.Add(child.gameObject);
            }
        }
    }

    // Called when block is picked up from ground(ev. chests)
    internal void UpdateUI(InventorySlot[] inventory)
    {
        for (int i = 0; i < UISlotList.Count; i++)
        {
            if (UISlotList[i].transform.childCount == 0 && inventory[i] != null)
            {
                GameObject itemObject = Instantiate(UIItemPrefab, UISlotList[i].transform);
                Instantiate(amountTextPrefab, UISlotList[i].transform);
                itemObject.GetComponent<RawImage>().texture = inventory[i].item.blockImage;
                UISlotList[i].GetComponentInChildren<Text>().text = inventory[i].amount.ToString();
            }
            else if(inventory[i] != null)
            {
                UISlotList[i].GetComponentInChildren<Text>().text = inventory[i].amount.ToString();
            }
        }
    }
    public void RemoveSlot(int index)
    {
        foreach (Transform child in UISlotList[index].transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetCurrentSlot(int index)
    {
        UISlotList[index].GetComponent<RawImage>().color = new Color32(120, 120, 120, 255);
        for (int i = 0; i < UISlotList.Count; i++)
        {
            if (i != index)
                UISlotList[i].GetComponent<RawImage>().color = new Color32(90, 90, 90, 255);
        }
    }

    internal void ToggleInventory()
    {
        backpack.SetActive(!backpack.activeSelf);
    }
}
