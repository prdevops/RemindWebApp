using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RemindWebApp.ViewModels;

namespace RemindWebApp.Controllers
{
   
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Index(EmailViewModel emailViewModel)
        {
            MailMessage message = new MailMessage("musfiq.sm@gmail.com", "mushfigma@code.edu.az");
            message.Subject = emailViewModel.Subject +"Message from User:"+ emailViewModel.From;
            message.Body = emailViewModel.Body;
            message.IsBodyHtml = false;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            NetworkCredential networkCredential = new NetworkCredential("musfiq.sm@gmail.com", "mushu.1475963.0@.0_1.");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = networkCredential;
            smtpClient.Send(message);
            ViewBag.Message = "Your message has been  send succesfully";

            return View();
        }
    }
}