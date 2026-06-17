using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : Spawner
{
    [SerializeField] private PowerUp prefab;
    private PlayerController player;

    new void Start()
    {
        base.Start();
        player = FindObjectOfType<PlayerController>();
    }

    protected override IEnumerator ObjectGenerator()
    {
        yield return new WaitForSeconds(delay);
        if (active && !FindObjectOfType<PlayerController>().hasPowerUp)
        {
            float y = Random.Range(-6f, 6f);
            Vector3 randPosition = new Vector3(transform.position.x, y, transform.position.z);

            PowerUp powerUp = Instantiate(prefab, transform.position, transform.rotation);
            PowerType type = (PowerType)Random.Range(0, 3);

            switch (type)
            {
                case PowerType.Life:
                    powerUp.power = player.LifePower();
                    powerUp.color = Color.red;
                    break;
                case PowerType.Strength:
                    powerUp.power = player.StrengthPower();
                    powerUp.color = Color.blue;
                    break;
                case PowerType.Jump:
                    powerUp.power = player.JumpPower();
                    powerUp.color = Color.green;
                    break;
            }
            ResetDelay();
        }
        StartCoroutine(ObjectGenerator());
    }
}

public enum PowerType
{
    Life,
    Strength,
    Jump,
}