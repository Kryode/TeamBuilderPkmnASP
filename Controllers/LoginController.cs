using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamBuilderPkmnASP.Data;
using TeamBuilderPkmnASP.Models;
using SendGrid.Helpers.Mail;
using System.Security.Cryptography;
using KryodeHelpers;

namespace TeamBuilderPkmnASP.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserContext _context;
        
        public LoginController(UserContext context)
        {
            _context = context;
        }

        public ActionResult Signin()
        {
            string userMail = HttpContext.Request.Form["mail"].ToString();
            string pwd = HttpContext.Request.Form["pwd"].ToString();
            IQueryable<User> users = _context.User.Where(user => user.Mail == userMail);
            if (users.Any())
            {
                TempData["Error"] = "Mail déjà enregistré";
                return RedirectToAction("Login", "Home");
            }
            if(HttpContext.Request.Form["validPwd"].ToString() != pwd)
            {
                TempData["Error"] = "Les mots de passe ne sont pas identiques";
                return RedirectToAction("Signin", "Home");
            }
            else
            {
                User user = new User();
                user.Mail = HttpContext.Request.Form["mail"];
                user.Pseudo = HttpContext.Request.Form["pseudo"];
                user.IsPwdInputHashed = false;
                user.Password = HttpContext.Request.Form["pwd"];
                user.VerificationToken = Guid.NewGuid().ToString();
                user.IsVerified = false;
                try
                {
                    _context.User.Add(user);

                    var link = "https://localhost:44392/Login/VerifyMail/" + user.VerificationToken;
                    var message = new SendGridMessage();
                    message.AddTo(user.Mail);
                    message.From = new EmailAddress("verification@pkmnteambuilder.com");
                    message.Subject = "Vérifiez votre compte PokemonTeamBuilder !";
                    message.HtmlContent = "<p>Veuillez cliquer sur le lien ci-après pour valider votre compte !</p>" +
                        "<a href=\"" + link + "\">Valider votre compte</a>";
                    _context.SaveChanges();
                    DatabaseConnection.MailClient.SendEmailAsync(message);
                }
                catch (Exception ex)
                {
                    ///TODO: Gestion propre d'exception
                    var log = ex.Message;
                }
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Login()
        {
            string mail = HttpContext.Request.Form["Mail"];
            IQueryable<User> users = _context.User.Where(user => user.Mail == mail);
            User user = users.FirstOrDefault();
            string hashedEntry = ConnectionHelper.SHA256StringConstructor(HttpContext.Request.Form["pwd"]);
           /* using (var sha = new SHA256Managed())
            {
                string transpPwd = HttpContext.Request.Form["pwd"].ToString();
                byte[] array = Encoding.Default.GetBytes(HttpContext.Request.Form["pwd"].ToString());
                array = sha.ComputeHash(array);
                StringBuilder sb = new StringBuilder();
                foreach (var b in array)
                {
                    sb.Append(b.ToString("x2"));
                }
                hashedEntry = sb.ToString();
            }*/
            if (user != null && user.Password == hashedEntry)
            {
                if (!user.IsVerified)
                {
                    TempData["Error"] = "Compte non validé, veuillez vérifier vos mails";
                    return RedirectToAction("Signin", "Home");
                }
                ISession session = HttpContext.Session;
                session.SetString("User", user.Mail);
                ViewData["User"] = user;
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["Error"] = "Identifiants invalides";
                return RedirectToAction("Login", "Home");
            }
        }

        public ActionResult Logoff()
        {
            ISession session = HttpContext.Session;
            session.Clear();
            return RedirectToAction("Index","Home");
        }

        public ActionResult VerifyMail(string id)
        {
            
            User user = _context.User.Where(user => user.VerificationToken == id).FirstOrDefault();

            if (user != null)
            {
                TempData["Usermail"] = user.Mail;
                TempData["Token"] = user.VerificationToken;
                TempData["Error"] = "Connect to confirm your mail";
                //user.IsVerified = true;
                //_context.SaveChanges();
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ///TODO: Setup contact mail
                TempData["Error"] = "The link is wrong or expired, please confirm the link in mail or contact an administrator at ''";
                return RedirectToAction("Signin", "Home");
            }
        }
        public ActionResult VerifyLogin()
        {
            string usermail = HttpContext.Request.Form["mail"];
            string password = ConnectionHelper.SHA256StringConstructor(HttpContext.Request.Form["pwd"]);
            string verificationToken = HttpContext.Request.Form["Token"];
            User user = _context.User.Where(user => user.Mail == usermail).FirstOrDefault();
            if(user != null && password == user.Password && verificationToken == user.VerificationToken)
            {
                user.IsVerified = true;
                _context.SaveChanges();
                ISession session = HttpContext.Session;
                session.SetString("User", user.Mail);
                ViewData["User"] = user;
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["Error"] = "Login error, please verify your mail and password";
                return RedirectToAction("VerifyMail", "Login");
            }
        }
    }
}
