using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
using UnityEngine.Events;
using UnityEngine.UI;

// This object will react when it's touched in VR with the hands using Oculus hand tracking. 

public class PoiTouchHandler : MonoBehaviour
{
    public UnityEvent _proximityEvent;
    bool _active;

    void Start()
    {
        GetComponent<ButtonController>().InteractableStateChanged.AddListener(InitiateEvent);
    }


    public void InitiateEvent(InteractableStateArgs args)
    {
        if (args.NewInteractableState == InteractableState.ProximityState)
            _proximityEvent.Invoke();
    }


    public void ActivatePoi(string soundName)
    {
        //called when object is touched with hands

        if (_active)
        {
            GameLogicHandler.Instance.PoiActivated(gameObject, soundName);
        }
    }

    public void Toggle(bool activate)
    {
        GetComponent<Animator>().SetBool("Activate", activate);
        _active = activate;
    }

}
