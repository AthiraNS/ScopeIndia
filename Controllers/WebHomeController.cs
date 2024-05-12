using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using MimeKit;
using ScopeIndia.Models;
using System.Diagnostics;
using MailKit.Net.Smtp;
using MailKit.Security;
using ScopeIndia.Data;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace ScopeIndia.Controllers    
{
    public class WebHomeController : Controller
    {
        private readonly ILogger<WebHomeController> _logger;
        private readonly IRegistration _registration;
        private readonly ICourse _course;

        public WebHomeController(ILogger<WebHomeController> logger, ICourse course, IRegistration registration)
        {
            _logger = logger;
            _registration = registration;
            _course = course;
        }

        private bool CheckSession()
        {

            string Cookies = HttpContext.Request.Cookies["Cookies"];
            string CookiesId = HttpContext.Request.Cookies["CookiesId"];
            string Session = HttpContext.Session.GetString("Session");

            if (Session != null || Cookies != null)
            {
                if (Session == null)
                {

                    HttpContext.Session.SetString("Session", Cookies);
                    HttpContext.Session.SetString("SessionId", CookiesId);
                }
                ViewBag.Session = Session;
                return true;
            }
            else
            {
                ViewBag.SessionOut = "Session Ends. Please login";
                return false;
            }
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
            if (TempData.ContainsKey("EmailVerification"))
            {
                ViewBag.EmailVerification = TempData["EmailVerification"];
            }

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
            string OTP = GenerateTempPassword();
            registration.Otp = OTP;
            string checkotp = OTP;
            //string SubEmail = registration.Email;
            string Hobbies = "";
            registration.ImagePath = UploadImage(registration.ImageFile);
            registration.Hobbies = string.Join(",", registration.Hobbieslist);
            _registration.Insert(registration);
            List<Registration> registrations = _registration.GetAll();
            Registration registration1 = _registration.GetByEmail(registration.Email);
            string url = $"https://localhost:7185/WebHome/PasswordGeneration";
            string url2 = $"https://localhost:7185/WebHome/EmailVerification/{registration1.Id}";


            //Console.WriteLine($"Temporary password for {registration.Email}: {tempPassword}");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("nandaathi@gmail.com"));
            email.To.Add(MailboxAddress.Parse(registration.Email));
            email.Subject = "Registration Confirmation";



            //string emailBody = $"Hi, {registration.FirstName},\n\nYour registered mail address is {registration.Email}.\n\nClick here to confirm registration {url}\n\n. Otp: {OTP}\n.Link To Verify Email Address:{url2}";
            string emailBody = $"Hi, {registration.FirstName},\n\nYour registered mail address is {registration.Email}.\n\n Click this Link To Verify your registered Email Address:{url2}";

            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"{emailBody}"
            };


            var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("ns.athira93@gmail.com", "zbcsiwajzleyapbb");
            smtp.Send(email);
            smtp.Disconnect(true);

            
            ViewBag.Email = "You have been successfully registered with us. Please Check your Registered email address for further details.";
            TempData["checkotp"] = OTP;
            TempData["Email"] = registration.Email;
            return View();



            //return View("Login"); // Assuming you want to redirect to the Login view after successful registration
        }
        private string GenerateTempPassword()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            const string numbers = "0123456789";

            var random = new Random();
            var tempPassword = new StringBuilder(8);

            // Add 4 random letters
            for (int i = 0; i < 4; i++)
            {
                tempPassword.Append(letters[random.Next(letters.Length)]);
            }

            // Add 4 random numbers
            for (int i = 0; i < 4; i++)
            {
                tempPassword.Append(numbers[random.Next(numbers.Length)]);
            }

            // Shuffle the characters in the password
            for (int i = 0; i < tempPassword.Length; i++)
            {
                int randIndex = random.Next(tempPassword.Length);
                char tempChar = tempPassword[i];
                tempPassword[i] = tempPassword[randIndex];
                tempPassword[randIndex] = tempChar;
            }

            return tempPassword.ToString();
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
                    //ViewBag.UploadStatus += file_name + "-Already File Exist";
                    Random file_number = new Random();
                    file_name = file_number.Next().ToString() + file_name;
                    upload_path = Path.Combine(upload_folder, file_name);
                }
                using (var uploadStream = new FileStream(upload_path, FileMode.Create))
                {
                    ImageFile.CopyTo(uploadStream);
                    ViewBag.UploadStatus += "Uploaded Sucessfully";
                }

            }

            catch (Exception ex)
            {
                ViewBag.UploadStatus += "Error Occured :" + ex.Message;

            }
            return file_name;
        }

        [Route("/WebHome/EmailVerification/{id}")]
        public IActionResult EmailVerification(int Id)
        {
            Registration registration = _registration.GetById(Id);
            if (registration == null)
            {
                return RedirectToAction("notverifed");
            }
            registration.IsVerified = true;
            _registration.Update(registration);

            //sent mail method...................
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("ns.athira93@gmail.com"));
            email.To.Add(MailboxAddress.Parse(registration.Email));

            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"\n\nYou have been successfully registered with us"


            };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("ns.athira93@gmail.com", "zbcsiwajzleyapbb");
            smtp.Send(email);
            smtp.Disconnect(true);

            ViewBag.EmailVerification = "Email Validated";
            TempData["EmailVerification"] = ViewBag.EmailVerification;
            return View("Registration");
        }














        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Login(Login login)
        {
            if (!ModelState.IsValid)
            {

                return View(login);
            }

            string Email = login.Email;
            string Password = login.Password;

            Registration update = _registration.GetByEmail(Email);


            //if ((Email == update.Email) && (Password == update.Password) && update != null)
            if (update != null && Email == update.Email && Password == update.Password)
            {
                if (login.RememberMe == true)
                {
                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTimeOffset.Now.AddHours(1);
                    HttpContext.Response.Cookies.Append("Cookies", "passwordGeneration", options);
                    HttpContext.Response.Cookies.Append("CookiesId", update.Id.ToString(), options);
                }
                HttpContext.Session.SetString("Session", "Login");
                HttpContext.Session.SetString("SessionId", update.Id.ToString());
                ViewBag.Values = login;
                TempData["EmailCourse"] = Email;
                TempData["EmailProfile"] = Email;

                return RedirectToAction("StudentDashboard");

            }
            else
            {
                ViewBag.LoginMessage = "Incorrect Email or Password";

                return View();
            }

        }



        //[HttpGet]
        //public IActionResult PasswordGeneration()
        //{
        //    if (!TempData.ContainsKey("PasswordGeneration"))
        //    {
        //        return RedirectToAction("Login");

        //    }
        //    ViewBag.Email = TempData["PasswordGeneration"];
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult PasswordGeneration(PasswordGeneration passwordGenereation )
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return View(passwordGenereation);
        //    }

        //    string Email = passwordGenereation.Email.ToString();
        //    Registration update = _registration.GetByEmail(Email);
        //    update.Password = passwordGenereation.NewPassword;
        //    _registration.Update(update);
        //    List<Registration> s = _registration.GetAll();
        //    return RedirectToAction("Home");
        //}
        //private bool VerifyOTP(string userEnteredOTP, string storedOTP)
        //{

        //    return userEnteredOTP == storedOTP;
        //}
        // Method to verify OTP (You need to implement this logic)
        //private bool VerifyOTP(Registration registration, string otp, PasswordGeneration passwordgenereation)
        //{
        //    string storedOTP = registration.Otp;
        //    string userEnteredOTP = passwordgenereation.TempPassword;
        //    if (VerifyOTP(userEnteredOTP, storedOTP))
        //    {
        //        // OTP verification successful
        //        ViewBag.OtpSuccess = "OTP validated successfully";
        //        // Proceed with the next steps
        //    }
        //    else
        //    {
        //        // OTP verification failed
        //        ViewBag.OtpFailure = "Invalid OTP. Please try again.";
        //        // Handle the failure (e.g., display an error message to the user)
        //    }
        //}


       

        public IActionResult PasswordGeneration()
        {
            if (!TempData.ContainsKey("PasswordGeneration"))
            {
                return RedirectToAction("Login");

            }
            ViewBag.Email = TempData["PasswordGeneration"];
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PasswordGeneration(PasswordGeneration passwordGeneration)
        {
            if (!ModelState.IsValid)
            {
                return View(passwordGeneration);
            }

            string Email = passwordGeneration.Email.ToString();
            Registration update = _registration.GetByEmail(Email);
            update.Password = passwordGeneration.NewPassword;
            _registration.Update(update);
            List<Registration> registration = _registration.GetAll();
            return RedirectToAction("Index");
        }

        public IActionResult OTPVerification()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OTPVerification(string otp)
        {
            string mailOTP = otp;

            string checkotp = TempData["checkotp"].ToString();
            try
            {
                if (mailOTP == checkotp)
                {
                    if (TempData.ContainsKey("Email"))
                        TempData["PasswordGeneration"] = TempData["Email"].ToString();
                    else
                        return RedirectToAction("Login");

                    return RedirectToAction("PasswordGeneration");
                }
                else
                    return RedirectToAction("Login");

            }
            catch (Exception ex)
            {
                ViewBag.otpmessage = $"OTP entered doesn't match,try again.{ex}";
            }

            return View();

        }




        public IActionResult FirstTimeLogin()
        {

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FirstTimeLogin(PasswordGeneration passwordGeneration)
        {
            string OTP = GenerateTempPassword();
            string checkotp = OTP;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("nandaathi@gmail.com"));
            email.To.Add(MailboxAddress.Parse(passwordGeneration.Email));
            email.Subject = "Password Setup";

            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"Hello there! Please note down your OTP to login the first time\n" +
                $"OTP={OTP}"

            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("ns.athira93@gmail.com", "zbcsiwajzleyapbb");
            smtp.Send(email);
            smtp.Disconnect(true);


            TempData["checkotp"] = OTP;
            TempData["Email"] = passwordGeneration.Email;
            return RedirectToAction("OTPVerification");

        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(PasswordGeneration passwordGeneration)
        {
            string OTP = GenerateTempPassword();
            string checkotp = OTP;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("ns.athira93@gmail.ctom"));
            email.To.Add(MailboxAddress.Parse(passwordGeneration.Email));
            email.Subject = "Temporary Password";

            email.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"Hello there! Your OTP to reset password\n" +
                $"OTP={OTP}"

            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("ns.athira93@gmail.com", "zbcsiwajzleyapbb");
            smtp.Send(email);
            smtp.Disconnect(true);


            TempData["checkotp"] = OTP;
            TempData["Email"] = passwordGeneration.Email;
            return RedirectToAction("OTPVerification");

        }




        public IActionResult UpdatePassword()
        {


            if (!CheckSession())
                return RedirectToAction("Login");

            int SessionId = Convert.ToInt32(HttpContext.Session.GetString("SessionId"));
            Registration profile = _registration.GetById(SessionId);


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult UpdatePassword(UpdatePassword update)
        {

            string Cookies = HttpContext.Request.Cookies["Cookies"];
            string Session = HttpContext.Session.GetString("Session");

            if (Session != null)
            {
                ViewBag.Session = Session;
            }
            else
            {
                ViewBag.SessionOut = "Session Ends. Please login";
                return RedirectToAction("Login");
            }

            if (Cookies != null)
            {

                ViewBag.Cookies = Cookies;
            }
            else
            {
                ViewBag.Message = "Cookies Deleted";

            }

            int SessionId = Convert.ToInt32(HttpContext.Session.GetString("SessionId"));
            Registration profile = _registration.GetById(SessionId);

            if (profile.Password == update.OldPassword)
            {
                profile.Password = update.NewPassword;
                _registration.Update(profile);

                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.PassMessage = "Existing Password doesn't match the records";

            }

            return View();
        }
        //public IActionResult StudentDashboard()
        //{
        //    return View();
        //}



        public IActionResult StudentDashboard()
        {


            if (!CheckSession())
                return RedirectToAction("Login");

            int SessionId = Convert.ToInt32(HttpContext.Session.GetString("SessionId"));
            Registration profile = _registration.GetById(SessionId);
            ViewBag.ProfileFirstName = profile.FirstName;
            ViewBag.ProfileLastName = profile.LastName;
            ViewBag.ProfileEmail = profile.Email;
            ViewBag.ProfileAvatar = profile.ImagePath;

            if (profile.Courses != null)
            {
                return RedirectToAction("SignedUpCourses");
            }

            List<CourseDetails> c = _course.GetAll();


            return View(c);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StudentDashboard(int CourseId)
        {
            string Cookies = HttpContext.Request.Cookies["Cookies"];
            string Session = HttpContext.Session.GetString("Session");

            int SessionId = Convert.ToInt32(HttpContext.Session.GetString("SessionId"));
            List<CourseDetails> c = _course.GetAll();

            if (!ModelState.IsValid)
            {
                Registration profile = _registration.GetById(SessionId);
                ViewBag.ProfileFirstName = profile.FirstName;
                ViewBag.ProfileLastName = profile.LastName;
                ViewBag.ProfileEmail = profile.Email;
                ViewBag.ProfileAvatar = profile.ImagePath;



                return View(c);

            }
            if (Session != null)
            {
                ViewBag.Session = Session;
            }
            _registration.UpdateCourseId(SessionId,CourseId);

            return RedirectToAction("SignedUpCourses");
        }

        public IActionResult SignedUpCourses()
        {
            if (!CheckSession())
                return RedirectToAction("Login");


            int SessionId = Convert.ToInt32(HttpContext.Session.GetString("SessionId"));
            Registration profile = _registration.GetById(SessionId);

            int CourseId = Convert.ToInt32(profile.Courses);
            CourseDetails details = _course.GetById(CourseId);

            ViewBag.ProfileFirstName = profile.FirstName;
            ViewBag.ProfileLastName = profile.LastName;
            ViewBag.ProfileEmail = profile.Email;
            ViewBag.ProfileAvatar = profile.ImagePath;
            ViewBag.CourseId = profile.Courses;
            ViewBag.ProfileCountry = profile.Country;
            ViewBag.ProfileState = profile.State;
            ViewBag.ProfileCity = profile.City;
            ViewBag.CourseName = details.CourseName;
            ViewBag.CourseDuration = details.CourseDuration;
            ViewBag.CourseFee = details.CourseFee;

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignedUpCourses(int courseId)
        {
            string Cookies = HttpContext.Request.Cookies["Cookies"];
            string Session = HttpContext.Session.GetString("Session");

            if (Session != null)
            {
                ViewBag.Session = Session;
            }
            else
            {
                ViewBag.SessionOut = "Session Ends. Please login";
                return RedirectToAction("Login");
            }

            if (Cookies != null)
            {

                ViewBag.Cookies = Cookies;


            }
            else
            {
                ViewBag.Message = "Cookies Deleted";

            }




            int SessionId = Convert.ToInt32(HttpContext.Session.GetString("SessionId"));
            Registration profile = _registration.GetById(SessionId);

            int CourseId = Convert.ToInt32(profile.Courses);
            CourseDetails details = _course.GetById(CourseId);

            ViewBag.ProfileFirstName = profile.FirstName;
            ViewBag.ProfileLastName = profile.LastName;
            ViewBag.ProfileEmail = profile.Email;
            ViewBag.ProfileAvatar = profile.ImagePath;
            ViewBag.CourseId = profile.Courses;
            ViewBag.ProfileCountry = profile.Country;
            ViewBag.ProfileState = profile.State;
            ViewBag.ProfileCity = profile.City;
            ViewBag.CourseName = details.CourseName;
            ViewBag.CourseDuration = details.CourseDuration;
            ViewBag.CourseFee = details.CourseFee;

            if (courseId == 0)
            {

                profile.Courses = null;
                _registration.Update(profile);

                return RedirectToAction("StudentDashboard");
            }
            return View();
        }

        public IActionResult Profile()
        {


            if (!CheckSession())
                return RedirectToAction("Login");


            int SessionId = Convert.ToInt32(HttpContext.Session.GetString("SessionId"));
            Registration profile = _registration.GetById(SessionId);

            int CourseId = Convert.ToInt32(profile.Courses);
            CourseDetails details = _course.GetById(CourseId);

            StudentProfile studentProfile = new StudentProfile();


            studentProfile.FirstName = profile.FirstName;
            studentProfile.LastName = profile.LastName;
            studentProfile.Email = profile.Email;
            studentProfile.Country = profile.Country;
            studentProfile.State = profile.State;
            studentProfile.City = profile.City;
            studentProfile.PhoneNumber = profile.MobileNumber;
            studentProfile.AvatarPath = profile.ImagePath;

            ViewBag.ProfileFirstName = studentProfile.FirstName;
            ViewBag.ProfileLastName = studentProfile.LastName;
            ViewBag.ProfileEmail = studentProfile.Email;
            ViewBag.ProfileAvatar = profile.ImagePath;
            ViewBag.ProfileGender = profile.Gender;
            ViewBag.ProfileEmail = studentProfile.Email;
            ViewBag.ProfilePhone = studentProfile.PhoneNumber;
            ViewBag.ProfileHobbies = profile.Hobbies;
            ViewBag.ProfileDateOfBirth = profile.DateOfBirth;
            ViewBag.ProfileCountry = studentProfile.Country;
            ViewBag.ProfileState = studentProfile.State;
            ViewBag.ProfileCity = studentProfile.City;
            ViewBag.CourseName = details.CourseName;
            ViewBag.CourseDuration = details.CourseDuration;
            ViewBag.CourseFee = details.CourseFee;
            ViewBag.ProfilePassword = profile.Password;
            ViewBag.CourseId = details.CourseId;
            ViewBag.Avatar = studentProfile.Avatar;

            return View(studentProfile);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(StudentProfile studentprofile)
        {

            string Cookies = HttpContext.Request.Cookies["Cookies"];
            string Session = HttpContext.Session.GetString("Session");

            if (Session != null)
            {
                ViewBag.Session = Session;
            }
            else
            {
                ViewBag.SessionOut = "Session Ends. Please login";
                return RedirectToAction("Login");
            }

            if (Cookies != null)
            {

                ViewBag.Cookies = Cookies;


            }
            else
            {
                ViewBag.Message = "Cookies Deleted";

            }

            int SessionId = Convert.ToInt32(HttpContext.Session.GetString("SessionId"));
            Registration profile = _registration.GetById(SessionId);

            int CourseId = Convert.ToInt32(profile.Courses);
            CourseDetails details = _course.GetById(CourseId);


            ViewBag.ProfileFirstName = profile.FirstName;
            ViewBag.ProfileLastName = profile.LastName;
            ViewBag.ProfileEmail = profile.Email;
            ViewBag.ProfileAvatar = profile.ImagePath;
            ViewBag.ProfileGender = profile.Gender;
            ViewBag.ProfileEmail = profile.Email;
            ViewBag.ProfilePhone = profile.MobileNumber;
            ViewBag.ProfileHobbies = profile.Hobbies;
            ViewBag.ProfileDateOfBirth = profile.DateOfBirth;
            ViewBag.ProfileCountry = profile.Country;
            ViewBag.ProfileState = profile.State;
            ViewBag.ProfileCity = profile.City;
            ViewBag.CourseId = details.CourseId;
            ViewBag.CourseName = details.CourseName;
            ViewBag.CourseDuration = details.CourseDuration;
            ViewBag.CourseFee = details.CourseFee;
            ViewBag.ProfilePassword = profile.Password;

            Registration update = _registration.GetById(SessionId);

            update.FirstName = studentprofile.FirstName;
            update.LastName = studentprofile.LastName;
            update.Email = studentprofile.Email;
            update.Country = studentprofile.Country;
            update.State = studentprofile.State;
            update.City = studentprofile.City;
            update.MobileNumber = studentprofile.PhoneNumber;
            if (studentprofile.Avatar != null)
                studentprofile.AvatarPath = UploadImage(studentprofile.Avatar);

            _registration.Update(update);
            return RedirectToAction("Login");

        }


        public IActionResult Logout()
        {
            Response.Cookies.Delete("Cookies");
            Response.Cookies.Delete("CookiesId");
            HttpContext.Session.Remove("Session");
            HttpContext.Session.Remove("SessionId");

            return RedirectToAction("Login");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
