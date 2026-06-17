using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MovingObject
{
    public int size;
    [SerializeField] private GameObject crumbs;

    private void OnCollisionEnter(Collision collision)
    {
        // compare size of food to player to see if big enough to eat
        if (collision.gameObject.tag == "Player" && SizeHUD.Instance.curSize >= size)
        {
            collision.gameObject.GetComponent<Animator>().SetTrigger("EatFood");
            StartCoroutine(EatFood());
        }
        else if (collision.gameObject.tag == "Player" && SizeHUD.Instance.curSize <= size)
        {
            gameObject.layer = collision.gameObject.layer;
        }
    }

    IEnumerator EatFood()
    {
        Instantiate(crumbs, transform.position, crumbs.transform.rotation);
        yield return new WaitForSeconds(0.25f);
        SizeHUD.Instance.GrowPlayer();
        Destroy(gameObject);
    }
}
