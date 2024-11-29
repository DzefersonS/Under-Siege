using UnityEngine;

public class RestartCredits : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void ReplayAnimation()
    {
        animator.Play("Credits Roll", 0, 0f);
    }
}
