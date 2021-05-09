using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject bullet;
    public int bullet_count;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("ShootBullet")]
    public void BulletAttack()
    {
        int number = bullet_count;
        float offset = Random.Range(0.0f, 360.0f);
        for (int i = 0; i <= number - 1; i++)
        {
            GameObject bullet_obj = Instantiate(bullet, gameObject.transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));
            bullet_obj.GetComponent<BulletManager>().direction = Quaternion.Euler(0.0f, i * 360 / number + offset, 0.0f);
        }
    }
}
