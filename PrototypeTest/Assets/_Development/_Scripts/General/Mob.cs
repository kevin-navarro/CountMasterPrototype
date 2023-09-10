using UnityEngine;
using UnityEngine.AI;

public class Mob : MonoBehaviour
{
    [SerializeField] Animator anim;
    //[SerializeField] Rigidbody rb;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject splashVFX;

    Transform destination;
    bool triggered;

    public void Destiny(Transform pos)
    {
        destination = pos;
    }

    public void Run(Transform pos)
    {
        anim.SetBool("run", true);
        destination = pos;
    }

    private void Update()
    {
        if (destination != null)
        {
            agent.destination = destination.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !triggered)
        {
            if (other.TryGetComponent<Enemy>(out Enemy enemy))
            {
                triggered = true;
                enemy.Triggered();
                PlayerManager.Instance.RemoveMob(this);
                Instantiate(splashVFX, transform.position, Quaternion.identity);
                Destroy(gameObject,0.1f);
            }            
        }

        if (other.CompareTag("Obstacle"))
        {
            Instantiate(splashVFX, transform.position, Quaternion.identity);
            PlayerManager.Instance.RemoveMob(this);
            Destroy(gameObject, 0.1f);
        }

        if (other.CompareTag("Boss"))
        {
            Instantiate(splashVFX, transform.position, Quaternion.identity);
            PlayerManager.Instance.RemoveMob(this);
            PlayerManager.Instance.HitBoss();
            Destroy(gameObject, 0.1f);
        }
    }
}
