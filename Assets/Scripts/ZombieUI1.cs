using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieUI1 : MonoBehaviour
{
    public Zombie1 zombie1;//������ �� Zombie (����� ��� ������� ���)

    public Slider healthSlider;//������ �� Slider (����� ��� ������� ���)

    //public Image healthSlider2;//������ �� Slider2

    void Start()
    {
        healthSlider.maxValue = zombie1.health;//������������ �������� = �������� �����
        healthSlider.value = zombie1.health;//�������� �������� = �������� �����

        zombie1.HealthChanged += UpdateHealthBar;//���������� ��������� ��������
    }


    public void UpdateHealthBar(/*int health*/)//���������� ��������� ��������
    {
        healthSlider.value = zombie1.health; //���������� �������� � ������
    }


    private void LateUpdate()//����������� ����� Update() ����� �� ���������
    {
        transform.rotation = Quaternion.identity;//Quaternion.identity - ������� �������� �������
    }
}
