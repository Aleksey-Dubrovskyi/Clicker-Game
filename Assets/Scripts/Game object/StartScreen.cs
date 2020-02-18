using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    Animator startScreenAnimator;
    [SerializeField]
    GameObject startScreen;
    float timeToStart;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        timeToStart = 3f;
        while (timeToStart > 0)
        {
            timeToStart--;
            yield return new WaitForSeconds(1f);
        }
        if (timeToStart == 0)
        {
            startScreenAnimator.Play("StartScreen");
            Destroy(this.gameObject, 2f);
        }
        yield return new WaitForEndOfFrame();
    }
}
