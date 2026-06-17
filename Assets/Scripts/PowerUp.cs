using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MovingObject
{
    public Color color;
    //public PowerType type;
    public IEnumerator power;
    public Renderer render;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        render = GetComponent<Renderer>();
        render.material.color = color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.ActivatePowerUp(this);
            }
        }
    }
}