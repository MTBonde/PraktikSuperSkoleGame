using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneChange : MonoBehaviour
{
    public void SwitchToMonsterT�rn(string gamemode)
    {
        SceneManager.LoadScene("MonterTower katapult");
        //use gamemode to change which gamemode is used
    }
    public void SwitchToGrov�der(string gamemode)
    {
        SceneManager.LoadScene("Grov�der");
        //use gamemode to change which gamemode is used
    }

}
