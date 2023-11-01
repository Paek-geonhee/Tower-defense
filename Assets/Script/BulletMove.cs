using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{

    public GameObject target;
    public int speed;
    public int damage;
    Rigidbody2D box;
    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent <Rigidbody2D>();
    }

    private void Move()
    {
        if (target == null)
            Destroy(gameObject);
        else
            transform.position = Vector2.MoveTowards(transform.position, target.gameObject.transform.position, speed * Time.deltaTime);
    }


    // Update is called once per frame
    void Update()
    {
        Move();
        SetAngleToTarget();
    }

    void SetAngleToTarget()
    {
        if (target != null)
        {
            Vector2 direction = new Vector2(
                transform.position.x - target.transform.position.x,
                transform.position.y - target.transform.position.y
            );

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 12.0f);
            transform.rotation = rotation;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject enemy = collision.gameObject;

        if (enemy.tag == "Enemy") { 
            EnemyInfo info = enemy.GetComponent<EnemyInfo>();
            info.curHP -= damage;
            Destroy(gameObject);
        }
    }

}
