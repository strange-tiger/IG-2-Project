namespace Defines
{
    public enum ESwitchController
    {
        Left,
        Right,
        End,
    }

    public enum ESceneNumder
    {
        LogIn,
        StartRoom,
        MakeCharacterRoom,
        FantasyLobby,
        WesternLobby,
        ArenaRoom,
        ShootingWaitingRoom,
        ShootingGameRoom,

        End,
    }

    public enum EParticleDurationTime
    {
        Now,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,

        TwentyFive = 25,
        Sixty = 60,

    }

    public enum CoolTime
    {
        NoCool = 0,
        Sixty = 60,
        TwentyFive = 25,
        Ten = 10,
    }

    public enum EDegrees
    {
        RightAngle = 90,
        TurnAround = 180,
    }

    public enum Estate
    {
        IDLE,
        Run,
        Attack,
        Skill,
        Damage,
        Death,
    }
    
    public enum EJobClass
    {
        Prototype,
        HighClassKnight,
        HighClassAdventurer,
        FireWizard,
        IceWizard,

    }

    public enum EAttackKind
    {
        Attack1,
        Attack2,
        HighClassKnightAttack,
    }

    public enum EDamage
    {
        Damage10 = 10,
        Damage15 = 15,
        Damage20 = 20,
        Damage30 = 30,
        Damage40 = 40,
    }

    public enum EHp
    {
        Hp100 = 100,
        Hp200 = 200,
    }
}