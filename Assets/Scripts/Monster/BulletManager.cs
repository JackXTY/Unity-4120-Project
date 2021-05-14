using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Quaternion direction;
    public int damage;
    private Rigidbody2D rb;

    public float speed;

    public int die_time;
    public int update_die_time;

    void Start()
    {
        transform.rotation = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            //transform.Translate(transform.forward * Time.deltaTime, Space.Self);
            float degree;
            degree = direction.eulerAngles.y;
            //rb = GetComponent<Rigidbody2D>();
            //rb.AddForce(Vector3.right * speed * Mathf.Sin(0 * Mathf.Deg2Rad));
            //rb.AddForce(Vector3.up * speed * Mathf.Cos(0 * Mathf.Deg2Rad));
            float newX = transform.position.x + speed * Mathf.Cos(degree * Mathf.Deg2Rad);
            float newY = transform.position.z + speed * Mathf.Sin(degree * Mathf.Deg2Rad);
            transform.position = new Vector3(newX, transform.position.y, newY);

            update_die_time++;
            if (update_die_time == die_time)
            {
                Destroy(gameObject);
            }
        }
    }
}
