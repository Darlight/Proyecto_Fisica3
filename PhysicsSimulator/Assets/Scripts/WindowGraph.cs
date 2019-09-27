using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private Sprite circleSprite;
    private RectTransform graphContainer;
    public TMP_InputField Amplitud;
    public TMP_InputField nOnda;
    public TextMeshProUGUI lambda;
    private float altura;

    public TMP_InputField desfase;
    public TMP_InputField periodo;
    public TextMeshProUGUI frecuencia;
    public TextMeshProUGUI tension;
    public TextMeshProUGUI aceleracion;
    public TextMeshProUGUI velocidad;
    public TMP_InputField velocidad_angular;
    public TMP_InputField nodos;


    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<GameObject> toDestroy;
    private List<float> valueList;

    private void Awake()
    {
        toDestroy = new List<GameObject>();
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        velocidad = GetComponent<TextMeshProUGUI>();
        //desfase.SetText("0");
        //periodo.SetText("0");
        //frecuencia.SetText("0");
        //tension.SetText("0");
        //aceleracion.SetText("0");
        //velocidad.SetText("0");
        //Amplitud.SetText("0");
        //velocidad_angular.SetText("0");
        //nOnda.SetText("0");

    }

    private void Update()
    {
        /*
        float amp= 0, ondas = 0, desfases = 0;
        //string temp = Amplitud.text;
        if(Amplitud.text.Length > 0)
            amp = float.Parse(Amplitud.text);

        if (nOnda.text.Length > 0)
            ondas = float.Parse(nOnda.text);

        if (desfase.text.Length > 0)
            desfases = float.Parse(desfase.text);



        valueList = new List<float>() { };

        timer += Time.deltaTime;
        if(timer > 1.0f)
        {
            timer = 0.0f;
            for (int i = 0; i < 350; i++)
            {
                altura = amp * Mathf.Sin(ondas * i * 0.01f + desfases);
                valueList.Add(altura);
            }
            ShowGraph(valueList);
        }
        */
    }
    public void drawGraph()
    {
        float amp = 0, ondas = 0, desfases = 0, f_angular = 0, time = 0;
        //string temp = Amplitud.text;
        if (Amplitud.text.Length > 0)
            amp = float.Parse(Amplitud.text);

        if (nOnda.text.Length > 0)
            ondas = float.Parse(nOnda.text);

        if (desfase.text.Length > 0)
            desfases = float.Parse(desfase.text);
        if (velocidad_angular.text.Length > 0)
            f_angular = float.Parse(velocidad_angular.text);
        if (periodo.text.Length > 0)
            time = float.Parse(periodo.text);

        if (toDestroy.Count > 0)
        {
            DestroyGraph();
            valueList = new List<float>() { };
            for (int i = 0; i < 350; i++)
            {
                altura = amp * Mathf.Sin(ondas * i * 0.01f + desfases + (f_angular*time));
                valueList.Add(altura);
            }
            ShowGraph(valueList);
        }
        else
        {
            valueList = new List<float>() { };
            for (int i = 0; i < 350; i++)
            {
                altura = amp * Mathf.Sin(ondas * i * 0.01f + desfases);
                valueList.Add(altura);
            }
            ShowGraph(valueList);
        }

        float vel = amp * f_angular;
        velocidad.text = vel.ToString();
        

    }
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("Knob", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
        
    }

    private void ShowGraph(List<float> valueList)
    {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 10f;
        float xSize = 2f;

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight + 190f;
            GameObject circleGameObject; 
            circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            toDestroy.Add(circleGameObject);
            if (lastCircleGameObject != null)
            {
                CreatDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);

            }
            lastCircleGameObject = circleGameObject;

            /*RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, 250);
            labelX.GetComponent<Text>().text = i.ToString();
            */
            //RectTransform dashX = Instantiate(dashTemplateX);
            //dashX.SetParent(graphContainer);
            //dashX.gameObject.SetActive(true);
            //dashX.anchoredPosition = new Vector2(xPosition, 12f);
        }
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateX);
            labelY.SetParent(graphContainer);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-30f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = (normalizedValue * yMaximum).ToString();

            //RectTransform dashY = Instantiate(dashTemplateY);
            //dashY.SetParent(graphContainer);
            //dashY.gameObject.SetActive(true);
            //dashY.anchoredPosition = new Vector2(-345, normalizedValue * graphHeight);
        }


    }

    private void CreatDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * 0.5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

    public void DestroyGraph()
    {
        //Destruye todos los objetos.
        toDestroy.Clear();
        valueList.Clear();
        foreach (Transform child in graphContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }


    }
}
