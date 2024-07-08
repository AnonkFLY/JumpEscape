using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSc : MonoBehaviour
{

    public float radius;     // 圆形区域半径

    private void Update()
    {
        // 检测圆形范围内是否有其他2D物体
        Collider2D collider = Physics2D.OverlapCircle(transform.position, radius);
        collider?.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        // 在Scene视图中绘制圆形范围
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
