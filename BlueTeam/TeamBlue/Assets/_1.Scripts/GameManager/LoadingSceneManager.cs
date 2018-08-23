﻿using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ProjectB.GameManager
{
    public enum LoadType
    {
        Village,
        BrickDungeon,
        WoodDungeon,
        SheepDungeon,
        IronDungeon,
        VillageCheckDownLoad
    }

    enum SceneName
    {
        Login,
        Loading,
        Empty,
    }
    public class LoadingSceneManager : MonoBehaviour
    {
      
        public static string NextScene;
        string assetBundleDirectory;
        string currentAssetName = "";
        const string ironDungeonBundleURL = "https://docs.google.com/uc?export=download&id=1OIrwmK_rDL-pq1GiIJtBtZW7grqKdWOf";
        const string commonbundleURL = "https://docs.google.com/uc?export=download&id=1KJkJRMUZDUzw5Htn2cskA-vJBZWOuqCr";
        const string brickDungeonBundleURL = "https://docs.google.com/uc?export=download&id=1Xu6YYEwLsXaFn781_w9S8FalDCvILEvr";
        const string townBundleURL = "https://docs.google.com/uc?export=download&id=1XG4i1tzvIsjMNPByrAC6nniFoASn12tZ"; //Town URL;
        const string dungeonUIURL = "https://docs.google.com/uc?export=download&id=1QPRGJgieB2GdZkU3GU8BRBIO_tbcWI0J"; //dungeonUIURL;
        const string playerBundleURL = "https://docs.google.com/uc?export=download&id=16HyAztYxMCHG-ERUwGVal4hjGwJYB3gX";  //PLAYER URL;
        const string woodDungeonBundleURL = "https://docs.google.com/uc?export=download&id=1HVXmTDcsFJqZY68VQWf8udJkz4Y_WVD3";  //woodDungeon URLL;
        const string sheepDungeonBundleURL = "https://docs.google.com/uc?export=download&id=1iPKkHSOYZaZBpyAAZefRxQ63Q0qEhDiM"; //sheepdungeon URL;
        

        int totalBundleCount = 8;
        static int userBundleCount = 0;
        static LoadType currentType;

        bool IsDownLoadDone = false;
        int percentage;

        [SerializeField]
        Image progressBar;
        [SerializeField]
        Text progressText;
        [SerializeField]
        Text currentBundleText;

        public delegate void LoadInGameScene();

        public LoadInGameScene LoadInGameSceneDelegater;

        // Use this for initialization
        void Awake()
        {
            if (currentType.Equals(LoadType.VillageCheckDownLoad) && userBundleCount < totalBundleCount - 1)
            {
                IsDownLoadDone = false;
                currentAssetName = "게임 준비중...";
                DownLoadAndSaveBundles();
            }
            else
            {
                totalBundleCount = 1;
                IsDownLoadDone = true;

            }

            StartCoroutine(LoadBundles());

            StartCoroutine(LoadScene());

        }


        void DownLoadAndSaveBundles()
        {
            StartCoroutine(SaveAssetBundleOnDisk(playerBundleURL, "playerbundle"));
            StartCoroutine(SaveAssetBundleOnDisk(woodDungeonBundleURL, "wooddungeonbundle"));
            StartCoroutine(SaveAssetBundleOnDisk(sheepDungeonBundleURL, "sheepdungeonbundle"));
            StartCoroutine(SaveAssetBundleOnDisk(townBundleURL, "townbundle"));
            StartCoroutine(SaveAssetBundleOnDisk(commonbundleURL, "commonbundle"));
            StartCoroutine(SaveAssetBundleOnDisk(brickDungeonBundleURL, "brickdungeonbundle"));
            StartCoroutine(SaveAssetBundleOnDisk(ironDungeonBundleURL, "irondungeonbundle"));
        }


        IEnumerator LoadBundles()
        {
            bool IsAssetLoadDone = false;

            while (!IsAssetLoadDone)
            {
                if (IsDownLoadDone)
                {

                    GameObjectsManager.Instance.ClearPool();
                    GameObjectsManager.Instance.DestroyObject();

                    switch (currentType)
                    {

                        case LoadType.Village:
                            break;
                        case LoadType.BrickDungeon:
                            currentAssetName = "흙 던전 로드중..";

                            Test_AssetBundleManager.Instance.LoadArea(AreaType.BrickDungeon);
                            GameObjectsManager.Instance.SetAreaPrefab(GameControllManager.Instance.CurrentIndex);
                            GameObjectsManager.Instance.SetObject(ObjectType.Area);
                            //       Test_PoolManager.Instance.SetArea(Test_AssetBundleManager.Instance.LoadObject(BundleType.Area, "Stage1"));
                            //      Test_PoolManager.Instance.SetPanel(Test_AssetBundleManager.Instance.LoadObject(BundleType.Common, "PlayerControlPanel"));
                            break;
                        case LoadType.WoodDungeon:
                            currentAssetName = "나무 던전 로드중..";

                            Test_AssetBundleManager.Instance.LoadArea(AreaType.WoodDungeon);
                            GameObjectsManager.Instance.SetAreaPrefab(GameControllManager.Instance.CurrentIndex);
                            GameObjectsManager.Instance.SetObject(ObjectType.Area);
                            //    Test_PoolManager.Instance.SetArea(Test_AssetBundleManager.Instance.LoadObject(BundleType.Area, "Stage1"));
                            //   Test_PoolManager.Instance.SetPanel(Test_AssetBundleManager.Instance.LoadObject(BundleType.Common, "PlayerControlPanel"));
                            //   Test_PoolManager.Instance.SetMonster(Test_AssetBundleManager.Instance.LoadObject(BundleType.Monster, "TestMonster"));
                            break;
                        case LoadType.SheepDungeon:
                            currentAssetName = "양 던전 로드중..";

                            Test_AssetBundleManager.Instance.LoadArea(AreaType.SheepDungeon);
                            GameObjectsManager.Instance.SetAreaPrefab(1);
                            GameObjectsManager.Instance.SetObject(ObjectType.Area);
                            //  Test_PoolManager.Instance.SetArea(Test_AssetBundleManager.Instance.LoadObject(BundleType.Area, "Stage1"));
                            //  Test_PoolManager.Instance.SetPanel(Test_AssetBundleManager.Instance.LoadObject(BundleType.Common, "PlayerControlPanel"));
                            //  Test_PoolManager.Instance.SetMonster(Test_AssetBundleManager.Instance.LoadObject(BundleType.Monster, "TestMonster"));
                            break;
                        case LoadType.IronDungeon:
                            Test_AssetBundleManager.Instance.LoadArea(AreaType.IronDungeon);
                            GameObjectsManager.Instance.SetAreaPrefab(GameControllManager.Instance.CurrentIndex);
                            GameObjectsManager.Instance.SetObject(ObjectType.Area);
                            break;
                        case LoadType.VillageCheckDownLoad:
                            currentAssetName = "마을 로드중...";
                            Test_AssetBundleManager.Instance.LoadArea(AreaType.Village);
                            GameObjectsManager.Instance.SetAreaPrefab(GameControllManager.Instance.CurrentIndex);
                            GameObjectsManager.Instance.SetObject(ObjectType.Area);
                            GameObjectsManager.Instance.SetPrefab();
                            GameObjectsManager.Instance.SetObject(ObjectType.Player);
                            //  Test_PoolManager.Instance.SetPlayer(Test_AssetBundleManager.Instance.LoadObject(BundleType.Player, "PlayerCharacter"));
                            //  Test_PoolManager.Instance.SetArea(Test_AssetBundleManager.Instance.LoadObject(BundleType.Area, "Village"));
                            break;
                    }
                    GameObjectsManager.Instance.SetMonsterPrefab();
                    GameObjectsManager.Instance.SetObject(ObjectType.Canvas);
                    GameObjectsManager.Instance.SetPool();
                    IsAssetLoadDone = true;
                }

                else
                {
                    yield return new WaitForSeconds(0.3f);
                }

             

                
            }
            yield return null;
        }

        IEnumerator LoadScene()
        {
            yield return null;

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(NextScene);
            asyncOperation.allowSceneActivation = false;

            float timer = 0.0f;
            while (!asyncOperation.isDone)  //종료되기 전까지 while문 실행
            {
                yield return null;

                timer += Time.deltaTime;

                if (asyncOperation.progress >= 0.9f)
                {
                    currentBundleText.text = currentAssetName;
                    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1.0f / totalBundleCount, timer);
                    progressBar.color = Color.gray;
                    percentage = Convert.ToInt32(progressBar.fillAmount * 100);
                    progressText.text = percentage.ToString() + "%";

                    if (progressBar.fillAmount >= 0.94f) //가득 찼다면
                    {
                        currentAssetName = "완료!";
                        progressText.text = "100%";

                        if (Input.anyKeyDown)
                        {
                            asyncOperation.allowSceneActivation = true;

                        }

                    }

                    else
                    {
                        progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, asyncOperation.progress / totalBundleCount, timer);

                        if (progressBar.fillAmount > asyncOperation.progress)
                        {
                            timer = 0.0f;
                        }
                    }
                }
            }
        }

        public static void LoadScene(LoadType mapType, int index)
        {
            switch (mapType)
            {
                case LoadType.Village:
                    currentType = LoadType.Village;
                    break;
                case LoadType.BrickDungeon:
                    currentType = LoadType.BrickDungeon;
                    break;
                case LoadType.WoodDungeon:
                    currentType = LoadType.WoodDungeon;
                    break;
                case LoadType.SheepDungeon:
                    currentType = LoadType.SheepDungeon;
                    break;
                case LoadType.IronDungeon:
                    currentType = LoadType.IronDungeon;
                    break;
                case LoadType.VillageCheckDownLoad:
                    currentType = LoadType.VillageCheckDownLoad;
                    userBundleCount = CheckDownLoadFile();
                    break;
                default:
                    break;
            }

            NextScene = SceneName.Empty.ToString();
            SceneManager.LoadScene(SceneName.Loading.ToString());


        }




        static int CheckDownLoadFile()
        {
            try
            {
                // Only get files that begin with the letter "c."
                string[] dirs = Directory.GetFiles(Application.persistentDataPath + "/AssetBundles", "*_unity3D");
                Debug.Log("현재 파일 개수:" + dirs.Length);
                return dirs.Length;
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());

            }

            return -1;

        }




        IEnumerator SaveAssetBundleOnDisk(string URL, string AssetName)
        {
            string uri = URL;

            // 웹 서버에 요청을 생성한다.
            UnityWebRequest request = UnityWebRequest.Get(uri);

            yield return request.Send();

            // 에셋 번들을 저장할 경로

            assetBundleDirectory = Application.persistentDataPath + "/AssetBundles";

            // 에셋 번들을 저장할 경로의 폴더가 존재하지 않는다면 생성시킨다.
            if (!Directory.Exists(assetBundleDirectory))
            {
                Directory.CreateDirectory(assetBundleDirectory);
            }

            // 파일 입출력을 통해 받아온 에셋을 저장하는 과정
            FileStream fs = new FileStream(assetBundleDirectory + "/" + AssetName + "_unity3D", System.IO.FileMode.Create, FileAccess.Write);

            fs.Write(request.downloadHandler.data, 0, (int)request.downloadedBytes);

            fs.Close();



            if (totalBundleCount > 1)
            {
                totalBundleCount--;

                if (totalBundleCount == 1)
                {
                    IsDownLoadDone = true;
                    Debug.Log("모든 파일 다운로드 완료");
                }
            }

            currentAssetName = AssetName + " 다운로드 중...";

            Debug.Log(AssetName + " 번들 다운로드 완료 " + totalBundleCount);
        }

    }
}