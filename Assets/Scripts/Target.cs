using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


// 1. Abstraction
public abstract class Target : MonoBehaviour, IHittable // 2. Interfaces
{
    [Header("Target Stats")]
    [Tooltip("How many paintballs it takes for the target to break.")]
    [SerializeField] private int maxHits = 1;

    [Tooltip("Points the player gets from breaking the target")]
    [SerializeField] private int points = 1;

    [Header("Hit Flash")]
    [Tooltip("How long the target flashes when hit.")]
    [SerializeField] private float flashDuration = .1f;
    
    // 3. Encapsulation - internal state
    private int _hitsRemaining;
    private Renderer _targetRenderer;
    private Color _baseColor = Color.white;
    private float flashTimer = 0f;

    //3. Encapsulation - Properties
    public int Points => points;
    public int HitsRemaining => _hitsRemaining;  
    public bool IsDefeated { get; private set; }

    // 3. Encapsulation - Protected functions and variables - Private for everybody, public for children.
    protected Color BaseColor
    {
        get { return _baseColor; }
        set
        {
            _baseColor = value;

        }

    }

    protected virtual void Awake()
    {
        _hitsRemaining = Mathf.Max(1, maxHits);

        _targetRenderer = GetComponent<Renderer>(); // TODO: Check if we do it in child.

        _baseColor = _targetRenderer.material.color;

    }

    protected virtual void Update()
    {
        if (IsDefeated) return;

        Move(); // 4. Polymorphism
        UpdateFlash();

    }

    protected abstract void Move();
    protected void UpdateFlash()
    {
        if (flashTimer <= 0f) return;
        flashTimer -= Time.deltaTime;
        if(flashTimer <= 0f)
        {
            SetRendererColor(_baseColor);
        }
    }

    private void SetRendererColor(Color baseColor)
    {
        _targetRenderer.material.color = baseColor;
    }

    public void OnPaintballHit(Color paintballColor, Vector3 hitPoint)
    {
        if (IsDefeated) return;
        _hitsRemaining--;
        OnHit(paintballColor, hitPoint);
        if (_hitsRemaining <= 0)
        {
            IsDefeated = true;
            Ondefeated();
        }
    }
    protected virtual void OnHit(Color paintballColor, Vector3 hitPoint)
    {
        Flash(paintballColor);
    }

    protected void Flash(Color paintballColor)
    {
        if (flashDuration <= 0f) return;
        flashTimer = flashDuration;
        SetRendererColor(paintballColor);
    }

    protected virtual void Ondefeated()
    {
        // TODO: Add points to the Score Manager.
        Destroy(gameObject);
    }

    
}
