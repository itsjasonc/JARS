using System.Collections.Generic;
using UnityEngine;

public class Killable : Interactable {
    [SerializeField] protected AudioClip mHitSound;
    [SerializeField] protected AudioClip mWeakspotSound;
    [SerializeField] protected List<AudioClip> mDeathSoundEffects;

    protected int mWeakspotMultiplier;

    protected int mHealth;
    protected int mWorth;
    protected bool mAlive;
    protected int mAttackDamage;
    protected float mAttackSpeed;
    protected float mLastAttackTime;
    protected float mMoveSpeed;
    protected float mMaxMoveSpeed;
    protected float mStopDistance;

    public virtual void Init() {
        mWeakspotMultiplier = 5;
        mLastAttackTime = 0;
        mAlive = true;
    }

    protected float checkDistance(Vector3 position) {
        return Vector3.Distance(transform.position, position);
    }

    public int getHealth() { return mHealth; }
    public void setHealth(int health) { mHealth = health; }
    public void decrementHealth(int health) { mHealth -= health; }
    public void incrementHealth(int health) { mHealth += health; }
    public int getWorth() { return mWorth; }
    public void setWorth(int worth) { mWorth = worth; }
    public bool getAlive() { return mAlive; }
    public void setAlive(bool alive) { mAlive = alive; }
    public int getAttackDamage() { return mAttackDamage; }
    public void setAttackDamage(int attackDamage) { mAttackDamage = attackDamage; }
    public float getAttackSpeed() { return mAttackSpeed; }
    public void setAttackSpeed(float attackSpeed) { mAttackSpeed = attackSpeed; }
    public float getLastAttackTime() { return mLastAttackTime; }
    public void setLastAttackTime(float lastAttackTime) { mLastAttackTime = lastAttackTime; }
    public float getMoveSpeed() { return mMoveSpeed; }
    public void setMoveSpeed(float moveSpeed) { mMoveSpeed = moveSpeed; }
    public float getMaxMoveSpeed() { return mMaxMoveSpeed; }
    public void setMaxMoveSpeed(float maxMoveSpeed) { mMaxMoveSpeed = maxMoveSpeed; }
    public float getStopDistance() { return mStopDistance; }
    public void setStopDistance(float stopDistance) { mStopDistance = stopDistance; }
    public AudioClip getDeathSound() { return mDeathSoundEffects.Count == 0 ? null : mDeathSoundEffects[Helper.randomInt(0, mDeathSoundEffects.Count)]; }
    public int getWeakspotMultiplier() { return mWeakspotMultiplier; }
    public void setWeakspotMultiplier(int weakspotMultiplier) { mWeakspotMultiplier = weakspotMultiplier; }

    public virtual void onHit(Interactable sender, int damage) {
        AudioSource.PlayClipAtPoint(mHitSound, transform.position);
        decrementHealth(damage);
        if (mMoveSpeed > (mMaxMoveSpeed / 2)) {
            mMoveSpeed -= ((mMoveSpeed / mMaxMoveSpeed));
        }
        if (sender is Player) {
            ((Player)sender).GetComponent<Player>().incrementScore(mWorth);
        }
    }

    public virtual void onWeakspot(Interactable sender, int damage) {
        AudioSource.PlayClipAtPoint(mWeakspotSound, transform.position);
        decrementHealth(damage);
        if (sender is Player) {
            sender.GetComponent<Player>().incrementShotsWeakSpot();
            sender.GetComponent<Player>().incrementScore(getWorth() * getWeakspotMultiplier());
        }
    }

    public virtual void onDeath(Interactable sender) {
        AudioSource.PlayClipAtPoint(getDeathSound(), transform.position);
        setAlive(false);
        if (sender is Player) {
            sender.GetComponent<Player>().incrementEnemiesKilled();
        }
    }

    public override void onInteract(object[] objs) {
        base.onInteract(objs);
        if (mHealth > 0) {
            if ((bool)objs[3]) {
                onWeakspot((Interactable)objs[0], (int)objs[2] * mWeakspotMultiplier);
            } else {
                onHit((Interactable)objs[0], (int)objs[2]);
            }
            if (getHealth() <= 0) {
                onDeath((Interactable)objs[0]);
            }
        }
    }
}
