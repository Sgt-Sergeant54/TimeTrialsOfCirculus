using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class PlayerControllerNetwork : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb2d;

    private IPlayerStateNetwork currentState;

    [SerializeField] private float playerSpeed;
    public NetworkVariable<float> direction = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public float jumpHeight;
    public bool jumpTwice;

    [SerializeField] private float groundingDistance;
    public bool isOnWall;

    public NetworkVariable<bool> grappleActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> grappleX = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<float> grappleY = new NetworkVariable<float>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public DistanceJoint2D distanceJoint;
    [SerializeField] private float distance;

    public float teleDistance;
    [SerializeField] private int teleportCount;
    [SerializeField] private CanvasGroup[] teleportChargeCanvas;
    [SerializeField] private float teleportRecharge;

    public float grappleSpeed;
    [SerializeField] private Transform sprite;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            grappleActive.Value = false;
            grappleX.Value = 0;
            grappleY.Value = 0;
            direction.Value = 0;
        }
    }

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        distanceJoint = GetComponent<DistanceJoint2D>();
        distanceJoint.enabled = false;

        if (!IsOwner) return;
        rb2d = GetComponent<Rigidbody2D>();
        
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        GameObject.FindWithTag("Cinemachine").GetComponent<CinemachineVirtualCamera>().Follow = gameObject.transform;

        isOnWall = false;
        

        teleportCount = 3;
        teleportRecharge = 0f;

        jumpTwice = false;

        teleportChargeCanvas = new CanvasGroup[4];
        teleportChargeCanvas[0] = GameObject.FindGameObjectWithTag("Zero").GetComponent<CanvasGroup>();
        teleportChargeCanvas[1] = GameObject.FindGameObjectWithTag("One").GetComponent<CanvasGroup>();
        teleportChargeCanvas[2] = GameObject.FindGameObjectWithTag("Two").GetComponent<CanvasGroup>();
        teleportChargeCanvas[3] = GameObject.FindGameObjectWithTag("Three").GetComponent<CanvasGroup>();

        updateTeleCount();

        currentState = new PlayerStandingNetwork();
    }

    private void Update()
    {
        if (IsOwner)
        {
            if (RaceManager.instance.raceStart.Value && RaceManager.instance.localRaceStart)
            {
                transform.position = RaceManager.instance.currentCheckPoint;
                RaceManager.instance.RaceStarting();
            }


            if (transform.position.y < -50)
            {
                transform.position = new Vector2(0, 0);
            }

            teleportRecharge += Mathf.Abs(rb2d.velocityX) + Mathf.Abs(rb2d.velocityY);

            if (teleportRecharge > 9000 && teleportCount < 3)
            {
                teleportCount++;
                updateTeleCount();
                teleportRecharge = 0;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) && teleportCount > 0)
            {

                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 playerPosition = (Vector2)transform.position;

                Vector2 direction = mousePos - playerPosition;

                LayerMask mask = LayerMask.GetMask("Ground");
                LayerMask progressionMask = LayerMask.GetMask("ProgressionFlag");

                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, teleDistance, mask);
                RaycastHit2D progressionFlagHit = Physics2D.Raycast(transform.position, direction, teleDistance, progressionMask);

                if (progressionFlagHit.collider != null)
                {
                    progressionFlagHit.collider.gameObject.GetComponent<ProgressionFlagNetwork>().IsPassed = true;
                }

                if (hit.collider == null)
                {

                    transform.position = UnityEngine.Vector2.MoveTowards(playerPosition, (direction * teleDistance) + playerPosition, teleDistance);
                }
                else
                {
                    transform.position = UnityEngine.Vector2.MoveTowards(playerPosition, hit.point - direction.normalized, teleDistance);
                }
                ZeroGravityNetwork zeroGrav = gameObject.AddComponent<ZeroGravityNetwork>();
                zeroGrav.effectDuration = 0.3f;
                teleportCount -= 1;
                updateTeleCount();

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

            input.onWall = isOnWall;

            input.moveHorizontal = Input.GetAxisRaw("Horizontal");

            UpdateState(input);
        }

        if (direction.Value > 0)
        {
            sprite.Rotate(new UnityEngine.Vector3(transform.rotation.x, transform.rotation.y, sprite.rotation.z - 9));
        }
        else if (direction.Value < 0)
        {
            sprite.Rotate(new UnityEngine.Vector3(transform.rotation.x, transform.rotation.y, sprite.rotation.z + 9));
        }

        if (!IsOwner)
        {
            if (grappleActive.Value)
            {
                lineRenderer.SetPosition(0, new Vector2(grappleX.Value, grappleY.Value));
                lineRenderer.SetPosition(1, transform.position);
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;
        float changedPlayerSpeed = playerSpeed;

        if (GetComponent<Damage>() != null)
        {
            changedPlayerSpeed *= GetComponent<Damage>().damageEffect;
        }

        Vector2 movement = new Vector2(direction.Value, 0);

        transform.Translate(movement * Time.fixedDeltaTime * changedPlayerSpeed);
    }

    private void UpdateState(PlayerInputs input)
    {
        IPlayerStateNetwork newState = currentState.Tick(this, input);
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
        if (GetComponent<GravityInverterNetwork>() == null)
        {
            if (Physics2D.Raycast(transform.position, UnityEngine.Vector2.down, groundingDistance, mask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
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
        transform.position = NetworkGameManager.instance.currentCheckPoint;
        mainCamera.transform.position = transform.position;
        if (GetComponent<Damage>() != null)
        {
            Destroy(GetComponent<Damage>());
        }
        distanceJoint.enabled = false;
        lineRenderer.enabled = false;
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
        if (!IsOwner) return;
        if (collision.gameObject.tag == "Wall")
        {
            isOnWall = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsOwner) return;
        if (collision.gameObject.tag == "Wall")
        {
            isOnWall = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;
        if (collision.GetComponent<ProgressionFlagNetwork>() != null && RaceManager.instance.levelStart)
        {
            collision.GetComponent<ProgressionFlagNetwork>().IsPassed = true;
        }

        if (collision.gameObject.GetComponent<CheckPointFlagNetwork>() != null)
        {
            if (RaceManager.instance.endFlag == collision.GetComponent<CheckPointFlagNetwork>() && RaceManager.instance.raceStart.Value == true)
            {
                RaceManager.instance.newLap();
            }
        }
    }
}
