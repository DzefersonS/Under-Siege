using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utils;





public class Cultist : MonoBehaviour
{

    [SerializeField] private float _speed = 1f;

    [SerializeField] private State state = State.Idle;

    [SerializeField] private Animator m_Animator;

    private Transform _graveyardLocation;

    [SerializeField] private DeadBodyEventSO _deadBodyEventSO;



    //All bools will be deleted once state system is fully done, for now , let them stay
    public bool IsCarryingBody { get; private set; }
    public bool isBusy { get; private set; }
    public bool isIdling { get; set; }



    public void MoveToXPosition(float xPos)
    {
        StartCoroutine(MoveToXPositionRoutine(xPos));
    }

    private void Start()
    {
        LocateGraveyard();
    }


    private void LocateGraveyard()
    {
        Collider2D[] bodiesInRange = Physics2D.OverlapCircleAll(transform.position, 9999);

        foreach (Collider2D body in bodiesInRange)
        {
            // Check if the collider is tagged "DeadBody"

            if (body.name == "Graveyard")
            {
                _graveyardLocation = body.transform;
            }
        }
    }

    private IEnumerator MoveToXPositionRoutine(float xPos)
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
    public void Idle()
    {

        //This requires a bit of fixing
        // StartCoroutine(IdleRoutine());
    }

    private IEnumerator CollectDeadBodyCoroutine(GameObject _deadbody)
    {
        Transform closestBody = _deadbody.transform;

        if (closestBody != null)
        {
            isBusy = true;
            state = State.Collect;

            m_Animator.SetTrigger("Sprint");

            yield return MoveToXPositionRoutine(closestBody.position.x);
            m_Animator.SetTrigger("Idle");

            //If other cultist managed to pick up the body before this one(shouldn't happen, but just in case)
            if (closestBody == null)
            {
                state = State.Idle;
                isBusy = false;
                yield break;
            }
            // Destroy the dead body once close enough
            m_Animator.SetTrigger("Collect");
            yield return new WaitForSeconds(1f);

            closestBody.GetComponent<DeadBody>().FreeToPool();
            IsCarryingBody = true;
            state = State.Carry;

            // Enable child component (dead body) of cultist
            transform.GetChild(0).gameObject.SetActive(true);

            m_Animator.SetTrigger("Carry");

            // Walk towards the grave
            yield return MoveToXPositionRoutine(_graveyardLocation.position.x); //for now its -10, will put a graveyard position later

            transform.GetChild(0).gameObject.SetActive(false);//Disable the deadbody object

            IsCarryingBody = false;

            // Switch to idle state
            isBusy = false;
            state = State.Idle;

        }
        else
        {
            state = State.Idle;
            isBusy = false;
            Debug.Log("No DeadBody found in range.");
        }
    }

    //Will be implemented in the future
    IEnumerator IdleRoutine()
    {
        isIdling = true;
        state = State.Idle;
        //Bounds for base, once we have them
        float xBoundsMin = -10;
        float xBoundsMax = 10;

        StartCoroutine(MoveToXPositionRoutine(Random.Range(xBoundsMin, xBoundsMax)));

        yield return null;

    }

    public void ChangeState(State newState)
    {
        state = newState;
    }

    public State GetState()
    {
        return state;
    }

    public void Death()
    {
        //Initiate the death sequence

    }



    public enum State
    {
        Idle,
        Collect,
        Carry,
        COUNT
    }


}

