using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemListScript : MonoBehaviour
{
    public int ButtonNumber;
    private Image ButtonImage;

    // Start is called before the first frame update
    void Start()
    {
        ButtonImage = gameObject.GetComponent<Image>();
    }
    public void SetupButton(int _buttonNumber, Sprite ImageToShow)
    {
        ButtonImage = gameObject.GetComponent<Image>();
        ButtonNumber = _buttonNumber;
        ButtonImage.sprite = ImageToShow;
    }
}
