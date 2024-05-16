using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image heartPrefab;
    public Sprite fullHeartSprite;//bieu hien con mang
    public Sprite emptyHeartSprite;//bieu hien da mat mang


    private List<Image> heartsList = new List<Image>();


    public void SetMaxHearts(int maxHearts)
    {
        foreach(Image heart in heartsList)
        {
            Destroy(heart.gameObject);
        }
        heartsList.Clear();

        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Instantiate(heartPrefab, transform);
            newHeart.sprite = fullHeartSprite;
            newHeart.color = Color.red;
            heartsList.Add(newHeart);
        }
    }


    public void UpdateHearts(int currentHealth)
    {//update neu nhu co heart da mat
        for(int i=0;i<heartsList.Count; i++)
        {
            if(i< currentHealth)
            {
                heartsList[i].sprite = fullHeartSprite;
                heartsList[i].color = Color.red;
            }
            else
            {
                heartsList[i].sprite = emptyHeartSprite;
                heartsList[i].color = Color.white;
            }
        }
    }
}
