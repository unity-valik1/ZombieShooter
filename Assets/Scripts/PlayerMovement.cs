using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f; //�������� ������������

    Rigidbody2D rb;//������ cc���� �� Rigidbody2D � Player

    Animator animator;//������ cc���� �� Animator � Player

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();//�������� cc���� �� Rigidbody2D
        animator= GetComponent<Animator>();//�������� cc���� �� Animator
    }


    void Update()
    {
        Move(); //��������
        Rotate(); //�������

    }

    private void Move()
    {
        //2D ������(Rigidbody2D) �������� � Vector2, ����� ������������ Vector2

        float inputX = Input.GetAxis("Horizontal");//�������� �����-������
        float inputY = Input.GetAxis("Vertical");//�������� �����-����

        //�������� ����������� �������
        Vector2 direction = new Vector2(inputX, inputY);

        //��������
        //���� �� ��������� ��������� ������� 
        if (direction.magnitude > 1)
        {
            //�� ������������� ��������
            direction = direction.normalized; 
        }

        //� �������� ���������� ����� �������� ��������
        //���� �������� ��������� �������� > 0
        //���� �� ��������� �������� = 0
        animator.SetFloat("Speed", direction.magnitude);

        //�������� �������
        rb.velocity = direction * speed;



        ////�������� ��������� �� �������� ��� w-a-s-d
        //Vector3 padNewPosition = transform.position;//����� ������� ��� ���������

        //float inputX = Input.GetAxis("Horizontal");//�������� �����-������
        //float inputY = Input.GetAxis("Vertical");//�������� �����-����

        //padNewPosition.x += speed * Time.deltaTime * inputX;//�������� �����-������
        //padNewPosition.y += speed * Time.deltaTime * inputY;//�������� �����-����

        //transform.position = padNewPosition;//��������� ����� �������

    }

    private void Rotate()
    {
        //transform �������� � Vector3, ����� ������������ Vector3
        Vector3 playerPosition = transform.position; //������� ������

        //������� ����� (���� ����� �������� �����)
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //������� ������� ����(�������)
        Vector3 direction = mousePosition - playerPosition;

        //z = ����, �������� �� ����� ����������
        direction.z = 0;

        //������� ������� ������� (transform.up - ������� �������)
        transform.up = -direction; 

    }
}
