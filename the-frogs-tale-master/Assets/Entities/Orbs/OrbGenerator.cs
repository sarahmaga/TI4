using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbGenerator : MonoBehaviour
{
    [SerializeField] GameObject healthOrb;
    [SerializeField] GameObject skillOrb;

    private const float durationLimit = 10f;

    private const int lowerX = -4, lowerY = -6, higherX = 4, higherY = 4;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawner());
    }

    IEnumerator EnemySpawner()
    {
        while (true)
        {
            GameObject newHealthOrb = Instantiate(healthOrb, new Vector2(Random.Range(lowerX, higherX + 1) + .5f,
            Random.Range(lowerY, higherY + 1) + .5f), Quaternion.identity);

            GameObject newSkillOrb = Instantiate(skillOrb, new Vector2(Random.Range(lowerX, higherX + 1) + .5f,
                Random.Range(lowerY, higherY + 1) + .5f), Quaternion.identity);

            Destroy(newHealthOrb, durationLimit);
            Destroy(newSkillOrb, durationLimit);

            yield return new WaitForSeconds(15);
        }
    }
}
