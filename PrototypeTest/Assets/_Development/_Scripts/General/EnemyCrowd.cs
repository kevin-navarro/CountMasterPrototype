using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCrowd : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int enemyCount = 5;

    [SerializeField] List<Enemy> enemiesList = new();

    [Header("References")]
    [SerializeField] Image bubble;
    [SerializeField] SpriteRenderer[] redArea;
    [SerializeField] Enemy enemyPrefab;
    [SerializeField] TextMeshProUGUI enemyCountTMP;
    [SerializeField] Transform spawnPos;

    bool fighting;

    private void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Enemy newEnemy = Instantiate(enemyPrefab, spawnPos.position, Quaternion.identity,transform);
            enemiesList.Add(newEnemy);
            newEnemy.Destiny(transform);
            newEnemy.dead += RemoveEnemy;
        }

        enemyCountTMP.text = enemiesList.Count.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob") && !fighting)
        {
            fighting = true;
            PlayerManager.Instance.Fight(enemiesList);
        }
    }

    void RemoveEnemy(Enemy enemy)
    {
        enemiesList.Remove(enemy);

        if (enemiesList.Count <= 0)
        {
            bubble.DOFade(0, .5f);
            foreach (var area in redArea) area.DOFade(0, .25f);
        }

        enemy.dead -= RemoveEnemy;
        enemyCountTMP.text = enemiesList.Count.ToString();
        Destroy(enemy.gameObject);
    }
}
