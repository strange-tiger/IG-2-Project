using UnityEngine;

public static class AIAnimatorID
{
    // �⺻�ڼ�
    public static readonly int isIdle = Animator.StringToHash("isIdle");

    // �⺻ �̵�
    public static readonly int isRun = Animator.StringToHash("isRun");

    // �⺻����
    public static readonly int isAttack = Animator.StringToHash("isAttack");
    public static readonly int isAttack1 = Animator.StringToHash("isAttack1");
    public static readonly int isAttack2 = Animator.StringToHash("isAttack2");
    public static readonly int isAttack3 = Animator.StringToHash("isAttack3");

    // ��ų
    public static readonly int isSkill = Animator.StringToHash("isSkill");

    // �ǰ�
    public static readonly int isDamage = Animator.StringToHash("isDamage");

    // ���
    public static readonly int onDeath = Animator.StringToHash("onDeath");   
}
