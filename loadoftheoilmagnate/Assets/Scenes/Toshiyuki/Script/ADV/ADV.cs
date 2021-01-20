using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ADV : MonoBehaviour
{
    [SerializeField] Text name1;
    [SerializeField] Text name2;
    [SerializeField] Text text1;
    [SerializeField] Text text2;
    [SerializeField] Image fukidashi1;
    [SerializeField] Image fukidashi2;
    [SerializeField] Sprite left_top;
    [SerializeField] Sprite left_under;
    [SerializeField] Sprite right_top;
    [SerializeField] Sprite right_under2;

    byte _talkTurn = 0;
    void Start()
    {
        StartCoroutine("DebugCoroutin");
    }

    void Fukidashi(Text name1,Text text1, Text name2, Text text2, Image L_Fukidashi,Image R_Fukidashi,Sprite L_Top,Sprite L_Under,Sprite R_Top,Sprite R_Under)
    {
        if (_talkTurn % 2 == 0)
        {
            name1.color = new Color(255f, 255f, 255f, 0f);
            text1.color = new Color(255f, 255f, 255f, 0f);
            L_Fukidashi.sprite = L_Top;
            L_Fukidashi.transform.SetAsLastSibling();
            name2.color = new Color(255f, 255f, 255f, 255f);
            text2.color = new Color(255f, 255f, 255f, 255f);
            R_Fukidashi.sprite = R_Under;
        }
        else
        {
            name2.color = new Color(255f, 255f, 255f, 0f);
            text2.color = new Color(255f, 255f, 255f, 0f);
            R_Fukidashi.sprite = R_Top;
            R_Fukidashi.transform.SetAsLastSibling();
            name1.color = new Color(255f, 255f, 255f , 255f);
            text1.color = new Color(255f, 255f, 255f, 255f);
            L_Fukidashi.sprite = L_Under;
        }
        _talkTurn += 1;
    }

    IEnumerator DebugCoroutin()
    {
        Fukidashi(name1, text1, name2, text2, fukidashi1, fukidashi2, left_top, left_under, right_top, right_under2);
        yield return new WaitForSeconds(2f);
        Fukidashi(name1, text1, name2,  text2, fukidashi1, fukidashi2, left_top, left_under, right_top, right_under2);
        yield return new WaitForSeconds(2f);
        Fukidashi(name1, text1, name2, text2, fukidashi1, fukidashi2, left_top, left_under, right_top, right_under2);
    }
}
