using UnityEngine;


public class LinkOpener : MonoBehaviour
{
    public void OpenLink(string p_linkURL)
    {
        Application.OpenURL(p_linkURL);
    }
}