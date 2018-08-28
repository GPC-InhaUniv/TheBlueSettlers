﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectB.Characters;
using ProjectB.GameManager;

namespace ProjectB.Characters.Monsters
{
    public class Boss : Monster
    {
        BossState bossState;
        ISkillUsableBridge defenceSkillUsable;
        ISkillUsableBridge entangleSkillUsable;
        float stateHandleNum;
    
        private void OnEnable()
        {
            NoSkill.SetState += ChangeStateToWalking;

        }
        private void OnDisable()
        {
            NoSkill.SetState -= ChangeStateToWalking;

        }
        void Start()
        {
            //Setstate

            monsterMove = GetComponent<MonsterMove>();
            animator = GetComponent<Animator>();
            startPosition = transform.position;
            waitBaseTime = 2.0f;
            waitTime = waitBaseTime;


            //bossState = new PhaseOne(animator, attackable, defenceSkillUsable, skillUsable);
            bossState = new PhaseTwo(animator, attackable, defenceSkillUsable, skillUsable);
            //bossState = new PhaseThree(animator, attackable, defenceSkillUsable, skillUsable, entangleSkillUsable);

            monsterType = MonsterType.Boss;

            walkRange = 30;
            skillCoolTime = 10;
            speed = 2;


            healthPoint = maxHealthPoint;
            hitParticle.SetActive(false);


        }
        void Update()
        {
            switch (state)
            {
                case State.Walking:
                    WalkAround();
                    break;
                case State.Chasing:
                    ChaseTarget();
                    break;
            }
            if (state != currentState)
            {
                state = currentState;
                switch (state)
                {
                    case State.Attacking:
                        AttackTarget();
                        break;
                    case State.Skilling:
                        UseSkill();
                        break;
                    case State.Died:
                        Died();
                        break;
                }
            }
            //Debug.Log(animator.GetInteger(AniStateParm.Attack.ToString()));

            if (Input.GetKeyDown(KeyCode.F))
            {
                Died();
            }
        }
        protected override void AttackTarget()
        {
            bossState.Attack();
            SoundManager.Instance.SetSound(SoundFXType.EnemyAttack);

        }
        protected override void UseSkill()
        {

            bossState.UseSkill(attackTarget);

        }
        public override void ReceiveDamage(float damage)
        {
            if (!isInvincibility)
            {
                int defencePossibility = Random.Range(1, 9);
                if (defencePossibility == 1)
                {
                    animator.SetTrigger(AniStateParm.Defence.ToString());
                }
                else if (defencePossibility == 2)
                {
                    animator.SetTrigger(AniStateParm.Defence.ToString());
                    bossState.UseDefenceSkill(gameObject.transform);
                }
                else
                {
                    animator.SetTrigger(AniStateParm.Hitted.ToString());
                    StartCoroutine(HitCoroutine(1.0f));

                    healthPoint -= damage;

                    SoundManager.Instance.SetSound(SoundFXType.EnemyHit);


                    if (healthPoint <= maxHealthPoint * (2 / 3) && stateHandleNum == 0)
                    {
                        bossState = new PhaseTwo(animator, attackable, defenceSkillUsable, skillUsable);

                        stateHandleNum++;
                    }
                    else if (healthPoint <= maxHealthPoint * (1 / 3) && stateHandleNum == 1)
                    {
                        bossState = new PhaseThree(animator, attackable, defenceSkillUsable, skillUsable, entangleSkillUsable);
                        stateHandleNum++;
                    }
                    else if (healthPoint <= 0)
                    {
                        healthPoint = 0;
                        ChangeState(State.Died);
                    }
                }

            }
        }
    }
}
