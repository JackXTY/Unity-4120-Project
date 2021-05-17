using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public GameObject bullet;
    public int bullet_count;
    public bool can_shoot_bullet;

    public List<GameObject> waypoints;
    GameObject current_target_waypoint;
    public int forth_back = 1;
    public float sound_radius;
    public float sight_radius;
    public float rotate_speed;
    public float walk_speed;
    public float FOV;
    public float close_attack_range;

    public GameObject player;
    public bool patrol;
    public bool chasing;

    int attack_cd_period = 500;
    int inner_clock = 0;

    public UnityEngine.AI.NavMeshAgent agent;
    public int hp = 50;

    public float revover_time = 1.5f;
    private float remain_recover_time = 0;


    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<UnityEngine.AI.NavMeshAgent>() != null)
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        }
        foreach(GameObject pt in waypoints)
        {
            Vector3 pos = pt.transform.position;
            pos.y = transform.position.y;
            pt.transform.position = pos;
        }
        current_target_waypoint = ClosestWayPoint();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0)
        {
            if (chasing)
            {
                Vector3 heading = ForceSameLevel(player.transform.position) - transform.position;
                if (heading.sqrMagnitude >= Mathf.Pow(close_attack_range, 2))
                {
                    Quaternion rotation = Quaternion.LookRotation(ForceSameLevel(player.transform.position) - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotate_speed);
                    GetComponent<Animator>().SetFloat("Turn", 1);
                    transform.position += Time.deltaTime * walk_speed * transform.forward;
                    GetComponent<Animator>().SetFloat("Forward", 1);
                }
                if (heading.sqrMagnitude < Mathf.Pow(close_attack_range, 2))
                {
                    //chasing = false;
                    if (inner_clock == 0)
                    {
                        GetComponent<Animator>().SetFloat("Turn", 0);
                        GetComponent<Animator>().SetFloat("Forward", 0);
                        GetComponent<Animator>().SetTrigger("CloseAttack");
                        InterfaceController.Instance.Damage(10, true);
                        inner_clock++;
                    }
                    else
                    {
                        inner_clock = (inner_clock + 1) % attack_cd_period;
                    }
                }
                if (!canSensePlayer())
                {
                    chasing = false;
                    GetComponent<Animator>().SetFloat("Turn", 0);
                    GetComponent<Animator>().SetFloat("Forward", 0);
                    patrol = true;
                    current_target_waypoint = ClosestWayPoint();
                    forth_back = 1;
                    inner_clock = 0;
                }
            }
            else
            {
                if (canSensePlayer())
                {
                    chasing = true;
                    patrol = false;

                    //BulletAttack();
                    //GetComponent<Animator>().SetTrigger("BulletAttack");

                }
            }

            if (patrol)
            {
                Vector3 heading = ForceSameLevel(current_target_waypoint.transform.position) - transform.position;
                if (heading.sqrMagnitude >= 1f)
                {
                    Quaternion rotation = Quaternion.LookRotation(ForceSameLevel(current_target_waypoint.transform.position) - transform.position);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotate_speed);
                    GetComponent<Animator>().SetFloat("Turn", 1);
                    transform.position += Time.deltaTime * walk_speed * transform.forward;
                    GetComponent<Animator>().SetFloat("Forward", 1);
                }
                if (heading.sqrMagnitude < 1f)
                {
                    if (current_target_waypoint == waypoints[waypoints.Count - 1])
                    {
                        forth_back = -1;
                    }
                    else if (current_target_waypoint == waypoints[0])
                    {
                        forth_back = 1;
                    }

                    int index = waypoints.IndexOf(current_target_waypoint);
                    current_target_waypoint = waypoints[(index + forth_back) % waypoints.Count];
                }
            }
        }
        if(remain_recover_time>0){
            remain_recover_time -= Time.deltaTime;
        }
        
    }

    Vector3 ForceSameLevel(Vector3 target)
    {
        return new Vector3(target.x, transform.position.y, target.z);
    }

    bool canSensePlayer()
    {
        Vector3 heading = ForceSameLevel(player.transform.position) - transform.position;
        if (heading.sqrMagnitude < sound_radius * sound_radius) return true;
        Quaternion rotation = Quaternion.LookRotation(ForceSameLevel(player.transform.position) - transform.position);
        //Debug.Log(rotation.eulerAngles.y);
        if (heading.sqrMagnitude < sight_radius * sight_radius && (rotation.eulerAngles.y - 90 <= FOV/2 || rotation.eulerAngles.y - 90 >= 360f - FOV/2))   
        {
            //Debug.Log("Start raycasting");
            RaycastHit hit;
            //Physics.IgnoreRaycastLayer(8);
            if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out hit, sight_radius)) //cast ray to see if obstacle in between is present
            {              
                if (hit.transform.gameObject == player)
                {
                    return true;
                }
            }

        }
        return false;
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

    public GameObject ClosestWayPoint()
    {
        float min_dist = float.MaxValue;
        int index = 0;
        for(int i = 0; i <= waypoints.Count - 1; i++)
        {
            if(Vector2.Distance(waypoints[i].transform.position, transform.position) < min_dist)
            {
                min_dist = Vector2.Distance(waypoints[i].transform.position, transform.position);
                index = i;
            }
        }
        return waypoints[index];
    }

    public void hit(int attackValue){
        if(remain_recover_time > 0){
            Debug.Log(remain_recover_time);
            return;
        }
        else if(attackValue >= hp){
            Debug.Log("monster defeated!");
            Destroy(this.gameObject);
        }else{
            Debug.Log("monster get hit " + attackValue);
            hp -= attackValue;
            remain_recover_time = revover_time;
        }
    }
}
