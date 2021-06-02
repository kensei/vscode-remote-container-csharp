using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TodoController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_scrollContent;
    [SerializeField]
    private RectTransform m_scrollElement;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            var listElement = Instantiate(m_scrollElement, Vector3.zero, Quaternion.identity);
            listElement.SetParent(m_scrollContent.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickAddButton()
    {
        Debug.Log("OnClickAddButton");
    }
}
