public class Boss1 : Enemy {
    private const int MAX_HEALTH = 50;

    void Awake()
    {
        mWeakspotMultiplier = 2;
        mHealth = MAX_HEALTH;
        mWorth = 500;
        mAttackSpeed = 3;
        mAttackDamage = 1;
        mMaxMoveSpeed = 10f;
        mStopDistance = 2.3f;
        Init();
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateEnemy();
    }

    public override void MoveToTarget() {
        base.MoveToTarget();
        mAnimator.SetBool("Running", !mNavAgent.isStopped);
        mAnimator.SetBool("Attacking", false);
    }

    public override bool canAttack() {
        mAnimator.SetBool("Running", !mNavAgent.isStopped);
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
        mAnimator.SetBool("Running", false);
    }

    public int getMaxHealth() { return MAX_HEALTH; }
}
