using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageMarkerUI : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    Camera usedCamera;
    [SerializeField] GameObject marker;
    [SerializeField] RectTransform markerAnchor;
    [SerializeField] Canvas markerCanvas;
    [SerializeField] Vector2 borderHorizontal = new Vector2(25f, 25f); // So viel Pixel soll die Marker Anchor Position vom Rand weg sein
    [SerializeField] Vector2 borderVertical = new Vector2(25f, 25f);

    Vector3 transformRelativePosition;

    bool alarmIsActive = true;

    // Rotation of Sprite
    [SerializeField]
    RectTransform rotationImage;

    private void Awake()
    {
        usedCamera = Camera.main;
    }
    private void Update()
    {
        //if (GameManager.Instance.gameIsRunning)

        if (followTarget != null)
        {
            if (alarmIsActive)
            {
                UpdatePosition();
                //RotateSpriteToTarget();
            }
            else
                marker.SetActive(false);
        }
        else
        {
            Destroy(markerCanvas.gameObject);
        }


    }

    private void UpdatePosition()
    {
        // Gibt die Position relativ zur kamera zurück - in einem normalizeten Space (0,0 -> 1,1 etc.)
        transformRelativePosition = usedCamera.WorldToViewportPoint(followTarget.position);
        // Setzt den marker Aktiv, wenn ein Koordinatenwert außerhalb des screens ist <3
       // marker.SetActive(transformRelativePosition.x < 0 || transformRelativePosition.x > 1 || transformRelativePosition.y < 0 || transformRelativePosition.y > 1);
        // Wenn der Marker nicht an ist - brauchen wir den Rest nicht
        //if (!marker.activeSelf)
           // return;


        Vector3 screenPosition = usedCamera.ViewportToScreenPoint(transformRelativePosition);

        float canvasScale = markerCanvas.transform.localScale.y;

        // Clamp "beschneidet" einen Wert auf ein Minimum oder ein Maximum
        screenPosition.x = Mathf.Clamp(screenPosition.x, borderHorizontal.x * canvasScale, markerCanvas.pixelRect.width - borderHorizontal.y * canvasScale);
        screenPosition.y = Mathf.Clamp(screenPosition.y, borderVertical.x * canvasScale, markerCanvas.pixelRect.height - borderVertical.y * canvasScale);
        screenPosition.z = 0;


        // Sorgt dafür dass die Screenposition mit der Scalierung vom Canvas verrechnet wird - Sonst funktioniert es only für fullHD
        screenPosition.x /= markerCanvas.transform.localScale.x;
        screenPosition.y /= markerCanvas.transform.localScale.y;



        markerAnchor.anchoredPosition = screenPosition;
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    void RotateSpriteToTarget()
    {
        Vector3 screenPositionTarget = usedCamera.WorldToScreenPoint(followTarget.position);
        Vector2 direction = screenPositionTarget - rotationImage.position;
        float angle = Vector2.SignedAngle(Vector3.left, direction);
        rotationImage.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
