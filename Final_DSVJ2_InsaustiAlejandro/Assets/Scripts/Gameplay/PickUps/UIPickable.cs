using UnityEngine;
using TMPro;

public class UIPickable : MonoBehaviour
{
	[SerializeField] PickableController controller;
	[SerializeField] Canvas canvas;
	[SerializeField] TextMeshProUGUI valueText;

    //Unity Events
    private void Start()
    {
        canvas.worldCamera = Camera.main;
        valueText.text = "+" + controller.publicValue;
    }
    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}