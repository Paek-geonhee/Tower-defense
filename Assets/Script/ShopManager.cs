using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject basePanel;
    public GameObject shopPanel;
    public GameObject infoPanel;

    public GameObject normalTower;
    public GameObject speedTower;
    public GameObject splashTower;
    public GameObject slowTower;

    public Text status;
    public float statusChanged;

    // Start is called before the first frame update
    void Start()
    {
        basePanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    public void PanelON() {
        basePanel.SetActive(false);
        shopPanel.SetActive(true);
    }

    public void PanelOFF()
    {
        basePanel.SetActive(true);
        shopPanel.SetActive(false);
    }

    public void SpawnNormal() {
        

        int p = normalTower.GetComponent<TowerAttack>().price;
        if (GameManager.goldScore >= p)
        {
            PanelOFF();
            Instantiate(normalTower, Vector2.one, Quaternion.identity);
        }
        else {
            status.text = "골드가 부족합니다.";
            statusChanged = 3.0f;
        }
    }

    public void SpawnSpeed()
    {


        int p = speedTower.GetComponent<TowerAttack>().price;
        if (GameManager.goldScore >= p)
        {
            PanelOFF();
            Instantiate(speedTower, Vector2.one, Quaternion.identity);
        }
        else
        {
            status.text = "골드가 부족합니다.";
            statusChanged = 3.0f;
        }
    }

    public void SpawnSplash()
    {


        int p = splashTower.GetComponent<TowerAttack>().price;
        if (GameManager.goldScore >= p)
        {
            PanelOFF();
            Instantiate(splashTower, Vector2.one, Quaternion.identity);
        }
        else
        {
            status.text = "골드가 부족합니다.";
            statusChanged = 3.0f;
        }
    }

    public void SpawnSlow()
    {


        int p = slowTower.GetComponent<TowerAttack>().price;
        if (GameManager.goldScore >= p)
        {
            PanelOFF();
            Instantiate(slowTower, Vector2.one, Quaternion.identity);
        }
        else
        {
            status.text = "골드가 부족합니다.";
            statusChanged = 3.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (statusChanged > 0f)
        {
            statusChanged -= Time.deltaTime;
        }
        else
        {
            status.text = "";
        }
    }
}
