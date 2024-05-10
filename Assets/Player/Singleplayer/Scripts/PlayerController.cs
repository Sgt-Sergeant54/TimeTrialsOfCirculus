using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField] private Rigidbody2D rb2d;

    private IPlayerState currentState;

    public float playerSpeed;
    public UnityEngine.Vector2 direction;

    public float jumpHeight;
    public bool jumpTwice;

    [SerializeField] private float groundingDistance;
    public bool IsOnWall;

    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    [SerializeField] private float distance;

    public float teleDistance;
    [SerializeField] private int teleportCount;
    [SerializeField] private CanvasGroup[] teleportChargeCanvas;
    [SerializeField] private float teleportRecharge;

    public float grappleSpeed;

    private List<Tutorial> guards = new List<Tutorial>();

    [SerializeField] private Transform sprite;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint = GetComponent<DistanceJoint2D>();

        distanceJoint.enabled = false;

        teleportCount = 3;
        teleportRecharge = 0f;

        jumpTwice = false;

        updateTeleCount();

        currentState = new PlayerStanding();

        IsOnWall = false;
    }

    private void Update()
    {
        if (transform.position.y < -100)
        {
            Respawn();
        }

        teleportRecharge += Mathf.Abs(rb2d.velocityX) + Mathf.Abs(rb2d.velocityY);

        if (teleportRecharge > 9000 && teleportCount < 3)
        {
            teleportCount++;
            updateTeleCount();
            teleportRecharge = 0;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && GetComponent<ZeroGravity>() == null && teleportCount > 0)
        {
            UnityEngine.Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            UnityEngine.Vector2 playerPosition = (UnityEngine.Vector2)transform.position;

            UnityEngine.Vector2 direction = mousePos - playerPosition;

            LayerMask mask = LayerMask.GetMask("Ground");
            LayerMask progressionMask = LayerMask.GetMask("ProgressionFlag");

            float distance = teleDistance;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, mask);
            RaycastHit2D progressionFlagHit = Physics2D.Raycast(transform.position, direction, distance, progressionMask);

            if (progressionFlagHit.collider != null)
            {
                progressionFlagHit.collider.gameObject.GetComponent<ProgressionFlag>().IsPassed = true;
            }

            if (hit.collider == null)
            {

                transform.position = UnityEngine.Vector2.MoveTowards(playerPosition, (direction * teleDistance) + playerPosition, teleDistance);
            }
            else
            {
                transform.position = UnityEngine.Vector2.MoveTowards(playerPosition, hit.point - direction.normalized, teleDistance);
            }
            ZeroGravity zeroGrav = gameObject.AddComponent<ZeroGravity>();
            zeroGrav.effectDuration = 0.3f;
            teleportCount -= 1;
            updateTeleCount();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (Tutorial t in guards)
            {
                t.nextPrompt();
            }
        }
        PlayerInputs input = new PlayerInputs();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            input.jump = true;
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            input.grappleIn = true;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            input.grapple = true;
        }

        input.onWall = IsOnWall;

        input.moveHorizontal = Input.GetAxisRaw("Horizontal");

        UpdateState(input);

        if (direction.x > 0)
        {
            sprite.Rotate(new UnityEngine.Vector3(transform.rotation.x, transform.rotation.y, sprite.rotation.z - 9));
        }
        else if (direction.x < 0)
        {
            sprite.Rotate(new UnityEngine.Vector3(transform.rotation.x, transform.rotation.y, sprite.rotation.z + 9));
        }
        
    }

    private void FixedUpdate()
    {
        
        float changedPlayerSpeed = playerSpeed;

        if (GetComponent<Damage>() != null)
        {
            changedPlayerSpeed *= GetComponent<Damage>().damageEffect;
        }

        transform.Translate(direction * Time.fixedDeltaTime * changedPlayerSpeed);
    }

    private void UpdateState(PlayerInputs input)
    {
        IPlayerState newState = currentState.Tick(this, input);

        if (newState != null)
        {
            currentState.Exit(this);
            currentState = newState;
            currentState.Enter(this);
        }
    }

    public bool groundCheck()
    {
        LayerMask mask = LayerMask.GetMask("Ground");
        if (GetComponent<GravityInverter>() == null)
        {
            if (Physics2D.Raycast(transform.position, UnityEngine.Vector2.down, groundingDistance, mask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }else
        {
            if (Physics2D.Raycast(transform.position, UnityEngine.Vector2.up, groundingDistance, mask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void Respawn()
    {
        transform.position = LevelManager.instance.currentCheckPoint;
        mainCamera.transform.position = transform.position;
        if (GetComponent<Damage>() != null)
        {
            Destroy(GetComponent<Damage>());
        }
        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
        LevelManager.instance.newLapReset();
    }

    private void updateTeleCount()
    {
        foreach (CanvasGroup c in teleportChargeCanvas)
        {
            c.alpha = 0;
        }
        teleportChargeCanvas[teleportCount].alpha = 1;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            IsOnWall = true;   
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            IsOnWall = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Tutorial>() != null)
        {
            guards.Add(collision.gameObject.GetComponent<Tutorial>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Tutorial>() != null)
        {
            guards.Remove(collision.gameObject.GetComponent<Tutorial>());
        }
    }
}
