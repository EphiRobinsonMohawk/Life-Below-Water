using TMPro;
using UnityEngine;

public class CheckbookEntry : MonoBehaviour
{
    public string reason;
    public string fundChange;
    public TextMeshProUGUI fundText;
    public TextMeshProUGUI reasonText;


    public void Awake()
    {
        
    }

    public void SetText(int _fundChange, string _reason)
    {
        if(_fundChange < 0) fundText.color = Color.red;
        else if (_fundChange > 0) fundText.color = Color.green;
        else fundText.color = Color.grey;

        fundText.text = _fundChange.ToString();
        reasonText.text = _reason;
    }
}
