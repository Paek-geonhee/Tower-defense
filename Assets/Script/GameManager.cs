using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject createPointObject; // enemy ���� ����
    private Vector2 createPoint;         // enemy ���� ��ǥ
    public GameObject endPointObject;    // enemy �ı� ����
    private Vector2 endPoint;            // enemy �ı� ��ǥ
    public Text status;
    // enemy ���� �� �ı��� ���õ� �����̴�.

    public int maxLife;                  // �ִ� ������ ��
    static public int currentLife;             // ���� ������ ��
    private int countLife;
    public int stageNumber;              // �������� ��ȣ
    public int maxStageNumber;           // �ִ� �������� ��ȣ
    // ���� ���ῡ ���õ� �����̴�.

    static public int goldScore;         // ��ȭ��
    // ��ȭ���� ���� ���� �޴������� ����� ����.

    static public int enemyHP;           // �� ü��
    static public int enemySpeed;        // �� �ӵ�
    public GameObject enemyObject;       // �� ������Ʈ
    // �� ������Ʈ�� �ش� ������ �Ѱ� ����ü���� �浹�� ������Ų��.

    static public int killCount;        // �� óġ ��
    static private int spawnCount;       // ������ �� ��
    // �������� ����� ���õ� �����̴�.

    public int[] hpData;         // �� ü�� ����
    public int[] speedData;      // �� �ӵ� ����
    public int[] spawnData;      // �� ���� �� ����
    // �� ������������ ������ �� ������Ʈ�� �����̴�.

    static public bool stageStarted;   //  �������� ���� ����

    public GameObject imageGroup; // ��Ʈ �̹��� �׷�
    private Image[] images;  // ��Ʈ �̹��� �迭

    public Text stage;
    public Text enemyhp;
    public Text life;
    public Text gold;

    public float stageTime;
    public float statusChange;

    //public Text status;
    //public float statusChanged;
    void Awake()
    {
        images = imageGroup.GetComponentsInChildren<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        stageTime = 10.0f;
        stageNumber = 0;
        currentLife = countLife = maxLife;
        Transform c_tr = createPointObject.GetComponent<Transform>();
        createPoint = c_tr.localPosition;
        Transform e_tr = createPointObject.GetComponent<Transform>();
        endPoint = e_tr.localPosition;

        enemyHP = hpData[0];
        enemySpeed = speedData[0];
        spawnCount = spawnData[0];

        EnemyInfo info = enemyObject.GetComponent<EnemyInfo>();
        info.maxHP = enemyHP;
        info.speed = enemySpeed;
        

        stageStarted = false;
        goldScore = 220;


    }

    void TimeSet() {
        Debug.Log(stageTime);
        if (statusChange > 0)
            statusChange -= Time.deltaTime;

        if (!stageStarted) {
            stageTime -= Time.deltaTime;
        }
        if (stageTime <= 0) {
            stageStarted = true;
            stageTime = 10.0f;
            StartCoroutine("c_SpawnEnemy");
        }
    }

    void SetNextStage() {
        stageNumber++;
        if (stageNumber >= maxStageNumber) {
            SceneManager.LoadScene("EndingScene");
        }
        enemyHP = hpData[stageNumber];
        enemySpeed = speedData[stageNumber];
        spawnCount = spawnData[stageNumber];

        EnemyInfo info = enemyObject.GetComponent<EnemyInfo>();
        info.maxHP = enemyHP;
        info.speed = enemySpeed;
    }

    void CheckStageEnd() {
        if (killCount == spawnCount)
        {
            stageStarted = false;
            SetNextStage();
            killCount = 0;
        }
    }

    IEnumerator c_SpawnEnemy() {
        for (int num = spawnCount; num > 0; num--)
        {
            Instantiate(enemyObject, createPoint, Quaternion.identity);
            yield return new WaitForSeconds(2.0f/ enemySpeed);
        }
        yield return null;
    }

    void TextControl()
    {
        stage.text = "Stage : " + (stageNumber + 1);
        enemyhp.text = "Enemy HP : " + enemyHP;
        life.text = "Life : " + currentLife;
        gold.text = "Gold : " + goldScore;

        if (stageTime < 10)
            status.text = ((int)stageTime + 1).ToString() + "�� �� ���������� �����մϴ�.";
        else
        {
            statusChange = 1.0f;

            if (statusChange <= 0)
                status.text = "";

        }
    }

    void ControllHearts() {
        if (currentLife < countLife) {
            while (currentLife != countLife)
            {
                countLife--;
                Color c = images[countLife].GetComponent<Image>().color;
                images[countLife].GetComponent<Image>().color = new Color(c.r, c.g, c.b, 0);
            }
        }

        if (currentLife <= 0)
        { 
            // ���� ����
            ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentLife + "/" + maxLife);
        // �Լ� 1
        // �������� ������ ���� ���� �ӵ�, ü��, ���� ���� �޾ƿͼ� ���������� �����Ѵ�.
        // ���� ųī��Ʈ�� ����ī��Ʈ�� ���ٸ� ���������� ����ƴٰ� �Ǵ��Ͽ�, Ÿ�̸Ӹ� �����Ѵ�.
        // Ÿ�̸Ӱ� �����ų� ��ŵ ��ư�� ������ ��� ���� ���������� �����Ѵ�.
        // �������� ���� 20�� ���� ������ �ݺ��Ѵ�.

        // �Լ� 2
        // endPoint�� �浹�� �����ϸ� �浹�� �ܺ� ������Ʈ�� �ı��ϰ� ���� �������� 1 ���δ�.
        // ���� ���� �������� 0�̸� ���� ����.

        // �Լ� 3
        // �Լ� 1�� ������ �¸��� ��Ÿ���� ������ �Ѿ�ų� �г��� ���� ������ �����Ѵ�.
        
        TimeSet();
        CheckStageEnd();
        ControllHearts();
        TextControl();
    }
}
