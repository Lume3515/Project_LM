using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class PlayerHP : MonoBehaviour
{
    // 최대 체력
    private int maxHP;

    // 현재 체력
    private int currHP;

    // 플레이어 체력 바
    [SerializeField] Image playerHpBar;

    private void Start()
    {
        maxHP = 100;

        currHP = maxHP;
    }

    // 체력 감소
    public void MinousHP(int damage)
    {
        //Debug.Log("2");
        currHP -= damage;

        StartCoroutine(MinousHP_Gauge());
    }

    private IEnumerator MinousHP_Gauge()
    {
        if (currHP <= 0)
        {
            Die();
        }

        while (playerHpBar.fillAmount != currHP)
        {

            playerHpBar.fillAmount = Mathf.Lerp(playerHpBar.fillAmount, currHP / 100f, Time.deltaTime * 10);

            yield return null;
        }

        //Debug.Log(currHP);


        yield break;
    }

    private void Die()
    {
        Gamemanager.Instance.GamoOver();

        ScoreManager.Instance.GameDataGet_Kill();

    }
}
