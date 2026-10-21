using UnityEngine;

public class MovingTarget : Target
{
    [Header("Movement")]
    [Tooltip("How far it slides (in Unity units)")]
    [SerializeField] private float slideDistance = 2f;

    [Tooltip("How fast it slides")]
    [SerializeField] private float slideSpeed = 2f;

    private Vector3 _startPosition;
    private float _timeOffset;

    protected override void Awake()
    {
        base.Awake();

        _startPosition = transform.position;
        _timeOffset = Random.Range(0f, 10f);
    }

    protected override void Move()
    {
        float offset = Mathf.Sin((Time.time + _timeOffset) * slideSpeed) * slideDistance ;

        transform.position = _startPosition + transform.right * offset ;



    }

}
