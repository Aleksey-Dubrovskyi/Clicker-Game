using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ShopWindow;
    private Animator shopAnim;
    private bool isOpen = false;
    [SerializeField]
    private GameObject soundButton;
    [SerializeField]
    private Sprite soundOn;
    [SerializeField]
    private Sprite soundOf;
    [SerializeField]
    private GameObject sFXManager;
    private void Start()
    {
        shopAnim = ShopWindow.GetComponent<Animator>();
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                soundButton.GetComponent<Image>().sprite = soundOf;
                sFXManager.SetActive(false);
            }
            else
            {
                soundButton.GetComponent<Image>().sprite = soundOn;
                sFXManager.SetActive(true);
            }
        }
        else
        {
            soundButton.GetComponent<Image>().sprite = soundOn;
            sFXManager.SetActive(true);
        }
    }

    public void OnShop()
    {
        if (isOpen == false)
        {
            shopAnim.SetBool("Out", true);
            isOpen = true;
        }
        else if (isOpen == true)
        {
            shopAnim.SetBool("Out", false);
            isOpen = false;
        }
    }

    public void AudioButton()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetInt("Sound") == 0)
            {
                PlayerPrefs.SetInt("Sound", 1);
                soundButton.GetComponent<Image>().sprite = soundOn;
                sFXManager.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("Sound", 0);
                soundButton.GetComponent<Image>().sprite = soundOf;
                sFXManager.SetActive(false);
            }
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
            soundButton.GetComponent<Image>().sprite = soundOf;
            sFXManager.SetActive(false);
        }
    }
}
