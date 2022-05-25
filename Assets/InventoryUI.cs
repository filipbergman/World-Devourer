using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<GameObject> UISlotList = new List<GameObject>();



    internal void UpdateUI(List<InventorySlot> inventory)
    {
        for (int i = 0; i < UISlotList.Count; i++)
        {
            // TODO: use image istead?
            //GameObject icon = inventory[i].item.itemPrefab;
            //icon.GetComponent<BoxCollider>().enabled = false;
            //icon.GetComponent<Rigidbody>().useGravity = false;
            //icon.transform.rotation = new Quaternion(0, 90, 0, 0);
            //icon.transform.localScale = new Vector3(icon.transform.localScale.x, 80, 80);
            //Instantiate(inventory[i].item.itemPrefab, UISlotList[i].transform);
        }
    }
}
