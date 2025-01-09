using Kuse_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Kuse_App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Ootan sind minu peole! Palun tule!!!";
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 10 ? "Tere hommikust!" : "Tere päevast!";
            return View();
        }

        [HttpGet]
        public ViewResult Ankeet(Guest guest)
        {
            E_mail(guest);
            if (ModelState.IsValid)
            {
                return View("Thanks", guest);
            }
            else
            {
                return View();
            }
        }
        public void E_mail(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "allikvaleria@gmail.com";
                WebMail.Password = "iqer zkvm czuv lgqn"; //change
                WebMail.From = "allikvaleria@gmail.com";
                WebMail.Send("allikvaleria@gmail.com", "Vastus kutsele", guest.Name + " vastas " + (guest.WillAttend.HasValue && guest.WillAttend.Value ? "tuleb peole" : "ei tule peole"));
                ViewBag.Message = "Kiri on saatnud!";
            }
            catch (Exception)
            {
                ViewBag.Message = "Mul on kahju!Ei saa kirja saada!!!";
            }
        }
    }
}