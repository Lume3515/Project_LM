using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    // ���� ȿ�� �̹���
    [SerializeField] Image horrorEffect_Image;

    // ����
    private Color color;

    // ���� ��
    private float alpha;

    private bool horrorEffect;
    public bool HorrorEffect_bool => horrorEffect;
    private void Start()
    {
        // 0 ~ 1������ �̷� ������ «
        color = new Color(46f / 255f, 42f / 255f, 43f / 255f, 0f);
    }

    // ������ ȿ��
    public IEnumerator HorrorEffect()
    {     
        horrorEffect = true;

        while (alpha < 0.7f)
        {
            alpha += Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 0.7f); // ���İ� ����
            color.a = alpha;
            horrorEffect_Image.color = color;

            yield return null;
        }
        //Debug.Log("@");

        // 2�� ��ٸ���
        yield return new WaitForSeconds(2f);

        StartCoroutine(Check());
        //Debug.Log("#");       

        yield break;
    }

    // ������ �ϴϱ� ȣ���� ������ ���� ����;;
    private IEnumerator Check()
    {
        while (alpha > 0)
        {

            alpha -= Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 0.7f); // ���İ� ����
            color.a = alpha;
            horrorEffect_Image.color = color;

            yield return null;
        }

        horrorEffect = false;

        yield break;
    }
    
}
