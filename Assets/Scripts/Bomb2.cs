using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb2 : MonoBehaviour
{
    //����� � ��������� ��� � ���������

    public GameObject explosionEffectPrefab; //������ ������ �� �����-�� GameObject

    void Awake()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //��� ��� ������ ����

        Explode();

    }


    private void Explode()
    {
        //������� �����, � ����� �������, ��� ��������� 
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        //���������� �����
        Destroy(gameObject);
    }
}
