using UnityEngine;

public static class AIAnimatorID
{
    public static readonly int isIdle = Animator.StringToHash("isIdle");
    public static readonly int isRun = Animator.StringToHash("isRun");
    public static readonly int isAttack1 = Animator.StringToHash("isAttack1");
    public static readonly int isAttack2 = Animator.StringToHash("isAttack2");
    public static readonly int isSkill = Animator.StringToHash("isSkill");
    public static readonly int isDamage = Animator.StringToHash("isDamage");
    public static readonly int isDeath = Animator.StringToHash("isDeath");
    public static readonly int Death = Animator.StringToHash("Death");
}
