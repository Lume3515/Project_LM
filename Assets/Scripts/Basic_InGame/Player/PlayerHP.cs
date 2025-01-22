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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Die();
        }
    }

    // 체력 감소
    public IEnumerator MinousHP(int damage)
    {
        currHP -= damage;

        if (currHP <= 0)
        {
            Die();
        }

        while (playerHpBar.fillAmount != currHP)
        {          

            playerHpBar.fillAmount = Mathf.Lerp(playerHpBar.fillAmount, currHP / 100f, Time.deltaTime * 5);

            yield return null;
        }
        //Debug.Log(currHP);


        yield break;
    }

    // 체력 더하기
    private IEnumerator AddHP()
    {
        yield return null;
    }

    private void Die()
    {
        Gamemanager.Instance.GamoOver();

        ScoreManager.Instance.GameDataGet_Kill();

    }
}
