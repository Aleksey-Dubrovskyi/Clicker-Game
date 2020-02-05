using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    private Vector2 movePosition;
    [SerializeField]
    private float duration = 0.5f;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        transform.localPosition = Vector2.zero;
        movePosition = new Vector2(Random.Range(-3, 3), Random.Range(-3, 3));
        Sequence sequence = DOTween.Sequence();
        sequence.Prepend(transform.DOMove(movePosition, duration, false)).Join(text.DOFade(0, duration));
        Destroy(this.gameObject, duration);
    }
}
