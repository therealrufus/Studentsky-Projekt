using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Range(0, 1)]
    [Tooltip("(not working) the minimal angle to be grounded")] public float groundedAngle;
    public float gravity = 10f;

    [Space]
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
    public int groundedForFrames;

    [HideInInspector]
    public Vector3 arrowInput;
    [HideInInspector]
    public Vector3 rawArrowInput;

    [Space]
    [Tooltip("DEBUG ONLY!!!")] public Text speedText;
    [Tooltip("DEBUG ONLY!!!")] public Text typeText;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        foreach (var move in moves)
        {
            move.master = this;
            move.Inicialize();
        }
    }

    void Update()
    {
        GetMovementInput();

        //choose a move
        if (!hasDuration) ChooseMoves();
        else hasDuration = moves[currentMove].ShouldContinue();

        //add velocity based on moves
        moves[currentMove].Move();
        moves[currentMove].OnMove.Invoke();

        ApplyMovement();

        //debug
        speedText.text = Mathf.Round(SPEED.magnitude).ToString();
        speedText.color = grounded ? Color.black : Color.grey;
        typeText.text = moves[currentMove].ToString();

        groundedForFrames++;
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
        if (flags == CollisionFlags.None) /*grounded = false;*/ SetGrounded(false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //grounded = 1 - Vector3.Dot(hit.normal, Vector3.up) <= groundedAngle;
        SetGrounded(1 - Vector3.Dot(hit.normal, Vector3.up) <= groundedAngle);
        moves[currentMove].Collide(hit);
    }

    void ChooseMoves()
    {
        //choose a move
        int lastMove = currentMove;

        int priority = -99999;
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i].priority > priority)
            {
                if (moves[i].ShouldStart())
                {
                    currentMove = i;
                    priority = moves[i].priority;
                }
            }
        }

        //execute the move
        if (lastMove != currentMove)
        {
            moves[currentMove].OnBegin.Invoke();
            moves[lastMove].OnEnd.Invoke();
        }

        hasDuration = moves[currentMove].hasDuration;
        if (hasDuration) moves[currentMove].Begin();
    }

    void SetGrounded(bool value)
    {
        if (value != grounded) groundedForFrames = 0;
        grounded = value;
    }
}
