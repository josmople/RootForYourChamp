using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrowdCheering : MonoBehaviour
{
    private float[] waveData;

    public float speed;

    public Vector3 crowdOffset;

    public Vector3 vibrateOffset;

    public GameObject template;

    private List<GameObject> crowd;

    public AudioDetection detector;
    // Start is called before the first frame update
    private void Start()
    {
        crowd = new List<GameObject>();

        for(int i = 0; i < detector.sampleWindow; i++)
        {
            var obj = Instantiate(template);
            obj.transform.position = transform.position + crowdOffset * i;
            crowd.Add(obj);
        }
    }

    private void Update() 
    {
        waveData = detector.GetAudioWaveFromMic();

        int[] random_i = Enumerable.Range(0, waveData.Length).OrderBy(x => Random.Range(-1, 1)).ToArray();
        for (int i = 0; i < waveData.Length; i++)
        {
            float input = waveData[random_i[i]];
            

            Vector3 position = input * vibrateOffset ;

            var targetPosition = transform.position + crowdOffset * i + position;

            crowd[i].transform.position = Vector3.Lerp(crowd[i].transform.position, targetPosition, speed * Time.deltaTime);
        }

        
    }
}
