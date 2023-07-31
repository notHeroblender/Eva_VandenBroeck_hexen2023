using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void StartButton()
    {
        Loader.Load(Loader.Scene.HexenGame);
    }

    public void MenuButton()
    {
        Loader.Load(Loader.Scene.Menu);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
