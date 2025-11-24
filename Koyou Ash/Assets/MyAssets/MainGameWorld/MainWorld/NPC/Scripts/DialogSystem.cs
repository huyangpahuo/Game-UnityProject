using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [Header("UI组件")] public Text textLabel;
    public Image faceImage;

    [Header("文本文件")] public TextAsset textFile;
    public int index;

    [Header("头像")] public Sprite face01, face02;

    List<string> textList = new List<string>();

    public float testSpeed; //文本显示速度
    private bool textFinished; //判断文本是否显示完毕
    private bool cancelTyping; //取消打字

    void Awake()
    {
        GetTextFromFile(textFile);
        index = 0;
    }

    private void OnEnable()
    {
        //textLabel.text = textList[index];
        //index++;
        textFinished = true;
        StartCoroutine(SetTextUI());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && index == textList.Count)
        {
            gameObject.SetActive(false);
            index = 0;
        }

        // if (Input.GetKeyDown(KeyCode.Space) && textFinished)
        // {
        //     if (index < textList.Count)
        //     {
        //         //每按一次R键就会显示下一行文本
        //         // textLabel.text = textList[index];
        //         //index++;
        //         StartCoroutine(SetTextUI());
        //     }
        //     else
        //     {
        //         gameObject.SetActive(false);
        //         index = 0;
        //     }
        // }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (textFinished && !cancelTyping)
            {
                if (index < textList.Count)
                {
                    //每按一次R键就会显示下一行文本
                    // textLabel.text = textList[index];
                    //index++;
                    StartCoroutine(SetTextUI());
                }
                else
                {
                    gameObject.SetActive(false);
                    index = 0;
                }
            }
            else if (!textFinished && !cancelTyping)
            {
                cancelTyping = true;
            }
        }
    }

    void GetTextFromFile(TextAsset file)
    {
        //清空文本列表,不然就会重复添加
        textList.Clear();
        index = 0;

        //按行分割文本文件
        var lineData = file.text.Split('\n');
        foreach (var line in lineData)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        switch (textList[index])
        {
            case "A":
                faceImage.sprite = face01;
                index++;
                break;
            case "B":
                faceImage.sprite = face02;
                index++;
                break;
        }


        // for (int i = 0; i < textList[index].Length; i++)
        // {
        //     textLabel.text += textList[index][i];
        //     yield return new WaitForSeconds(testSpeed);
        // }

        int letter = 0;
        while ((letter < textList[index].Length - 1) && !cancelTyping)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(testSpeed);
        }

        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }
}