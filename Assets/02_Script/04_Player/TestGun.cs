using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TestGun : MonoBehaviour
{
    public static TestGun instance;

    //public GameObject projectile;
    public float projectileForce;
    [SerializeField] private string shotSoundName;

    [SerializeField] private int currentBulletIndex = 0;
    private Item currentBullet;
    [SerializeField] private Item basicBullet;
    private Text damageTxt;
    private Text bulletNumber;
    private Image bulletImg;

    private Inventory inventory;

    private Transform aimTransform;
    private Transform aimGunEndPointTransform;
    private Animator aimAnimator;

    public event EventHandler<OnShootEventArgs> OnShoot;

    public class OnShootEventArgs : EventArgs
    {
        public Vector3 gunEndPointPosition;
        public Vector3 shootPosition;
    }

    private void Awake()
    {
        aimTransform = transform.Find("Aim");
        aimAnimator = aimTransform.GetComponent<Animator>();
        aimGunEndPointTransform = aimTransform.Find("GunEndPointPosition");
        if (instance == null)
        {
            instance = this;
            Initialize();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Aiming();
        Shooting();
        ChangeWeapon();
    }

    void Initialize()
    {
        inventory = FindObjectOfType<Inventory>();
        bulletNumber = GameObject.Find("BulletNumber").GetComponent<Text>();
        bulletImg = GameObject.Find("BulletImg").GetComponent<Image>();
        damageTxt = GameObject.Find("BulletDamage").GetComponent<Text>();
        
        currentBullet = basicBullet;
        currentBulletIndex = -1;
        bulletNumber.text = "무한대";
        damageTxt.text = "공격력 : " + currentBullet.value;
        bulletImg.sprite = currentBullet.itemImage;

    }

    private void Aiming()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimlocalScale = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            aimlocalScale.y = -1f;
        }
        else
        {
            aimlocalScale.y = +1f;
        }
        aimTransform.localScale = aimlocalScale;
    }

    private void Shooting()
    {

        if (GameController.instance.CanShot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                aimAnimator.SetTrigger("Shoot");

                OnShoot?.Invoke(this, new OnShootEventArgs
                {
                    gunEndPointPosition = aimGunEndPointTransform.position,
                    shootPosition = mousePosition,
                });
                //GameObject gun = Instantiate(projectile, aimGunEndPointTransform.position, Quaternion.identity);
                GameObject gun = Instantiate(currentBullet.prefab, aimGunEndPointTransform.position, Quaternion.identity);

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 myPos = transform.position;
                Vector2 direction = (mousePos - myPos).normalized;
                gun.GetComponent<Rigidbody2D>().velocity = direction * projectileForce;
                gun.GetComponent<ProjectileMoveScript>().damage = currentBullet.value;
                SoundManager.instance.PlaySE(shotSoundName, 0.1f);


                if(currentBullet != basicBullet)
                {
                    int cnt = int.Parse(bulletNumber.text);
                    cnt--;
                    bulletNumber.text = cnt.ToString();
                    inventory.ShotBullet(currentBullet.itemName);
                }
            }
        }
    }

    private void ChangeWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (inventory.bulletName.Count > 0)
            {
                currentBulletIndex = currentBulletIndex + 1 >= inventory.bulletName.Count ? -1 : currentBulletIndex + 1;
                SetBullet();
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (inventory.bulletName.Count > 0)
            {
                currentBulletIndex = currentBulletIndex - 1 < -1 ? inventory.bulletName.Count - 1 : currentBulletIndex - 1;
                SetBullet();
            }
        }
    }

    public void SetBulletInfo()
    {
        currentBulletIndex--;
        SetBullet();
    }

    private void SetBullet()
    {
        if (currentBulletIndex == -1)
        {
            currentBullet = basicBullet;
            bulletNumber.text = "무한";
        }
        else
        {
            currentBullet = inventory.GetBulletItem(currentBulletIndex);
            bulletNumber.text = inventory.GetBulletCnt(currentBullet.itemName);
        }
        bulletImg.sprite = currentBullet.itemImage;
        damageTxt.text = "공격력 : " + currentBullet.value.ToString();
    }
}
