using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSc : MonoBehaviour
{

    public float radius;     // Բ������뾶

    private void Update()
    {
        // ���Բ�η�Χ���Ƿ�������2D����
        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius);
        collider?.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        // ��Scene��ͼ�л���Բ�η�Χ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
