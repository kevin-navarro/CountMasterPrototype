using UnityEngine;

public class BossArea : MonoBehaviour
{
    [SerializeField] Boss boss;
    bool fighting;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob") && !fighting)
        {
            fighting = true;
            PlayerManager.Instance.BossFight(boss);
            boss.Attack();
            CameraManager.Instance.SetTarget(boss.transform);
        }
    }
}
