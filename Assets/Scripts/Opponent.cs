using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour
{
    private int _length;
    public string word;
    public Cube cubePrefab;
    public float timeBetweenSpawns;
    private static Opponent _instance;

    public static Opponent Instance
    {
        get { return _instance; }
        set => _instance = value;
    }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void Start()
    {
        DefaultBlocks();
    }

    private void Update()
    {
        word = GameManager.Instance.Questions[GameManager.Instance.QuestionCurrentCount].answers[0];
    }

    public void DefaultBlocks()
    {
        var b = Instantiate(cubePrefab, transform.position + new Vector3(0, -1, 0), Quaternion.identity);
        var a = Instantiate(cubePrefab, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        a._text.text = "";
        b._text.text = "";
    }
    
    public void OpSetBlocks()
    {
        Vector3 pos=Vector3.zero;
        Vector3 originPos = new Vector3(2, 0, 0);
        _length = word.Length;
        for (int i = 0; i < _length; i++)
        {
            pos = originPos + new Vector3(0, 1, 0)*(i+1);
            var cube = Instantiate(cubePrefab, pos, Quaternion.identity);
            cube._text.text = word[_length-1-i].ToString();
            transform.position = cube.transform.position + new Vector3(0, 1, 0);
        }
    }
    
    public void SpawnBlocks()
    {
        StartCoroutine(CreateBlocks());
    }
    public IEnumerator CreateBlocks()
    {
        Vector3 pos=Vector3.zero;
        Vector3 originPos = transform.position+new Vector3(0,-1,0);
        _length = word.Length;
        for (int i = 0; i < _length; i++)
        {
            pos = originPos + new Vector3(0, 1, 0)*(i+1);
            var cube = Instantiate(cubePrefab, pos, Quaternion.identity);
            cube._text.text = word[_length-1-i].ToString();
            transform.position = cube.transform.position + new Vector3(0, 1, 0);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }
}
