using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSpawner : Spawner
{
    [SerializeField] private FlyingObstacle[] flyingPrefabs;

    new void Start()
    {
        base.Start();
        delay = 10f;
        delayRange = new Vector3(5, 15);
    }

    protected override IEnumerator ObjectGenerator()
    {
        yield return new WaitForSeconds(delay);
        if (active)
        {
            float y = Random.Range(0f, 5f);

            FlyingObstacle obj = Instantiate(flyingPrefabs[GameManager.Instance.stage], transform);
            obj.transform.position = new Vector3(transform.position.x, y, 0);
            obj.transform.localScale *= scaleFactor;
            ResetDelay();
        }
        StartCoroutine(ObjectGenerator());
    }
}
