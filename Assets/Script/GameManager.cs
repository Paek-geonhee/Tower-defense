using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject createPointObject; // enemy 생성 지점
    private Vector2 createPoint;         // enemy 생성 좌표
    public GameObject endPointObject;    // enemy 파괴 지점
    private Vector2 endPoint;            // enemy 파괴 좌표
    public Text status;
    // enemy 스폰 및 파괴와 관련된 변수이다.

    public int maxLife;                  // 최대 라이프 수
    static public int currentLife;             // 현재 라이프 수
    private int countLife;
    public int stageNumber;              // 스테이지 번호
    public int maxStageNumber;           // 최대 스테이지 번호
    // 게임 종료에 관련된 변수이다.

    static public int goldScore;         // 재화량
    // 재화량은 추후 상점 메니저에서 사용할 예정.

    static public int enemyHP;           // 적 체력
    static public int enemySpeed;        // 적 속도
    public GameObject enemyObject;       // 적 오브젝트
    // 적 오브젝트에 해당 정보를 넘겨 투사체와의 충돌을 감지시킨다.

    static public int killCount;        // 적 처치 수
    static private int spawnCount;       // 생성될 적 수
    // 스테이지 종료와 관련된 변수이다.

    public int[] hpData;         // 적 체력 정보
    public int[] speedData;      // 적 속도 정보
    public int[] spawnData;      // 적 생성 수 정보
    // 각 스테이지마다 가지는 적 오브젝트의 정보이다.

    static public bool stageStarted;   //  스테이지 시작 여부

    public GameObject imageGroup; // 하트 이미지 그룹
    private Image[] images;  // 하트 이미지 배열

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
            status.text = ((int)stageTime + 1).ToString() + "초 뒤 스테이지를 시작합니다.";
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
            // 게임 오버
            ;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(currentLife + "/" + maxLife);
        // 함수 1
        // 스테이지 정보로 부터 적의 속도, 체력, 생성 수를 받아와서 스테이지를 시작한다.
        // 만약 킬카운트와 스폰카운트가 같다면 스테이지가 종료됐다고 판단하여, 타이머를 시작한다.
        // 타이머가 끝나거나 스킵 버튼을 누르면 즉시 다음 스테이지를 시작한다.
        // 스테이지 수가 20이 넘을 때까지 반복한다.

        // 함수 2
        // endPoint가 충돌을 감지하면 충돌된 외부 오브젝트를 파괴하고 현재 라이프를 1 줄인다.
        // 만약 현재 라이프가 0이면 게임 오버.

        // 함수 3
        // 함수 1이 끝나면 승리를 나타내는 씬으로 넘어가거나 패널을 띄우고 게임을 종료한다.
        
        TimeSet();
        CheckStageEnd();
        ControllHearts();
        TextControl();
    }
}
