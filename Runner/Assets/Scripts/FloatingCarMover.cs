using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCarMover : MonoBehaviour
{
    private Vector3 target;
    private float speed;

    public void SetTarget(Vector3 targetPos, float moveSpeed)
    {
        target = targetPos;
        speed = moveSpeed;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            Destroy(gameObject); // Destruir cuando llegue al destino
        }
    }
}
