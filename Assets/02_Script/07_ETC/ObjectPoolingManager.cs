using System.Collections.Generic;
using UnityEngine;

public enum BulletName
{
    Green,
    Dark,
}

public class ObjectPoolingManager : MonoBehaviour
{
    public static ObjectPoolingManager instance = null;

    [SerializeField] private GameObject basicBulletPrefab;
    [SerializeField] private GameObject followBulletPrefab;
    [SerializeField] private GameObject boundBulletPrefab;
    [SerializeField] private RuntimeAnimatorController[] bulletAnims;  // 불렛들 애니메이터컨트롤러

    // 복제한 불렛과 Animator 리스트
    [SerializeField] private List<GameObject> basicBulletList;
    private List<Animator> basicBulletAnimatorList;
    [SerializeField] private List<GameObject> followBulletList;
    private List<Animator> followBulletAnimatorList;
    [SerializeField] private List<GameObject> boundBulletList;
    private List<Animator> boundBulletAnimatorList;

    public const string BASIC = "BASIC", FOLLOW = "FOLLOW", BOUND = "BOUND";

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        basicBulletList = new List<GameObject>();
        basicBulletAnimatorList = new List<Animator>();

        followBulletList = new List<GameObject>();
        followBulletAnimatorList = new List<Animator>();

        boundBulletList = new List<GameObject>();
        boundBulletAnimatorList = new List<Animator>();
    }
    
    /// <summary>
    /// 비활성화 불렛 Get함수
    /// </summary>
    /// <param name="_type">불렛 종류</param>
    /// <returns></returns>
    public GameObject GetBullet(string _type, BulletName _bulletName = BulletName.Green)
    {
        // 불렛 리스트 체크 => 비활성화 불렛 리턴
        switch(_type)
        {
            case BASIC:
                for (int i = 0; i < basicBulletList.Count; i++)
                {
                    if (!basicBulletList[i].activeSelf)
                    {
                        basicBulletAnimatorList[i].runtimeAnimatorController = bulletAnims[(int)_bulletName];
                        basicBulletList[i].SetActive(true);
                        return basicBulletList[i];
                    }
                }
                break;
            case FOLLOW:
                for (int i = 0; i < followBulletList.Count; i++)
                {
                    if (!followBulletList[i].activeSelf)
                    {
                        followBulletAnimatorList[i].runtimeAnimatorController = bulletAnims[(int)_bulletName];
                        followBulletList[i].SetActive(true);
                        return followBulletList[i];
                    }
                }
                break;
            case BOUND:
                for (int i = 0; i < boundBulletList.Count; i++)
                {
                    if (!boundBulletList[i].activeSelf)
                    {
                        boundBulletAnimatorList[i].runtimeAnimatorController = bulletAnims[(int)_bulletName];
                        boundBulletList[i].SetActive(true);
                        return boundBulletList[i];
                    }
                }
                break;
        }

        // 불렛 부족 => 불렛 생산 후 리턴
        return GenerateBullet(_type, bulletAnims[(int)_bulletName]);
    }

    /// <summary>
    /// 불렛 생성
    /// </summary>
    /// <param name="_type">불렛 종류</param>
    /// <returns></returns>
    private GameObject GenerateBullet(string _type, RuntimeAnimatorController _animCtl)
    {
        GameObject obj = null;
        Animator animator;

        switch (_type)
        {
            case BASIC:
                obj = Instantiate(basicBulletPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                animator = obj.GetComponent<Animator>();
                animator.runtimeAnimatorController = _animCtl;
                basicBulletAnimatorList.Add(animator);
                basicBulletList.Add(obj);
                break;
            case FOLLOW:
                obj = Instantiate(followBulletPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                animator = obj.GetComponent<Animator>();
                animator.runtimeAnimatorController = _animCtl;
                followBulletAnimatorList.Add(animator);
                followBulletList.Add(obj);
                break;
            case BOUND:
                obj = Instantiate(boundBulletPrefab, Vector3.zero, Quaternion.identity);
                obj.transform.SetParent(gameObject.transform);
                animator = obj.GetComponent<Animator>();
                animator.runtimeAnimatorController = _animCtl;
                boundBulletAnimatorList.Add(animator);
                boundBulletList.Add(obj);
                break;
        }

        return obj;
    }
}
