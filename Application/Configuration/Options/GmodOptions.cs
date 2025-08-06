using System.ComponentModel.DataAnnotations;
using Presentation.Configuration.Options;

namespace Application.Configuration.Options;

public class GmodOptions : IConfigurationOptions
{
    public static string SectionName { get; } = "Gmod";
    
    [Required]
    public required DiscordWebhookOptions DiscordWebhook { get; init; }
}