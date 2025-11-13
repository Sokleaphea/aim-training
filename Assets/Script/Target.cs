using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float maxRadius = 0.6f;
    private float spawnTime;

    void Start() => spawnTime = Time.time;

    public void OnHit(Vector3 hitPoint) {
        Collider targetCollider = GetComponent<Collider>();
        float distance = Vector3.Distance(hitPoint, targetCollider.bounds.center);
        float accuracy = Mathf.Clamp01(1 - (distance / maxRadius));

        int score = Mathf.RoundToInt(accuracy * 100);

        float reactionTime = Time.time - spawnTime;
        float timeBonus = Mathf.Max(0, 1.5f - reactionTime);
        score += Mathf.RoundToInt(timeBonus * 20);

        GameManager.Instance.AddScore(score);
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
