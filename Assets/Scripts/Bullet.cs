using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    public float speed = 20f; //�������� ����
    public int damage = 10;//���� ����

    Rigidbody2D rb; //������ cc���� �� Rigidbody2D � ����

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//�������� cc���� �� Rigidbody2D

    }


    private void Start()
    {
        //����������� ���� 
        rb.velocity = -transform.up * speed;
    }


    //���� �� ����� - ������� ��
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
