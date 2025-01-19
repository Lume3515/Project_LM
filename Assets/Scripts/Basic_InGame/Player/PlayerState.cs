using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    // ���� ȿ�� �̹���
    [SerializeField] Image horrorEffect;

    // ����
    private Color color;

    // ���� ��
    private float alpha;

    private void Start()
    {
        color = new Color(255f / 255f, 124f / 255f, 124f / 255f, 0f);
    }

    // ������ ȿ��
    public IEnumerator HorrorEffect()
    {
        while (alpha < 0.5f)
        {
            alpha += Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 0.5f); // ���İ� ����
            color.a = alpha;
            horrorEffect.color = color;

            yield return null;
        }

        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 0.5f); // ���İ� ����
            color.a = alpha;
            horrorEffect.color = color;

            yield return null;
        }

        yield break;
    }
}
