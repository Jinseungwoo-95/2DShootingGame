using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    public ParticleSystem ps;
    private Vector2 targetPos;
    public float dashRange;
    public float speed;
    public bool dashCoolDown;
    private Vector2 direction;
    private Animator animator;
    private enum Facing {UP,DOWN,LEFT,RIGHT};
    private Facing FacingDir = Facing.DOWN;

    RaycastHit2D rayHit;
    float DashDistance = 2f;
    int layerMask;

    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float dashCoolTime;
    [SerializeField] private Image dashImage;
    private float curDashCoolTime;
    private int audioIndex = 0;
    private bool isSound = false;
    private WaitForSeconds waitTime;
    BoxCollider2D boxCollider;
    void Start()
    {
        animator = GetComponent<Animator>();
        waitTime = new WaitForSeconds(0.3f);
        boxCollider = GetComponent<BoxCollider2D>();
        dashImage = GameObject.Find("CoolTimeImage").GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (GameController.instance.CanMove)
        {
            TakeInput();
            if (!dashCoolDown)
            {
                TakeDash();
            }
            else
            {
                curDashCoolTime -= Time.deltaTime;
                dashImage.fillAmount = curDashCoolTime / dashCoolTime;
                if(curDashCoolTime <= 0)
                {
                    dashCoolDown = false;
                }
            }
            Move();
        }
    }

    private void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        if(direction.x != 0 || direction.y != 0)
        {
            SetAnimatorMovement(direction);
            
            if (!audio.isPlaying && !isSound && SoundManager.instance.effectOn)
            {
                StartCoroutine(SoundDelay());
                audio.clip = audioClips[audioIndex];
                audio.Play();
            }
        }
        else
        {
            animator.SetLayerWeight(1, 0);
            if (audio.isPlaying)
                audio.Stop();
        }

    }

    private void TakeInput()
    {
        direction = Vector2.zero;

        if(Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
            FacingDir = Facing.UP;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
            FacingDir = Facing.LEFT;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
            FacingDir = Facing.DOWN;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
            FacingDir = Facing.RIGHT;
        }
    }
    private void TakeDash()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 currentPos = transform.position;
            targetPos = Vector2.zero;
            
            if (FacingDir == Facing.UP)
            {
                targetPos.y = 1;
            }
            else if (FacingDir == Facing.DOWN)
            {
                targetPos.y = -1;
            }
            else if (FacingDir == Facing.LEFT)
            {
                targetPos.x = -1;
            }
            else if (FacingDir == Facing.RIGHT)
            {
                targetPos.x = 1;
            }

            Debug.DrawRay(transform.position, direction * DashDistance, Color.blue, 0.2f);
            layerMask = 1 << LayerMask.NameToLayer("Obstacle");
            rayHit=Physics2D.Raycast(transform.position, direction, DashDistance,layerMask);
            if (rayHit.collider==null)
            {
                StartCoroutine("DashRun");
            }
        }
    }
    IEnumerator DashRun()
    {
        boxCollider.enabled = false;
        dashCoolDown = true;
        curDashCoolTime = dashCoolTime;
        ps.Play();
        float count=0;
        while (dashRange > count)
        {
            count += 0.1f;
            transform.Translate(targetPos * 0.05f);
            yield return new WaitForSeconds(0.005f);
        }
        boxCollider.enabled = true;
    }
    private void SetAnimatorMovement(Vector2 direction)
    {
        animator.SetLayerWeight(1, 1);
        animator.SetFloat("xDir", direction.x);
        animator.SetFloat("yDir", direction.y);
    }

    IEnumerator SoundDelay()
    {
        audioIndex = (audioIndex > 0) ? 0 : 1;
        isSound = true;
        yield return waitTime;
        isSound = false;
    }
}
