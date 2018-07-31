﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Execute();
}

public class CommandAttack1 : ICommand
{
    Player player = null;
    public CommandAttack1()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void Execute()
    {
        player.attackNum = 1;
        player.PlayerAttack();
    }
}
public class CommandAttack2 : ICommand
{
    Player player = null;
    public CommandAttack2()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void Execute()
    {
        player.attackNum = 2;
        player.PlayerAttack();
    }
}
public class CommandAttack3 : ICommand
{
    Player player = null;
    public CommandAttack3()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void Execute()
    {
        player.attackNum = 3;
        player.PlayerAttack();
    }
}
public class CommandAttack4 : ICommand
{
    Player player = null;
    
    public CommandAttack4()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    public void Execute()
    {
        player.attackNum = 4;
        player.PlayerAttack();
    }
}
//공격명령을 쌓는 동안은 배틀상태로가라
//배틀 상태일떄만 공격이 나가며
//걷거나 달릴때는 공격"못"함