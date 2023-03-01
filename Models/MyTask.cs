using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FinalAspReact.Models
{
    public class MyTask
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Назва задачі")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Опис")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Дата закінчення задачі")]
        public DateTime DeadLine { get; set; }

        [MaxLength(100)]
        [Display(Name = "Хештег")]
        public string Hashtag { get; set; }

        [Display(Name = "Файл")]
        public string File { get; set; }

        [Required]
        public bool Done { get; set; }

        [Required]
        [Display(Name = "Категорія")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        public virtual List<Subtask> MyTasks { get; set; }

        [NotMapped]
        public string DeleteMyTask { get; set; }    

    }
}
