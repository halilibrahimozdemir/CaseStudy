using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
 
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static GameManager instance;
    #endregion

    private bool _isWaterRised;
    private bool _canGoNext;
    public GameObject playButton;
    public GameObject question;
    public Text questionText;
    public GameObject inputField;
    public Text inputText;
    public GameObject answerButton;
    public GameObject water;
    public GameObject GameOverWindow;
    public GameObject WinningWindow;
    [SerializeField]
    private bool isWaterRising;
    public float riseSpeed;
    [SerializeField]
    private Vector3 _waterOldPos;
    [SerializeField]
    private Vector3 _desiredWaterPos;
    public GameObject waterAlert;
    private const int QuestionCount = 2;
    [SerializeField]
    private int _questionCurrentCount;
    
    public int QuestionCurrentCount
    {
        get
        {
            //Some other code
            return _questionCurrentCount;
        }
        set
        {
            //Some other code
            _questionCurrentCount = value;
        }
    }
    
    [System.Serializable]
    public struct Question
    {
        public string question;
        public string[] answers;
    }

    public Question[] Questions;
    private void Start()
    {
        _questionCurrentCount = 0;
    }

    private void Update()
    {
        if (Player.Instance.transform.position.y <= water.transform.position.y)
        {
            GameOverWindow.SetActive(true);
        }

        if (Opponent.Instance.transform.position.y <= water.transform.position.y)
        {
            WinningWindow.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        if (isWaterRising)
        {
            if (Mathf.Abs(water.transform.position.y-_desiredWaterPos.y)>=0.05f)
            {
                water.transform.position=Vector3.Lerp(water.transform.position,
                    _desiredWaterPos,riseSpeed*Time.deltaTime);
            }
            else
            {
                isWaterRising = false;
                waterAlert.SetActive(false);
                _isWaterRised = true;
                if (_questionCurrentCount < Questions.Length-1)
                {
                    _questionCurrentCount++;   
                }
            }  
        }

        if (_isWaterRised && !GameOverWindow.activeSelf && !WinningWindow.activeSelf)
        {
            _canGoNext = true;
        }

        if (_canGoNext)
        {
            playButton.SetActive(true);
            _canGoNext = false;
            _isWaterRised = false;
        }
    }

    public void StartGame()
    {
        inputText.color=Color.black;
        playButton.SetActive(false);
        questionText.text = Questions[_questionCurrentCount].question;
        question.SetActive(true);
        inputField.SetActive(true);
        answerButton.SetActive(true);
    }
    
    public void WaterRise()
    {
        waterAlert.SetActive(true);
        _waterOldPos = water.transform.position;
        _desiredWaterPos = _waterOldPos + new Vector3(0, 3, 0); 
        
        isWaterRising = true;
    }

    public IEnumerator WaitAndRiseWater()
    {
        yield return new WaitForSeconds(4f);
        WaterRise();
    }

    public void Answered()
    {
        answerButton.SetActive(false);
        bool isTrue=false;
        foreach (var q in Questions)
        {
            foreach (var answer in q.answers)
            {
                if (Player.Instance.word == answer )
                {
                    var texts = inputField.GetComponentsInChildren<Text>();
                    foreach (var text in texts)
                    {
                        text.GetComponent<Text>().color=Color.green;
                    }

                    isTrue = true;
                    break;
                }
                else
                {
                    var texts = inputField.GetComponentsInChildren<Text>();
                    foreach (var text in texts)
                    {
                        text.GetComponent<Text>().color=Color.red;
                    }
                } 
            }
            if (isTrue)
            {
                StartCoroutine(WaitABitTrue());
                break;
            }   
        }
        
        /*if (isTrue)
        {
            StartCoroutine(WaitABitTrue());
        }*/   
        if (!isTrue)
        {
            StartCoroutine(WaitABitFalse());
        }
    }

    public IEnumerator WaitABitTrue()
    {
        yield return new WaitForSeconds(1f);
        Player.Instance.SpawnBlocks();
        Opponent.Instance.SpawnBlocks();
        StartCoroutine(WaitAndRiseWater());
        question.SetActive(false);
        inputField.SetActive(false);
    }
    
    public IEnumerator WaitABitFalse()
    {
        yield return new WaitForSeconds(1f);
        
        Opponent.Instance.SpawnBlocks();
        StartCoroutine(WaitAndRiseWater());
        question.SetActive(false);
        inputField.SetActive(false);
        answerButton.SetActive(false);
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}

