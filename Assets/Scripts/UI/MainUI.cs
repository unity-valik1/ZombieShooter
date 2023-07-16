using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainUI : MonoBehaviour
{
    public Slider playerHealth;//������ �� Slider
    //TODO public Image playerPortrait;// ����� ������
    public Player player;//C����� �� Player

    public GameObject gameOverPanel;//������ ����� ����


    void Start()
    {
        //Player player = FindObjectOfType<Player>();
        player.OnHealthChange += UpdateHealth;//���������� ��������� ��������

        player.OnDeath += ShowGameOver;//���������� �� ������

        playerHealth.maxValue = player.health;//������������ �������� = �������� ������
        playerHealth.value = player.health;//�������� �������� = �������� ������

    }

    //private void ShowGameOver()//���������� �� ������
    //{
    //    gameOverPanel.SetActive(true);//����������� ������ ����� ����
    //}   
    private void ShowGameOver()//���������� �� ������
    {
        StartCoroutine(ShowGameOverWhithDelay(1));//TODO 1 to field ����� �������
    }


    IEnumerator ShowGameOverWhithDelay (float delay)
    {
        yield return new WaitForSeconds(delay);
        gameOverPanel.SetActive(true);//����������� ������ ����� ����
    }

    private void UpdateHealth()//���������� ��������� ��������
    {
        playerHealth.value = player.health;//���������� �������� � ������
    }
}
