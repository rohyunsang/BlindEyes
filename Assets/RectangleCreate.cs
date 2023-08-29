using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RectangleCreate : MonoBehaviour
{
    public GameObject rectPrefab;
    public Transform faceImage;
    public GameObject buttonPrefab;
    public GameObject buttonInstance;
    public GameObject deleteButtonsPrefab;

    private Vector2 localPointerPosition;

    private void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { OnPointerDown((PointerEventData)eventData); });

        eventTrigger.triggers.Add(pointerDownEntry);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RectTransform faceImageRect = faceImage as RectTransform;

            // Convert screen position to local position in faceImage
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(faceImageRect, eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                // Instantiate the button at the right-click position
                buttonInstance = Instantiate(buttonPrefab, faceImage);
                buttonInstance.GetComponent<RectTransform>().anchoredPosition = localPointerPosition;

                // Find the CreateButton and add a listener
                Button createButton = buttonInstance.transform.Find("CreateButton").GetComponent<Button>();
                if (createButton != null)
                {
                    createButton.onClick.AddListener(CreateRectangle);
                    // Destroy the CreateButton after adding the listener so that it doesn't appear on the UI
                    
                }

                // Find the CancelButton and add a listener
                Button cancelButton = buttonInstance.transform.Find("CancelButton").GetComponent<Button>();
                if (cancelButton != null)
                {
                    cancelButton.onClick.AddListener(() => Destroy(buttonInstance));
                }
            }
        }
    }

    public void CreateRectangle()
    {
        GameObject rectInstance = Instantiate(rectPrefab, faceImage);
        rectInstance.GetComponent<RectTransform>().anchoredPosition = localPointerPosition;
        rectInstance.gameObject.name = "EyeBlindRect";

        EventTrigger eventTrigger = rectInstance.AddComponent<EventTrigger>();
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((eventData) => { OnRectRightClick((PointerEventData)eventData, rectInstance); });
        eventTrigger.triggers.Add(pointerDownEntry);

        Destroy(buttonInstance.gameObject);
    }
    private void OnRectRightClick(PointerEventData eventData, GameObject rectInstance)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Instantiate the delete buttons prefab next to the rectInstance
            GameObject deleteButtonsInstance = Instantiate(deleteButtonsPrefab, faceImage);

            RectTransform rectTransform = rectInstance.GetComponent<RectTransform>();
            deleteButtonsInstance.GetComponent<RectTransform>().anchoredPosition = rectTransform.anchoredPosition + new Vector2(rectTransform.rect.width, 0) + new Vector2(-100f,50f); // adjust as needed

            // Find the DeleteButton and add a listener to delete the rectangle
            Button deleteButton = deleteButtonsInstance.transform.Find("DeleteButton").GetComponent<Button>();
            if (deleteButton != null)
            {
                deleteButton.onClick.AddListener(() => {
                    Destroy(rectInstance);
                    Destroy(deleteButtonsInstance);
                });
            }

            // Find the CancelButton and add a listener to delete the deleteButtonsInstance
            Button cancelButton = deleteButtonsInstance.transform.Find("CancelButton").GetComponent<Button>();
            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(() => Destroy(deleteButtonsInstance));
            }
        }
    }


}
