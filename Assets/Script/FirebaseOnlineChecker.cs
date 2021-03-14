using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseOnlineChecker : MonoBehaviour
{
    LoginScript theLoginScript;
    private bool StatusIsChecking;
    public float StatuCheckInterval = 60;
    public float StatuCheckTime = 60;
    public bool InEliminationGame = false;

    private void Awake()
    {
        theLoginScript = gameObject.GetComponent<LoginScript>();
    }

    private void Start()
    {
        theLoginScript = gameObject.GetComponent<LoginScript>();
    }

    private void Update()
    {
        if(InEliminationGame)
        {
            if (!StatusIsChecking)
            {
                if (StatuCheckTime <= 0)
                {
                    SendStatusOnline();
                    StatusIsChecking = true;
                }
                else
                {
                    StatuCheckTime -= 1 * Time.deltaTime;
                }
            }
            else
            {
                StatuCheckTime = StatuCheckInterval;
            }
        }
    }

    public void SendStatusOnline()
    {
        StartCoroutine(ChangeStatusToOnline());
    }

    public void SendStatusOffline()
    {
        StartCoroutine(ChangeStatusToOffline());
    }

    IEnumerator ChangeStatusToOnline()
    {
        var ATask = theLoginScript.DBreference.Child("users").Child(theLoginScript.User.UserId).Child("Status").SetValueAsync("Online");
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);
        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
            StatusIsChecking = false;
        }
    }

    IEnumerator ChangeStatusToOffline()
    {
        var ATask = theLoginScript.DBreference.Child("users").Child(theLoginScript.User.UserId).Child("Status").SetValueAsync("Offline");
        yield return new WaitUntil(predicate: () => ATask.IsCompleted);
        if (ATask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ATask.Exception}");
        }
        else
        {
        }
    }
}
