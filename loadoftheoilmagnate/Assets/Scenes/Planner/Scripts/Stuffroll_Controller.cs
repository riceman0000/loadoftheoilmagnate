using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stuffroll_Controller : MonoBehaviour
{
    [SerializeField]
    private GameObject StuffText;
    [SerializeField]
    private GameObject FadePanel;
    // Start is called before the first frame update
    void Start()
    {
        FadePanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (StuffText.transform.position.y > 3000)
        {
            FadePanel.gameObject.SetActive(true);
        }
    }
}
