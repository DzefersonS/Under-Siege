using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist : MonoBehaviour
{

    [SerializeField] private float _speed = 1f;

    void Start()
    {
        MoveToXPosition(0);
    }

    void Update()
    {
    }

    public void MoveToXPosition(int xPos)
    {
        StartCoroutine(MoveToXPositionCoroutine(xPos));

    }

    private IEnumerator MoveToXPositionCoroutine(int xPos)
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
}

