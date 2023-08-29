using UnityEngine;
using UnityEngine.UI;

public class Portrait : MonoBehaviour
{
    public GameObject checkingImage;
    public GameObject faceImageObj;
    public GameObject TextureCombineManagerObj;

    public void PortraitClick()
    {
        faceImageObj = GameObject.Find("faceImage");
        faceImageObj.GetComponent<RawImage>().texture = transform.GetComponent<Image>().sprite.texture;

        TextureCombineManagerObj = GameObject.Find("TextureCombineManager");
        TextureCombineManagerObj.GetComponent<TextureCombine>().currentImageName = transform.gameObject.name;
    }
}


