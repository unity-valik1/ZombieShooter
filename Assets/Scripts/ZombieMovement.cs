using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMovement : MonoBehaviour
{

    public float speed = 10f; //�������� ������������

    public Vector3 targetPosiyion;//����� �������� �������

    //Player player; //���� cc���� �� ����� ������ Player

    Rigidbody2D rb;//���� cc���� �� c��� ��������� Rigidbody2D � Enemy

    Animator animator;//���� cc���� �� c��� ��������� Animator � Enemy

    void Awake()//���� ���������� ���� ����� Awake
    {
        rb = GetComponent<Rigidbody2D>();//�������� cc���� �� c��� ��������� Rigidbody2D
        animator = GetComponent<Animator>();//�������� cc���� �� c��� ��������� Animator
    }

    //private void Start()//������ ������� ���� ����� Start
    //{
    //    player = FindObjectOfType<Player>();//�������� cc���� ����� ������ Player
    //}


    void Update()
    {    

        Move(); //�������� 
        Rotate(); //������� 

    }

    private void Move()
    {

        //transform �������� � Vector3, ����� ������������ Vector3
        Vector3 zombiePosition = transform.position; //������� �����

        //������� ������ (���� ����� �������� �����)
        //Vector3 playerPosition = player.transform.position;

        //������� ������� ������-�� �������, ������� �� ������
        Vector3 direction = targetPosiyion - zombiePosition;


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
        Vector3 zombiePosition = transform.position; //������� �����

        //������� ������ (���� ����� �������� �����)
        //Vector3 playerPosition = player.transform.position;

        //������� ������� ������-�� �������, ������� �� ������
        Vector3 direction = targetPosiyion - zombiePosition;

        //z = ����, �������� �� ����� ����������
        direction.z = 0;

        //������� ������� ������� (transform.up - ������� �������)
        transform.up = -direction;

    }


    //����������,����� ������ ����������� (�������� ����������� ������)
    private void OnDisable()
    {
        rb.velocity = Vector2.zero;//�������� ������������ ����� ������������ �� 0
    }
}
