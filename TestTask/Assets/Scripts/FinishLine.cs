using UnityEngine;
using UnityEngine.UI; // дл€ UI Text

public class FinishTrigger : MonoBehaviour
{
    public LapRecorder recorder;
    public GhostRunner ghostManager;
    public Text lapText; // —сылка на UI текст в Canvas
    private int lapCount = 0;

    private void Start()
    {
        UpdateLapUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            lapCount++;
            UpdateLapUI();

            if (recorder != null)
            {
                var (path, rotations) = recorder.StopRecording();
                if (path.Count > 0)
                    ghostManager.SpawnGhost(path, rotations);
            }

            recorder.StartRecording();
        }
    }

    private void UpdateLapUI()
    {
        if (lapText != null)
        {
            lapText.text = "Laps: " + lapCount;
        }
    }
}
