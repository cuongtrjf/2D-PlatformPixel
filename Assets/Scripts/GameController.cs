using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int progressAmount;//tien do nhat gem
    public Slider progressSlider;//thanh tien do level


    public GameObject player;
    public GameObject loadCanvas;
    public List<GameObject> levels;
    private int currentLevelIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        progressAmount = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;//dang ky su kien OnGemCollect cho Gem
        //no se kich hoat cac phuong thuc, trong truong hop nay chi co mot phuong thuc IncreaseProgressAmount
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;//dki su kien load level khi Hold R

        progressSlider.value = 0;

        loadCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;
        if (progressAmount >= 100)
        {
            //qua level
            loadCanvas.SetActive(true);
            Debug.Log("Level complete!");
        }
    }


    private void LoadNextLevel()
    {
        //neu level hien tai chua phai level cuoi chua, neu roi thi level tiep theo bang 1, con k thi bang lv +1
        int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;

        loadCanvas.SetActive(false);//thanh tai level

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[nextLevelIndex].gameObject.SetActive(true);

        player.transform.position = new Vector3(0, 0, 0);
        currentLevelIndex = nextLevelIndex;


        //reset thanh tien do
        progressAmount = 0;
        progressSlider.value = 0;
    }
}
