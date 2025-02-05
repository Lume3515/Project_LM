using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{
    // 공포 효과 이미지
    [SerializeField] Image horrorEffect_Image;

    // 색깔
    private Color color;

    // 알파 값
    private float alpha;

    private bool horrorEffect;
    public bool HorrorEffect_bool => horrorEffect;
    private void Start()
    {
        // 0 ~ 1까지라 이런 식으로 짬
        color = new Color(46f / 255f, 42f / 255f, 43f / 255f, 0f);
    }

    // 깜빡임 효과
    public IEnumerator HorrorEffect()
    {     
        horrorEffect = true;

        while (alpha < 0.7f)
        {
            alpha += Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 0.7f); // 알파값 제한
            color.a = alpha;
            horrorEffect_Image.color = color;

            yield return null;
        }
        //Debug.Log("@");

        // 2초 기다리기
        yield return new WaitForSeconds(2f);

        StartCoroutine(Check());
        //Debug.Log("#");       

        yield break;
    }

    // 위에다 하니까 호출중 죽으면 버그 생김;;
    private IEnumerator Check()
    {
        while (alpha > 0)
        {

            alpha -= Time.deltaTime;
            alpha = Mathf.Clamp(alpha, 0, 0.7f); // 알파값 제한
            color.a = alpha;
            horrorEffect_Image.color = color;

            yield return null;
        }

        horrorEffect = false;

        yield break;
    }
    
}
