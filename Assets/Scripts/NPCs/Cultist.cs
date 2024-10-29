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
    [SerializeField] private State state = State.Idle;
    [SerializeField] private bool isBusy = false;


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

    public void CollectDeadBody(GameObject _deadbody)
    {
        StartCoroutine(CollectDeadBodyCoroutine(_deadbody));
    }

    private IEnumerator CollectDeadBodyCoroutine(GameObject _deadbody)
    {
        Transform closestBody = _deadbody.transform;
        state = State.Collect;

        if (closestBody != null)
        {
            isBusy = true;

            yield return MoveToXPositionCoroutine(closestBody.position.x);

            if (closestBody == null)
            {
                isBusy = false;
                StopAllCoroutines();
            }
            // Destroy the dead body once close enough
            Destroy(closestBody.gameObject);

            // Enable child component (dead body) of cultist
            transform.GetChild(0).gameObject.SetActive(true);


            // Walk towards the grave
            yield return MoveToXPositionCoroutine(-10); //for now its -10, will put a graveyard position later

            transform.GetChild(0).gameObject.SetActive(false);// first child element of cultist

            // Switch to idle state
            isBusy = false;
        }
        else
        {
            isBusy = false;
            Debug.Log("No DeadBody found in range.");
        }
    }

    public bool IsBusy()
    {
        return isBusy;
    }

}

