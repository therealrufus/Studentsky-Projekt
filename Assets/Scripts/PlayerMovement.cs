using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Range(0, 1)]
    [Tooltip("(not working) the minimal angle to be grounded")] public float groundedAngle;
    public float gravity = 10f;

    public PlayerMoveOption[] moves;
    int currentMove;
    bool hasDuration;

    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public Vector3 SPEED;
    [HideInInspector]
    public bool grounded;
    [HideInInspector]
    public Vector3 arrowInput;
    [HideInInspector]
    public Vector3 rawArrowInput;

    public Text speedText;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        foreach (var move in moves)
        {
            move.master = this;
        }
    }

    void Update()
    {
        GetMovementInput();

        if (!hasDuration)
            CheckMoves();
        else hasDuration = moves[currentMove].ShouldContinue();

        moves[currentMove].Move();

        ApplyMovement();

        speedText.text = Mathf.Round(SPEED.magnitude).ToString();
        speedText.color = grounded ? Color.black : Color.grey;
    }

    void GetMovementInput()
    {
        rawArrowInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        arrowInput = (transform.right * rawArrowInput.x + transform.forward * rawArrowInput.z).normalized;
    }

    void ApplyMovement()
    {
        Debug.DrawRay(transform.position, SPEED, Color.red);

        CollisionFlags flags = controller.Move(SPEED * Time.deltaTime);
        if (flags == CollisionFlags.None) grounded = false;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        grounded = 1 - Vector3.Dot(hit.normal, Vector3.up) <= groundedAngle;
        moves[currentMove].Collide(hit);
    }

    void CheckMoves()
    {
        int priority = -99999;
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i].priority > priority)
            {
                if (moves[i].CheckState())
                {
                    currentMove = i;
                    priority = moves[i].priority;
                }
            }
        }

        hasDuration = moves[currentMove].hasDuration;
    }
}
