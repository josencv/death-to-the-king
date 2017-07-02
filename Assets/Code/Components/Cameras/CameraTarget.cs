using UnityEngine;
using UnityEditor;

public class CameraTarget : ScriptableObject
{
    private GameObject gameObject;
    public float distanceFromCenter;
    public float priority = 1.0f;

    public CameraTarget(GameObject gameObject)
    {
        this.gameObject = gameObject;
    }

    public Vector3 getPosition()
    {
        return this.gameObject.transform.position;
    }

    public void setDistanceFromCenter(Vector3 center)
    {
        distanceFromCenter = Vector3.Distance(center, getPosition());
    }

    public bool isPullingLeash(float leashLength)
    {
        if (distanceFromCenter >= leashLength) return true;
        return false;
    }
}