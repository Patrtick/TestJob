using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostRunner : MonoBehaviour
{
    public GameObject ghostPrefab;
    public float ghostSpeed = 10f;
    public string playerLayerName = "Player";
    public string ghostLayerName = "Ghost";

    public void SpawnGhost(List<Vector3> path, List<Quaternion> rotations)
    {
        if (path.Count < 2) return;

        Vector3 spawnPos = AdjustToGround(path[0]);
        GameObject ghost = Instantiate(ghostPrefab, spawnPos, rotations[0]);

        SetLayerRecursively(ghost, 6);

        Collider[] cols = ghost.GetComponentsInChildren<Collider>();
        foreach (var col in cols)
            Destroy(col);

        StartCoroutine(MoveGhostSmooth(ghost, path, rotations));
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    private IEnumerator MoveGhostSmooth(GameObject ghost, List<Vector3> path, List<Quaternion> rotations)
    {
        int index = 0;
        Vector3 velocity = Vector3.zero; 
        float smoothTime = 0.05f; 

        while (index < path.Count - 1)
        {
            Vector3 startPos = AdjustToGround(path[index]);
            Vector3 endPos = AdjustToGround(path[index + 1]);
            Quaternion startRot = Quaternion.Euler(0f, rotations[index].eulerAngles.y, 0f);
            Quaternion endRot = Quaternion.Euler(0f, rotations[index + 1].eulerAngles.y, 0f);

            float distance = Vector3.Distance(startPos, endPos);
            float travelTime = distance / ghostSpeed;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / travelTime;

                Vector3 targetPos = Vector3.Lerp(startPos, endPos, t);
                targetPos = AdjustToGround(targetPos);
                ghost.transform.position = Vector3.SmoothDamp(ghost.transform.position, targetPos, ref velocity, smoothTime);

                ghost.transform.rotation = Quaternion.Slerp(startRot, endRot, t);

                yield return null;
            }

            index++;
        }

        Destroy(ghost);
    }

    private Vector3 AdjustToGround(Vector3 originalPos)
    {
        RaycastHit hit;
        Vector3 rayOrigin = originalPos + Vector3.up * 5f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 20f))
        {
            return hit.point;
        }

        return originalPos;
    }
}
