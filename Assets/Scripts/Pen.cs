using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Pen : MonoBehaviour
{

    public bool draw = false;
    private bool prevDraw = false;
    private Transform pose;
    public string transformDir = "";
    private float speed = 100000.0f;
    private float drawPeriod = 0.01f;
    //public GameObject drawingContainer;
    public GameObject drawingContainer;
    private Coroutine drawingCoroutine;

    public bool debug = true;
    public Transform debugFollowObject;

    public Material inkMaterial;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Keep reading the file and update the transform
    void readTransform()
    {
        List<Vector3> data = new List<Vector3>();

        StreamReader reader = new StreamReader(this.transformDir);

        List<string> lines = new List<string>();
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            //Debug.Log(line);
            lines.Add(line);

        }
        reader.Close();
        string[] parsedLine = lines[0].Split(char.Parse(" "));
        pose.position = new Vector3(float.Parse(parsedLine[0]), float.Parse(parsedLine[1]), float.Parse(parsedLine[2]));
        parsedLine = lines[1].Split(char.Parse(" "));
        pose.eulerAngles = new Vector3(float.Parse(parsedLine[0]), float.Parse(parsedLine[1]), float.Parse(parsedLine[2]));
    }


    IEnumerator DrawingCoroutine()
    {
        Queue<Vector3> pointQueue = new Queue<Vector3>();
        GameObject spawnedObj = new GameObject();
        LineRenderer curLineContainer = spawnedObj.AddComponent<LineRenderer>();
        curLineContainer.material = this.inkMaterial;
        pointQueue.Enqueue(transform.position);
        pointQueue.Enqueue(transform.position);
        while (draw)
        {
            pointQueue.Enqueue(transform.position);
            //Debug.Log(pointQueue.Count);
            curLineContainer.positionCount = pointQueue.Count;
            curLineContainer.SetPositions(pointQueue.ToArray());
            yield return new WaitForSeconds(drawPeriod);
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            pose = debugFollowObject;
        }
        else
        {
            readTransform();
        }
        transform.position = Vector3.MoveTowards(transform.position, pose.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, pose.rotation, speed * Time.deltaTime);
        if (draw)
        {
            if (!prevDraw)
            {
                drawingCoroutine = StartCoroutine(DrawingCoroutine());
                prevDraw = true;
            }
        }
        else
        {
            if (prevDraw)
            {
                StopCoroutine(drawingCoroutine);
                prevDraw = false;
            }
        }
    }
}
