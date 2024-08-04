using UnityEngine;

public class HammerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 5f;
    private GroundChecker groundChecker;
    private bool isGrounded;

    private Vector3 direction;
    private Animator animator;
    private GameObject player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        groundChecker = GetComponent<GroundChecker>();
        player = BossStateController.instance.player;
        SetPlayerPosition();
    }

    private void Update()
    {
        isGrounded = groundChecker.CheckGround();

        if (!isGrounded)
        {
            MoveHammer();
        }
        else
        {
            animator.SetTrigger("Landed");
            Destroy(gameObject);
        }
    }

    private void MoveHammer()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetPlayerPosition()
    {
        direction = player.transform.position - transform.position;
        direction.x += (5 * BossStateController.instance.direction);
    }


}
