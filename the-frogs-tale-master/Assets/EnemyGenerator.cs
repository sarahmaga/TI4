using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject basicEnemy;
    [SerializeField] GameObject rangedEnemy;

    private const int lowerX1 = -17, lowerY1 = 2, higherX1 = -13, higherY1 = 6;
    private const int lowerX2 = 14, lowerY2 = 2, higherX2 = 18, higherY2 = 6;
    private const int lowerX3 = -17, lowerY3 = -12, higherX3 = -13, higherY3 = -8;
    private const int lowerX4 = 14, lowerY4 = -12, higherX4 = 18, higherY4 = -8;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }

    IEnumerator EnemySpawner()
    {
        while (true)
        {
            int option = Random.Range(0, 2);
            Instantiate(option == 0 ? basicEnemy : rangedEnemy, new Vector2(Random.Range(lowerX1, higherX1 + 1) + .5f,
                Random.Range(lowerY1, higherY1 + 1) + .5f), Quaternion.identity);

            option = Random.Range(0, 2);
            Instantiate(option == 0 ? basicEnemy : rangedEnemy, new Vector2(Random.Range(lowerX2, higherX2 + 1) + .5f,
                Random.Range(lowerY2, higherY2 + 2) + .5f), Quaternion.identity);

            option = Random.Range(0, 2);
            Instantiate(option == 0 ? basicEnemy : rangedEnemy, new Vector2(Random.Range(lowerX3, higherX3 + 1) + .5f,
               Random.Range(lowerY3, higherY3 + 1) + .5f), Quaternion.identity);

            option = Random.Range(0, 2);
            Instantiate(option == 0 ? basicEnemy : rangedEnemy, new Vector2(Random.Range(lowerX4, higherX4 + 1) + .5f,
                Random.Range(lowerY4, higherY4 + 1) + .5f), Quaternion.identity);
            
            yield return new WaitForSeconds(10);
        }
    }
}
