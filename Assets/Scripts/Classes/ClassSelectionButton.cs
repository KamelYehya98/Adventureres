using UnityEngine;
using UnityEngine.UI;

public class ClassSelectionButton : MonoBehaviour
{
    public int classIndex;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        GameManager.Instance.SelectClass(classIndex);
    }
}