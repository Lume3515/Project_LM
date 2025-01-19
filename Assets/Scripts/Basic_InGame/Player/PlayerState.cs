using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    // 공포 효과 이미지
    [SerializeField] Image horrorEffect;

    // 색깔
    private Color color;

    // 알파 값
    private float alpha;

    private void Start()
    {
        color = new Color(255f / 255f, 124f / 255f, 124f / 255f, 0f);
    }

    // 깜빡임 효과
    public IEnumerator HorrorEffect()
    {
        while (alpha < 0.5f)
        {
            alpha += Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 0.5f); // 알파값 제한
            color.a = alpha;
            horrorEffect.color = color;

            yield return null;
        }

        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 0.5f); // 알파값 제한
            color.a = alpha;
            horrorEffect.color = color;

            yield return null;
        }

        yield break;
    }
}
