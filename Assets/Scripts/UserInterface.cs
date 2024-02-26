using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UserInterface : MonoBehaviour
{
    private VisualElement userInterface;

    // Start is called before the first frame update
    void Start()
    {
        userInterface = GetComponent<UIDocument>().rootVisualElement;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPieceCamp(PieceCamp camp)
    {
        var label = userInterface.Q<Label>("PieceCamp");

        if(camp == PieceCamp.Money) {
            label.text = "Money";
        } else
        {
            label.text = "Nature";
        }
    }
}
