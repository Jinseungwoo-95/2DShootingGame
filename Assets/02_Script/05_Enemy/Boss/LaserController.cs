using UnityEngine;

public class LaserController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            GameController.instance.DamagePlayer(1);
    }
}
