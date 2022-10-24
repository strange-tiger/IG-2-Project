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
        FantasyLobby,
        WesternLobby,
        VikingLobby,
        ArenaRoom,

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
        Damage,
        Death,
    }

}