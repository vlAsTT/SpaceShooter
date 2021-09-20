namespace Player
{
    public static class PlayerDelegates
    {
        public delegate void PlayerEvent();
    
        public static event PlayerEvent onPlayerDeath;

        public static void OnPlayerDeath()
        {
            onPlayerDeath?.Invoke();
        }
    }
}
