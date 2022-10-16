using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CustomColors : ScriptableObject
{
    public Color front = Color.red;
    public Color back = Color.cyan;
    public Color left = new Color(60, 141, 13);
    public Color right = new Color(236, 182, 55);
    public Color top = new Color(130, 49, 165);
    public Color down = new Color(153, 255, 255);
}
