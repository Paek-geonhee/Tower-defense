using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    public int Level;
    public int maxHP;
    public int curHP;
    public int speed;
    public GameObject[] pointList;
    public Queue<GameObject> pointQueue = new Queue<GameObject>();
    public GameObject end;
    GameObject point;
    Transform tr;
    Vector2 pos;

    public Image HPbarBase;
    public Image HPbar;

    // Start is called before the first frame update
    void Start()
    {
        curHP = maxHP;
        tr = GetComponent<Transform>();
        for (int i=0; i<pointList.Length; i++) {
            pointQueue.Enqueue(pointList[i]);
        }
        if (pointQueue.Count != 0)
            point = pointQueue.Dequeue();
        else
            point = end;
    }
    // Update is called once per frame

    void AutoMove() {
        // 지정된 목적지로 이동하는 함수이다.
        // 오브젝트는 파괴되기 전까지 지정된 목적지로 speed값 만큼 지속적으로 이동한다.
        tr.position = Vector2.MoveTowards(tr.position, point.transform.position, Time.deltaTime * speed);
    }

    void SetDestination(){
        pos = point.transform.position;
        if (tr.position.x == pos.x && tr.position.y == pos.y) {
            if (pointQueue.Count != 0)
                point = pointQueue.Dequeue();
            else
                point = end;
        }
    }

    void DestroyObj()
    {
        if (tr.position.x == end.transform.position.x && tr.position.y == end.transform.position.y) {
            GameManager.currentLife--;
            GameManager.killCount++;
            Destroy(gameObject);
        }
    }

    void HPBarControll() {
        HPbarBase.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + 0.5f, 0));
        HPbar.fillAmount = (float)curHP / maxHP;
    }

    void CheckHP() {
        if (curHP <= 0)
        {
            GameManager.killCount++;
            Destroy(gameObject);
            GameManager.goldScore += Random.Range(1 + (int)(speed * 1.3f), 8 + (int)(speed * 1.7f));
        }
    }

    void Update()
    {
        // 오브젝트는 생성되자마자 사전 설정된 루트를 따라 이동한다.

        // pointList에서 꺼낸 point로 이동한다.
        // point로 이동했다면 다음 point로 이동한다.
        // 더 이상 pointList에 목적지가 없다면 endPoint를 목적지로 설정한다.

        // 만약 오브젝트가 endpoint에 도착했다면 오브젝트를 파괴하고 GameManager의 currentLife 값을 1 줄인다.

        // 만약 오브젝트가 투사체에 해당하는 오브젝트와 충돌했다면 해당 투사체가 가진 damage값 만큼 HP값을 줄인다.

        HPBarControll();
        AutoMove();
        SetDestination();
        DestroyObj();
        CheckHP();
    }
}
