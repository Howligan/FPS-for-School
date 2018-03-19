using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour {

    public float health = 100f;

    bool isInView;

    public Transform target;

    int cycle = 10;
    int counter = 0;

    public void Tick()
    {
        counter++;

        if (counter > cycle)
        {
            FieldOfView();
            counter = 0;
        }

        if (isInView)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0;
            if (direction == Vector3.zero)
                direction = transform.forward;

            float angle = Vector3.Angle(transform.forward, direction);

            if (angle < 45)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void FieldOfView()
    {
        if (target)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            isInView = (distance < 200);
        }

    }
}
