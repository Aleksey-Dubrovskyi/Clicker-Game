using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static Enemy instance;
    public Animator anim;

    private void Start()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }
}
