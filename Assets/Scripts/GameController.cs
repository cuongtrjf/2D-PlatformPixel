using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    int progressAmount;//tien do nhat gem
    public Slider progressSlider;//thanh tien do level

    public GameObject player;
    public GameObject loadCanvas;
    public List<GameObject> levels = new List<GameObject>();
    private int currentLevelIndex = 0;


    public static event Action OnReset;
    public static event Action OnRestart;


    //game over or surrival level
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public GameObject completeScreen;
    public TMP_Text passLvText;
    private int passLvCount;


    // Start is called before the first frame update
    void Start()
    {
        progressAmount = 0;
        Gem.OnGemCollect += IncreaseProgressAmount;//dang ky su kien OnGemCollect cho Gem
        //no se kich hoat cac phuong thuc, trong truong hop nay chi co mot phuong thuc IncreaseProgressAmount
        HoldToLoadLevel.OnHoldComplete += LoadNextLevel;//dki su kien load level khi Hold R
        PlayerHealth.OnPlayedDied += GameOverScreen;

        progressSlider.value = 0;

        loadCanvas.SetActive(false);

        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        completeScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        MusicManager.PlayerBackgroundMusic(true);//reset lai bai hat
        passLvCount = 0;//reset choi lai tu dau
        LoadLevel(0, false);
        OnReset.Invoke();

        Time.timeScale = 1;//thoi gian tiep tuc
    }



    private void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        MusicManager.PauseBackgroundMusic();
        passLvText.text = "PASS " + passLvCount + " LEVEL";
        if (passLvCount > 1)
        {
            passLvText.text += "S";
        }
        Time.timeScale = 0;//khi chet thi dung hoat dong
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



    private void LoadLevel(int levelNext,bool stateCompleteLV)
    {
        player.transform.parent = null;
        loadCanvas.SetActive(false);//thanh tai level

        levels[currentLevelIndex].gameObject.SetActive(false);
        levels[levelNext].gameObject.SetActive(true);

        player.transform.position = new Vector3(0, 0, 0);
        player.GetComponent<SpriteRenderer>().color= new Color32(216, 134, 217, 255);
        currentLevelIndex = levelNext;


        //reset thanh tien do
        progressAmount = 0;
        progressSlider.value = 0;
        if(stateCompleteLV) passLvCount++;//phuc vu load text pass level
    }

    private void LoadNextLevel()
    {
        //neu level hien tai chua phai level cuoi chua, neu roi thi level tiep theo bang 1, con k thi bang lv +1
        //int nextLevelIndex = (currentLevelIndex == levels.Count - 1) ? 0 : currentLevelIndex + 1;
        //LoadLevel(nextLevelIndex,true);
        if(currentLevelIndex == levels.Count - 1)//level cuoi
        {
            StartCoroutine(ActiveScreenComplete());
        }
        else
        {
            LoadLevel(currentLevelIndex + 1, true);
        }
    }

    IEnumerator ActiveScreenComplete()
    {
        //ham active
        yield return new WaitForSeconds(1f);
        completeScreen.SetActive(true);
        Time.timeScale = 0;
    }

    public void PlayAgain()
    {
        completeScreen.SetActive(false);
        MusicManager.PlayerBackgroundMusic(true);//reset lai bai hat
        passLvCount = 0;//reset choi lai tu dau
        LoadLevel(0, false);
        OnReset.Invoke();

        Time.timeScale = 1;//thoi gian tiep tuc
    }

    public void OnPause()
    {
        pauseScreen.SetActive(true);
        MusicManager.PauseBackgroundMusic();
        Time.timeScale = 0;
    }

    public void OnContinue()
    {
        pauseScreen.SetActive(false);
        MusicManager.PlayerBackgroundMusic(false);
        Time.timeScale = 1;
    }


    public void OnClickRestart()
    {
        pauseScreen.SetActive(false);
        MusicManager.PlayerBackgroundMusic(true);
        Time.timeScale = 1;
        //passLvCount = currentLevelIndex;
        OnRestart.Invoke();
        RestartLevel();
    }

    public void RestartLevel()
    {
        LoadLevel(currentLevelIndex,false);
    }


    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }
}
