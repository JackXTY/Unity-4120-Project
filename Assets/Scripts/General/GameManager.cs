using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Item> possessed_items;
    public List<int> item_count;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void UseItem(Item item)
    {
        switch (item.usage)
        {
            case "heal":
                InterfaceController.Instance.Heal((int) item.usage_value);
                ConsumeItem(item);
                break;
            case "restore":
                InterfaceController.Instance.Restore((int)item.usage_value);
                ConsumeItem(item);
                break;
            case "full_heal":
                InterfaceController.Instance.Heal(9999);
                ConsumeItem(item);
                break;
        }
    }

    public void PickUpItem(Item item)
    {
        if (possessed_items.Contains(item))
        {
            int index = possessed_items.IndexOf(item);
            item_count[index]++;
        }
        else
        {
            possessed_items.Add(item);
            item_count.Add(1);
        }
    }

    public void ConsumeItem(Item item)
    {
        int index = possessed_items.IndexOf(item);
        item_count[index]--;
        if(item_count[index] == 0)
        {
            possessed_items.RemoveAt(index);
            item_count.RemoveAt(index);
        }
    }
}
