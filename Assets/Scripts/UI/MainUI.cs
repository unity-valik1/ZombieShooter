using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUI : MonoBehaviour
{
    public Slider playerHealth;//ссылка на Slider
    //TODO public Image playerPortrait;// фотка игрока
    public Player player;//Cсылка на Player

    public GameObject gameOverPanel;//панель конца игры


    void Start()
    {
        //Player player = FindObjectOfType<Player>();
        player.OnHealthChange += UpdateHealth;//подписался изменение здоровья

        player.OnDeath += ShowGameOver;//подписался на смерть

        playerHealth.maxValue = player.health;//максимальное здоровье = здоровье игрока
        playerHealth.value = player.health;//ползунок здоровья = здоровье игрока

    }

    //private void ShowGameOver()//подписался на смерть
    //{
    //    gameOverPanel.SetActive(true);//открывается панель конца игры
    //}   
    private void ShowGameOver()//подписался на смерть
    {
        StartCoroutine(ShowGameOverWhithDelay(1));//TODO 1 to field через секунду
    }


    IEnumerator ShowGameOverWhithDelay (float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverPanel.SetActive(true);//открывается панель конца игры
    }

    private void UpdateHealth()//подписался изменение здоровья
    {
        playerHealth.value = player.health;//изменяется здоровье с уроном
    }
}
