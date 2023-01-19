using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Marker : MonoBehaviour
{
    [Header("Target Follow")]
    [SerializeField] Transform followTarget;
    Camera usedCamera;
    [SerializeField] GameObject marker;
    [SerializeField] RectTransform markerAnchor;
    [SerializeField] Canvas markerCanvas;
    [SerializeField] Vector2 borderHorizontal = new Vector2(25f, 25f); // So viel Pixel soll die Marker Anchor Position vom Rand weg sein
    [SerializeField] Vector2 borderVertical = new Vector2(25f, 25f);

    [Header("Fadeout")]
    [SerializeField] Transform fadeoutTarget;
    [SerializeField] float fadeoutSpeed;
    [SerializeField] float FadeoutTime;

    Vector3 transformRelativePosition;

    bool followIsActive = true;



    // Manager
    LevelManager myManager;




    // FEEDBACK-----------------
    Animator anim;


    private void Awake()
    {
        usedCamera = Camera.main;
        myManager = FindObjectOfType<LevelManager>();

        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (myManager.CurrentGameState == LevelManager.gameState.running)
        {
            if (followTarget != null)
            {
                if (followIsActive) // Wenn der follow state aktiv ist folgt das ui dem target
                {
                    UpdateFollowPosition();
                }
                else // ansonsten bewegt sich das UI zum Score Target und verschwindet dort
                {

                    UpdateFadeoutPosition();
                }
            }
            else
            {
                Destroy(markerCanvas.gameObject);
            }

        }
    }

    private void UpdateFollowPosition()
    {
        // Gibt die Position relativ zur kamera zurück - in einem normalizeten Space (0,0 -> 1,1 etc.)
        transformRelativePosition = usedCamera.WorldToViewportPoint(followTarget.position);
        // Setzt den marker Aktiv, wenn ein Koordinatenwert außerhalb des screens ist <3
        //marker.SetActive(transformRelativePosition.x < 0 || transformRelativePosition.x > 1 || transformRelativePosition.y < 0 || transformRelativePosition.y > 1);
        // Wenn der Marker nicht an ist - brauchen wir den Rest nicht
        if (!marker.activeSelf)
            return;


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

    private void UpdateFadeoutPosition()
    {
        // In diese Richtung muss sich der Marker bewegen
        Vector2 moveDirection = (fadeoutTarget.transform.position - markerAnchor.position).normalized;

        markerAnchor.anchoredPosition = markerAnchor.anchoredPosition + (moveDirection * fadeoutSpeed * Time.deltaTime);
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }
    public void SetFadeoutTarget(Transform target)
    {
        followTarget = target;
    }

    public void DeactivatePlayerFollow()
    {
        followIsActive = false;

        Destroy(markerCanvas.gameObject, FadeoutTime);

        anim.SetTrigger("fadeout");
    }

}
