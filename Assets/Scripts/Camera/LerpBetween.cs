using UnityEngine;
using UnityEngine.UI;

public class LerpBetween : MonoBehaviour
{
    private float m_DistanceFrom = 0f;  // The distance the camera has travelled so far (e.g. distance from start) [0 - 1f]
    private Vector3 m_StartPos;         // The current position of the camera (at start)
    private Quaternion m_StartRot;      // The current rotation of the camera (at start)
    private Vector3 m_EndPos;           // The destination position for the camera
    private Quaternion m_EndRot;        // The destination rotation for the camera
    private bool m_IsLerping = false;   // Whether the camera is currently transitioning to it's destination
    private GameObject m_BlurObject;    // The Blur GameObject
    private Material m_BlurMat;         // The material component contained within the Blur GameObject
    private float m_OrgBlurSize;        // The original 'Size' property of the Blur shader

    public float m_TravelSpeed = 10f;   // Movement speed in units/sec.
    public Transform m_EndCamera;       // The end destination for this scene's camera
    public GameObject m_Canvas;         // The canvas GameObject which contains all UI elements
    public bool IsLerping               // A public getter for m_isLerping
    {
        get { return m_IsLerping; }
    }

    // Use this for initialization
    private void Start () {
        m_EndPos = m_EndCamera.transform.position;
        m_EndRot = m_EndCamera.transform.rotation;

        m_BlurObject = m_Canvas.transform.GetChild(0).gameObject;
        m_BlurMat = m_BlurObject.GetComponent<Image>().material;
        m_OrgBlurSize = m_BlurMat.GetFloat("_Size");
    }

    // Called to from ButtonManager
    public void Begin()
    {
        GetComponent<OrbitCamera>().isOrbiting = false; // immediately stops the camera from orbiting

        // assigns these two member variables now that camera has stopped orbiting
        m_StartPos = transform.position;
        m_StartRot = transform.rotation;

        // for music transition (Note: this only works with linear interpolation, i.e. not with Slerp!)
        float distance = Vector3.Distance(m_StartPos, m_EndPos);  // calculates the distance between these two points in units
        float lerpTime = (distance / m_TravelSpeed) / 20;  // gives the amount of time it would take to travel this distance in seconds
        //Debug.Log("(distace: "+ distance + " / m_TravelSpeed: " + m_TravelSpeed + ") = lerpTime: " + lerpTime);

        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.TransitionScene(1, 2f);  // TODO fix these magic numbers

        m_IsLerping = true;
    }
	
	private void Update () {
        if (m_IsLerping)
        {
            if (transform.position != m_EndPos) // camera still transitioning from original to end position
            {
                UpdateTransform();
                UpdateCanvas();
            }
            else // camera has reached it's destination so lerping is finished
            {
                // deactivate Blur layer (as fade-out is now complete):
                m_BlurObject.SetActive(false);

                // reset's shader's _Size property:
                m_BlurMat.SetFloat("_Size", m_OrgBlurSize);

                m_IsLerping = false;
            }
        }
    }

    // updates lerp progress of camera's position and slerp progress of camera's rotation
    private void UpdateTransform()
    {
        m_DistanceFrom += Time.deltaTime * m_TravelSpeed;  // effectively the distance travelled so far

        transform.position = Vector3.Lerp(m_StartPos, m_EndPos, m_DistanceFrom);
        transform.rotation = Quaternion.Slerp(m_StartRot, m_EndRot, m_DistanceFrom);  // kept as quaternion to avoid gimble locking, not that that's a massive problem here
    }

    // updates the progress of the UI and Blur game objects increasing their transparency
    private void UpdateCanvas()
    {
        float distanceTo = 1 - m_DistanceFrom;  // the inverse of m_DistanceFrom

        // linearly decreases the size property of the Blur Shader
        if (m_BlurMat.GetFloat("_Size") > 0)
            m_BlurMat.SetFloat("_Size", m_OrgBlurSize * distanceTo);

        // linearly increases transparency of all Canvas game objects (twice as fast)
        if (m_Canvas.GetComponent<CanvasGroup>().alpha > 0)
            m_Canvas.GetComponent<CanvasGroup>().alpha = distanceTo / 2;
    }
}
