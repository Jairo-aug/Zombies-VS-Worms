using UnityEngine;

public class ShowBoxInfo : MonoBehaviour
{

    public GameObject boxInfo;

    void Start()
    {
        boxInfo.SetActive(false);
    }

    void OnMouseOver()
    {
        boxInfo.SetActive(true);
    }

    void OnMouseExit()
    {
        boxInfo.SetActive(false);
    }
}
