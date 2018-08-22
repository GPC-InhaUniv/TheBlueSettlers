﻿using System.Collections;
using UnityEngine;
using ProjectB.GameManager;
using ProjectB.UI.Presenter;

namespace ProjectB.Characters.Players
{
    public enum PlayerCharacterWeaponState
    {
        ShortSword,
        LongSword,
    }
    public enum PlayerStates
    {
        PlayerCharacterIdleState,
        PlayerCharacterAttackState,
        PlayerCharacterSkillState,
        PlayerCharacterRunState,
        PlayerCharacterBackStepState,
        PlayerCharacterDieState
    }

    public class Player : Character , IInitializable
    {
        public PlayerPresenter PlayerPresenter { get { return playerPresenter; } }
        PlayerPresenter playerPresenter;

        public PlayerAnimation PlayerAinmaton { get { return playerAinmaton; } }
        PlayerAnimation playerAinmaton;

        public Rigidbody PlayerRigidbody { get { return playerRigidbody; } }
        Rigidbody playerRigidbody;

        public Collider Collider { get { return collider; } }
        Collider collider;
     
        public Vector3 TargetVector { get { return targetVector; } }
        Vector3 targetVector;

        public int AttackNumber { get { return attackNumber; } private set { } }
        int attackNumber;

        public float PlayerMaxExp { get { return playerMaxExp; } private set { } }
        float playerMaxExp;

        public Vector3 MoveVector;

        public bool IsRunning;
  
        public PlayerCharacterWeaponState CurrentWeaponState;

        public PlayerCharacterState PlayerState;

        float preAttckPower;

        bool isDied;
        bool isRunningHitCoroutine = false;
        Weapon weapon;

        [SerializeField]
        GameObject hitParticle;

        int equipmentHp = 0;
        int equipmentAttackPower = 0;
        int equipmentDefensePowr = 0;

        bool isHit;

        private void Awake()
        {     
            playerRigidbody = GetComponent<Rigidbody>();
            weapon = GetComponent<Weapon>();
            playerAinmaton = GetComponent<PlayerAnimation>();
            collider = GetComponent<CapsuleCollider>();

            ChangeState(PlayerStates.PlayerCharacterIdleState);
            CurrentWeaponState = PlayerCharacterWeaponState.ShortSword;

   
        }

        void Start()
        {
            hitParticle.SetActive(false);

            targetVector = new Vector3(0, 0, 1);
            
            playerAinmaton.InitWeapon();
        }

        public void Initialize()
        {
            playerPresenter = GameObject.FindGameObjectWithTag("PlayerPresenter").GetComponent<PlayerPresenter>();
           
            GetCharacterStatusFromDataManager();
            SetCharacterStatus();
            playerPresenter.UpdateUI();
        }

        private void Update()
        {
            if (MoveVector != Vector3.zero)
            {
                targetVector = MoveVector;
            }
            playerAinmaton.RunAnimation(IsRunning);
        }

        public void ReturnIdleState() //다시 살리는 함수
        {
            isDied = false;
            ChangeState(PlayerStates.PlayerCharacterIdleState);
            playerAinmaton.InitStateAnimation();
            playerAinmaton.InitWeapon();
            SetCharacterStatus();
            playerPresenter.UpdateUI();
        }

        public void SetState(PlayerCharacterState state)
        {
            if(isDied == true)
            {
                return;
            }
            if (PlayerState == state)
            {
                return;
            }
            else
            {
                PlayerState = state;
                PlayerState.Tick(MoveVector);
            }
        }

        public void ChangeState(PlayerStates playerState)
        {
            switch (playerState)
            {
                case PlayerStates.PlayerCharacterIdleState:
                    SetAttackPower(1.0f);
                    SetState(new PlayerCharacterIdleState(PlayerAinmaton, PlayerRigidbody, transform, Collider, AttackNumber, TargetVector));
                    break;
                case PlayerStates.PlayerCharacterAttackState:
                    SetAttackPower(1.2f);
                    SetState(new PlayerCharacterAttackState(PlayerAinmaton, PlayerRigidbody, transform, Collider, AttackNumber, TargetVector));
                    break;
                case PlayerStates.PlayerCharacterSkillState:
                    SetAttackPower(2.0f);
                    SetState(new PlayerCharacterSkillState(PlayerAinmaton, PlayerRigidbody, transform, Collider, AttackNumber, TargetVector));
                    break;
                case PlayerStates.PlayerCharacterRunState:
                    SetState(new PlayerCharacterRunState(PlayerAinmaton, PlayerRigidbody, transform, Collider, AttackNumber, TargetVector));
                    break;
                case PlayerStates.PlayerCharacterBackStepState:
                    SetState(new PlayerCharacterBackStepState(PlayerAinmaton, PlayerRigidbody, transform, Collider, AttackNumber, TargetVector));
                    break;
                case PlayerStates.PlayerCharacterDieState:
                    SetState(new PlayerCharacterDieState(PlayerAinmaton, PlayerRigidbody, transform, Collider, AttackNumber, TargetVector));
                    break;
                default:
                    break;
            }
        }

        public void SetAttackNumber(int number)
        {
            attackNumber = number;
        }

        void SetAttackPower(float power) 
        {
            characterAttackPower = preAttckPower  * power;
        }

        public override void ReceiveDamage(float damage)
        {
            if(isDied == false && isRunningHitCoroutine == false)
            {
                StartCoroutine(HitCoroutine(1.2f));

                playerAinmaton.HitAnimation();
                characterHealthPoint -= CalDamage(damage);
                SoundManager.Instance.SetSoundType(SoundFXType.PlayerHit);

                playerPresenter.UpdateUI();
            }

            if (CharacterHealthPoint <= 0)
            {
                characterHealthPoint = 0;
                ChangeState(PlayerStates.PlayerCharacterDieState);
                isDied = true;                          
                
                GameControllManager.Instance.CheckGameOver();

                playerPresenter.UpdateUI();
            }
        }

        float CalDamage(float damage)
        {
            if (0 < damage - characterDefensivePower * 0.1f)
            {
                return damage - characterDefensivePower * 0.1f;
            }
            else return 0;
        }

        public void WeaponSwitching(PlayerCharacterWeaponState NewWeaponState)
        {
            if (CurrentWeaponState == NewWeaponState)
            {
                return;
            }
            else if (IsRunning == false)
            {
                playerAinmaton.WeaponSwapAnimation(NewWeaponState);

                weapon.SetWeapon(true, NewWeaponState, CurrentWeaponState);

                CurrentWeaponState = NewWeaponState;
            }
        }

        //데이터 불러오기 및 정리하기
        void GetCharacterStatusFromDataManager()
        {
            GameDataManager.Instance.GetEquipmentStatus(ref equipmentHp,ref equipmentAttackPower,ref equipmentDefensePowr);
            characterExp = GameDataManager.Instance.PlayerInfomation.PlayerExp;
            characterLevel = GameDataManager.Instance.PlayerInfomation.PlayerLevel;
        }

        void SetCharacterStatus()
        {
            characterMaxHealthPoint = characterLevel * 100 + equipmentHp;
            characterHealthPoint = characterMaxHealthPoint;

            characterAttackPower = characterLevel * 10;
            characterDefensivePower += equipmentDefensePowr;

            playerMaxExp = (1000 * 1.2f * CharacterLevel);
            preAttckPower = characterAttackPower + equipmentAttackPower;
        }
        //데이터 불러오기 및 정리하기

        //애니메이션 이벤트
        public void AttackStart()
        {
            SoundManager.Instance.SetSoundType(SoundFXType.PlayerAttack);
        }
        public void AttackDone()
        {
            ChangeState(PlayerStates.PlayerCharacterIdleState);
        }

        public void BackStepStart()
        {
            //추후 확장 가능성
        }

        public void BackStepEnd()
        {
            collider.enabled = true;
            ChangeState(PlayerStates.PlayerCharacterIdleState);
        }
        //애니메이션 이벤트

        IEnumerator HitCoroutine(float time)
        {
            isRunningHitCoroutine = true;

            hitParticle.SetActive(true);

            yield return new WaitForSeconds(time);

            hitParticle.SetActive(false);

            isRunningHitCoroutine = false;
        }

      
    }


}

