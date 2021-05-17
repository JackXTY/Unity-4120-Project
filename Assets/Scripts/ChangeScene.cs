using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if(scene.name == sceneName){
    //         SceneManager.sceneLoaded -= OnSceneLoaded;
    //         Destroy(this.gameObject);
    //     }
    // }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            // DontDestroyOnLoad(this.gameObject);
            // SceneManager.sceneLoaded += OnSceneLoaded; 
            SceneManager.LoadScene(sceneName);
        }
    }
}
