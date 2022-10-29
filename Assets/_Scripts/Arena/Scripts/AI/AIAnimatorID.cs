using UnityEngine;

public static class AIAnimatorID
{
    // 기본자세 모음
    public static readonly int isIdle = Animator.StringToHash("isIdle");

    public static readonly int isHighClassKnightIdle = Animator.StringToHash("isHighClassKnightIdle");
    public static readonly int isHighClassAdventurerIdle = Animator.StringToHash("isHighClassAdventurerIdle");
    public static readonly int isFireWizardIdle = Animator.StringToHash("isFireWizardIdle");
    public static readonly int isIceWizardIdle = Animator.StringToHash("isIceWizardIdle");

    // 기본 이동 모음
    public static readonly int isRun = Animator.StringToHash("isRun");

    public static readonly int isHighClassKnightRun = Animator.StringToHash("isHighClassKnightRun");
    public static readonly int isHighClassAdventurerRun = Animator.StringToHash("isHighClassAdventurerRun");
    public static readonly int isFireWizardRun = Animator.StringToHash("isFireWizardRun");
    public static readonly int isIceWizardRun = Animator.StringToHash("isIceWizardRun");

    // 기본공격 모음
    public static readonly int isAttack1 = Animator.StringToHash("isAttack1");
    public static readonly int isAttack2 = Animator.StringToHash("isAttack2");

    public static readonly int isHighClassKnightAttack = Animator.StringToHash("isHighClassKnightAttack");
    public static readonly int isHighClassAdventurerAttack = Animator.StringToHash("isHighClassAdventurerAttack");
    public static readonly int isFireWizardAttack = Animator.StringToHash("isFireWizardAttack");
    public static readonly int isIceWizardAttack = Animator.StringToHash("isIceWizardAttack");

    // 스킬 모음
    public static readonly int isSkill = Animator.StringToHash("isSkill");

    public static readonly int isHighClassKnightSkill = Animator.StringToHash("isHighClassKnightSkill");
    public static readonly int isHighClassAdventurerSkill = Animator.StringToHash("isHighClassAdventurerSkill");
    public static readonly int isFireWizardSkill = Animator.StringToHash("isFireWizardSkill");
    public static readonly int isIceWizardSkill = Animator.StringToHash("isIceWizardSkill");

    // 피격 모음
    public static readonly int isDamage = Animator.StringToHash("isDamage");

    public static readonly int isHighClassKnightDamage = Animator.StringToHash("isHighClassKnightDamage");
    public static readonly int isHighClassAdventurerDamage = Animator.StringToHash("isHighClassAdventurerDamage");
    public static readonly int isFireWizardDamage = Animator.StringToHash("isFireWizardDamage");
    public static readonly int isIceWizardDamage = Animator.StringToHash("isIceWizardDamage");

    // 사망 모음
    public static readonly int isDeath = Animator.StringToHash("isDeath");

    public static readonly int isHighClassKnightDeath = Animator.StringToHash("isHighClassKnightDeath");
    public static readonly int isHighClassAdventurerDeath = Animator.StringToHash("isHighClassAdventurerDeath");
    public static readonly int isFireWizardDeath = Animator.StringToHash("isFireWizardDeath");
    public static readonly int isIceWizardDeath = Animator.StringToHash("isIceWizardDeath");
}


//          HighClassKnight,
//        HighClassAdventurer,
//        FireWizard,
//        IceWizard,