using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
using UnityEngine.SceneManagement;

public enum LogInType
{
    logIn,
    newName,
    PVP,
    Rank,   
    Null


}

public class Registaration
{
    private static Registaration instance = null;

    private bool newName;

    public static Registaration Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Registaration();
            }

            return instance;
        }
    }



    public void SignUp(string id, string pw, TextMeshProUGUI console)
    {
        // Step 2. È¸ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï±ï¿½ ï¿½ï¿½ï¿½ï¿½        

        Debug.Log("È¸ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ã»ï¿½Õ´Ï´ï¿½.");

        var responceOfBackEnd = Backend.BMember.CustomSignUp(id, pw);

        if (responceOfBackEnd.IsSuccess())
        {
            console.text = $"È¸ï¿½ï¿½ï¿½ï¿½ï¿½Ô¿ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ß½ï¿½ï¿½Ï´ï¿½. \nID : {id}ï¿½ï¿½";

            Nickname(id, console);
        }
        else
        {
            console.text = $"È¸ï¿½ï¿½ï¿½ï¿½ï¿½Ô¿ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ß½ï¿½ï¿½Ï´ï¿½. : {responceOfBackEnd}";

        }
    }

    public void Login(string id, string pw, TextMeshProUGUI console, LogInType type, string text)
    {
        // Step 3. ï¿½Î±ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï±ï¿½ ï¿½ï¿½ï¿½ï¿½
        Debug.Log("ï¿½Î±ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ã»ï¿½Õ´Ï´ï¿½.");

        var responceOfBackEnd = Backend.BMember.CustomLogin(id, pw);
        //Debug.Log(responceOfBackEnd);

        if (responceOfBackEnd.IsSuccess())
        {
            if (type == LogInType.logIn)
            {
                LoadingManager.name_Scene = "InGame";
                LoadingManager.loading = Loading.InGame;

                SceneManager.LoadScene(2);

            }
            else if (type == LogInType.newName)
            {

                Registaration.Instance.Nickname(text, console);
                Debug.Log(responceOfBackEnd);
            }
            else if (type == LogInType.PVP)
            {
                MainMenuManager.Instance.PVPSetting();
            }
            else if (type == LogInType.Rank)
            {
                MainMenuManager.Instance.RankSetting();
            }



        }
        else
        {
            console.text = $"ë¡œê·¸?¸ì´ ?¤íŒ¨?ˆìŠµ?ˆë‹¤. : {responceOfBackEnd}";

            console.text = $"·Î±×ÀÎÀÌ ½ÇÆĞÇß½À´Ï´Ù. : {responceOfBackEnd}";

        }
    }

    public void Nickname(string nickname, TextMeshProUGUI console)
    {
        // Step 4. ï¿½Ğ³ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï±ï¿½ ï¿½ï¿½ï¿½ï¿½

        Debug.Log("ï¿½Ğ³ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ã»ï¿½Õ´Ï´ï¿½.");

        var bro = Backend.BMember.UpdateNickname(nickname);

        if (bro.IsSuccess())
        {
            console.text = "?‰ë„¤??ë³€ê²??„ë£Œ!";
        }
        else
        {
            console.text = ("?‰ë„¤??ë³€ê²½ì— ?¤íŒ¨?ˆìŠµ?ˆë‹¤ : " + bro);

            if (bro.IsSuccess() && newName)
            {
                console.text = "´Ğ³×ÀÓ º¯°æ ¿Ï·á!";
            }
            else
            {
                console.text = ("´Ğ³×ÀÓ º¯°æ¿¡ ½ÇÆĞÇß½À´Ï´Ù : " + bro);

            }


        }
    }
}