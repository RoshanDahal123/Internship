using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public class TodoItem
{
    [Key]
    public int Id { get; set; }
    [Required(ErrorMessage ="Title is Required")]
    [StringLength(100,ErrorMessage ="Title cannot exceed 100 characters")]
    public string Title { get; set; } = string.Empty;
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }= DateTime.Now;
    public DateTime? CompletedAt { get; set; };
    public int Priority { get; set; } = 1; // 1=Low, 2=Medium, 3=High


}