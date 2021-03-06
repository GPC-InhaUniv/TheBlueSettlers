﻿using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using ProjectB.GameManager;

public class AccountInfo : MonoBehaviour
{
    static AccountInfo instance;

    public string Id;
    public string Password;


    [SerializeField]
    GetPlayerCombinedInfoResultPayload info;

    public GetPlayerCombinedInfoResultPayload Info
    {
        get { return info; }
        set { info = value; }
    }


    public static AccountInfo Instance
    {
        get { return instance; }
        set { instance = value; }
    }

    void Awake()
    {
        if (instance == null)
        {
            if (instance != this)
                instance = this;


            DontDestroyOnLoad(gameObject);
        }
    }

    public static void Register(string username, string password)
    {
        string email = username + "@temp.com";

        RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            Email = email,
            Username = username,
            Password = password,

        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegister, GameErrorManager.OnAPIError);


    }

    static void OnRegister(RegisterPlayFabUserResult result)
    {
        Debug.Log("Registered with: " + result.PlayFabId);
        GameDataManager.Instance.SetGameDataToServer();
        Debug.Log("회원가입 완료");

    }

    public static void Login(string username, string password)
    {
        LoginWithPlayFabRequest request = new LoginWithPlayFabRequest
        {
            TitleId = PlayFabSettings.TitleId,
            Username = username,
            Password = password,
         
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLogin, GameErrorManager.OnAPIError);
       
    }

   
    static void OnLogin(LoginResult result)
    {
        Debug.Log("Login with: " + result.PlayFabId);
        GetAccountInfo(result.PlayFabId);
       

    }

    public static void GetAccountInfo(string playfabId)
    {
        GetPlayerCombinedInfoRequestParams paramInfo = new GetPlayerCombinedInfoRequestParams()
        {
            GetTitleData = true,
            GetUserInventory = true,
            GetUserAccountInfo = true,
            GetUserVirtualCurrency = true,
            GetPlayerProfile = true,
            GetPlayerStatistics = true,
            GetUserData = true,
            GetUserReadOnlyData = true,

        };

        GetPlayerCombinedInfoRequest request = new GetPlayerCombinedInfoRequest()
        {
            PlayFabId = playfabId,
            InfoRequestParameters = paramInfo
        };

        PlayFabClientAPI.GetPlayerCombinedInfo(request, OnGotAccountInfo, GameErrorManager.OnAPIError);
    }

    static void OnGotAccountInfo(GetPlayerCombinedInfoResult result)
    {
        Debug.Log("Update Account Infomation!");
        Instance.info = result.InfoResultPayload;
        GameDataManager.Instance.GetGameDataFromServer();      
    }


    void SetUpAccount()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        UpdateUserDataRequest request = new UpdateUserDataRequest()
        {
            Data = data,

        };
        PlayFabClientAPI.UpdateUserData(request, UpdateDataInfo, GameErrorManager.OnAPIError);

    }


    void UpdateDataInfo(UpdateUserDataResult result)
    {
        Debug.Log("UpdateDataInfo");
      
    }

    public void UpdateLoginInfo()
    {
        
    }







}


public static class GameErrorManager
{

    public static void OnAPIError(PlayFabError error)
    {
        Debug.LogError(error.GetType());
        Debug.LogError(error.ErrorMessage.GetType());
      
    }

}




