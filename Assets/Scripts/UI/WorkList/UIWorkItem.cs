using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

public enum WorkType
{
    Image,
    Audio,
    Model
}

public class UIWorkItem : MonoBehaviour
{
    [SerializeField] private Text idWork;
    [SerializeField] private Text titleWork;

    [SerializeField] private Image iconPreview;
    [SerializeField] private Image bgImage;
    [SerializeField] private Image moderationIcon;
    
    [SerializeField] private Sprite[] icons = new Sprite[3];
    
    [HideInInspector] public WorkItemStack currentItem;
    
    public void SetItem(WorkItemStack itemStack)
    {
        currentItem = itemStack;

        idWork.text = itemStack.Item.WorkID.ToString();
        titleWork.text = itemStack.Item.WorkTitle;

        switch (itemStack.Item.WorkType.ToLower())
        {
            case "image":
                iconPreview.sprite = icons[0];
                break;
            case "music":
                iconPreview.sprite = icons[1];
                break;
            case "model":
                iconPreview.sprite = icons[2];
                break;
        }

        if (itemStack.Item.IsModerated)
        {
            moderationIcon.gameObject.SetActive(true);
        }
        else
        {
            moderationIcon.gameObject.SetActive(false);
        }
    }
    
    public void SetInactiveItem()
    {
        currentItem = null;
        idWork.gameObject.SetActive(false);
        titleWork.gameObject.SetActive(false);
        bgImage.gameObject.SetActive(false);
        iconPreview.gameObject.SetActive(false);
        moderationIcon.gameObject.SetActive(false);
    }
}