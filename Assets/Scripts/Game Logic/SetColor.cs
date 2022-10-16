using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetColor : MonoBehaviour
{
    
    public void SetFaceColor(CustomColors colorSet)
    {
        if (this.gameObject.name.StartsWith("F"))
            Utiltities.SetMaterialColor(this.GetComponent<MeshRenderer>(), colorSet.front);
        else if (this.gameObject.name.StartsWith("B"))
            Utiltities.SetMaterialColor(this.GetComponent<MeshRenderer>(), colorSet.back);
        else if (this.gameObject.name.StartsWith("T"))
            Utiltities.SetMaterialColor(this.GetComponent<MeshRenderer>(), colorSet.top);
        else if (this.gameObject.name.StartsWith("D"))
            Utiltities.SetMaterialColor(this.GetComponent<MeshRenderer>(), colorSet.down);
        else if (this.gameObject.name.StartsWith("L"))
            Utiltities.SetMaterialColor(this.GetComponent<MeshRenderer>(), colorSet.left);
        else if (this.gameObject.name.StartsWith("R"))
            Utiltities.SetMaterialColor(this.GetComponent<MeshRenderer>(), colorSet.right);
    }
    
}
