using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //��������� ������� ������� ����� ���������� (�������� ����������)//����� using System;
    public Action OnHealthChange = delegate { };//delegate { } - ������ ��������, ����� �� ���� ������ � ������, ���� ����� �� ����������
    public Action OnDeath = delegate { };//delegate { } - ������ ��������, ����� �� ���� ������ � ������, ���� ����� �� ����������



    public Bullet bulletPrefab; // ��� ����

    //������ GameObject shootPosition �� ���� �������� ����
    public GameObject shootPosition;
    public bool isDead = false;// ����� �����

    public float fireRate = 1f; //������� �������� ������ ���� (1f - ��� � �������)
    public int health = 100;//�������� ������

    float nextFire; //����� ������� ����� ���������� ��������� ������� ��������� �������

    Animator animator;//������ cc���� �� Animator � Player


    private void Awake()
    {
        animator= GetComponent<Animator>();//�������� cc���� �� Animator
    }

    public void UpdateHealth(int amount)
    {
        health += amount;//������ �������� �� ��������� ���-��
        OnHealthChange();

        if (health <= 0)//���� �������� ������ <= 0
        {
            isDead = true;//�� ����� �������
            animator.SetTrigger("Dead");//�������� ������
            OnDeath(); //������ ������
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckFire(); //��������

    }

    private void CheckFire()//��������
    {
        //GetKeyDown (������ � �����)
        //GetButtonDown (����������� ������ �� InputManager)
        //GetKeyDown � GetButtonDown �� �������
        //GetKey � GetButton ����� ��������

        //���� ������ �������� ������ � ����� ��� �������� ����� 
        if (Input.GetButton("Fire1") && nextFire <= 0)
        {
            Shoot(); //�������� � ���� �������
        }

        // �������� (�������� ������ ����� �� ���������� ��������)
        //�.�. �� 1 ��� �� 0 ���) � ����� �� ���� � �� ��������)
        if (nextFire > 0)
        {//Time.deltaTime;  �����,������� ������ � ����������� �����
         // ��������� ����� ����� �������� (��������: 1��� - ��������� �����)
            nextFire -= Time.deltaTime;
        }
    }

    private void Shoot()//�������� � ���� �������
    {
        animator.SetTrigger("Shoot");//�������� �������� �������� (�� �����) ��� ������ ����
                                     //����� ������� ����� ����,
                                     //� �������� ������� ������ ���� = ���� ������� +
                                     //+ ������ GameObject shootPosition,
                                     //� �������� �������� ������ ���� = ���� ��������)

        Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);

        //��������� ������� ����� �������� �����
        nextFire = fireRate;

        //������ ����������� ���
        //������� ������, ���� ����� ����, ���� �������, ������������ 10f
        //Debug.DrawRay(transform.position, (shootPosition.transform.position - transform.position) * 30, Color.green, 10f);

    }
}
