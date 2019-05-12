using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected int hp;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float damageTime;
    [SerializeField]
    protected float rangeToAttack;
    [SerializeField]
    protected float moveSpeed;
    [SerializeField]
    protected UnitType unitType;

    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected HPBar hpBar;

    protected UnitState unitState;
    protected int currentHp;
    protected int killingHp;
    protected Unit targetUnit;
    protected Vector3 target;

    protected AIDestinationSetter destinationSetter;
    [HideInInspector]
    public  AIPath aiPath;
    protected float endReachedDistance;

    public bool isChangingForm;
    bool takingOverStarted;
    bool killingStarted;
    RaycastHit2D hit2d;

    [Header("AUDIO")]
    [SerializeField]
    AudioClip footStepSound;
    [SerializeField]
    AudioClip attackSound;
    [SerializeField]
    AudioClip gettinHitSound;
    [SerializeField]
    AudioSource sfxSource;

    public UnitState UnitState
    {
        get { return unitState; }
        set { unitState = value; }
    }

    public UnitType UnitType
    {
        get { return unitType; }
        
    }

    protected virtual void Start()
    {
        if(sfxSource == null)
        {
            gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = GameManager.Instance.sfxAudioGroup;
        }
        InitHp();
        hpBar.SetupHPBar(currentHp,hp);
        unitState = UnitState.IDLE;
    }

    public void TakeDamage(int dmg, bool isKilling = false)
    {
        unitState = UnitState.BEING_ATTACKED;
        if (!isKilling)
        {
            currentHp -= dmg;
            if (hpBar != null)
                hpBar.SetFiller(currentHp, hp);
        }
        if(isKilling)
        {
            killingHp -= dmg;
            if (hpBar != null)
                hpBar.SetFiller(killingHp, hp);
        }
        if(currentHp <= 0)
        {
            currentHp = 0;
        }
        if(killingHp <= 0)
        {
            killingHp = 0;
        }
    }

    public void ResetCurrentHp(Unit unit)
    {
        unit.currentHp = unit.hp;
        if (unit.hpBar != null)
        {
            unit.hpBar.SetFiller(unit.currentHp, unit.hp);
        }
    }

    void InitHp()
    {
        currentHp = hp;
        killingHp = hp;
    }

    public void GiveDamage(Unit unit,bool killable = false)
    {
        unit.TakeDamage(damage, killable);
    }

    public void ChangeForm(UnitType attackerType)
    {
        isChangingForm = true;
        InterruptPathFollowing();
        int randomNumber = 0;
        if (attackerType.ToString().Contains("enemy"))
        {
            randomNumber = Random.Range(4, 6);
        }
        else
        {
            randomNumber = (int)attackerType;
        }
        Debug.Log("Change form! Called from: " + attackerType.ToString());
        Instantiate(GameManager.Instance.unitsPrefabs[(UnitType)randomNumber], transform.position+new Vector3(0,0,0.1f),
                        Quaternion.identity, GameManager.Instance.unitsParent);
        Die();
    }

    public virtual void Die()
    {
        GameManager.Instance.allUnits.Remove(this); 
        Destroy(gameObject);
    }

    public void BlowOver()
    {
        unitState = UnitState.KILLING;
        foreach(EnemyUnit e in GameManager.Instance.enemyUnits)
        {
            if(Vector3.Distance(transform.position,e.transform.position)<rangeToAttack)
            {
                StartCoroutine(Kill(e));
            }
        }
    }

    public IEnumerator TakeOver(Unit unit)
    {
        unit.InterruptPathFollowing();
        while (unit.currentHp > 0)
        {
            unitState = UnitState.TAKING_OVER;
            yield return new WaitForSeconds(damageTime);
            unit.aiPath.canMove = false;
            GiveDamage(unit);
            if (unitState != UnitState.TAKING_OVER)
            {
                ResetCurrentHp(unit);
                unit.unitState = UnitState.IDLE;
                unit.aiPath.canMove = true;
                break;
            }
        }
        if(unit.currentHp <=0)
        {
            unitState = UnitState.IDLE;
            if (unit != null && !unit.isChangingForm)
            {
                unit.ChangeForm(unitType);
            }
            takingOverStarted = false;
            unit.aiPath.canMove = false;
        }
    }

    public IEnumerator Kill(Unit unit)
    {
        while (unit.killingHp > 0)
        {
            yield return new WaitForSeconds(damageTime);
            if (unit == null)
            {
                killingStarted = false;
                unitState = UnitState.IDLE;
                break;
            }
            GiveDamage(unit,true);
            if (unitState != UnitState.KILLING)
            {
                killingStarted = false;
                yield break;
            }
            if(Vector3.Distance(transform.position,unit.transform.position)>rangeToAttack)
            {
                killingStarted = false;
                yield break;
            }

        }
        if (unit != null)
        {
            if (unit.killingHp <= 0)
            {
                unitState = UnitState.IDLE;
                if (unit != null)
                {
                    unit.Die();
                }
                killingStarted = false;
            }
        }
        else
        {
            yield break;
        }
    }

    public void TakeOverSelectedTarget(Unit targetObj)
    {
        unitState = UnitState.TAKING_OVER;
        targetUnit = targetObj;
    }

    protected virtual void FollowAndTakeOver()
    {

        target = targetUnit.transform.position;
        target = new Vector3(target.x, target.y, transform.position.z);

        destinationSetter.TargetPositionSet(target);
        if (Vector3.Distance(transform.position, target) <= rangeToAttack && !takingOverStarted)
        {
            takingOverStarted = true;
            StartCoroutine(TakeOver(targetUnit));
        }
    }

    protected void FollowAndBlowOver()
    {
        target = targetUnit.transform.position;
        target = new Vector3(target.x, target.y, transform.position.z);

        destinationSetter.TargetPositionSet(target);
        if (Vector3.Distance(transform.position, target) <= rangeToAttack && !takingOverStarted)
        {
            takingOverStarted = true;
            BlowOver();
            Invoke("Die",0.2f);
        }
    }

    protected void FollowAndKill()
    {
        target = targetUnit.transform.position;
        target = new Vector3(target.x, target.y, transform.position.z);
        destinationSetter.TargetPositionSet(target);
        if (Vector3.Distance(transform.position, target) <= rangeToAttack && !killingStarted)
        {
            Debug.DrawRay(transform.position, (targetUnit.transform.position-transform.position).normalized, Color.red,3f);
            hit2d = Physics2D.Raycast(transform.position, (targetUnit.transform.position - transform.position).normalized,
                rangeToAttack);

            Debug.Log("Raycast hit : " + hit2d.collider.gameObject.layer.ToString());

            killingStarted = true;
            target = transform.position;
            StartCoroutine(Kill(targetUnit));
        }
    }

    public void InterruptPathFollowing()
    {
        target = transform.position;
        targetUnit = null;
    }

    protected virtual void Update()
    {
        if (unitState == UnitState.IDLE && !aiPath.canMove)
        {
            //aiPath.canMove = true;
        }
    }

    protected virtual void HandleAnimations()
    {
        switch (unitState)
        {
            case UnitState.IDLE:
                {
                    if (targetUnit == null && aiPath.reachedEndOfPath)
                    {
                        animator.SetBool("Idle", true);
                        animator.SetBool("isAttacking", false);
                        animator.SetBool("isBeingAttacked", false);
                        animator.SetBool("isWalking", false);
                    }
                    if (targetUnit != null || !aiPath.reachedEndOfPath )
                    {
                        animator.SetBool("Idle", false);
                        animator.SetBool("isAttacking", false);
                        animator.SetBool("isBeingAttacked", false);
                        animator.SetBool("isWalking", true);
                        PlayLoopedFX(footStepSound);
                    }
                    break;
                }
            case UnitState.TAKING_OVER:
                {
                    if (targetUnit != null)
                    {
                        if (Vector3.Distance(transform.position, targetUnit.transform.position) < rangeToAttack)
                        {
                            animator.SetBool("Idle", false);
                            animator.SetBool("isAttacking", true);
                            animator.SetBool("isBeingAttacked", false);
                            animator.SetBool("isWalking", false);
                            if(unitType!=UnitType.playerBlower)
                            {
                                PlayLoopedFX(attackSound);
                            }
                            else
                            {
                                PlaySingleFx(attackSound);
                            }
                        }
                        else
                        {
                            animator.SetBool("Idle", false);
                            animator.SetBool("isAttacking", false);
                            animator.SetBool("isBeingAttacked", false);
                            animator.SetBool("isWalking", true);
                            PlayLoopedFX(footStepSound);
                        }
                    }
                    break;
                }
            case UnitState.BEING_ATTACKED:
                {
                    animator.SetBool("Idle", false);
                    animator.SetBool("isAttacking", false);
                    animator.SetBool("isBeingAttacked", true);
                    animator.SetBool("isWalking", false);
                    PlayLoopedFX(gettinHitSound);
                    break;
                }
            case UnitState.KILLING:
                {
                    animator.SetBool("Idle", false);
                    animator.SetBool("isAttacking", false);
                    animator.SetBool("isBeingAttacked", false);
                    animator.SetBool("isWalking", false);
                    animator.SetBool("isKilling", true);
                    PlayLoopedFX(attackSound);
                    break;
                }
        }
    }

    void PlayLoopedFX(AudioClip clip)
    {
        if (sfxSource.clip != clip)
        {
            if (sfxSource.clip != null)
            {
                sfxSource.Stop();
            }
            sfxSource.loop = true;
            sfxSource.clip = clip;
            sfxSource.Play();
        }
    }

    void PlaySingleFx(AudioClip clip)
    {
        if (sfxSource.clip != clip)
        {
            if (sfxSource.clip != null)
            {
                sfxSource.Stop();
            }
            sfxSource.loop = false;
            sfxSource.PlayOneShot(clip);
        }
    }
}
