﻿using UnityEngine;
using ProjectB.GameManager;

namespace ProjectB.Characters.Monsters
{
    public class Boss : Monster
    {
        BossState bossState;
        float stateHandleNum;

        void OnEnable()
        {
            NoSkill.SetState += ChangeStateToWalking;
        }
        void Start()
        {        
            monsterType = MonsterType.Boss;
            SetMonsterInfo();
            bossState = new PhaseOne(animator);
            //TEST//
            //bossState = new PhaseTwo(animator);
            //bossState = new PhaseThree(animator);
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
                    animator.SetTrigger(AniStateParm.Defence.ToString());
                else if (defencePossibility == 2)
                {
                    animator.SetTrigger(AniStateParm.Defence.ToString());
                    bossState.UseDefenceSkill(gameObject.transform);
                }
                else
                {
                    StartCoroutine(ShowHitEffect(1.0f));
                    healthPoint -= damage;
                    SoundManager.Instance.SetSound(SoundFXType.EnemyHit);

                    if (healthPoint <= maxHealthPoint * (2.0f / 3.0f) && stateHandleNum == 0)
                    {
                        bossState = new PhaseTwo(animator);
                        stateHandleNum++;
                    }
                    else if (healthPoint <= maxHealthPoint * (2.0f / 3.0f) && stateHandleNum == 1)
                    {
                        bossState = new PhaseThree(animator);
                        stateHandleNum++;

                    }
                    else if (healthPoint <= 0)
                    {
                        healthPoint = 0;
                        ChangeState(State.Died);
                    }
                    else
                        animator.SetTrigger(AniStateParm.Hitted.ToString());
                }
                StartCoroutine(AvoidAttack());
            }
        }
        void OnDisable()
        {
            NoSkill.SetState -= ChangeStateToWalking;
        }
    }
}
