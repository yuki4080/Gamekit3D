using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;
using Gamekit3D;


public class PlayerInput : MonoBehaviour
{
    private Controls inputs;

    public static PlayerInput Instance
    {
        get { return s_Instance; }
    }

    protected static PlayerInput s_Instance;

    [HideInInspector]
    public bool playerControllerInputBlocked;

    protected Vector2 m_Movement;
    protected Vector2 m_Camera;
    protected bool m_Jump;
    protected bool m_Attack;
    protected bool m_ExternalInputBlocked;

    public Vector2 MoveInput
    {
        get
        {
            if(playerControllerInputBlocked || m_ExternalInputBlocked)
                return Vector2.zero;
            return m_Movement;
        }
    }

    public Vector2 CameraInput
    {
        get
        {
            if(playerControllerInputBlocked || m_ExternalInputBlocked)
                return Vector2.zero;
            return m_Camera;
        }
    }

    public bool JumpInput
    {
        get { return m_Jump && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }

    public bool Attack
    {
        get { return m_Attack && !playerControllerInputBlocked && !m_ExternalInputBlocked; }
    }

    WaitForSeconds m_AttackInputWait;
    Coroutine m_AttackWaitCoroutine;

    const float k_AttackInputDuration = 0.03f;

    void Awake()
    {
        inputs = new Controls();
        inputs.Player.Movement.performed += ctx => m_Movement.Set(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y);
        inputs.Player.Movement.canceled += ctx => m_Movement.Set(0, 0);
        inputs.Player.Camera.performed += ctx => m_Camera.Set(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y);
        inputs.Player.Camera.canceled += ctx => m_Camera.Set(0, 0);
        inputs.Player.Jump.performed += ctx => m_Jump = true;
        inputs.Player.Jump.canceled += ctx => m_Jump = false;
        inputs.Player.Attack.started += ctx =>
        {
            if (m_AttackWaitCoroutine != null)
                StopCoroutine(m_AttackWaitCoroutine);

            m_AttackWaitCoroutine = StartCoroutine(AttackWait());
        };

        m_AttackInputWait = new WaitForSeconds(k_AttackInputDuration);

        if (s_Instance == null)
            s_Instance = this;
        else if (s_Instance != this)
            throw new UnityException("There cannot be more than one PlayerInput script.  The instances are " + s_Instance.name + " and " + name + ".");
    }

    IEnumerator AttackWait()
    {
        m_Attack = true;

        yield return m_AttackInputWait;

        m_Attack = false;
    }

    public bool HaveControl()
    {
        return !m_ExternalInputBlocked;
    }

    public void ReleaseControl()
    {
        m_ExternalInputBlocked = true;
    }

    public void GainControl()
    {
        m_ExternalInputBlocked = false;
    }

    private void OnDisable() => inputs.Disable();
    private void OnDestroy() => inputs.Disable();
    private void OnEnable() => inputs.Enable();
}
