using System.Text;
using System.Net.Mail;
using System.Net;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;

namespace SportsStore.Domain.Concrete
{
	public class EmailSettings
	{
		public string MailToAddress = "orders@example.com";
		public string MailFromAddress = "sportsstore@example.com";
		public bool UseSsl = true;
		public string Username = "MySmtpUsername";
		public string Password = "MySmtpPassword";
		public string ServerName = "smtp.example.com";
		public int ServerPort = 587;
		public bool WriteAsFile = false;
		public string FileLocation = Globals.LocalPath + "EmailFiles";
	}

	public class EmailOrderProcessor : IOrderProcessor
	{
		private EmailSettings EmailSettings;

		public EmailOrderProcessor(EmailSettings emailSettings)
		{
			EmailSettings = emailSettings;
		}

		public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
		{
			using (SmtpClient smtpClient = new SmtpClient())
			{
				smtpClient.EnableSsl = EmailSettings.UseSsl;
				smtpClient.Host = EmailSettings.ServerName;
				smtpClient.Port = EmailSettings.ServerPort;
				smtpClient.UseDefaultCredentials = false;
				smtpClient.Credentials = new NetworkCredential(EmailSettings.Username, EmailSettings.Password);

				if (EmailSettings.WriteAsFile)
				{
					smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
					smtpClient.PickupDirectoryLocation = EmailSettings.FileLocation;
					smtpClient.EnableSsl = false;
				}

				StringBuilder body = new StringBuilder()
					.AppendLine("A new order has been submitted")
					.AppendLine("---")
					.AppendLine("Items:");
				foreach (CartLine line in cart.Lines)
				{
					decimal subtotal = line.Product.Price * line.Quantity;
					body.AppendFormat("{0} x {1} (subtotal: {2:c}", line.Quantity, line.Product.Name, subtotal);
				}

				body.AppendFormat("Total order value: {0:c}", cart.ComputeTotalValue())
					.AppendLine("---")
					.AppendLine("Ship to:")
					.AppendLine(shippingInfo.Name)
					.AppendLine(shippingInfo.Line1)
					.AppendLine(shippingInfo.Line2 ?? "")
					.AppendLine(shippingInfo.Line3 ?? "")
					.AppendLine(shippingInfo.City)
					.AppendLine(shippingInfo.State ?? "")
					.AppendLine(shippingInfo.Country)
					.AppendLine(shippingInfo.Zip)
					.AppendLine("---")
					.AppendFormat("Gift wrap: {0}",
					shippingInfo.GiftWrap ? "Yes" : "No");

				MailMessage mailMessage = new MailMessage(
					EmailSettings.MailFromAddress, // From
					EmailSettings.MailToAddress, // To
					"New order submitted!", // Subject
					body.ToString() // Body
				);

				if (EmailSettings.WriteAsFile)
				{
					mailMessage.BodyEncoding = Encoding.ASCII;
				}

				smtpClient.Send(mailMessage);
			}
		}
	}

}
