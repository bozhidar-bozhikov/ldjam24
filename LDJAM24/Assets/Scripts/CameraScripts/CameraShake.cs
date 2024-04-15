using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static CameraShake instance; // Singleton instance
    public static CameraShake Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CameraShake>();
                if (instance == null)
                {
                    Debug.LogError("CameraShake instance not found in the scene!");
                }
            }
            return instance;
        }
    }

    private Vector3 originalPos;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }
        instance = this;
        originalPos = transform.localPosition;
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public static void Start_Shake(float duration, float magnitude)
    {
        Instance.StartCoroutine(Instance.Shake(duration, magnitude));
    }
}
