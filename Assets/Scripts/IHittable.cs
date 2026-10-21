using UnityEngine;

public interface IHittable
{
    void OnPaintballHit(Color paintballColor, Vector3 hitPoint);

}
