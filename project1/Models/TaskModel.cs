namespace project1.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public int KullaniciId { get; set; }
        public string? Aciklama { get; set; }
        public DateTime Tarih { get; set; }
        public bool Durum { get; set; }
    }
}