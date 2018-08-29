using UnityEngine;

//[RequireComponent(typeof(Text))]
public class TitleAnimate : MonoBehaviour {

    private float m_BaseScale;          // The original scale of the title
    private float m_BaseRot;            // The original (X) rotation of the title
    private float m_BasePosX;           // The original X position of the title

    public float m_BobFrequency = 2f;   // The frequency of the scale change
    public float m_BobRange = 0.075f;   // The range of the scale change
    public float m_RotFrequency = 1f;   // The frequency of the rotation change
    public float m_RotRange = 10f;      // The range of the rotation change
    public float m_PosXFrequency = 2f;  // The frequency of the X position change
    public float m_PosXRange = 20f;     // The range of the X position change

    private void Start()
    {
        m_BaseScale = transform.localScale.x;
        m_BaseRot   = transform.rotation.x;
        m_BasePosX  = transform.position.x;
    }

    void Update ()
    {
        // core calculation, using sin wave to generate a change over time
        float bobAnimation  = m_BaseScale + Mathf.Sin(Time.time * m_BobFrequency) * m_BobRange;
        float rotAnimation  = m_BaseRot + Mathf.Sin(Time.time * m_RotFrequency) * m_RotRange;
        float posXAnimation = m_BasePosX + Mathf.Sin(Time.time * m_PosXFrequency) * m_PosXRange;

        // appropriate assignment
        Vector3 scale = Vector3.one * bobAnimation;
        Quaternion rot = Quaternion.Euler(Vector3.one * rotAnimation);
        Vector3 posX = new Vector3(posXAnimation, transform.position.y, transform.position.z);

        // setting relevant transform variable
        transform.localScale = scale;
        transform.SetPositionAndRotation(posX, rot);
    }
}
