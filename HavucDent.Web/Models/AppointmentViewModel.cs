namespace HavucDent.Web.ViewModels
{
	public class AppointmentViewModel
	{
		// Randevu bilgileri
		public int? Id { get; set; }
		public DateTime AppointmentDate { get; set; }
		public int DoctorId { get; set; }
		public int PatientId { get; set; }
		public bool IsAvailable { get; set; } = true;
		public decimal TotalFee { get; set; }
		public bool PaymentStatus { get; set; }
		public bool IsCompleted { get; set; }
		public int? AssistantId { get; set; }


		// Hasta bilgileri
		public string TcNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public DateTime BirthDate { get; set; }
	}
}