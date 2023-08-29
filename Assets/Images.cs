using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Images : MonoBehaviour
{
    public List<string> imageNames = new List<string>();
    public List<byte[]> imageBytes = new List<byte[]>();
    public List<Texture2D> imageTexture2Ds = new List<Texture2D>();
    public GameObject initPanelPortraitSpawn;
    public GameObject mainPanelPortraitSpawn;
    public GameObject portraitPrefab;
    public GameObject textureCombineObj;

    public void ClearVariables()
    {
        imageNames.Clear();
        imageBytes.Clear();
        imageTexture2Ds.Clear();
    }

    public void AddImageName(string imageName)
    {
        imageNames.Add(imageName);
    }

    public void ByteToTexture2D()
    {
        foreach (byte[] imageByte in imageBytes)
        {
            Texture2D texture = new Texture2D(2, 2);  // 이 줄을 foreach 루프 내로 이동
            texture.LoadImage(imageByte);
            imageTexture2Ds.Add(texture);
            int width = texture.width;
            int height = texture.height;
            textureCombineObj.GetComponent<TextureCombine>().PIXEL_WIDTH = width;
            textureCombineObj.GetComponent<TextureCombine>().PIXEL_HEIGHT = height;
            Debug.Log("Texture Width: " + width);
            Debug.Log("Texture Height: " + height);
        }
    }

    public void AddImageBytes(byte[] imageByte)
    {
        imageBytes.Add(imageByte);
    }

    public void InitPortraitInstantiate()
    {
        for (int i = 0; i < imageBytes.Count; i++)
        {
            GameObject portraitInstance = Instantiate(portraitPrefab, initPanelPortraitSpawn.transform);
            portraitInstance.GetComponent<Image>().sprite = Sprite.Create(imageTexture2Ds[i], new Rect(0, 0, imageTexture2Ds[i].width, imageTexture2Ds[i].height), Vector2.one * 0.5f);
            Destroy(portraitInstance.GetComponent<Button>());
        }
        
    }
    public void MainPortraitInstatiate()
    {
        for (int i = 0; i < imageBytes.Count; i++)
        {
            GameObject portraitInstance = Instantiate(portraitPrefab, mainPanelPortraitSpawn.transform);
            portraitInstance.name = imageNames[i];
            portraitInstance.GetComponent<Image>().sprite = Sprite.Create(imageTexture2Ds[i], new Rect(0, 0, imageTexture2Ds[i].width, imageTexture2Ds[i].height), Vector2.one * 0.5f);
        }
    }
}
