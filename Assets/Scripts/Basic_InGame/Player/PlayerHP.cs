using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerHP : MonoBehaviour
{
    // 최대 체력
    private int maxHP;

    // 현재 체력
    private int currHP;

    // 플레이어 체력 바
    [SerializeField] Image playerHpBar;

    public delegate void PlayerDie();
    public static event PlayerDie AllStop;

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

        if (currHP <= 0)
        {
            Die();
        }

        StartCoroutine(MinousHP_Gauge());
    }

    private IEnumerator MinousHP_Gauge()
    {       

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
        AllStop();             

        SceneManager.LoadSceneAsync(0); // 비동기

        ScoreManager.Instance.GameDataGet_Kill();

        UnityEngine.Cursor.lockState = CursorLockMode.None;
        UnityEngine.Cursor.visible = true;
    }
}
