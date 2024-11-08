using Microsoft.AspNetCore.SignalR;

namespace HavucDent.Web.Hubs
{
	public class AppointmentHub : Hub
	{
		public async Task UpdateAppointments()
		{
			await Clients.All.SendAsync("ReceiveAppointmentsUpdate");
		}

		//public async Task RefreshAppointments()
		//{
		//	await Clients.All.SendAsync("ReceiveRefreshAppointments");
		//}


		//public async Task NotifyAppointmentUpdated()
		//{
		//	await Clients.All.SendAsync("ReceiveAppointmentUpdate");
		//}
	}
}