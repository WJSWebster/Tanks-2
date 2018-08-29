using UnityEngine;
using UnityEngine.UI;

public class HueWheel : MonoBehaviour
{
    private readonly float m_Scale = 360f;
    private float m_SliderAngle;  // the angle in degrees from [0 - 360]
    private float m_SliderValue;  // the slider's angle / 360  [0 - 1.0]
    private GameObject TankRenderers { get { return m_Tank.transform.GetChild(0).gameObject; } }
    private int TankNumber { get { return m_Tank.GetComponent<TankMovement>().m_PlayerNumber; } }
    private Vector2 TransformCenter { get { return transform.position; } }  // only wants to be a Vector2 as is a Screen-Space UI element
    private RectTransform ButtonContainer { get { return transform.GetChild(0).GetComponent<RectTransform>(); } }//[CircleHealthBar]  // the container which is rotated
    private Image HueHandle { get { return ButtonContainer.GetChild(0).GetComponent<Image>(); } }//[HueSlider]  // the handle GO which will be re-coloured on changes

    public GameObject m_Tank;             // The tank which we will be changing colour & getting tank no. from
    public bool m_isInteractable = true;  // Whether or not the slider value can be changed by the user

    // get the mouse's position from the transform of this object
    // hit by Drag or PointerClick Event Triggers
    public void CircularRangeControl()
    {
        // TODO instantiate a listener, with a call to CircularRangeControl() on slider value changing

        if (m_isInteractable)
        {
            float mouseAngle = Vector3.Angle(Vector2.up, (Vector2)Input.mousePosition - TransformCenter);
            bool onTheRight = Input.mousePosition.x > TransformCenter.x;

            m_SliderAngle = (onTheRight ? mouseAngle : 180 + (180 - mouseAngle)); //if mouse position is on the left, then need to adjust the 'angle'
            m_SliderValue = m_SliderAngle / m_Scale;  // assigns a percentage from 0f to 1f

            // call to all relevant methods
            RotateButtonContainer();
            HueSlider();
            TankHueRender();
            UpdatePlayerPrefs();
        }
    }

    // change the rotation of the button-container once CircularRangeControl is done
    private void RotateButtonContainer()
    {
        ButtonContainer.localEulerAngles = new Vector3(0, 180 , m_SliderAngle);
    }

    // change the colour of the handle
    private void HueSlider()
    {
        HueHandle.color = Color.HSVToRGB(m_SliderValue, 1f, 1f);
    }

    // change the colour of the tank
    private void TankHueRender()
    {
        Color tankColour = Color.HSVToRGB(m_SliderValue, 0.8f, 0.8f);

        for (int i = 0; i < 4; i++)
        {
            TankRenderers.transform.GetChild(i).GetComponent<Renderer>().material.color = tankColour;
        }
    }

    // updates playerprefs
    private void UpdatePlayerPrefs()
    {
        string name = "hue" + TankNumber;
        PlayerPrefs.SetFloat(name, m_SliderValue);
    }
}