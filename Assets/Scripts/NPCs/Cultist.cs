using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,
    Collect,
    COUNT
}



public class Cultist : MonoBehaviour
{

    [SerializeField] private float _speed = 1f;
    [SerializeField] private LayerMask bodyLayer;
    [SerializeField] private State state = State.Idle;

    private static HashSet<Transform> claimedBodies = new HashSet<Transform>();


    void Start()
    {
        CollectDeadBody();

    }

    void Update()
    {

    }

    public void MoveToXPosition(float xPos)
    {
        StartCoroutine(MoveToXPositionCoroutine(xPos));

    }

    private IEnumerator MoveToXPositionCoroutine(float xPos)
    {
        while (Mathf.Abs(transform.position.x - xPos) > 0.01f)
        {
            // Determine the direction of movement
            Vector2 direction = (xPos < transform.position.x) ? Vector2.left : Vector2.right;

            // Rotate the cultist based on direction
            if (direction == Vector2.left)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Face left
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
            }

            transform.Translate(direction * _speed * Time.deltaTime, Space.World);

            yield return null;
        }
    }

    private void CollectDeadBody()
    {
        StartCoroutine(CollectDeadBodyCoroutine());
    }

    private IEnumerator CollectDeadBodyCoroutine()
    {
        Transform closestBody = FindClosestDeadBody();

        if (closestBody != null)
        {
            yield return MoveToXPositionCoroutine(closestBody.position.x);


            // Destroy the dead body once close enough
            Debug.Log("Destroyed DeadBody at position: " + closestBody.position);
            Destroy(closestBody.gameObject);

            // Enable child component (dead body) of cultist
            transform.GetChild(0).gameObject.SetActive(true);


            // Walk towards the grave
            yield return MoveToXPositionCoroutine(-10); //for now its -10, will put a graveyard position later

            // Give money (or Graveyard itself will give money
            // ...
            transform.GetChild(0).gameObject.SetActive(false);// first child element of cultist

            claimedBodies.Remove(closestBody);

            // Switch to idle state
            // ...
        }
        else
        {
            Debug.Log("No DeadBody found in range.");
        }
    }

    private Transform FindClosestDeadBody()
    {
        // Find all colliders
        Collider2D[] bodiesInRange = Physics2D.OverlapCircleAll(transform.position, 9999);

        Transform closestBody = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D body in bodiesInRange)
        {
            // Check if the collider is tagged "DeadBody"
            if (body.CompareTag("DeadBody") && !claimedBodies.Contains(body.transform))
            {
                // Calculate the distance from the cultist to body
                float distance = Vector2.Distance(transform.position, body.transform.position);

                // Update the closest body if this one is closer
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBody = body.transform;
                }
            }
        }

        if (closestBody != null)
        {
            Debug.Log("Closest DeadBody found at position: " + closestBody.position);
            claimedBodies.Add(closestBody); // To ensure, that only 1 cultist will try to carry 1 body
        }
        else
        {
            Debug.Log("No DeadBody found in range.");
        }

        return closestBody;
    }

}

