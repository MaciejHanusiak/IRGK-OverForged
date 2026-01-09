using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CoinFlyUI : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private RectTransform coinsLayer;   // np. osobny panel/warstwa na monety (na wierzchu)
    [SerializeField] private RectTransform coinPrefab;   // prefab monety (RectTransform + Image)

    [Header("Spawn")]
    [SerializeField] private int maxCoinsVisual = 60;    // limit efektu
    [SerializeField] private float spawnInterval = 0.02f; // odstêp miêdzy monetami
    [SerializeField] private Vector2 startJitter = new Vector2(40f, 20f); // rozrzut startu

    [Header("Motion")]
    [SerializeField] private float flightTime = 0.6f;
    [SerializeField] private float arcHeight = 120f;     // wysokoœæ ³uku
    [SerializeField] private Vector2 endJitter = new Vector2(15f, 15f); // rozrzut koñca

    [Header("Scale")]
    [SerializeField] private float startScale = 1f;
    [SerializeField] private float endScale = 0.6f;

    public void Play(int finalReward, RectTransform startFrom, RectTransform targetTo)
    {
        if (coinsLayer == null || coinPrefab == null || startFrom == null || targetTo == null)
            return;

        int count = Mathf.Clamp(finalReward, 1, maxCoinsVisual);
        StartCoroutine(SpawnRoutine(count, startFrom, targetTo));
    }

    private IEnumerator SpawnRoutine(int count, RectTransform startFrom, RectTransform targetTo)
    {
        for (int i = 0; i < count; i++)
        {
            var coin = Instantiate(coinPrefab, coinsLayer);
            coin.gameObject.SetActive(true);

            Vector2 startPos = WorldToLayerPos(startFrom);
            startPos += new Vector2(
                Random.Range(-startJitter.x, startJitter.x),
                Random.Range(-startJitter.y, startJitter.y)
            );

            Vector2 endPos = WorldToLayerPos(targetTo);
            endPos += new Vector2(
                Random.Range(-endJitter.x, endJitter.x),
                Random.Range(-endJitter.y, endJitter.y)
            );

            // punkt kontrolny ³uku: miêdzy start i end, z podbiciem w górê
            Vector2 mid = (startPos + endPos) * 0.5f;
            Vector2 control = mid + Vector2.up * arcHeight;

            coin.anchoredPosition = startPos;
            coin.localScale = Vector3.one * startScale;

            StartCoroutine(FlyCoin(coin, startPos, control, endPos));

            yield return new WaitForSecondsRealtime(spawnInterval);
        }
    }

    private IEnumerator FlyCoin(RectTransform coin, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float t = 0f;
        while (t < flightTime)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / flightTime);

            // Quadratic Bezier
            Vector2 a = Vector2.Lerp(p0, p1, k);
            Vector2 b = Vector2.Lerp(p1, p2, k);
            Vector2 pos = Vector2.Lerp(a, b, k);

            if (coin != null)
            {
                coin.anchoredPosition = pos;
                float s = Mathf.Lerp(startScale, endScale, k);
                coin.localScale = Vector3.one * s;
            }

            yield return null;
        }

        if (coin != null)
            Destroy(coin.gameObject);
    }

    private Vector2 WorldToLayerPos(RectTransform rt)
    {
        // zamiana pozycji world na anchoredPosition w warstwie coinsLayer (ten sam Canvas)
        Vector2 screen = RectTransformUtility.WorldToScreenPoint(null, rt.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(coinsLayer, screen, null, out Vector2 local);
        return local;
    }
}