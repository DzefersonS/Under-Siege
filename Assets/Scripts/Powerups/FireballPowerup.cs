using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FireballPowerup : MonoBehaviour
{
    [SerializeField] private AudioSource m_DeniedActionAudioSource;

    public GameObject fireballPrefab;
    public Transform firePoint;
    public Image cooldownOverlay;
    public TMP_Text cooldownText;
    public float cooldownDuration = 5f;
    public GameObject aoeIndicator;

    private float cooldownTimer = 0f;
    private bool isAiming = false;
    private Vector3 targetPosition;

    private void Start()
    {
        aoeIndicator.SetActive(false);
    }

    private void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;

            float cooldownProgress = cooldownTimer / cooldownDuration;
            cooldownOverlay.fillAmount = cooldownProgress;
            cooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;
            cooldownText.text = "";
        }

        // Handle input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (cooldownTimer > 0.0f)
            {
                m_DeniedActionAudioSource.Play();
            }
            else if (cooldownTimer <= 0.0f && !isAiming)
            {
                StartAiming();
            }
            else if (isAiming)
            {
                Fire();
            }
        }

        if (isAiming)
        {
            UpdateAOEIndicator();
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        aoeIndicator.SetActive(true);
    }

    private void Fire()
    {
        isAiming = false;
        aoeIndicator.SetActive(false);

        targetPosition = aoeIndicator.transform.position;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        Fireball fireballScript = fireball.GetComponent<Fireball>();
        if (fireballScript != null)
        {
            fireballScript.SetTarget(targetPosition);
        }

        cooldownTimer = cooldownDuration;
        cooldownOverlay.fillAmount = 1.0f;
    }

    private void UpdateAOEIndicator()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0.0f;
        aoeIndicator.transform.position = new Vector3(cursorPosition.x, aoeIndicator.transform.position.y, 0.0f);
    }
}
