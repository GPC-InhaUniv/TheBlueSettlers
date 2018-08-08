﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ProjectB.Quest;

public class VillageUIPresenter : MonoBehaviour
{
    [SerializeField] private Text aVillageQuestContentsText;
    [SerializeField] private Text bVillageQuestContentsText;
    [SerializeField] private Text aQuestAcceptanceButtonText;
    [SerializeField] private Text bQuestAcceptanceButtonText;
    [SerializeField] private GameObject questViewPanel;
    [SerializeField] private GameObject WorldMapPanel;
    [SerializeField] private Button questExitButton;

    private IQuestViable aVillageQuest;
    private IQuestViable bVillageQuest;

    private void Awake()
    {
        aVillageQuest = new AVillageHuntingQuest();
        bVillageQuest = new BVillageHuntingQuest();
        aVillageQuestContentsText.text = aVillageQuest.ShowContentsOfQuest(QuestType.AVillageQuest, "완료");
        bVillageQuestContentsText.text = bVillageQuest.ShowContentsOfQuest(QuestType.BVillageQuest, "완료");
    }

    public void OnClickAcceptButton(int villageType)
    {
        aVillageQuestContentsText.text = aVillageQuest.ShowContentsOfQuest((QuestType)villageType, aQuestAcceptanceButtonText.text);
        bVillageQuestContentsText.text = bVillageQuest.ShowContentsOfQuest((QuestType)villageType, aQuestAcceptanceButtonText.text);

        aQuestAcceptanceButtonText.text = aVillageQuest.AcceptToQuest((QuestType)villageType);
        bQuestAcceptanceButtonText.text = bVillageQuest.AcceptToQuest((QuestType)villageType);
    }

    public void OnClickKillAMonster(int monsterType)
    {
        aQuestAcceptanceButtonText.text = aVillageQuest.ProceedToQuest((ConditionType)monsterType);
        bQuestAcceptanceButtonText.text = bVillageQuest.ProceedToQuest((ConditionType)monsterType);
    }

    public void OnClickQuestButton()
    {
        questViewPanel.SetActive(true);
    }

    public void OnClickQuestExitButton()
    {
        questViewPanel.SetActive(false);
    }

    public void OnClickEntranceDungeonButton()
    {
        WorldMapPanel.SetActive(true);
    }

    public void OnClickWoodDungeonButton(int dungeonNumber)
    {
        LoadingSceneManager.LoadScene(LoadType.WoodDungeon, dungeonNumber);
        Debug.Log("나무 던전 입장");
    }

    public void OnClickIronDungeonButton(int dungeonNumber)
    {
        LoadingSceneManager.LoadScene(LoadType.IronDungeon, dungeonNumber);
        Debug.Log("철광석 던전 입장");
    }

    public void OnClickBrickDungeonButton(int dungeonNumber)
    {
        LoadingSceneManager.LoadScene(LoadType.BrickDungeon, dungeonNumber);
        Debug.Log("돌 던전 입장");
    }

    public void OnClickSheepDungeonButton(int dungeonNumber)
    {
        LoadingSceneManager.LoadScene(LoadType.SheepDungeon, dungeonNumber);
        Debug.Log("양 던전 입장");
    }

    public void OnClickVillageButton(int dungeonNumber)
    {
        LoadingSceneManager.LoadScene(LoadType.Village, dungeonNumber);
        Debug.Log("마을 입장");
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            aQuestAcceptanceButtonText.text = aVillageQuest.ProceedToQuest(ConditionType.WoodMonster);
        }

        else if (Input.GetKey(KeyCode.S))
        {
            aQuestAcceptanceButtonText.text = aVillageQuest.ProceedToQuest(ConditionType.SheepMonster);
        }

        else if (Input.GetKey(KeyCode.Q))
        {
            bQuestAcceptanceButtonText.text = bVillageQuest.ProceedToQuest(ConditionType.BrickMonster);
        }

        else if (Input.GetKey(KeyCode.W))
        {
            bQuestAcceptanceButtonText.text = bVillageQuest.ProceedToQuest(ConditionType.IronMonster);
        }
    }
}
