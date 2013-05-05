using UnityEngine;


/// <summary>
/// </summary>
public class Nuevo_Juego : MonoBehaviour
{

    private bool loadingLevel = false;

    /// <summary>
    /// </summary>
    public string CfgLevelName;


    private void Update()
    {
        if (!loadingLevel && (Input.GetButtonDown("Fire1")))
        {
            loadingLevel = true;
            Application.LoadLevel(this.CfgLevelName);
        }
    }
}