using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class TextureCombine : MonoBehaviour
{
    public GameObject FileBrowserObj;
    public GameObject imagesManagerObj;
    public RawImage faceImage;
    public float[] points;

    public float PIXEL_WIDTH = 2136f;
    public float PIXEL_HEIGHT = 3216f;
    public float PIXEL_FACEIMAGE_WIDTH = 715f;
    private const float PIXEL_FACEIMAGE_HEIGHT = 1080f;

    public Texture2D currentTexture;
    public string currentImageName;

    public GameObject PortraitSpawn;

    public void ChangeRatio()
    {
        PIXEL_FACEIMAGE_WIDTH = (PIXEL_FACEIMAGE_HEIGHT * PIXEL_WIDTH) / PIXEL_HEIGHT;
        RectTransform rectTransform = faceImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(PIXEL_FACEIMAGE_WIDTH, rectTransform.sizeDelta.y);
    }

    public void DeleteRect()
    {
        foreach (Transform child in faceImage.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SaveButton()
    {
        CalculateRectangleCoordinates();
    }

    public void CheckingPortrait()
    {
        Transform childTransform = PortraitSpawn.transform.Find(currentImageName);

        if (childTransform != null)
        {
            GameObject childObject = childTransform.gameObject;

            // Find the "CheckingImage" child of the childObject
            Transform checkingImageTransform = childObject.transform.Find("CheckingImage");

            if (checkingImageTransform != null)
            {
                GameObject checkingImage = checkingImageTransform.gameObject;
                checkingImage.SetActive(true);
            }
            else
            {
                Debug.LogWarning($"Child named 'CheckingImage' not found under {currentImageName}.");
            }
        }
        else
        {
            Debug.LogWarning($"Child with the name {currentImageName} not found under PortraitSpawn.");
        }



    }


    public void CalculateRectangleCoordinates()
    {
        

        points = new float[4];
        List<GameObject> blindRects = new List<GameObject>();
        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.name == "EyeBlindRect")
            {
                blindRects.Add(obj);
            }
        }
        if (blindRects.Count == 0)
            return;

        foreach(GameObject blindRect in blindRects)
        {
            RectTransform rectTransform = blindRect.GetComponent<RectTransform>();

            Vector2 pivot = rectTransform.pivot;
            Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * rectTransform.sizeDelta.x, (0.5f - pivot.y) * rectTransform.sizeDelta.y);
            Vector2 adjustedPosition = rectTransform.anchoredPosition + pivotOffset;

            Vector2 center = adjustedPosition + new Vector2(PIXEL_FACEIMAGE_WIDTH / 2, PIXEL_FACEIMAGE_HEIGHT / 2);
            Vector2 topLeft = new Vector2(center.x - rectTransform.sizeDelta.x / 2, center.y + rectTransform.sizeDelta.y / 2);
            Vector2 bottomRight = new Vector2(center.x + rectTransform.sizeDelta.x / 2, center.y - rectTransform.sizeDelta.y / 2);

            float x1 = (topLeft.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
            float y1 = (bottomRight.y / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);
            float x2 = (bottomRight.x / PIXEL_FACEIMAGE_WIDTH * PIXEL_WIDTH);
            float y2 = (topLeft.y / PIXEL_FACEIMAGE_HEIGHT * PIXEL_HEIGHT);

            points[0] = x1;
            points[1] = y1;
            points[2] = x2;
            points[3] = y2;

            Texture2DCombine();
        }
        SaveJPG();
        CheckingPortrait();
        DeleteRect();
    }

    public void Texture2DCombine()
    {
        int idx = 0;
        foreach (string imageName in imagesManagerObj.GetComponent<Images>().imageNames)
        {
            if (imageName.Equals(currentImageName))
                break;
            idx++;
        }
        currentTexture = imagesManagerObj.GetComponent<Images>().imageTexture2Ds[idx];

        // points���� ��ǥ�� �����ɴϴ�.
        int xStart = Mathf.FloorToInt(points[0]);
        int yStart = Mathf.FloorToInt(points[1]);
        int xEnd = Mathf.FloorToInt(points[2]);
        int yEnd = Mathf.FloorToInt(points[3]);

        // Ư�� ������ �ȼ��� ���������� �����մϴ�.
        for (int x = xStart; x <= xEnd; x++)
        {
            for (int y = yStart; y <= yEnd; y++)
            {
                currentTexture.SetPixel(x, y, Color.black);
            }
        }

        // ���� ������ �����մϴ�.
        currentTexture.Apply();
    }

    public void SaveJPG()
    {
        byte[] bytes = currentTexture.EncodeToJPG();
        // �ؽ�ó�� JPG ����Ʈ �迭�� ���ڵ��մϴ�.
        
        string currentPath = FileBrowserObj.GetComponent<FileBrowserTest>().filePath;
        // ������ ���丮 ��θ� �����մϴ�.
        string directoryPath = Path.Combine(currentPath, "����̹���");

        string outputImageName = currentImageName;
        if (currentImageName.Contains("jpeg"))
        {
            outputImageName = currentImageName.Replace(".jpeg", ".jpg");
        }
        // ���丮�� ������ �����մϴ�.
        Directory.CreateDirectory(directoryPath);
        string filePath = Path.Combine(directoryPath, outputImageName);

        // ����Ʈ �迭�� ���Ϸ� �����մϴ�.
        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"Image saved to: {filePath}");
    }
}

