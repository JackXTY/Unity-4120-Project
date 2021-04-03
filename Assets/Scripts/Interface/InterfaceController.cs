using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    public static InterfaceController Instance { get; private set; }

    //-----------------Player Stats--------------------------------
    public int health;
    public int max_health;
    public int stamina;
    public int max_stamina;

    public Color good_health;
    public Color medium_health;
    public Color bad_health;

    public Image[] grids;

    public float damage_alpha = 0f;
    public float decrease_duration;

    public Image[] damage_aura;
    public GameObject scratch;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)   //singleton InterfaceController instance, easy for referencing in other scripts
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        InitializePlayerStats();
    }

    public void InitializePlayerStats()
    {
        //temporary, modify later
        health = max_health = stamina = max_stamina = 100;



        SetInterfaceColor();
    }

    [ContextMenu("OpenInterface")]
    public void OpenInterface()
    {
        GetComponent<FadeControl>().StartFadeIn();
    }

    [ContextMenu("CloseInterface")]
    public void CloseInterface()
    {
        GetComponent<FadeControl>().StartFadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Image i in damage_aura)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, damage_alpha);
        }
        damage_alpha -= 0.24f / decrease_duration;
        if(damage_alpha < 0)
        {
            damage_alpha = 0;
        }
    }
   
    [ContextMenu("damage")]
    public void damage()
    {
        int damage_point = 10;
        health -= damage_point;
        SetInterfaceColor();
        if(health <= 0)
        {
            die();
        }
        damage_alpha = 0.24f;
        GameObject temp = Instantiate(scratch, damage_aura[0].transform.parent);
    }

    public void die()
    {
        Debug.Log("You have died!");
    }

    public void heal(int heal_point)
    {
        health += heal_point;
        if(health > max_health)
        {
            health = max_health;     //no overhealing
        }
    }

    public void SetInterfaceColor()
    {
        float health_percent = (float)health / max_health;
        if (health_percent >= 0.5f)
        {
            foreach(Image i in grids)
            {
                Color new_color = Color.Lerp(medium_health, good_health, (health_percent - 0.5f) * 2);
                i.color = new Color(new_color.r, new_color.g, new_color.b, i.color.a);
            }
        }
        else
        {
            foreach (Image i in grids)
            {
                Color new_color = Color.Lerp(bad_health, medium_health, health_percent * 2);
                i.color = new Color(new_color.r, new_color.g, new_color.b, i.color.a);
            }
        }
    }
}
