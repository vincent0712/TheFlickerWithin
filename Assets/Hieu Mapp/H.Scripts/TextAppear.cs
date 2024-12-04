using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
    private string input;

    public void ReadCode(string code)
    {
        input = code;
        Debug.Log(input);
    }
}
