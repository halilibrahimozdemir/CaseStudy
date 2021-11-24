using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInput : MonoBehaviour
{
    private string _input;

    public void ReadInput(string s)
    {
        _input = s;
        Player.Instance.word = s;
    }
}
