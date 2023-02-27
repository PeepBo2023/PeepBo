using Naninovel;
using Naninovel.UI;
using PeepBo.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HogamComponent : MonoBehaviour
{
    [SerializeField] Sprite ���;
    [SerializeField] Sprite ����;
    [SerializeField] Sprite ����;
    [SerializeField] Sprite ����;
    [SerializeField] GameObject HogamEffect;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image targetImage;

    public void InitHogamComponent(HogamChoice hogamChoice)
    {
        var target = hogamChoice.target;
        int score = hogamChoice.score;
        Debug.Log(target);
        Debug.Log(score);
        if (target == "G")
            targetImage.sprite = ���;
        else if (target == "Y")
            targetImage.sprite = ����;
        else if (target == "W")
            targetImage.sprite = ����;
        else if (target == "D")
            targetImage.sprite = ����;

        var text = targetImage.GetComponentInChildren<TextMeshProUGUI>();
        text.text = $"ȣ���� + {score}";
    }

    public IEnumerator OnShow()
    {
        //HogamEffect.SetActive(true);
        while(true)
        {
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, scoreText.color.a + 0.03f);
            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, targetImage.color.a + 0.03f);
            if (targetImage.color.a >= 1f)
                yield break;
            yield return null;
        }
    }

    public IEnumerator Delay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    public IEnumerator OnHide()
    {
        while (true)
        {
            scoreText.color = new Color(scoreText.color.r, scoreText.color.g, scoreText.color.b, scoreText.color.a - 0.03f);
            targetImage.color = new Color(targetImage.color.r, targetImage.color.g, targetImage.color.b, targetImage.color.a - 0.03f);
            if (targetImage.color.a <= 0f)
            {
                //HogamEffect.SetActive(false);
                yield break;
            }
            yield return null;
        }
    }
}
