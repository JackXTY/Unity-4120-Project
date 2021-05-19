using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item item;
    public Image item_image;
    public string item_name;
    public GameObject item_detail_panel;

    

    public void InitializeItem()
    {
        item_image.sprite = item.sprite;
    }

    public void OpenItemPanel()
    {
        GameObject temp = Instantiate(item_detail_panel, transform.parent);
        temp.GetComponent<ItemPanel>().item = item;
        temp.GetComponent<ItemPanel>().InitializeItemPanel();
        InterfaceController.Instance.NewItemWindow(temp);

    }
}
