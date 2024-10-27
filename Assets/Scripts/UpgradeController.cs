using UnityEngine;

public enum UpgradeID
{
    Damage,
    AttackSpeed,
    MovementSpeed,
    COUNT
}

public class UpgradeController : MonoBehaviour
{
    [SerializeField] private UpgradesSO[] m_UpgradeSOs;

    private int[] m_UpgradeTiers = default;

    private void Awake()
    {
        m_UpgradeTiers = new int[(int)UpgradeID.COUNT];
    }

    public int RetrieveUpgradeLevel(UpgradeID abilityID)
    {
        return m_UpgradeTiers[(int)abilityID];
    }

    public bool UpgradeAbility(UpgradeID abilityID)
    {
        int currentTier = m_UpgradeTiers[(int)abilityID];
        int cost = 0;
        // do cost checking logic, if can buy upgrade:
        m_UpgradeTiers[(int)abilityID]++;
        return true; //return that it was a success to whatever initiated this operation
                     // else 
        return false;
    }
}