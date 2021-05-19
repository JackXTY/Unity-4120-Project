using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    public GameObject background;
    public GameObject score_text;
    public List<GameObject> elements;

    public void EndGame()
    {
        gameObject.SetActive(true);
        StartCoroutine(EndGameSequence());
        Cursor.lockState = CursorLockMode.None;
    }

    IEnumerator EndGameSequence()
    {
        GetComponent<FadeControl>().StartFadeIn();
        score_text.GetComponent<Text>().text = "<b>Score: " + InterfaceController.Instance.EndGameScore() + "</b>";
        yield return new WaitForSeconds(0.5f);
        //foreach(GameObject obj in elements)
        //{
        //    obj.GetComponent<FadeControl>().StartFadeIn();
        //}
    }
}
