using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform head;
    [Range(0, 1)]
    [Tooltip("(not working) the minimal angle to be grounded")] public float groundedAngle;
    public float gravity = 10f;

    [Space]
    public PlayerMoveOption[] moves;
    int currentMove;
    bool hasDuration;

    public PlayerMoveOption currentMoveOption
    {
        get { return moves[currentMove]; }
    }

    [HideInInspector]
    public CharacterController controller;

    [HideInInspector]
    public Vector3 SPEED;
    [HideInInspector]
    public Vector3 lastFrameSpeed = Vector3.zero;

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

    //effectors
    [HideInInspector] public List<PlayerEffector> activeEffectors;


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

        if (!GetEffectorCancel())
        {
            //choose a move
            if (!hasDuration || !currentMoveOption.ShouldContinue()) ChooseMoves();

            //add velocity based on moves
            currentMoveOption.Move();
            currentMoveOption.OnMove.Invoke();
        }

        //add effector forces
        SPEED += GetEffectorMoves(false);

        ApplyMovement();

        //debug
        if (speedText != null) speedText.text = Mathf.Round(SPEED.magnitude).ToString();
        if (speedText != null) speedText.color = grounded ? Color.black : Color.grey;
        if (typeText != null) typeText.text = currentMoveOption.ToString();

        groundedForFrames++;
    }

    private void LateUpdate()
    {
        lastFrameSpeed = SPEED;
    }

    void GetMovementInput()
    {
        rawArrowInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        arrowInput = (transform.right * rawArrowInput.x + transform.forward * rawArrowInput.z).normalized;
    }

    void ApplyMovement()
    {
        Debug.DrawRay(transform.position, SPEED, Color.red);

        Vector3 finalSpeed = SPEED;

        //add effector constant forces
        finalSpeed += GetEffectorMoves(true);

        CollisionFlags flags = controller.Move(finalSpeed * Time.deltaTime);
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
            currentMoveOption.OnBegin.Invoke();
            currentMoveOption.Begin();
            moves[lastMove].OnEnd.Invoke();
        }

        hasDuration = currentMoveOption.hasDuration;
    }

    void SetGrounded(bool value)
    {
        if (value != grounded) groundedForFrames = 0;
        grounded = value;
    }

    Vector3 GetEffectorMoves(bool constant)
    {
        Vector3 force = Vector3.zero;
        foreach (var effector in activeEffectors)
        {
            force += constant ? effector.ConstantMove() : effector.Move();
        }
        return force;
    }

    bool GetEffectorCancel()
    {
        foreach (var effector in activeEffectors)
        {
            if (effector.cancelMoves) return true;
        }
        return false;
    }
}
