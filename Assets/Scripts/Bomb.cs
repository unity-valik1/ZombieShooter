using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{ 
    //����� � ��������� ��� � � ������

    public LayerMask damageLayers;//���������� ����� LayerMask
    public float radius = 5f;//������ ������
    public int damage = 30;//���� �� ������ �����

    Animator animator;//������ cc���� �� Animator � Bomb


    void Awake()
    {
        animator = GetComponent<Animator>();//�������� cc���� �� Animator
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.GetComponent<Bullet>();//����� ����� ������������ � �������� -
                                                         //�� ���������������� ������ ����
        print("damage: " + bullet.damage);

        Explode();
    }


    private void Explode()
    {
        animator.SetTrigger("Explosion"); //�������� �������� ������ ��� �������
        Destroy(gameObject, 0.7f); //���������� �����

        //� Layer �������� ���� "Player", "Zombie" � ������ �� ����� ��������� ���� � ������� ������ �����
        //LayerMask layerMask = LayerMask.GetMask("Player", "Zombie");

        //������� ������ �����������
        //������� �����, ������ ������, ���� �� ���� ������� ������� � gameObject 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, damageLayers);

        foreach (Collider2D collider in colliders)
        {
            //if (collider.gameObject.CompareTag("Player"))
            //{
            //    Player player = collider.GetComponent<Player>();
            //}

            //������� gameObject ��������� "UpdateHealth" � �������� damage
            collider.gameObject.SendMessage("UpdateHealth", -damage);
        }
    }

    //������ ��� ������� ��� ������������ � ��� ��������� ��
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;//���������� ������� �����
        //(������ ����� = ������� �����, ������ ����� = ������ ������)
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
