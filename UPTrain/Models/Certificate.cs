namespace UPTrain.Models
{
    public class Certificate
    {
        public int CertificateId { get; set; }
        public DateTime DateIssued { get; set; }
        public string CertificatePath { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; }

        public int CourseId { get; set; }
        public Courses? Course { get; set; }
    }
}
