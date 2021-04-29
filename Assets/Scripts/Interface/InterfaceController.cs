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

    public GameObject general;
    public GameObject settings;
    public GameObject game_menu;

    public GameObject weapon_prefab;
    public List<GameObject> weapon_list;

    public bool in_settings;
    public bool in_game_menu;

    public Image HPBarFill;
    public Image StaminaBarFill;

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
        SetInterfaceColor();
        general.GetComponent<FadeControl>().StartFadeIn();
        Time.timeScale = 0;
        if (in_settings)
        {
            settings.SetActive(true);
            settings.GetComponent<FadeControl>().StartFadeIn();
        }
        else if (in_game_menu)
        {
            game_menu.SetActive(true);
            game_menu.GetComponent<FadeControl>().StartFadeIn();
        }
    }

    [ContextMenu("CloseInterface")]
    public void CloseInterface()
    {
        general.GetComponent<FadeControl>().StartFadeOut();
        if (in_settings)
        {
            in_settings = false;
            settings.GetComponent<FadeControl>().StartFadeOut();
        }
        else if (in_game_menu)
        {
            in_game_menu = false;
            game_menu.GetComponent<FadeControl>().StartFadeOut();
            foreach(GameObject weapon in weapon_list)
            {
                weapon.GetComponent<FadeControl>().StartFadeOut();
                Destroy(weapon, 0.5f);
            }
            weapon_list.Clear();
        }
        Time.timeScale = 1;
    }

    public void OpenSettings()
    {
        in_settings = true;
        OpenInterface();
    }

    public void OpenGameMenu()
    {
        in_game_menu = true;
        InitializeGameMenu();
        OpenInterface();
    }

    public void InitializeGameMenu()
    {
        weapon_list = new List<GameObject>();
        for(int i = 0; i <= 2; i++)
        {
            GameObject temp = Instantiate(weapon_prefab, game_menu.transform);
            temp.transform.localPosition = new Vector3(-120 + 120 * i, -134, 0);
            if(i == 1)
            {
                temp.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            }
            else
            {
                temp.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
            }
            weapon_list.Add(temp);
        }
    }

    public void WeaponListToLeft()
    {
        for (int i = 0; i <= 2; i++)
        {
            weapon_list[i].GetComponent<MoveTo>().startPosition = weapon_list[i].transform.localPosition;
            weapon_list[i].GetComponent<MoveTo>().destination = weapon_list[i].transform.localPosition + Vector3.left * 120;
            weapon_list[i].GetComponent<MoveTo>().ReplayMotion();
            if(i == 0)
            {
                weapon_list[i].GetComponent<ScaleChange>().startScale = 0.4f;
                weapon_list[i].GetComponent<ScaleChange>().finalScale = 0f;
                weapon_list[i].GetComponent<ScaleChange>().StartAnimate();
            }
            else if(i == 1)
            {
                weapon_list[i].GetComponent<ScaleChange>().startScale = 0.5f;
                weapon_list[i].GetComponent<ScaleChange>().finalScale = 0.4f;
                weapon_list[i].GetComponent<ScaleChange>().StartAnimate();
            }
            else if (i == 2)
            {
                weapon_list[i].GetComponent<ScaleChange>().startScale = 0.4f;
                weapon_list[i].GetComponent<ScaleChange>().finalScale = 0.5f;
                weapon_list[i].GetComponent<ScaleChange>().StartAnimate();
            }

        }
        Destroy(weapon_list[0], 0.5f);
        weapon_list.RemoveAt(0);
        GameObject temp = Instantiate(weapon_prefab, game_menu.transform);
        temp.transform.localPosition = new Vector3(240, -134, 0);
        temp.GetComponent<MoveTo>().startPosition = temp.transform.localPosition;
        temp.GetComponent<MoveTo>().destination = temp.transform.localPosition + Vector3.left * 120;
        temp.GetComponent<MoveTo>().ReplayMotion();
        temp.GetComponent<ScaleChange>().startScale = 0f;
        temp.GetComponent<ScaleChange>().finalScale = 0.4f;
        temp.GetComponent<ScaleChange>().StartAnimate();
        weapon_list.Add(temp);
    }

    public void WeaponListToRight()
    {
        for (int i = 0; i <= 2; i++)
        {
            weapon_list[i].GetComponent<MoveTo>().startPosition = weapon_list[i].transform.localPosition;
            weapon_list[i].GetComponent<MoveTo>().destination = weapon_list[i].transform.localPosition + Vector3.right * 120;
            weapon_list[i].GetComponent<MoveTo>().ReplayMotion();
            if (i == 0)
            {
                weapon_list[i].GetComponent<ScaleChange>().startScale = 0.4f;
                weapon_list[i].GetComponent<ScaleChange>().finalScale = 0.5f;
                weapon_list[i].GetComponent<ScaleChange>().StartAnimate();
            }
            else if (i == 1)
            {
                weapon_list[i].GetComponent<ScaleChange>().startScale = 0.5f;
                weapon_list[i].GetComponent<ScaleChange>().finalScale = 0.4f;
                weapon_list[i].GetComponent<ScaleChange>().StartAnimate();
            }
            else if (i == 2)
            {
                weapon_list[i].GetComponent<ScaleChange>().startScale = 0.4f;
                weapon_list[i].GetComponent<ScaleChange>().finalScale = 0f;
                weapon_list[i].GetComponent<ScaleChange>().StartAnimate();
            }

        }
        Destroy(weapon_list[2], 0.5f);
        weapon_list.RemoveAt(2);
        GameObject temp = Instantiate(weapon_prefab, game_menu.transform);
        temp.transform.localPosition = new Vector3(-240, -134, 0);
        temp.GetComponent<MoveTo>().startPosition = temp.transform.localPosition;
        temp.GetComponent<MoveTo>().destination = temp.transform.localPosition + Vector3.right * 120;
        temp.GetComponent<MoveTo>().ReplayMotion();
        temp.GetComponent<ScaleChange>().startScale = 0f;
        temp.GetComponent<ScaleChange>().finalScale = 0.4f;
        temp.GetComponent<ScaleChange>().StartAnimate();
        weapon_list.Insert(0, temp);
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!in_settings && !in_game_menu)
            {
                OpenSettings();
            }
            else if (in_settings)
            {
                CloseInterface();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!in_settings && !in_game_menu)
            {
                OpenGameMenu();
            }
            else if (in_game_menu)
            {
                CloseInterface();
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && in_game_menu)
        {
            WeaponListToLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && in_game_menu)
        {
            WeaponListToRight();
        }
    }
   
    [ContextMenu("damage")]
    public void damage()
    {
        int damage_point = 10;
        health -= damage_point;
        //HPBarFill.fillAmount = (float)health / max_health;
        HPBarFill.GetComponent<BarChange>().ChangeTo((float)health / max_health);
        HPBarFill.GetComponentInChildren<Flash>().StartFlash();
        
        SetInterfaceColor();
        if(health <= 0)
        {
            die();
        }
        damage_alpha = 0.24f;
        GameObject temp = Instantiate(scratch, damage_aura[0].transform.parent);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
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
                //Debug.Log(i);
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
