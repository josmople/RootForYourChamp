using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ManualCrowdCheering : MonoBehaviour
{
    public AudioDetection detector;

    public float speed = 1;
    public Vector3 vibrateOffset = new Vector3(0, 10, 0);

    public List<GameObject> crowd = new List<GameObject>();
    private List<Vector3> initialPositions = new List<Vector3>();



    private void Start()
    {
        crowd.ForEach(c => initialPositions.Add(c.transform.position));
    }

    public int computeCrowdIndex(int waveIndex, int waveLength, int crowdLength)
    {
        float ratio = waveIndex / waveLength;
        return (int)(ratio * crowdLength);
    }

    public int computeWaveIndex(float crowdIndex, float crowdLength, float waveLength)
    {
        var ratio = crowdIndex / crowdLength;
        return (int)(ratio * (waveLength - 1));
    }

    private void Update() 
    {
        var waveData = detector.GetAudioWaveFromMic();

        //for (int waveIndex = 0; waveIndex < waveData.Length; waveIndex++)
        //{
        //    var volume = waveData[waveIndex];
        //    var offset = volume * vibrateOffset;
        //    var initialPosition = initialPositions[waveIndex];

        //    var targetPosition = transform.position * waveIndex + offset;

        //    var crowdIndex = computeCrowdIndex(waveIndex, waveData.Length, initialPositions.Count);
        //    crowd[crowdIndex].transform.position = Vector3.Lerp(crowd[crowdIndex].transform.position, targetPosition, speed * Time.deltaTime);
        //}

        for (int crowdIndex = 0; crowdIndex < initialPositions.Count; crowdIndex++)
        {
            var current = crowd[crowdIndex];
            var waveIndex = computeWaveIndex(crowdIndex, initialPositions.Count, waveData.Length);

            Debug.Log(waveIndex);

            var volume = waveData[waveIndex];
            var offset = volume * vibrateOffset;

            var initialPosition = initialPositions[crowdIndex];
            var targetPosition = initialPosition + offset;
            var currentPosition = current.transform.position;

            current.transform.position = Vector3.Lerp(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}
