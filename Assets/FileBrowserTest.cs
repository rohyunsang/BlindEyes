using UnityEngine;
using System.Collections;
using System.IO;
using TMPro;
using SimpleFileBrowser;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class FileBrowserTest : MonoBehaviour
{
    public Dictionary<string, string> jsonStrings = new Dictionary<string, string>(); // fileName, fileBytes
    public GameObject ImageManagerObj;
    public GameObject TextureCombineManagerObj;

    public string filePath = "";

    public void ShowFileBrowser()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Files", ".jpg", ".png", ".json"), new FileBrowser.Filter("Text Files", ".txt", ".pdf"));
        FileBrowser.SetDefaultFilter(".json");
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        if (FileBrowser.Success)
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
                string extension = Path.GetExtension(FileBrowser.Result[i]);

                if (extension == "")
                {
                    // 현재 디렉토리의 .jpg 파일 처리
                    List<string> jpgFiles = GetFilesInCurrentDirectory(FileBrowser.Result[i], "*.jpg");
                    List<string> sortedJpgFiles = jpgFiles.OrderBy(Path.GetFileName).ToList();
                    foreach (string jpgFile in sortedJpgFiles)
                    {
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jpgFile);
                        ImageManagerObj.GetComponent<Images>().AddImageBytes(bytes);

                        // .jpg 파일의 현재 디렉토리를 가져옵니다.
                        string currentDirectory = Path.GetDirectoryName(jpgFile);
                        filePath = Path.GetDirectoryName(jpgFile);
                        Debug.Log("Current Directory of " + Path.GetFileName(jpgFile) + ": " + currentDirectory);
                        ImageManagerObj.GetComponent<Images>().AddImageName(Path.GetFileName(jpgFile));
                    }

                    // 현재 디렉토리의 .jpeg 파일 처리
                    List<string> jpegFiles = GetFilesInCurrentDirectory(FileBrowser.Result[i], "*.jpeg");
                    List<string> sortedJpegFiles = jpegFiles.OrderBy(Path.GetFileName).ToList();
                    foreach (string jpegFile in sortedJpegFiles)
                    {
                        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(jpegFile);
                        ImageManagerObj.GetComponent<Images>().AddImageBytes(bytes);

                        // .jpeg 파일의 현재 디렉토리를 가져옵니다.
                        string currentDirectory = Path.GetDirectoryName(jpegFile);
                        filePath = Path.GetDirectoryName(jpegFile);
                        Debug.Log("Current Directory of " + Path.GetFileName(jpegFile) + ": " + currentDirectory);
                        ImageManagerObj.GetComponent<Images>().AddImageName(Path.GetFileName(jpegFile));
                    }
                }
            }
            ImageManagerObj.GetComponent<Images>().ByteToTexture2D();
            ImageManagerObj.GetComponent<Images>().InitPortraitInstantiate();
            ImageManagerObj.GetComponent<Images>().MainPortraitInstatiate();
        }
    }

    public List<string> GetFilesInCurrentDirectory(string directoryPath, string searchPattern)
    {
        return new List<string>(Directory.GetFiles(directoryPath, searchPattern));
    }
}
