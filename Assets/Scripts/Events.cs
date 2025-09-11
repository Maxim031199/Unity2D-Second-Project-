using System;

public static class Events
{
    public static Action<int> PlayerHP;
    public static Action GameOver;
    public static Action EnemyKilled;     
    public static Action GameWon;
}
