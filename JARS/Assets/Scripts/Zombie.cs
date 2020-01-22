using UnityEngine;

public class Zombie : Enemy {

    private void Awake() {
        mHealth = 10;
        mWorth = 15;
        mAttackSpeed = 3;
        mAttackDamage = 1;
        mMaxMoveSpeed = 3.5f;
        mStopDistance = 2.15f;
        Init();
    }

    private void Update() {
        UpdateEnemy();
    }

    public override void MoveToTarget() {
        base.MoveToTarget();
        mAnimator.SetBool("Walking", true);
        mAnimator.SetBool("Attacking", false);
    }

    public override bool canAttack() {
        mAnimator.SetBool("Walking", false);
        return base.canAttack();
    }

    public override void Attack() {
        base.Attack();
        mAnimator.SetBool("Attacking", true);
    }

    public override void onHit(Interactable sender, int damage) {
        base.onHit(sender, damage);
        mAnimator.SetTrigger("Hit");
    }

    public override void onWeakspot(Interactable sender, int damage) {
        base.onWeakspot(sender, damage);
        mAnimator.SetTrigger("Hit");
    }

    public override void onDeath(Interactable sender) {
        base.onDeath(sender);
        mAnimator.SetTrigger("Death");
        mAnimator.SetBool("Attacking", false);
        mAnimator.SetBool("Walking", false);
    }
}