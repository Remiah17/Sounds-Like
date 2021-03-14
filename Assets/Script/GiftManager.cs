using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GiftManager : MonoBehaviour
{
    [SerializeField]
    private LoginScript theLoginScript;

    [SerializeField]
    private int[] GiftsID;

    [SerializeField]
    private Transform GiftListPanel;
    [SerializeField]
    private GameObject giftElement;

    void OnEnable()
    {
        StartCoroutine(CheckGifts());
    }

    IEnumerator CheckGifts()
    {
        theLoginScript.ShowLoadingScreen("Checking for gift datas.");
        var ATask = theLoginScript.DBreference.Child("users").Child(theLoginScript.User.UserId).Child("Reward Mail").GetValueAsync();

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
            theLoginScript.ShowLoadingScreen("Checking gift datas failed.");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = ATask.Result;

            //destroy any existing scoreboard elements
            foreach (Transform child in GiftListPanel.transform)
            {
                Destroy(child.gameObject);
            }

            int num = 0;
            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                GiftsID[num] = int.Parse(childSnapshot.Key.ToString());
                if(GiftsID[num] != 0)
                {
                    GameObject giftelement = Instantiate(giftElement, GiftListPanel);
                    giftelement.GetComponent<GiftElement>().SetupGiftButton(int.Parse(childSnapshot.Key.ToString()));
                }
                num += 1;
            }
            theLoginScript.HideLoadingScreen();
        }
    }

 

   public IEnumerator UpdateGifts(int IDToRemove)
    {
        theLoginScript.ShowLoadingScreen("Updating gift datas.");
        var ATask = theLoginScript.DBreference.Child("users").Child(theLoginScript.User.UserId).Child("Reward Mail").Child(IDToRemove.ToString()).SetValueAsync(null);

        yield return new WaitUntil(predicate: () => ATask.IsCompleted);

        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
            theLoginScript.ShowLoadingScreen("Updating gift datas failed.");
        }
        else
        {
            StartCoroutine(CheckGifts());
        }
    }

}
