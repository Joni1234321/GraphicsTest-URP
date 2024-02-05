using System.Collections;
using UnityEngine;
using TMPro;

public class FPSScript : MonoBehaviour
{
    public static FPSScript Instance;

    private const int FPS_SAMPLE_COUNT = 20;
    private readonly int[] _fpsSamples = new int[FPS_SAMPLE_COUNT];
    private int _sampleIndex;

    // Interval for calculating values
    public float Interval = 0.1f;
    [field: SerializeField] public int FPS { private set; get; }
    public TextMeshProUGUI Text;

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Too many FPSScripts Removing this!\n{Instance.name} is first instance");
            Destroy(this);
            return;
        }

        StartCoroutine(UpdateFps());
    }

    private void Update()
    {
        _fpsSamples[_sampleIndex++] = (int)(1.0f / Time.deltaTime);
        if (_sampleIndex >= FPS_SAMPLE_COUNT) _sampleIndex = 0;
    }

    IEnumerator UpdateFps()
    {
        while (true)
        {
            int sum = 0;
            for (int i = 0; i < FPS_SAMPLE_COUNT; i++)
                sum += _fpsSamples[i];
            FPS = sum / FPS_SAMPLE_COUNT;
            Text?.SetText(FPS.ToString());
            yield return new WaitForSeconds(Interval);
        }
    }
    

}
