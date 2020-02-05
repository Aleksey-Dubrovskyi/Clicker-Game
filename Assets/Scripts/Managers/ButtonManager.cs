using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ShopWindow;
    private Animator shopAnim;
    private bool isOpen = false;

    private void Start()
    {
        shopAnim = ShopWindow.GetComponent<Animator>();
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
}
