using UnityEngine;
using UnityEngine.UI;       // ?? UI elementleri için þart
using System.Collections;  // ?? Coroutine için þart


public class CardSwipe : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private DecisionCardData data;
    private GameManager manager;

    private RectTransform rectTransform;
    private bool isSwiping = false;
    private float swipeThreshold = 150f;

    public void Init(DecisionCardData cardData, GameManager gm)
    {
        data = cardData;
        manager = gm;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping)
        {
            endTouchPosition = Input.mousePosition;
            float deltaX = endTouchPosition.x - startTouchPosition.x;

            if (Mathf.Abs(deltaX) > swipeThreshold)
            {
                bool swipedRight = deltaX > 0;
                float targetX = swipedRight ? 2000f : -2000f;

                // Swipe animasyonu baþlat
                StartCoroutine(SwipeAndDestroy(targetX, swipedRight));
            }

            isSwiping = false;
        }
    }

    private IEnumerator SwipeAndDestroy(float targetX, bool swipedRight)
    {
        float duration = 0.3f;
        float elapsed = 0f;
        Vector3 startPos = rectTransform.anchoredPosition;
        Vector3 endPos = new Vector3(targetX, startPos.y, 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            rectTransform.anchoredPosition = Vector3.Lerp(startPos, endPos, elapsed / duration);
            yield return null;
        }

        manager.OnCardSwiped(data, swipedRight);
        Destroy(gameObject);
    }
}
