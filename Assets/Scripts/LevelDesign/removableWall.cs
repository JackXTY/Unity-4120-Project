using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removableWall : MonoBehaviour
{
    public int lifePoint = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (lifePoint <= 0) 
            Destroy(gameObject, 0.5F); 
    }

    private void OnTriggerEnter(Collider other)
	{
        Debug.Log(other.gameObject.tag);
		if (other.gameObject.tag == "Weapon")
		{
			lifePoint -= 10;
			if (lifePoint <= 0) 
				Destroy(gameObject, 0.5F); 
		}
	}
}
