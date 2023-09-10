using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [Header("Settings")]
    [SerializeField] float forwardSpeed = 5f;
    [SerializeField] float sideSpeed = 5f;
    [SerializeField] float moveRange = 5f;

    public List<Mob> mobList = new();

    [Header("References")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Mob mob;
    [SerializeField] TextMeshProUGUI mobCount;

    private bool isMoving = false;
    private bool canMove = true;

    GameManager game;
    Boss boss;

    void Start()
    {
        game = GameManager.Instance;        
    }

    public void LoadMobs()
    {
        Spawn(1);

        for (int i = 0; i < game.upgradeUnit; i++)
        {
            Spawn(1);
        }
    }

    void Update()
    {
        GetInput();
        Dead();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FinishLine"))
        {
            canMove = false;
        }
    }

    public void SpawnMob(int value, bool multiply = false)
    {
        if (multiply)
        {
            int newValue = mobList.Count * value;

            Spawn(newValue, true);
        }
        else
        {
            Spawn(value, true);
        }
    }

    public void Spawn(int count, bool running = false)
    {
        for (int i = 0; i < count; i++)
        {
            Mob newMob = Instantiate(mob, transform.position - Vector3.back, Quaternion.identity, transform);
            mobList.Add(newMob);
            if (running) newMob.Run(transform);
            newMob.Destiny(transform);
            mobCount.text = mobList.Count.ToString();
        }
    }

    public void RemoveMob(Mob mob)
    {
        mobList.Remove(mob);
        mobCount.text = mobList.Count.ToString();
    }

    void GetInput()
    {
        if (!canMove) return;

        if (Input.touchCount > 0)
        { 
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    rb.velocity = Vector3.forward * forwardSpeed;
                    foreach (var mob in mobList) mob.Run(transform);
                    CanvasManager.Instance.HideTutorial();
                    CanvasManager.Instance.HideUpgrades();
                }

                float touchDelta = touch.deltaPosition.x;

                // New position
                Vector3 newPosition = transform.position + sideSpeed * Time.deltaTime * touchDelta * Vector3.right;

                // Clamp the new position
                newPosition.x = Mathf.Clamp(newPosition.x, -moveRange, moveRange);

                // Update position
                transform.position = newPosition;
            }
        }
    }

    public void Fight(List<Enemy> enemyList)
    {
        StartCoroutine(Fighting(enemyList));
    }

    IEnumerator Fighting(List<Enemy> enemyList)
    {
        canMove = false;
        rb.velocity = Vector3.zero;

        while (mobList.Count > 0 && enemyList.Count > 0)
        {
            foreach (var mob in mobList)
            {
                if (enemyList.Count > 0)
                    mob.Destiny(enemyList[0].transform);
            }

            foreach (var enemy in enemyList)
            {
                if (mobList.Count > 0)
                    enemy.Attack(mobList[0].transform);
            }

            yield return new WaitForSeconds(.1f);
        }

        if (mobList.Count > 0)
        {
            foreach (var mob in mobList)
            {
                mob.Destiny(transform);
            }

            canMove = true;
            rb.velocity = Vector3.forward * forwardSpeed;
        }
        else
        {
            foreach (var enemy in enemyList)
            {
                enemy.Idle();
            }

            StartCoroutine(Lose());
        }
    }

    public void BossFight(Boss b)
    {
        boss = b;
        rb.velocity = Vector3.zero;
        foreach (var mob in mobList) mob.Destiny(boss.transform);
    }

    public void HitBoss()
    {
        if (boss != null) boss.TakeDamage();
    }

    public void Dead()
    {
        if (mobList.Count <= 0 && !game.levelComplete)
        {
            StartCoroutine(Lose());
        }
    }

    IEnumerator Lose()
    {
        yield return new WaitForSeconds(2);

        CanvasManager.Instance.GameOver();
    }
}
