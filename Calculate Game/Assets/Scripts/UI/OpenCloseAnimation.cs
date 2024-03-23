using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseAnimation : MonoBehaviour
{
    public Transform targetTransform; 
    public Vector3 targetScale = new Vector3(1, 1, 1); 

    public void OpenGameObject()
    {
        gameObject.SetActive(true);
        targetTransform.localScale = Vector3.zero;
        targetTransform.DOScale(targetScale, 0.2f);
    }


    public void CloseGameObject()
    {
        targetTransform.DOScale(Vector3.zero, 0.2f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
