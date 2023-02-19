using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenAiMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float avoidDistance = 2f;
    [SerializeField] private LayerMask Wall;

    private Vector3 targetPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPosition = hit.point;
                targetPosition.y = transform.position.y;
            }
        }

        Vector3 moveDirection = (targetPosition - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget > avoidDistance)
        {
            if (Physics.Raycast(transform.position, moveDirection, out RaycastHit hit, avoidDistance, obstacleLayer))
            {
                Vector3 normal = hit.normal.normalized;
                moveDirection = Vector3.ProjectOnPlane(moveDirection, normal);
            }
        }

        transform.rotation = Quaternion.LookRotation(moveDirection);
        transform.position += moveDirection * movementSpeed * Time.deltaTime;
    }
}
