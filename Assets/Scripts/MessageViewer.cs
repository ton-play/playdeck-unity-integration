using UnityEngine;
using UnityEngine.UI;

public class MessageViewer : MonoBehaviour
{
    [SerializeField] private Text _text;
    
    public void Show(string text)
    {
        _text.text = text;
        
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}