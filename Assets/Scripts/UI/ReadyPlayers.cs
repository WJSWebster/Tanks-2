using UnityEngine;
using UnityEngine.UI;

public class ReadyPlayers : MonoBehaviour {

    private bool m_IsPlayer1Ready = false;  // Whether the "Ready" toggle is checked for Player 1
    private bool m_IsPlayer2Ready = false;  // Whether the "Ready" toggle is checked for Player 2

    private GameObject m_Canvas;            // The Canvas object which contains all UI elements
    private GameObject m_PlayButton;        // The PlayButton object which is activated/deactived
    private GameObject m_Player1HueWheel;   // The HueSlider object for Player 1
    private GameObject m_Player2HueWheel;   // The HueSlider object for Player 2

    private void Start()
    {
        m_Canvas = transform.parent.gameObject;

        m_PlayButton      = m_Canvas.transform.GetChild(0).transform.GetChild(0).gameObject;
        m_Player1HueWheel = m_Canvas.transform.GetChild(1).transform.GetChild(1).gameObject;
        m_Player2HueWheel = m_Canvas.transform.GetChild(2).transform.GetChild(1).gameObject;
    }

    public bool IsPlayer1Ready
    {
        set
        {
            if(value != m_IsPlayer1Ready)
            {
                m_IsPlayer1Ready = value;

                UpdateHueSlider(m_IsPlayer1Ready, m_Player1HueWheel);
                
                CheckPlayersReady();
            }
        }
    }

    public bool IsPlayer2Ready
    {
        set
        {
            if (value != m_IsPlayer2Ready)
            {
                m_IsPlayer2Ready = value;

                UpdateHueSlider(m_IsPlayer2Ready, m_Player2HueWheel);

                CheckPlayersReady();
            }
        }
    }

    private void UpdateHueSlider(bool isReady, GameObject hueWheel)
    {
        float fadeAmount = isReady ? .25f : 1f;  // if toggle marks that player is readdy fadeAmount = .25f, else fadeAmount = 1f
        float fadeTime   = .15f;                 // amount of time (in seconds) that the fade will take

        // makes hueWheel no longer interactable if player marks themself as ready (and vise versa)
        hueWheel.GetComponent<HueWheel>().m_isInteractable = !isReady;

        // fades hueWheel in or out of transparency over a set amount of time
        hueWheel.GetComponent<Image>().CrossFadeAlpha(fadeAmount, fadeTime, false);

        // gets hueWheel's handle object and fades this identically to before
        GameObject handle = hueWheel.transform.GetChild(0).transform.GetChild(0).gameObject;
        handle.GetComponent<Image>().CrossFadeAlpha(fadeAmount, fadeTime, false);

        // gets handle's ring object and fades this identically to before
        GameObject handleRing = handle.transform.GetChild(0).gameObject;
        handleRing.GetComponent<Image>().CrossFadeAlpha(fadeAmount, fadeTime, false);
    }

    private void CheckPlayersReady()
    {
        // either enables or disables the Play! object dependent on if both players are ready
        m_PlayButton.SetActive(m_IsPlayer1Ready && m_IsPlayer2Ready);
    }
}
