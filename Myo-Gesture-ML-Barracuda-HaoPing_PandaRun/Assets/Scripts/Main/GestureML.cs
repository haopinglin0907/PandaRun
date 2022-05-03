using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Unity.Barracuda;
using TMPro;


public class GestureML : MonoBehaviour
{
    public NNModel modelFile;
    public GameObject Player;
    Rigidbody rb;
    public float speed = 7.0f;

    private Model model;
    private IWorker worker;

    private GameObject appManager;
    private DataHandling dataHandling;

    private int batchSize = 1;
    private int height = 30;
    private int width = 8;
    private int channels = 1;

    private float EMG_Magnitude;

    private List<string> labels = new List<string>
    {
        "Finger Mass Extension",
        "Fist",
        "Opposition",
        "Rest",
        "Wrist & Finger Extension"
    };

    [SerializeField] private Sprite[] sprites;

    // list to store past predictions
    private List<int> indexBuffer = new List<int>();

    // number of predictions to store
    private int indexBufferSize = 15;

    private Dictionary<int, List<int>> emgDictionary = new Dictionary<int, List<int>>() {
        { 0, new List<int>() },
        { 1, new List<int>() },
        { 2, new List<int>() },
        { 3, new List<int>() },
        { 4, new List<int>() },
        { 5, new List<int>() },
        { 6, new List<int>() },
        { 7, new List<int>() }
    };

    private int bufferSize = 30;

    private float[,,,] emgArray = new float[1, 30, 8, 1];


    private void Awake()
    {
        // get reference to data script from app manager
        appManager = GameObject.Find("AppManager");
        dataHandling = appManager.GetComponent<DataHandling>();

    }

    private void Start()
    {
        
        // initialise 4d array
        emgArray[0,0,0,0] = 0f;

        model = ModelLoader.Load(modelFile);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.CSharpBurst, model);
        rb = Player.transform.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        for (int i = 0; i < dataHandling.newEMG.Count; i++)
        {
            emgDictionary[i].AddRange(dataHandling.newEMG[i]);
            if (emgDictionary[i].Count > bufferSize) emgDictionary[i].RemoveRange(0, emgDictionary[i].Count - bufferSize);       
        }

        if (emgDictionary[0].Count == bufferSize)
        {
            for (int i = 0; i < emgDictionary.Count; i++)
            {
                List<int> channel = emgDictionary[i];

                for (int j = 0; j < channel.Count; j++)
                {
                    emgArray[0, j, i, 0] = (float)channel[j];
                }
            }

            RunModel();
        }
    }

    void RunModel()
    {
        StartCoroutine(RunModelRoutine());
    }

    IEnumerator RunModelRoutine()
    {
        Tensor inputTensor = new Tensor(batchSize, height, width, channels, emgArray);  

        worker.Execute(inputTensor);

        Tensor outputTensor = worker.PeekOutput("out_layer");

        //get largest output
        List<float> temp = outputTensor.ToReadOnlyArray().ToList();
        float max = temp.Max();
        int index = temp.IndexOf(max);

        // add index to buffer and remove oldest value
        indexBuffer.Add(index);
        if (indexBuffer.Count > indexBufferSize) indexBuffer.RemoveAt(0);

        // find mode of buffer
        int modeIndex =
         indexBuffer
         .GroupBy(x => x)
         .OrderByDescending(x => x.Count()).ThenBy(x => x.Key)
         .Select(x => (int)x.Key)
         .FirstOrDefault();

        Vector3 forwardMove = Player.transform.forward * speed * Time.deltaTime;
        Vector3 horizontalMove = Player.transform.right * 1 * speed * Time.deltaTime;
        // Control of the player
        switch (modeIndex)
        {
            case 0:
                horizontalMove *= 1;
                rb.MovePosition(rb.position + forwardMove + horizontalMove);
                break;
            case 3:
                horizontalMove *= -1;
                rb.MovePosition(rb.position + forwardMove + horizontalMove);
                break;
            case 4:
                horizontalMove *= 1;
                rb.MovePosition(rb.position + forwardMove + horizontalMove);
                break;
            default:
                horizontalMove *= -1;
                rb.MovePosition(rb.position + forwardMove + horizontalMove);
                break;
        }

        //dispose tensors
        inputTensor.Dispose();
        outputTensor.Dispose();

        yield return null;
    }

}
