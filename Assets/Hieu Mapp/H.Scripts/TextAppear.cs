using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;

public class TextAppear : MonoBehaviour
{
    public TextMeshPro tmpText;
    public int codeLength = 6;    // Length of the generated code
    private int code;

    void Start()
    {
        code = (int)UnityEngine.Random.Range(1000f, 9999f);
        tmpText.text = code.ToString();

    }

    /*string GenerateRandomCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] codeArray = new char[length];

        for (int i = 0; i < length; i++)
        {
            codeArray[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }

        return new string(codeArray);
    }*/
}

