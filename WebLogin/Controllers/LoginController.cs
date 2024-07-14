using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLogin.Data;
using WebLogin.Models;
using WebLogin.Services;

namespace WebLogin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Start
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            UserDTO user = DBUser.Validate(email, ServiceUtilities.ConvertSHA256(password));

            if(user != null)
            {
                if (!user.Confirmed)
                {
                    ViewBag.Message = $"You need to confirm your account, please check your inbox ({email}).";
                }
                else if(user.ResetPwd)
                {
                    ViewBag.Message = $"You have a pending password reset, please check your inbox ({email})";
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "Could not find any matches.";
            }

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserDTO user)
        {
            if(user.Pwd != user.ConfirmPwd)
            {
                ViewBag.UserName = user.UserName;
                ViewBag.Email = user.Email;
                ViewBag.Message = "Passwords do not match.";
                return View();
            }

            if(DBUser.GetUser(user.Email) == null)
            {
                user.Pwd = ServiceUtilities.ConvertSHA256(user.Pwd);
                user.Token = ServiceUtilities.CreateToken();
                user.ResetPwd = false;
                user.Confirmed = false;

                bool result = DBUser.Register(user);
                if(result)
                {
                    string templatePath = HttpContext.Server.MapPath("~/Template/ConfirmEmail.html");
                    string content = System.IO.File.ReadAllText(templatePath);

                    // Building the Url to confirm the account: 0. Get Https or http, 1. Gets the domain (localhost in this case), 2. Sets the content of the url (token).
                    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/Login/Confirm?token=" + user.Token);

                    // Replaces username and url on html template
                    string htmlBody = string.Format(content, user.UserName, url);

                    EmailDTO emailDTO = new EmailDTO()
                    {
                        To = user.Email,
                        Subject = "Confirmation email",
                        Content = htmlBody
                    };

                    bool sent = EmailService.SendEmail(emailDTO);
                    ViewBag.Created = true;
                    ViewBag.Message = $"Your account has been created. We sent an email to {user.Email} to confirm your account.";
                }
                else
                {
                    ViewBag.Message = "We could not create your account, please try again.";
                }
            }
            else
            {
                ViewBag.Message = "The email is already registered.";
            }

            return View();
        }

        public ActionResult Confirm(string token)
        {
            ViewBag.Response = DBUser.ConfirmUser(token);
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string email)
        {
            UserDTO userDTO = DBUser.GetUser(email);
            ViewBag.Email = email;
            if(userDTO != null)
            {
                bool response = DBUser.ResetUser(1, userDTO.Pwd, userDTO.Token);

                if(response)
                {
                    string templatePath = HttpContext.Server.MapPath("~/Template/ResetPassword.html");
                    string content = System.IO.File.ReadAllText(templatePath);

                    // Building the Url to confirm the account: 0. Get Https or http, 1. Gets the domain (localhost in this case), 2. Sets the content of the url (token).
                    string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Headers["host"], "/Login/UpdatePassword?token=" + userDTO.Token);

                    // Replaces username and url on html template
                    string htmlBody = string.Format(content, userDTO.UserName, url);

                    EmailDTO emailDTO = new EmailDTO()
                    {
                        To = email,
                        Subject = "Account reset",
                        Content = htmlBody
                    };

                    bool sent = EmailService.SendEmail(emailDTO);
                    ViewBag.Reset = true;
                }
                else
                {
                    ViewBag.Message = "We could not reset your account.";
                }
            }
            else
            {
                ViewBag.Message = "We could not find any matches for your email.";
            }
            return View();
        }

        public ActionResult UpdatePassword(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(string token, string pwd, string confirmPwd)
        {
            ViewBag.Token = token;
            if (pwd != confirmPwd)
            {
                ViewBag.Message = "Passwords do not match.";
                return View();
            }

            bool response = DBUser.ResetUser(0, ServiceUtilities.ConvertSHA256(pwd), token);

            if (response)
                ViewBag.Reset = true;
            else
                ViewBag.Message = "We could not update your password.";

            return View();
        }
    }
}