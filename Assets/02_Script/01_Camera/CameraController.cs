using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Room currRoom;

    private BoxCollider2D bound;
    private GameObject target;   // 플레이어!!
    [SerializeField] float moveSpeed;

    private Vector3 targetPosition; // 대상의 현재 위치 값

    // 박스 컬라이더 영역의 최소 최대 xyz값을 지님
    private Vector3 minBound;
    private Vector3 maxBound;

    // 카메라의 반너비, 반높이 값을 지닐 변수
    private float halfWidth;
    private float halfHeight;

    // 흔들림 효과
    [SerializeField] private float shakeAmount;
    private float shakeTime;
    public float ShakeTime { set { shakeTime = value; } }

    public BoxCollider2D Bound { set { bound = value; } get { return bound; } }

    void Awake()
    {
        Instance = this;
        halfHeight = GetComponent<Camera>().orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;  // Screen.width / Screen.height 는 해상도를 뜻함
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        UpdatePosition();
    }

    /// <summary>
    /// target의 이동에 따른 카메라 위치 변경
    /// </summary>
    void UpdatePosition()
    {
        if (shakeTime >= 0)
        {
            Vector2 shakePos = Random.insideUnitSphere * shakeAmount;
            transform.position = transform.position + (Vector3)shakePos;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            if (target.gameObject != null)
            {
                targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);

                this.transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed);

                // 카메라 영역 설정
                float clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
                float clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);

                transform.position = new Vector3(clampedX, clampedY, transform.position.z);
            }
        }
    }

    /// <summary>
    ///  맵이동으로 인한 Bound 변경
    /// </summary>
    /// <param name="newBound"></param>
    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}