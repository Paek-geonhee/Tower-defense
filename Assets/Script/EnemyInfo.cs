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
        // ������ �������� �̵��ϴ� �Լ��̴�.
        // ������Ʈ�� �ı��Ǳ� ������ ������ �������� speed�� ��ŭ ���������� �̵��Ѵ�.
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
        // ������Ʈ�� �������ڸ��� ���� ������ ��Ʈ�� ���� �̵��Ѵ�.

        // pointList���� ���� point�� �̵��Ѵ�.
        // point�� �̵��ߴٸ� ���� point�� �̵��Ѵ�.
        // �� �̻� pointList�� �������� ���ٸ� endPoint�� �������� �����Ѵ�.

        // ���� ������Ʈ�� endpoint�� �����ߴٸ� ������Ʈ�� �ı��ϰ� GameManager�� currentLife ���� 1 ���δ�.

        // ���� ������Ʈ�� ����ü�� �ش��ϴ� ������Ʈ�� �浹�ߴٸ� �ش� ����ü�� ���� damage�� ��ŭ HP���� ���δ�.

        HPBarControll();
        AutoMove();
        SetDestination();
        DestroyObj();
        CheckHP();
    }
}
