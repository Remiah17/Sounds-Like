using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuessItChoiceButton : MonoBehaviour
{
    public GuessItManager TheManager;
    public Image ItemImg;
    public string ItemString;
    public int ButtonNumber;
    public bool Gamestarted;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gamestarted = TheManager.LevelStarted;
        if(Gamestarted)
        {
            //ItemString = TheManager.CurrentItemChoice[ButtonNumber].ItemName;
           // if (TheManager.CurrentItemChoice[ButtonNumber].ItemSprite != null)
           // {
               // ItemImg.sprite = TheManager.CurrentItemChoice[ButtonNumber].ItemSprite;
            //}
        }
    }
}
