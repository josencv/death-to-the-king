using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameCamera : MonoBehaviour
{
    public List<CameraTarget> targets = new List<CameraTarget>();       // Public variable to store a reference to the players
    public Vector3 centerOffset;         // Private variable to store the offset distance between the center and camera
    public Vector3 center;
    // Thresholds configuration
    public float smallFovDistance = 4f;
    public float bigFovDistance = 8f;

    // FOV configuration
    public float smallFov;
    public float fovDifference = 8f;
    public float bigFov;

    // Operational variables
    public float leashLength;
    public float damping = 1;
    public Camera gameCamera;

    private bool isChangingFov = false;
    private float targetFov;


    public void addTarget(GameObject gameObject)
    {
        targets.Add(new CameraTarget(gameObject));
        center = getTargetsCenter();
        centerOffset = transform.position - center;
    }

    // TODO: make private what should be private 
    // Use this for initialization
    void Start()
    {
        gameCamera = GetComponent<Camera>();
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        //transform.LookAt(target.transform);
        //Offset to the center: or "real" target
        centerOffset = transform.position - getTargetsCenter(); // Need to center the camera view in the target, instead of only storing the offset.

        smallFovDistance = (Screen.height / 2.5f)/Screen.dpi;
        bigFovDistance = 2.5f * smallFovDistance;

        smallFov = gameCamera.fieldOfView;
        bigFov = smallFov + fovDifference;

        leashLength = smallFovDistance;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // big area
        Gizmos.DrawWireSphere(center, bigFovDistance);
        Gizmos.DrawLine(center, center + new Vector3(bigFovDistance, 0, 0));

        Gizmos.color = Color.yellow; // small area
        Gizmos.DrawWireSphere(center, smallFovDistance);
        Gizmos.DrawLine(center, center + new Vector3(1, smallFovDistance, 0));

        Gizmos.color = Color.white; // target
        targets.ForEach((element) => Gizmos.DrawLine(center, element.getPosition()));
    }

    private void setFieldOfView(float fov)
    {
        // ignore this frame or not ?
        //gameCamera.fieldOfView = Mathf.Lerp(gameCamera.fieldOfView, fov, Time.deltaTime * damping);
        isChangingFov = true;
        targetFov = fov;
    }

    private Vector3 getTargetsCenter()
    {
        if (targets.Count == 0) return new Vector3(0, 0, 0);
        if (targets.Count == 1) return targets[0].getPosition();

        List<Vector3> positions = targets.Select(item => item.getPosition()).ToList<Vector3>();
        Vector3 sum = Vector3.zero;
        positions.ForEach(pos => sum += pos);
        return sum / positions.Count();
    }

    private void useFov(float fov, float distance)
    {
        leashLength = distance;
        setFieldOfView(fov);
    }

    private void useBigFov() { useFov(bigFov, bigFovDistance); }
    private void useSmallFov() { useFov(smallFov, smallFovDistance); }


    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // BEGIN TODO:
        // 1. Change FOV when going from small to big -> ok know how to do it
        // 1b. One player wont ever get zoomed out. Poor little thing, right?
        // 2. You cant move outside radius or screen
        // 3. Okay, follow target if you try to go outside radius: 1 player OK, more players is trickier
        // 4. Targets fight for camera dominance... this is many players. At least 2.
        // END TODO
               
        // Change the FOV smoothly
        if (isChangingFov)
        {
            if (gameCamera.fieldOfView != targetFov)
            {
                gameCamera.fieldOfView = Mathf.Lerp(gameCamera.fieldOfView, targetFov, Time.deltaTime * damping);
            }
            else
            {
                isChangingFov = false;
            }
        }

        if (targets.Any(target => target.isPullingLeash(center, leashLength)))
        {
            // where to go
            List<CameraTarget> pullingTargets = targets.Where(target => target.isPullingLeash(center, leashLength)).ToList();
            Vector3 distanceOffset = Vector3.zero;
            if (pullingTargets.Count == 1) { distanceOffset = pullingTargets[0].getPosition() - center; }
            else if (pullingTargets.Count > 1) {
                // TODO: check that players are moving farther away between each other
                // use player's distance instead of leash to change FOV.
                if (leashLength == smallFovDistance)
                {
                    useBigFov();
                    gameCamera.fieldOfView = Mathf.Lerp(gameCamera.fieldOfView, bigFov, Time.deltaTime * damping);
                    return;
                }
                distanceOffset = pullingTargets[1].getPosition() - center;
            }
            Vector3 desiredPosition = transform.position + distanceOffset;

            // move camera
            center = Vector3.Lerp(center, center + distanceOffset, Time.deltaTime * damping);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        } else if (leashLength == bigFovDistance) {
            if (!targets.Any(target => target.isPullingLeash(center, smallFovDistance))) useSmallFov();
        }


        //if (distanceFromCenter >= leashLength) isPullingLeash = true;
        //if (distanceFromCenter < leashLength) isPullingLeash = false;

    }
}
