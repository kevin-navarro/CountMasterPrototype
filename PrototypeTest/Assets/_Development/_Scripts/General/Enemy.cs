using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject splashVFX;

    Transform destination;
    bool attacking;
    bool triggered;

    public Action<Enemy> dead;

    private void Update()
    {
        if (destination != null)
        {
            agent.destination = destination.position;
            transform.rotation = Quaternion.LookRotation(-Vector3.forward);
        }
    }

    public void Destiny(Transform pos)
    {
        destination = pos;
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(.25f);

        if (!attacking) destination = null;
    }

    public void Attack(Transform pos)
    {
        attacking = true;
        anim.SetBool("run", true);
        destination = pos;
    }

    public void Triggered()
    {
        if (triggered) return;
        triggered = true;
        Instantiate(splashVFX, transform.position, Quaternion.identity);
        dead?.Invoke(this);
    }

    public void Idle()
    {
        anim.SetBool("run", false);
        destination = null;
    }
}
