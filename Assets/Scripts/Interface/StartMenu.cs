using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    public List<GameObject> buttons;
    public GameObject bg_text;


    public void StartGame()
    {
        StartCoroutine(StartGameSequence());
    }

    IEnumerator StartGameSequence()
    {
        foreach(GameObject button in buttons)
        {
            button.GetComponent<FadeControl>().StartFadeOut();
        }
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }
        bg_text.GetComponent<FadeControl>().StartFadeIn();
        yield return new WaitForSeconds(6f);
        bg_text.GetComponent<FadeControl>().StartFadeOut();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        ThridPersonController.Instance.DisableMouseControl();

    }
}
