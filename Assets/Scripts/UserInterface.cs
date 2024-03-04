using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UserInterface : MonoBehaviour
{
    private VisualElement userInterface;
    private PieceCamp camp;

    // Start is called before the first frame update
    public void Init()
    {
        userInterface = GetComponent<UIDocument>().rootVisualElement;
        var button = userInterface.Q<Button>("GiveUp");
        button.RegisterCallback<ClickEvent>(HandleClickGiveUp);
    }

    private void HandleClickGiveUp(ClickEvent evt)
    {
        var label = userInterface.Q<Label>("WinnerText");

        if (camp == PieceCamp.Nature)
        {
            label.text = "Money WINS!";
        }
        else
        {
            label.text = "Nature WINS!";
        }

        userInterface.Q<VisualElement>("WinnerBackground").visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPieceCamp(PieceCamp camp)
    {
        var label = userInterface.Q<Label>("PieceCamp");

        this.camp = camp;

        if(camp == PieceCamp.Money) {
            label.text = "Money";
        } else
        {
            label.text = "Nature";
        }
    }
}
