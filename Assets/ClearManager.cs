using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearManager : MonoBehaviour
{
    public GameObject mainPanelScrollViewContent;
    public GameObject initPanelScrollViewContent;

    public void ClearObjs()
    {
        foreach (Transform child in mainPanelScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in initPanelScrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
