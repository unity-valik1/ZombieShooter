using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie1 : MonoBehaviour
{
    //��������� ������� ������� ����� ���������� (�������� ����������)//����� using System;
    public Action HealthChanged = delegate { };//delegate { } - ������ ��������, ����� �� ���� ������ � ������, ���� ����� �� ����������


    [Header("AI config")]
    public float moveRadius = 10; //������ �������������
    public float standbyRadius = 15; //������ ������ ������
    public float attackRadius = 3; //������ �����
    public int viewAngle = 90;//������ ������ ����� �����

    [Header("Gameplay config")]
    public float attackRate = 1f; //������� �������� ����� ����� (1f - ��� � �������)
    public int health = 100;//�������� �����
    public int damage = 20;//���� �����


    Player player;//���� cc���� �� ����� ������ Player

    ZombieState activeState;//��� � �������������� ���������� (����� ������� ������ 1 �� �������� �� enum ZombieState)

    Animator animator;//���� cc���� �� c��� ��������� Animator � Enemy
    AIPath aiPath;//���� cc���� �� c��� ��������� aiPath � Enemy //using Pathfinding;
    AIDestinationSetter aiDestinationSetter;

    float nextAttack; //����� ������� ����� ����������� ��������� ����� ��������� ����� 

    float distanceToPlayer;//��������� �� ������

    bool isDead = false;//����� �� �����

    Vector3 startPosition;//��������� �������

    //��������� ������������
    //�������� ����� ����� �����, ���� � �������
    enum ZombieState
    {
        //����� �������� (�������� �������)
        STAND,
        RETURN,
        MOVE_TO_PLATER,
        ATTACK
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();//�������� cc���� �� Animator � Enemy
        aiPath = GetComponent<AIPath>();//�������� cc���� �� aiPath � Enemy
        aiDestinationSetter = GetComponent<AIDestinationSetter>();//�������� cc���� �� aiDestinationSetter � Enemy
    }

    void Start()
    {
        player = FindObjectOfType<Player>();//�������� cc���� ����� ������ Player

        //��������� ������� = ������� �������
        startPosition = transform.position;
        //�������� ��������� ����� �� ������ ������� = �����
        ChangeState(ZombieState.STAND);//����� ����� ������ ��������, ������� � enum ZombieState

        player.OnDeath += PlayerDied;//������������� �� ������ playera
    }

    private void PlayerDied()//player ����
    {
        ChangeState(ZombieState.RETURN);//����� ��������� �� ���� �����
    }

    public void UpdateHealth(int amount)//����� ��������� ����� ����������� �������� ��������
    {
        health += amount;//�������� �������� �� �����������

        //���� �������� <= 0
        if (health <= 0)
        {
            isDead = true;//����� �����

            animator.SetTrigger("Dead");//�������� ������
            Destroy(gameObject, 0.9f); //���������� �����

            player.OnDeath -= PlayerDied;//������������ �� ������ playera (����� ������ �� ����������)
        }
        HealthChanged();//����� �������
    }


    private void OnTriggerEnter2D(Collider2D collision)//���� �� ����
    {
        Bullet bullet = collision.GetComponent<Bullet>();//�������� ���� � collision
        UpdateHealth(-bullet.damage);//�������� �������� �� ���� �� ����
    }

    void Update()
    {
        //���� ����
        if (isDead)
        {
            return;//������� �� Update() (������ �� ��������� ��� ����)
        }


        //��������� ����� ������� � �����
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


        //������ ������ ��� activeState
        switch (activeState)
        {
            case ZombieState.STAND://����� ����� �����
                DoStand();//����� ����� ����               
                break;
            case ZombieState.RETURN://����� ����� ������������ �� ��������� �������
                DoReturn();//����� ����� ����               
                break;
            case ZombieState.MOVE_TO_PLATER://����� ����� ����
                DoMove();//����� ����� �������
                break;
            case ZombieState.ATTACK://����� ����� �������
                DoAttack();//����� ����� ����
                break;
        }

    }


    private void ChangeState(ZombieState newState)
    {
        //������ ������ ��� activeState
        switch (newState)
        {
            case ZombieState.STAND://����� ����� �����
                aiPath.enabled = false;// �� ���������� ������, � ����� �� ����� (��������� ��������)
                break;
            case ZombieState.RETURN://����� ����� ������������ �� ��������� �������
                //aiDestinationSetter.target =
                //movement.targetPosiyion = startPosition;//����� ����� ������������ �� ��������� �������
                aiPath.enabled = true;// �� ���������� ������, � ���� �� ��������� �������
                break;
            case ZombieState.MOVE_TO_PLATER://����� ����� ����
                aiPath.enabled = true;//���������� ������
                aiDestinationSetter.target = player.transform;//����� ����� ���� �� �������
                break;
            case ZombieState.ATTACK://����� ����� �������
                aiPath.enabled = false;// ��������������� � �������
                break;
        }
        activeState = newState;//�������� ������� ����� ����� �������
    }

    private void DoStand()
    {
        if (!player.isDead) //���� Player �����
        {
            CheckMoveToPlayer();
        }
    }


    private void DoReturn()
    {
        if (!player.isDead && CheckMoveToPlayer())//���� Player ����� � ��������� �� ���� ������
        {
            return;//�� ������ �� ������, ������� �� ����� ������
        }

        //CheckMoveToPlayer();

        //��������� � ��������� �������
        //������� ����� ������� � ��������� �������
        float distanceToStart = Vector3.Distance(transform.position, startPosition);
        if (distanceToStart <= 0.05f)//���� ����� � ��������� ������� 
        {
            ChangeState(ZombieState.STAND);//�����111111111111111111111111111111111
            return;
        }
    }


    private bool CheckMoveToPlayer()//����� ������� ������������ �������� � ����� ��������� ����� ( �����, ���� � ��)
    {

        if (distanceToPlayer > moveRadius)
        {
            return false;
        }

        //��������� �����������
        //���� �� ����� ����� ����� ��� ���������
        //�� ������� ������ �������� ������� ������
        Vector3 directionToPlayer = player.transform.position - transform.position;

        //������ ����������� ���
        //������� �����, ������� ������, ���� �������
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);

        float angle =Vector3.Angle(-transform.up, directionToPlayer); //���� - ����� ����� � ������� ������
        if(angle > viewAngle/2)//���� ���� - ����� ����� � ������� ������ > ���� 90 �������� ����� �������
        {
            return false;//������ �� ������
        }

        LayerMask layerMask = LayerMask.GetMask("Obstacles");
        //RaycastHit2D ������� ��� � ��������� ���������� �� ����(���������  ���� �� �� ���� ���� ��� ������)
        //������� �����, ������� ������, ������ ����� ����� � �������, layerMask ???
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, directionToPlayer.magnitude, layerMask);
        if (hit.collider != null)//���� �������(���������) �� ���� ����
        {
            return false;
        }
        else//��� �������� �� ���� ����
        {

        }

        //������ �� �������
        ChangeState(ZombieState.MOVE_TO_PLATER);//����� ����� ����
        return true;
    }

    private void DoMove()
    {
        if (distanceToPlayer < attackRadius)
        {
            ChangeState(ZombieState.ATTACK);//����� ����� �������
            animator.SetFloat("Speed", 0);

            return;
        }
        if (distanceToPlayer > standbyRadius)
        {
            ChangeState(ZombieState.RETURN);//����� ����� ������������ �� ��������� �������
            animator.SetFloat("Speed", 0);

            return;
            //�������� ����� �� �����
        }
        animator.SetFloat("Speed", 1);
    }


    private void DoAttack()
    {
        if (distanceToPlayer > attackRadius)
        {
            ChangeState(ZombieState.MOVE_TO_PLATER);//����� ����� ����
            StopAllCoroutines();//���������� ��� �������� ������� ��������� � ����� �������
            return;
        }
        //�� �� ��� � ��������
        //Time.deltaTime;  �����,������� ������ � ����������� �����
        // ��������� ����� ����� ����� (��������: 1��� - ��������� �����)
        nextAttack -= Time.deltaTime;

        ////���� �� ���� ����� �������� < 0
        if (nextAttack <= 0)
        {
            animator.SetTrigger("Shoot");//�������� �����

            //player.UpdateHealth(-damage);// �������� ������� ������

            nextAttack = attackRate;// ��������� ����� �������� ����� ������� �����
        }
    }


    public void DamageToPlayer()
    {
        //���� ��������� �� ������ ������ ��� ������ ����� �����
        if (distanceToPlayer > attackRadius)
        {
            // �� ����� ������ �� ������� � �� ������� ���� ������
            return;
        }
        player.UpdateHealth(-damage);// �������� ������� ������
    }


    private void OnDrawGizmos() //������ ��� ������� ��� ������������ � ��� ��������� ��
    {
        Gizmos.color = Color.blue;//���������� ������ �����
        //(������ ����� = ������� ������, ������ ����� = ������ �������������)
        Gizmos.DrawWireSphere(transform.position, moveRadius);

        Gizmos.color = Color.red;//���������� �������� �����
        //(������ ����� = ������� ������, ������ ����� = ������ �����)
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.yellow;//���������� ������� �����
        //(������ ����� = ������� ������, ������ ����� = ������ ������ ������)
        Gizmos.DrawWireSphere(transform.position, standbyRadius);


        Gizmos.color = Color.cyan;//���������� ������� �����
        Vector3 lookDirection = -transform.up;//������ ����� ������
        //= ���� ������ ���(���� �� ������� ����� ����������, ��� �� ������� ����� ����������) * �� ������ ����� ������
        Vector3 leftViewVector = Quaternion.AngleAxis(viewAngle / 2, Vector3.forward) * lookDirection;
        //= ���� ������ ���( -���� �� ������� ����� ����������, ��� �� ������� ����� ����������) * �� ������ ����� ������
        Vector3 rightViewVector = Quaternion.AngleAxis(-viewAngle / 2, Vector3.forward) * lookDirection;
        Gizmos.DrawRay (transform.position, lookDirection * moveRadius);//���(������� �����, ������ ����� ������ * �� ������ ������ ������)
    }
}
