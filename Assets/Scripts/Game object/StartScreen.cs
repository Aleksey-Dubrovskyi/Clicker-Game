using System.Collections;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    [SerializeField]
    private Animator startScreenAnimator;
    [SerializeField]
    private GameObject startScreen;
    private float timeToStart;

    // Start is called before the first frame update
    private IEnumerator Start()
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
