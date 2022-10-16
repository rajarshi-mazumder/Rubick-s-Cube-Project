using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CreateColor : MonoBehaviour
{
    #region Canvas Elements
    [SerializeField]
    RectTransform colorPaletteTexture;

    [SerializeField]
    Texture2D refSprite;

    [SerializeField]
    GameObject SetColorBtn;

    public Image front;
    public Image back;
    public Image top;
    public Image down;
    public Image left;
    public Image right;
    #endregion

    public GameObject cubeParent;
    public CustomColors colorSet;

    
    int index;
    int noOfColorsSelected = 0;
    Color selectedColor;
    
    void Start()
    {
        cubeParent = GameObject.FindGameObjectWithTag("CubeParent");
        noOfColorsSelected = 0;        
    }
    private void Update()
    {
        if (noOfColorsSelected < 6) // if 6 colors have not been selected, keep the setColor button disabled
        {
            SetColorBtn.GetComponent<Button>().interactable = false;
        }
        else SetColorBtn.GetComponent<Button>().interactable = true;
    }
    public void PickColor()
    {
        Vector2 delta;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(colorPaletteTexture, Input.mousePosition, null, out delta);
        float width = colorPaletteTexture.rect.width;
        float height = colorPaletteTexture.rect.height;
        delta += new Vector2(width * 0.5f, height * 0.5f);

        float x = Mathf.Clamp(delta.x / width, 0f, 1f);
        float y = Mathf.Clamp(delta.y / height, 0f, 1f);

        int texX = Mathf.RoundToInt(x * refSprite.width);
        int texY = Mathf.RoundToInt(y * refSprite.height);
        selectedColor = refSprite.GetPixel(texX, texY);
        
    }
    public void SaveColor()
    {   
       // for each selected color, set the color of the corresponding CustomColors object, and the color of the preview image
        if(index==0)
        {
            colorSet.front = selectedColor;
            front.color = selectedColor;
            index++;
            noOfColorsSelected++;
        }
        else if (index == 1)
        {
            colorSet.back = selectedColor;
            back.color = selectedColor;
            index++;
            noOfColorsSelected++;
        }
        else if (index == 2)
        {
            colorSet.left = selectedColor;
            left.color = selectedColor;
            index++;
            noOfColorsSelected++;
        }
        else if (index == 3)
        {
            colorSet.right = selectedColor;
            right.color = selectedColor;
            index++;
            noOfColorsSelected++;
        }
        else if (index == 4)
        {
            colorSet.top = selectedColor;
            top.color = selectedColor;
            index++;
            noOfColorsSelected++;
        }
        else if (index == 5)
        {
            colorSet.down = selectedColor;
            down.color = selectedColor;
            noOfColorsSelected++;
            index =0;
        }

    }

    public void SetColor()
    {
        cubeParent.GetComponent<CubeManager>().SetCubeColor(colorSet);
    }
}
