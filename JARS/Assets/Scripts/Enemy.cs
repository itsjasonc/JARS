using UnityEngine;
using UnityEngine.AI;

public class Enemy : Killable {
    [SerializeField] protected GameObject mTarget;

    protected NavMeshAgent mNavAgent;
    protected Animator mAnimator;

    public override void Init() {
        base.Init();
        mAnimator = GetComponent<Animator>();
        mNavAgent = GetComponent<NavMeshAgent>();
        mAnimator.enabled = true;
        mNavAgent.speed = mMoveSpeed;
        mMoveSpeed = mMaxMoveSpeed;
    }

    public GameObject getTarget() { return mTarget; }
    public void setTarget(GameObject target) { mTarget = target; }
    protected Animator getAnimator() { return mAnimator; }
    protected void setAnimator(Animator animator) { mAnimator = animator; }

    public virtual void UpdateEnemy() {
        if (mTarget != null) {
            if (mAlive) {
                UpdateSpeed();
                float distance = checkDistance(mTarget.transform.position - (mTarget.transform.forward * mStopDistance));
                if (distance > mStopDistance) {
                    MoveToTarget();
                } else {
                    if (canAttack()) {
                        Attack();
                    }
                }
            }
        }
    }

    protected void UpdateSpeed() {
        if (mMoveSpeed <= mMaxMoveSpeed) {
            mMoveSpeed += mMoveSpeed / mMaxMoveSpeed;
        }
        if (mMoveSpeed > mMaxMoveSpeed) mMoveSpeed = mMaxMoveSpeed;
        mNavAgent.speed = mMoveSpeed;
    }

    public virtual void MoveToTarget() {
        mNavAgent.isStopped = false;
        mNavAgent.SetDestination(mTarget.transform.position - (mTarget.transform.forward * mStopDistance));
    }

    public virtual bool canAttack() {
        mNavAgent.isStopped = true;
        transform.rotation = Quaternion.LookRotation(mTarget.transform.forward); // Look at the target
        if ((Time.time - mLastAttackTime) >= mAttackSpeed) {
            return true;
        }
        return false;
    }

    public virtual void Attack() {
        if (mLastAttackTime == 0)
        {
            mLastAttackTime = Time.time - 2;
        }
        else
        {
            mLastAttackTime = Time.time;

            object[] data = new object[4];
            data[0] = this.gameObject;
            data[1] = Interactable.Type.Damage;
            data[2] = mAttackDamage;
            data[3] = false;

            mTarget.SendMessage("onInteract", data);
        }

    }

    public override void onDeath(Interactable sender) {
        base.onDeath(sender);
        mNavAgent.isStopped = true;
    }

    private void OnDrawGizmos() {
#if (UNITY_EDITOR)
        if (mTarget != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(mTarget.transform.position, 1);
        }
#endif
    }
}
