
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static int index = -1;
    public static bool select = false;
  public void GetIndis()
    {
        index = this.transform.GetSiblingIndex();
        
        select = true;
    }
    
    public void DeSelect()
    {
        select = false;
    }
}
