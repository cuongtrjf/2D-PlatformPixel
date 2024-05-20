using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenArea : MonoBehaviour
{
    public float fadeDuration = 1f;
    SpriteRenderer spriteRenderer;
    Color hiddenColor;
    Coroutine currentCoroutine;



    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        hiddenColor = spriteRenderer.color;
        GameController.OnReset += ResetAlpla;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && transform.gameObject.activeInHierarchy)
        { 
            if(currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeSprite(true));
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && transform.gameObject.activeInHierarchy)
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(FadeSprite(false));
        }
    }


    private IEnumerator FadeSprite(bool fadeOut)
    {
        Color startColor = spriteRenderer.color;
        Color targetColor = fadeOut ? new Color(hiddenColor.r, hiddenColor.g, hiddenColor.b, 0) : hiddenColor;
        //chi so 0 alpla la xac dinh mau trong suot voi nen la hidden color, hieu don gian neu true thi mau trogn suot
        float timeFading = 0f;

        while(timeFading < fadeDuration)
        {
            spriteRenderer.color = Color.Lerp(startColor, targetColor, timeFading / fadeDuration);
            timeFading += Time.deltaTime;
            yield return null;//dat vao vong while de moi khi trong 1 frame no se dung lai de render roi tiep den frame tiep theo
            //coi nhu giong ham update nhung dat trong vong while cua coroutine
        }
        spriteRenderer.color = targetColor;
    }

    void ResetAlpla()
    {
        spriteRenderer.color = hiddenColor;
    }

}
