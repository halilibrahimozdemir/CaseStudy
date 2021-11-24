using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int length;
    public string word;
    public Cube cubePrefab;
    
    private static Player _instance;

    public static Player Instance
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
  
    
    public void SetBlocks()
    {
        Vector3 pos=Vector3.zero;
        Vector3 originPos = new Vector3(0, -1, 0);
        length = word.Length;
        for (int i = 0; i < length; i++)
        {
            pos = originPos + pos + new Vector3(0, 1, 0);
            var cube = Instantiate(cubePrefab, pos, Quaternion.identity);
            cube._text.text = word[length-1-i].ToString();
            transform.position = cube.transform.position + new Vector3(0, 1, 0);
        }
    }
}
