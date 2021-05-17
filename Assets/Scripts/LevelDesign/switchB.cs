using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class switchB : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player;
        Vector3 heading;
        player = GameObject.FindGameObjectWithTag("Player");
        heading = player.transform.position - transform.position;
        if (heading.sqrMagnitude < 3.5)
        {
            Debug.Log("switch to B!");
            SceneManager.LoadScene (sceneName:"Area B");
        }
    }

}
