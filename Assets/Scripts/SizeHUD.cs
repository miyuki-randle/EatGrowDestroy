using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SizeHUD : MonoBehaviour
{
    public static SizeHUD Instance;
    public int curSize;
    public int baseSize;
    public int maxSize;

    [SerializeField] private GameManager manager;
    [SerializeField] private Slider meter;
    //[SerializeField] private GameObject barTick;
    [SerializeField] private GameObject maxGlow;
    [SerializeField] private GameObject hurt;

    //private int lastSizeCheck;
    private bool lastChangeGrow;
    private int lastStageChange;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null) Instance = this;
        curSize = 5;
        baseSize = curSize;
        maxSize = 100;
        meter.maxValue = maxSize;
        meter.value = curSize;
        lastChangeGrow = true;
        lastStageChange = 0;
    }

    public void GrowPlayer()
    {
        if (curSize < maxSize)
        {
            curSize += 1;
            EventBus.Instance.DispatchEvent(GameEvent.PlayerGrowth);
        }
        if (curSize == maxSize) maxGlow.SetActive(true);
        else maxGlow.SetActive(false);

        if (curSize % 5 == 0 && lastChangeGrow)
        {
            if (curSize % 25 == 0 && lastStageChange != curSize)
            {
                lastStageChange = curSize;
                if (manager.stage < 3) manager.stage += 1;
                manager.score += 1000;
                EventBus.Instance.DispatchEvent(GameEvent.StageIncrease);
            }
        }
        lastChangeGrow = true;
        RefreshHUD();
    }

    public void ShrinkPlayer()
    {
        if (curSize > 0)
        {
            Instantiate(hurt, FindObjectOfType<PlayerController>().transform);
            curSize -= 1;
            EventBus.Instance.DispatchEvent(GameEvent.PlayerShrink);
        }
        if (curSize % 3 == 0 && !lastChangeGrow)
        {
            LifeHUD.Instance.HurtPlayer();
            //EventBus.Instance.DispatchEvent(GameEvent.PlayerShrink);
        }
        lastChangeGrow = false;
        RefreshHUD();
    }

    void RefreshHUD()
    {
        //GameObject[] barTicks = GameObject.FindGameObjectsWithTag("ProgressTick");

        //if (curSize < barTicks.Length)
        //{
        //    Destroy(barTicks[curSize]);
        //}
        //else if (curSize > barTicks.Length)
        //{
        //    GameObject tick = Instantiate(barTick, transform);
        //    tick.transform.position = barTicks[barTicks.Length - 1].transform.position + new Vector3(10.8f, 0, 0);
        //}
        meter.value = curSize;
        if (curSize <= 0) StartCoroutine(manager.GameOver());
    }
}
