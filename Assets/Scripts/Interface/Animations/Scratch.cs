using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Scratch : MonoBehaviour
{
    public Sprite pic1;
    public Sprite pic2;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0f, 1f) > 0.5f)
        {
            GetComponent<Image>().sprite = pic1;
        }
        else
        {
            GetComponent<Image>().sprite = pic2;
        }
        GetComponent<RectTransform>().localPosition = new Vector3(Random.Range(-450f, 450f), Random.Range(-85f, -180f));
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        GetComponent<FadeControl>().StartFadeOut();
        Destroy(gameObject, 1.2f);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    
}
