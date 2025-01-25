namespace ZambeziDigital.Base.Models;

public class ActivityLog : BaseModel<int>
{
    [Key]
    public int ActivityId { get; set; }
    
    [Required]
    public string ActivityName { get; set; }
    
    [Required]
    public DateTime Timestamp { get; set; } = DateTime.Now;

    [Required]
    public string UserName { get; set; }
    
    [Required]
    public string UserId { get; set; }
    
    [Required]
    public string ActionEvent { get; set; }

    public string? ResourceId { get; set; }

    public string? Details { get; set; }
    
    [Required]
    public string ActivityStatus { get; set; }

    public string? SessionId { get; set; }
}