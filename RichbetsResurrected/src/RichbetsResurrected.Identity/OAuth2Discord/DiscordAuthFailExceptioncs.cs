namespace RichbetsResurrected.Identity.OAuth2Discord;

public class DiscordAuthFailException : Exception
{
    public DiscordAuthFailException(string message) : base(message)
    {
    }
}
