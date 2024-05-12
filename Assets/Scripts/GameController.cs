using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;//tien do nhat gem
    public Slider progressSlider;//thanh tien do level

    // Start is called before the first frame update
    void Start()
    {
        progressAmount = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;//dang ky sy kien OnGemCollect cho Gem
        //no se kich hoat cac phuong thuc, trong truong hop nay chi co mot phuong thuc IncreaseProgressAmount
      

        progressSlider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        Debug.Log(amount+" "+progressAmount);
        progressSlider.value = progressAmount;
        if (progressAmount >= 100)
        {
            //qua level
            Debug.Log("Level complete!");
        }
    }
}
