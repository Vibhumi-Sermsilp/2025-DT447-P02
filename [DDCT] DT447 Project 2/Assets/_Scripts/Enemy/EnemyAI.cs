using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAI : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private Renderer m_Renderer;
    [SerializeField] private Transform m_player;
    [SerializeField] private Camera m_playerCamera;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float catchDistance = 1f;
    [SerializeField] private float chaseDistance = 10f;
    private NavMeshAgent m_agent;
    private Vector3 _destination;
    private bool alert = false;
    private bool catched = false;

    [Header("Animation")]
    [SerializeField] private float _speedChangeRate = 10f;
    private Animator _animator;
    private int _animIDSpeed;
    private int _animIDMotionSpeed;
    private float _animationBlend;

    [Header("Audio")]
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Header("Event")]
    public UnityEvent OnCatchEvent = new();
    public UnityEvent OnReleaseEvent = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_player = GameObject.Find("TPS Player").transform;
        _animator = GetComponent<Animator>();
        _animator.enabled = false;

        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState != GameManager.GameState.PLAY) return;

        Chase();
        UpdateAnmation();
    }

    private void Chase()
    {
        //Calculate the player's Camera's frustum planes
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(m_playerCamera);
        float distance = Vector3.Distance(transform.position, m_player.position);

        //If the AI is in the player's Camera's view,
        if (GeometryUtility.TestPlanesAABB(planes, m_Renderer.bounds) || distance > chaseDistance)
        {
            m_agent.speed = 0;
            m_agent.SetDestination(transform.position);
        }

        //If the AI isn't in the player's Camera's view,
        if (!GeometryUtility.TestPlanesAABB(planes, m_Renderer.bounds) && (distance <= chaseDistance || alert))
        {
            alert = true;

            m_agent.speed = _moveSpeed;
            _destination = m_player.position;
            m_agent.destination = _destination;

            if (distance <= catchDistance && !catched)
            {
                GameManager.Instance.Pause();
                catched = true;
                m_agent.speed = 0;
                m_agent.SetDestination(transform.position);
                OnCatchEvent.Invoke();
                StartCoroutine(OnCatchRoutine());
            }
        }
    }

    private IEnumerator OnCatchRoutine()
    {
        m_player.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        OnReleaseEvent.Invoke();
        GameManager.Instance.Respawn();
        GameManager.Instance.Resume();
        yield return new WaitForSeconds(1f);
        catched = false;
        alert = false;
    }

    private void UpdateAnmation()
    {
        _animator.enabled = alert;

        _animationBlend = Mathf.Lerp(_animationBlend, _moveSpeed, Time.deltaTime * _speedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        float moveMagnitude = m_agent.speed > 0 ? 1 : 0;
        _animator.SetFloat(_animIDSpeed, _animationBlend);
        _animator.SetFloat(_animIDMotionSpeed, moveMagnitude);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, catchDistance);
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
#endif
}
