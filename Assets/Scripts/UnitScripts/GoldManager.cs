using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    [SerializeField] int goldIncrementPerInterval  = 1;
    [SerializeField] float goldIntervalInSeconds = 1;

    private bool canIncrementGold = true;
    private Dialog dialog;
   
    public int currentGold = 100;
    public int archerCost = 35;
    public int cavalryCost = 20;
    public int golemCost = 100;
    public int knightCost = 25;
    public int spearCost = 40;
    public TextMeshProUGUI currentGoldText;

    private void Start()
    {
        dialog = GameObject.Find("DialogManager").GetComponent<Dialog>();
    }
    private void Update()
    {
        currentGoldText.text = "Gold: " + currentGold;
        if (!canIncrementGold || dialog.isDialogPresent) return;
        StartCoroutine(GoldIncrementTimerCoroutine(goldIntervalInSeconds));
        GoldIncrement();
    }

    private void GoldIncrement() => currentGold += goldIncrementPerInterval;

    private IEnumerator GoldIncrementTimerCoroutine(float waitTime)
    {
        canIncrementGold = false;
        yield return new WaitForSeconds(waitTime);
        canIncrementGold = true;
    }
}
