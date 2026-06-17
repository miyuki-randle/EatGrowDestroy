using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : Spawner, IEventListener<GameEvent>
{
    [SerializeField] private GroundObstacle[] prefabs;
    [SerializeField] private GroundObstacle[] buildings;

    public bool strengthActive = false;

    protected override IEnumerator ObjectGenerator()
    {
        yield return new WaitForSeconds(delay);
        if (active)
        {
            float height = Random.Range(1f, 2.5f);
            float width = Random.Range(1f, 7f);

            GroundObstacle prefab = GameManager.Instance.stage == 3 ? buildings[Random.Range(0, buildings.Length)] : prefabs[GameManager.Instance.stage];
            //BoxCollider collider = prefab.gameObject.GetComponent<BoxCollider>();

            Vector3 newScale = new Vector3(width, height, 3) * scaleFactor;
            //float calcY = transform.position.y + ((platform.transform.position.y + platform.transform.localScale.y / 2) - (collider.center.y - collider.size.y * scaleFactor / 2));

            GroundObstacle obj = Instantiate(prefab, transform.position, prefab.transform.rotation);
            obj.transform.localScale = newScale;
            //obj.transform.position = new Vector3(transform.position.x, calcY, transform.position.z);

            if (strengthActive)
            {
                obj.breakable = true;
                obj.healthBar.gameObject.SetActive(true);
            }

            ResetDelay();
        }
        StartCoroutine(ObjectGenerator());
    }
}
