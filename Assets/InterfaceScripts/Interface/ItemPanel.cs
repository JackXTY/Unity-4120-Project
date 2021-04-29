using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanel : MonoBehaviour
{
    public Item item;
    public Image item_image;
    public Text item_name;
    public Text item_description;
    public Text stock;

    public bool destroyed;

    private void Update()
    {
        if(destroyed && !GetComponent<FadeControl>().FadeOut)
        {
            Destroy(gameObject);
        }
    }


    public void InitializeItemPanel()
    {
        Refresh();
        GetComponent<ScaleChange>().StartAnimate();
        GetComponent<FadeControl>().StartFadeIn();
    }

    public void Refresh()
    {
        item_image.sprite = item.sprite;
        item_name.text = "<b>" + item.item_name + "</b>";
        item_description.text = item.description;
        stock.text = "<b>Remaining: <color=#18FF00>" + GameManager.Instance.item_count[GameManager.Instance.possessed_items.IndexOf(item)].ToString() + "</color></b>";
    }

    public void CloseWindow()
    {
        GetComponent<ScaleChange>().StartAnimateReverse();
        GetComponent<FadeControl>().StartFadeOut();
        destroyed = true;
        //Destroy(gameObject, 0.5f);
    }

    public void UseItem()
    {
        GameManager.Instance.UseItem(item);
        if (GameManager.Instance.possessed_items.Contains(item))
        {
            Refresh();
        }
        else
        {
            CloseWindow();
        }
        
        
    }
}
