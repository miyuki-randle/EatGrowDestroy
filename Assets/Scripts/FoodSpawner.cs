using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : Spawner
{
    //[SerializeField] private GameObject platform;
    [SerializeField] private FoodItem[] objStage1;
    [SerializeField] private FoodItem[] objStage2;
    [SerializeField] private FoodItem[] objStage3;
    [SerializeField] private FoodItem[] objStage4;

    [SerializeField]private List<FoodItem[]> foodPrefabs;

    new void Start()
    {
        base.Start();
        foodPrefabs = new List<FoodItem[]>()
        {
            objStage1,
            objStage2,
            objStage3,
            objStage4
        };
    }

    protected override IEnumerator ObjectGenerator()
    {
        yield return new WaitForSeconds(delay);
        if (active)
        {
            FoodItem randomPrefab = foodPrefabs[GameManager.Instance.stage][Random.Range(0, foodPrefabs[GameManager.Instance.stage].Length)];

            FoodItem obj = Instantiate(randomPrefab, transform.position, randomPrefab.transform.rotation);
            obj.transform.localScale *= scaleFactor;
            ResetDelay();
        }
        StartCoroutine(ObjectGenerator());
    }
}
