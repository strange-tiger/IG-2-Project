namespace Defines
{
    public enum ESwitchController
    {
        Left,
        Right,
        End,
    }

    public enum ESceneNumber
    {
        LogIn,
        StartRoom,
        MakeCharacterRoom,
        FantasyLobby,
        WesternLobby,
        ArenaRoom,
        PrivateRoom,
        ShootingWaitingRoom,
        ShootingGameRoom,
        Lobby1TutorialRoom,
        ArenaTutorialRoom,

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

    public enum EVoiceType
    {
        None,
        Always,
        PushToTalk,
        MaxCount,
    }

    public enum ESoundType
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum EVoiceUIType
    {
        MasterVolume = 0,
        EffectVolume,
        BackGroundVolume,
        InputVolume,
        OutputVolume,
        MaxCount,
    }

    public enum EPianoKeyColor
    {
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple,
        Pink,
        MaxCount,
    }

    public enum ELogInUIIndex
    {
        LOGIN,
        SIGNIN,
        FINDPASSWORD,
        MAX
    }

    public enum ELogInErrorType
    {
        NONE,
        ID,
        PASSWORD,
        DUPLICATED,
        MAX
    }

    public enum EChangePasswordErrorType
    {
        NONE,
        ID,
        ANSWER,
        MAX
    }

    public enum ECharacterUIIndex
    {
        SELECT,
        MAKE,
        MAX
    }

    public enum EPrivateRoomUIIndex
    {
        JOIN,
        MAKE,
        MAX
    }

    public enum EPetUIIndex
    {
        CHAT,
        POPUP,
        PURCHASE,
        TRANSFORM,
        MAX
    }

    public enum ELobby2TutorialNumber
    {
        Start,
        FirstMoveAttack,
        WesternBeer,
        FindingGold,
        WesternCampfire,
        OakBarrel,
        GoldRush,
        End,
    }
}

namespace Defines.RPC
{
    public struct IsekaiRPCMethodName
    {
        public const string SpawnWeaponRPCHelper = "SpawnWeaponRPCHelper";
        public const string SpawnHelper = "SpawnHelper";
        public const string ReturnWeapon = "ReturnWeapon";
        public const string FlickHelper = "FlickHelper";
    }
    
}