using System.Collections.Generic;
using UnityEngine;

public class LapRecorder : MonoBehaviour
{
    public float recordInterval = 0.05f;
    private float timer;

    private List<Vector3> recordedPositions = new List<Vector3>();
    private List<Quaternion> recordedRotations = new List<Quaternion>();
    private bool isRecording = true;

    public void StartRecording()
    {
        recordedPositions.Clear();
        recordedRotations.Clear();
        isRecording = true;
        timer = 0f;
    }

    public (List<Vector3>, List<Quaternion>) StopRecording()
    {
        isRecording = false;
        return (new List<Vector3>(recordedPositions), new List<Quaternion>(recordedRotations));
    }

    private void Update()
    {
        if (!isRecording) return;

        timer += Time.deltaTime;
        if (timer >= recordInterval)
        {
            recordedPositions.Add(transform.position);
            recordedRotations.Add(transform.rotation);
            timer = 0f;
        }
    }
}
