using UnityEngine;

public class ClassSelectionButton : MonoBehaviour
{
    public int classIndex;

    public void OnButtonClick()
    {
        GameManager.Instance.SelectClass(classIndex);
    }
}