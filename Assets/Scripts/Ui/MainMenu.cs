using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _textField;

    IEnumerator Start()
    {
        WWW www = new WWW("https://otte.cz//ld40/index.php");
        yield return www;

        Text t = _textField.GetComponent<Text>();
        string data = www.text;
        string[] tmp = data.Split('|');
        t.text = "Top Players\n";
        int k = 0;
        for (int i = 0; i < tmp.Length - 1; i += 2)
        {
            string name = tmp[i + 0];

            if (name.Length > 12)
            {
                name = name.Substring(0, 12);
            }

            t.text += (i / 2 + 1).ToString() + " - " + name + " - " + tmp[i + 1] + "s\n";

            k++;
            if (k == 5)
            {
                break;
            }
        }
    }

    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void QuitToWindows()
    {
        Application.Quit();
    }
}
