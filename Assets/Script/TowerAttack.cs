using UnityEngine;
using UnityEngine.UI;

public class TowerAttack : MonoBehaviour
{
    public GameObject range;        // ��ž�� enemy�� �����ϴ� ����
    public GameObject bullet;       // źȯ
    public GameObject target;       // ���ǿ� ���� �����Ǵ� ��� -> źȯ���� �Ѱ��� ����̴�.
    public GameObject rangeUI;
    public int price;               // ��ž ���� �� �Һ�� ����
    public float delay;               // źȯ ���� ������, ���� �ӵ��̴�.
    public int damage;              // ��ž ������ -> źȯ���� �Ѱ��� ���̴�.
    public int level;               // ��ž ����. ���� �� ������ 1�̴�.
    public int killScore; // ..

    public int upgradePrice;        // ���׷��̵忡 �ʿ��� ����̴�.

    public bool onPossiblePoint;    // ��ž�� ������ ��ġ�� �ִ��� Ȯ��
    public bool onMousePointer;     // ��ž�� ���콺�� ����ٳ�� �ϴ��� Ȯ�� 

    public GameObject upgradePanel; // ��ž Ŭ�� �� ��Ÿ�� ���׷��̵� UI
    public bool onUI;

    public int totalPrice;
    bool buyed;

    float time;

    SpriteRenderer sprite;

    public GameObject infoPanel;
    public Text textUpPrice;
    public Text levelText;
    public Text damageText;
    public Text delayText;
    public Text killText;

    // Start is called before the first frame update
    void Start()
    {
        range.SetActive(false);
        infoPanel.SetActive(false);
        level = 1;
        onPossiblePoint = false;
        onMousePointer = true;
        time = 0;
        onUI = false;
        buyed = false;
        upgradePanel.SetActive(false);
        sprite = GetComponent<SpriteRenderer>();
        upgradePrice = 150;
        totalPrice = price;
    }

    // Update is called once per frame
    void Update()
    {



        time += Time.deltaTime;


        if (onMousePointer)
        {
            // ���� 1. (�ʱ�)�������� ��ž�� ������ ���(��带 �Һ����� �ʾҰ�, ������Ʈ�� ���콺�� ����ٳ���ϴ� �����̴�.)

            // �Լ� 1. ��ž ������Ʈ�� ���콺�� ����ٴѴ�.

            // �Լ� 2. ���콺 ������Ű�� ���� �� ������Ʈ�� �����Ѵ�.

            // �Լ� 3. ���콺�� ���� �ٴϴ� ��ž�� ��ġ ������ ��ġ�� �ִ��� Ȯ���ϰ� �� �������� ��ȯ�Ѵ�.

            // �Լ� 4. ���콺 ����Ű�� ��������, 3���� ���̶�� ������ ��尪�� �Һ��ϰ� �ش� ��ġ�� ������Ʈ�� ��ġ�� ��
            // ���� 2�� Ȱ��ȭ �Ѵ�.
            if (onPossiblePoint)
            {
                sprite.color = Color.green;
            }
            else {
                sprite.color = Color.red;
            }

            onPossiblePoint = isOnPossiblePoint();
            OnMousePointer();
            if (onPossiblePoint && Input.GetMouseButtonDown(0)) { 
                onMousePointer = false;
                if (!buyed)
                {
                    GameManager.goldScore -= price;
                    buyed = true;
                }

            }

            if (Input.GetMouseButtonDown(1))
            {
                Destroy(gameObject);
            }
            range.SetActive(false);  
        }
        else {

            // ���� 2. ��ž�� ����� ��ġ�� ���(��带 ���������� �Һ��ϰ� ������ ��ġ�� ��ġ�� �����̴�.)
            // �Լ� 1. circle���ο� enemy�� ���Դ��� Ȯ���ϰ� �ش� ������Ʈ�� ��ȯ�Ѵ�.

            // �Լ� 2. ������ źȯ�� ����� ��ȯ�� ������Ʈ�� ���ϰ� źȯ�� �����Ѵ�.
            // -> źȯ���� ����� �������ָ� �������ڸ��� �ش� ������� ������ �̵��ϹǷ� ���Ĵ� ����.

            // �Լ� 3. ������ ����� circle���� ���ŵǸ� ����� �����Ѵ�.

            // �Լ� 1 ���� �缳��
            // ����� ���ŵ� �� circle���ο� enemy�� ������ �ִٸ� �ִܰŸ��� enemy �� ������� �����Ѵ�.


            // �������� ������, ���������� ���� �����ϰ� źȯ�� ����.
            sprite.color = Color.white;
            SetTarget();
            bullet.GetComponent<BulletMove>().target = target;
            if (target != null && time >= delay) {
                Instantiate(bullet, transform.position, Quaternion.identity);
                time = 0f;
            }
            range.SetActive(true);
            DrawRange();
            MouseDownCheck();
            SetAngleToTarget();
        }
        SetPanel();
        
    }

    void SetAngleToTarget() {
        if (target != null)
        {
            Vector2 direction = new Vector2(
                transform.position.x - target.transform.position.x,
                transform.position.y - target.transform.position.y
            );

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, angleAxis, 12.0f * Time.deltaTime);
            transform.rotation = rotation;
        }
    }
    void MouseDownCheck()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.stageStarted)
        {
            Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(cast, Mathf.Infinity, (1 << 6));
            if (hit.collider != null)
            {
                if (hit.collider.gameObject == gameObject)
                    onUI = true;
                else
                    onUI = false;
            }
        }
    }

    void SetPanel() { 
        upgradePanel.SetActive(onUI);
        upgradePanel.transform.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y - 0.5f));
    }
    void DrawRange() {
        Ray cast = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(cast, Mathf.Infinity, (1<<6));
        if (hit.collider != null)
        {
            Debug.Log("�ȳ�");
            Debug.Log(hit.collider.gameObject);
            if (hit.collider.gameObject == gameObject)
            {
                rangeUI.SetActive(true);

                if (!onMousePointer)
                {
                    infoPanel.SetActive(true);
                    levelText.text = level.ToString();
                    damageText.text = damage.ToString();
                    textUpPrice.text = upgradePrice.ToString();
                    delayText.text = delay.ToString();
                }
                else
                {
                    infoPanel.SetActive(false);
                }
            }
            else {
                Debug.Log("�̰� �� �ȵ�");
                infoPanel.SetActive(false);
            }

        }
        else
        {
            Debug.Log("�߰�");
            rangeUI.SetActive(false);
            infoPanel.SetActive(false);
        }

        
    }

    void SetTarget() {
        target = range.GetComponent<RangeController>().targetSet;
    }

    void OnMousePointer() {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 1));
    }

    bool isOnPossiblePoint() {
        CircleCollider2D box = GetComponent<CircleCollider2D>();
        Vector2 pointAl, pointBl, pointAr, pointBr;

        pointAl = new Vector2(transform.position.x + box.offset.x - box.radius - 0.05f, transform.position.y + box.offset.y + box.radius + 0.02f);
        pointBl = new Vector2(transform.position.x + box.offset.x - box.radius - 0.05f, transform.position.y + box.offset.y - box.radius - 0.02f);
        pointAr = new Vector2(transform.position.x + box.offset.x + box.radius + 0.05f, transform.position.y + box.offset.y + box.radius + 0.02f);
        pointBr = new Vector2(transform.position.x + box.offset.x + box.radius + 0.05f, transform.position.y + box.offset.y - box.radius - 0.02f);

        Debug.DrawLine(pointAl, pointBl);
        Debug.DrawLine(pointAr, pointBr);

        Collider2D objL = Physics2D.OverlapArea(pointAl, pointBl, ~(1<<31));
        Collider2D objR = Physics2D.OverlapArea(pointAr, pointBr, ~(1<<31));


        return (objL == null && objR == null);
    }

    public void Upgrade() {
        // ���׷��̵� ��ư Ŭ�� �� ���׷��̵��� �� �ִ� �Լ�
        if (GameManager.goldScore >= upgradePrice && level < 3) {
            totalPrice += upgradePrice;
            GameManager.goldScore -= upgradePrice;
            level++;
            upgradePrice = (int)(upgradePrice * 2);
            damage = (int)(damage * 1.5f);
            delay = (delay * 0.85f);
            
            //upgradePanel.SetActive(false);
            onUI = false;
        }
    }

    public void Sell() {
        // �Ǹ� Ŭ�� �� ���ݱ��� ����� �ݾ��� 50%�� ȸ���ϴ� �Լ�
        GameManager.goldScore += (int)(totalPrice * 0.5f);
        onUI = false;
        Destroy(gameObject);
    }

    public void MoveTo() {
        // �̵� Ŭ�� �� ������Ʈ�� ���콺�� ����ٴϰ� �ϴ� �Լ�
        onUI = false;
        onMousePointer = true;
        
    }

    public void Cancel() {
        onUI = false;
    }
}
