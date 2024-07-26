using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject mainImage;
    public Sprite gameOverSpr;
    public Sprite gameClearSpr;
    public GameObject panel;
    public GameObject restartButton;
    public GameObject nextButton;

    Image titleImage;
    
    //시간제한 관련
    public GameObject timeBar;
    public GameObject timeText;
    TimeController timeCnt;

    //점수 관련
    public GameObject scoreText;
    public static int totalScore;
    public int stageScore = 0;

    public AudioClip meGameOver; //게임 오버
    public AudioClip meGameClear; //게임 클리어
    void Start()
    {
        Invoke("InactiveImage", 1.0f);
        panel.SetActive(false);

        timeCnt = GetComponent<TimeController>();
        if(timeCnt != null){
            if(timeCnt.gameTime == 0.0f){
                timeBar.SetActive(false);
            }
        }
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.gameState == "gameclear"){
            mainImage.SetActive(true);
            Debug.Log("clear");
            panel.SetActive(true);
            Button bt = restartButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameClearSpr;
            PlayerController.gameState = "gameend";

            if (timeCnt != null){
                timeCnt.isTimeOver = true;
                int time = (int)timeCnt.displayTime;
                totalScore += time * 10;
            }

            totalScore += stageScore;
            stageScore = 0;
            UpdateScore();

            //사운드 재생
            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                Debug.Log("123");
                //BGM 정지
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameClear);
            }
        }
        else if (PlayerController.gameState == "gameover"){
            mainImage.SetActive(true);
            panel.SetActive(true);
            Button bt = nextButton.GetComponent<Button>();
            bt.interactable = false;
            mainImage.GetComponent<Image>().sprite = gameOverSpr;
            PlayerController.gameState = "gameend";

            if (timeCnt != null){
                timeCnt.isTimeOver = true;
            }

            AudioSource soundPlayer = GetComponent<AudioSource>();
            if (soundPlayer != null)
            {
                Debug.Log("123");
                soundPlayer.Stop();
                soundPlayer.PlayOneShot(meGameOver);
            }
        }
        else if (PlayerController.gameState == "playing"){
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            PlayerController playerCnt = player.GetComponent<PlayerController>();

            if (timeCnt != null){
                if (timeCnt.gameTime > 0.0f){
                    int time = (int)timeCnt.displayTime;
                    timeText.GetComponent<Text>().text = time.ToString();
                    if (time == 0){
                        playerCnt.GameOver();
                    }
                }
            }
            if (playerCnt.score != 0){
                stageScore += playerCnt.score;
                playerCnt.score = 0;
                UpdateScore();
            }
        }
    }
    void InactiveImage(){
        mainImage.SetActive(false);
    }

    void UpdateScore(){
        int score = stageScore + totalScore;
        scoreText.GetComponent<Text>().text = score.ToString();
    }
}
