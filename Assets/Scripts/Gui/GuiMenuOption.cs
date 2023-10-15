using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class GuiMenuOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI selector;

    [SerializeField] private UnityEvent action;

    public void Select()
    {
        selector.text = ">";
    }

    public void Deselect()
    {
        selector.text = "";
    }

    public void PerformAction()
    {
        action?.Invoke();
    }
}
