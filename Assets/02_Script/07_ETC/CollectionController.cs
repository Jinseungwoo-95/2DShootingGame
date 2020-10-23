using UnityEngine;
public class CollectionController : MonoBehaviour
{
    [SerializeField] private float coin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameController.instance.AddCoin(coin);
            Destroy(gameObject);
        }
    }
}
