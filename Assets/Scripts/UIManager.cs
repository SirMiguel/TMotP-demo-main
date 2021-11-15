using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField]
    private Image formSprite;
    [SerializeField]
    private Sprite[] formList;
    [SerializeField]
    private Image stepSprite;
    [SerializeField]
    private Sprite[] stepList;

    // Start is called before the first frame update
    void Start()
    {
        formSprite.sprite = formList[0];
        stepSprite.sprite = stepList[0];
    }

    public void setForm(int count)
    {
        formSprite.sprite = formList[count];
    }

    public void setStep(int count)
    {
        if (count == -1)
        {
            stepSprite.enabled = false;
        }
        else
        {
            stepSprite.enabled = true;
            stepSprite.sprite = stepList[count];
        }
    }
}
