using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratingController : MonoBehaviour
{
    public GameObject enemyPrefab;

    public int maxEnemiesOnScene = 50;
    public int currentEnemiessOnScene = 0;
    [SerializeField] private int minEnemiesOnScene = 20;
    public List<GameObject> enemiesOnScene = new();

    private Coroutine generateCoroutine = null;
    // Update is called once per frame
    void Update()
    {
        currentEnemiessOnScene = GameController.instance.enemiesOnScene.Count;

        if (currentEnemiessOnScene < minEnemiesOnScene)
            generateCoroutine ??= StartCoroutine(GenerateEnemies());

    }

    private IEnumerator GenerateEnemies()
    {
        //Debug.Log("Gerando novos inimigos: " + minEnemiesOnScene);
        int offset = maxEnemiesOnScene - currentEnemiessOnScene;
        for (int i = 0; i < offset; i++)
        {
            yield return new WaitForSeconds(0.02f);
            currentEnemiessOnScene++;
            Vector3 rPos = new(Random.Range(-60, 60), 0, Random.Range(-60, 60));
            GameObject e = Instantiate(enemyPrefab, rPos, Quaternion.identity);
            GameController.instance.enemiesOnScene.Add(e);
        }

        generateCoroutine = null;
        minEnemiesOnScene = Random.Range(20, 35);
    }
}
