using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class PlayerHP : MonoBehaviour
{
    // �ִ� ü��
    private int maxHP;

    // ���� ü��
    private int currHP;

    // �÷��̾� ü�� ��
    [SerializeField] Image playerHpBar;


    private void Start()
    {
        maxHP = 100;

        currHP = maxHP;
    }

    // ü�� ����
    public IEnumerator MinousHP(int damage)
    {
        currHP -= damage;
        while (playerHpBar.fillAmount != currHP)
        {          

            playerHpBar.fillAmount = Mathf.Lerp(playerHpBar.fillAmount, currHP / 100f, Time.deltaTime * 10);

            yield return null;
        }
        //Debug.Log(currHP);

        if (currHP <= 0)
        {
            Die();
        }

        yield break;
    }

    // ü�� ���ϱ�
    private IEnumerator AddHP()
    {
        yield return null;
    }

    private void Die()
    {
        Gamemanager.Instance.GamoOver();
    }
}
