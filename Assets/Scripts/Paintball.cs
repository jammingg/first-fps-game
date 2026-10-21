using UnityEngine;
using UnityEngine.UIElements;

public class Paintball : MonoBehaviour
{
    public float splatSizeMin = 0.3f;
    public float splatSizeMax = 0.6f;

    public float splatLifeTime = 30f;

    public float paintbaallLifeTime = 5f;

    public Color paintballColor = Color.aliceBlue;

    private bool _hasHit = false;

    private void Start()
    {
        Destroy(gameObject, paintbaallLifeTime);

    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(_hasHit) return;
        _hasHit = true;

        ContactPoint contact = collision.contacts[0];
        Vector3 hitPoint = contact.point;
        Vector3 hitNormal = contact.normal;

        CreateSplat(hitPoint, hitNormal, collision.transform);

        IHittable hittable = collision.collider.GetComponent<IHittable>(); // 6. Polymorphism
        if (hittable != null)
        {
            hittable.OnPaintballHit(paintballColor, hitPoint);    
        }

        Destroy(gameObject);
    }

    public void Initialize(Color color)
    {
        paintballColor = color;
        Renderer renderer = GetComponent<Renderer>();
        Material mat = renderer.material;
        mat.color = paintballColor;

    }

    private void CreateSplat(Vector3 position, Vector3 normal, Transform parent)
    {
        GameObject splat = GameObject.CreatePrimitive(PrimitiveType.Quad);
        splat.name = "PaintSplat";
        splat.transform.position = position + normal *0.01f;  // a bit different level
        splat.transform.rotation = Quaternion.LookRotation(-normal);
        splat.transform.Rotate(0, 0, Random.Range(0f, 360f));

        float size = Random.Range(splatSizeMin, splatSizeMax);
        splat.transform.localScale = new Vector3(size, size, 1f);

        splat.transform.SetParent(parent);

        Destroy(splat.GetComponent<Collider>());

        Renderer rend = splat.GetComponent<Renderer>();
        Material mat = rend.material;

        mat.color = paintballColor;
        Destroy(splat,splatLifeTime);
    }




}
