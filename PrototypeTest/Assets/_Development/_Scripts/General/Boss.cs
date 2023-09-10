using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] int hp = 20;
    [SerializeField] Animator anim;
    [SerializeField] BoxCollider[] cols;

    bool dead;
    bool attacking;

    public void Attack()
    {
        if (attacking) return;
        attacking = true;
        anim.SetTrigger("attack");
    }

    public void TakeDamage()
    {
        hp -= 1;

        if (hp <= 0 && !dead)
        {
            foreach (var col in cols)
            {
                col.enabled = false;
            }
            dead = true;
            anim.SetTrigger("die");
            CanvasManager.Instance.LevelComplete();
        }
    }
}
