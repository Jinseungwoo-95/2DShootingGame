using UnityEngine;
using UnityEngine.UI;

public class RandomItem : MonoBehaviour
{
    [SerializeField] Item[] bulletItems;
    [SerializeField] Item[] usedItems;
    [SerializeField] Sprite coinSprite;
    [SerializeField] private GameObject ItemAcquire;

    private void OnEnable()
    {
        ItemAcquire = GameObject.Find("UI").transform.GetChild(5).gameObject;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            int index = Random.Range(0, 3);
            int cnt;
            ItemAcquire.SetActive(true);
            Text acquireText = ItemAcquire.GetComponentInChildren<Text>();
            Image acquireImage = ItemAcquire.transform.GetChild(1).GetComponent<Image>();

            // 코인 획득
            if(index == 0)
            {
                GameController.instance.AddCoin(300);
                acquireImage.sprite = coinSprite;
                acquireText.text = "300원을 획득하셨습니다.";
            }
            // 불렛 획득
            else if(index == 1)
            {
                index = Random.Range(0, bulletItems.Length);
                cnt = Random.Range(1, 3);
                GameObject.FindObjectOfType<Inventory>().AcquireItem(bulletItems[index], cnt);
                acquireImage.sprite = bulletItems[index].itemImage;
                acquireText.text = bulletItems[index].itemName + "을 획득하셨습니다.";
            }
            else
            {
                index = Random.Range(0, usedItems.Length);
                cnt = Random.Range(1,4);
                GameObject.FindObjectOfType<Inventory>().AcquireItem(usedItems[index], cnt);
                acquireImage.sprite = usedItems[index].itemImage;
                acquireText.text = usedItems[index].itemName + "을 획득하셨습니다.";
            }
            Invoke("Deactivate", 2f);
            Destroy(gameObject);
        }
    }

    void Deactivate()
    {
        ItemAcquire.SetActive(false);
    }

}
