using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using ScopeIndia.Models;
using System.Diagnostics;
using MailKit.Net.Smtp;
using MailKit.Security;
using ScopeIndia.Data;

namespace ScopeIndia.Controllers
{
    public class WebHomeController : Controller
    {
        private readonly ILogger<WebHomeController> _logger;
        private readonly IRegistration _registration;

        public WebHomeController(ILogger<WebHomeController> logger, IRegistration registration)
        {
            _logger = logger;
            _registration = registration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("nandaathi@gmail.com"));
            email.To.Add(MailboxAddress.Parse("ns.athira93@gmail.com"));
            string emailBody = $"Name: {contact.FullName}\n" +
                   $"Email: {contact.EmailAddress}\n" +
                   $"Phone: {contact.PhoneNumber}\n\n";
                  

            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = emailBody
            };


            var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("ns.athira93@gmail.com", "zbcsiwajzleyapbb");
            smtp.Send(email);
            smtp.Disconnect(true);

            ViewBag.Email = "Email Has been sent Successfully. We will contact you Shortly";
            return View();
        }

        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration(Registration registration)
        {
            if (!ModelState.IsValid)
            {
                return View(registration);
            }
            string Hobbies = "";
            registration.ImagePath = UploadImage(registration.ImageFile);
            registration.Hobbies = string.Join(",", registration.Hobbieslist);
            _registration.Insert(registration);
            List<Registration> list = _registration.GetAll();
            Registration registration1 = _registration.GetByEmail(registration.Email);
            string url = $"http://localhost:7187/WebHome/PasswardGeneration/";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("nandaathi@gmail.com"));
            email.To.Add(MailboxAddress.Parse(registration.Email));

            
            string emailBody = $"Hi, {registration.FirstName}\nYour registered mail address is {registration.Email}\n";



            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"\n\nClick on this url to confirm Registration"+$"{url}"
            };

            var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("ns.athira93@gmail.com", "zbcsiwajzleyapbb");
            smtp.Send(email);
            smtp.Disconnect(true);

            ViewBag.Email = "You have been successfully registered with us";
            return View();


            //return View("Login"); // Assuming you want to redirect to the Login view after successful registration
        }
       
        
        
        public string? UploadImage(IFormFile ImageFile)
        {
            string? upload_path = null;
            var file_name = ImageFile.FileName;
            file_name = Path.GetFileName(file_name);
            string upload_folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads\\Images");
            if (!Directory.Exists(upload_folder))
            {
                Directory.CreateDirectory(upload_folder);
            }

            try
            {


                upload_path = Path.Combine(upload_folder, file_name);
                if (System.IO.File.Exists(upload_path))
                {
                    ViewBag.UploadStatus += file_name + "-Already File Exist";
                    Random file_number = new Random();
                    file_name = file_number.Next().ToString() + file_name;
                    upload_path = Path.Combine(upload_folder, file_name);
                }
                using (var uploadStream = new FileStream(upload_path, FileMode.Create))
                {
                    ImageFile.CopyTo(uploadStream);
                    ViewBag.UploadStatus += file_name + "Uploaded Sucessfully";
                }
                
            }

            catch (Exception ex)
            {
                ViewBag.UploadStatus += "Error Occured :" + ex.Message;

            }
            return file_name;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult StudentDashboard()
        {
            return View();
        }
        public IActionResult OtpGeneration()
        {
            return View();
        }
        public IActionResult PasswordGeneration()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
