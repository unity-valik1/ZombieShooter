using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieUI : MonoBehaviour
{
    public Zombie zombie;//������ �� Zombie (����� ��� ������� ���)

    public Slider healthSlider;//������ �� Slider (����� ��� ������� ���)

    //public Image healthSlider2;//������ �� Slider2

    void Start()
    {
        healthSlider.maxValue = zombie.health;//������������ �������� = �������� �����
        healthSlider.value = zombie.health;//�������� �������� = �������� �����

        zombie.HealthChanged += UpdateHealthBar;//���������� ��������� ��������
    }


    public void UpdateHealthBar(/*int health*/)//���������� ��������� ��������
    {
        healthSlider.value = zombie.health; //���������� �������� � ������
    }


    private void LateUpdate()//����������� ����� Update() ����� �� ���������
    {
        transform.rotation = Quaternion.identity;//Quaternion.identity - ������� �������� �������
    }
}
