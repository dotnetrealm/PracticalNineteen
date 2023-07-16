using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PracticalNineteen.Domain.DTO;
using System.Net;

namespace PracticalNineteen.MVC.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        //HTTP Client
        readonly HttpClient _httpClient;

        public StudentController(IHttpClientFactory httpClient, IHttpContextAccessor accessor)
        {
            _httpClient = httpClient.CreateClient("api");
            var token = accessor.HttpContext?.User.Claims.FirstOrDefault(e => e.Type == "Token")?.Value;
            if (token is not null) _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        }

        /// <summary>
        /// Dashboard page with students list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            throw new Exception();
            IEnumerable<StudentModel>? students = await _httpClient.GetFromJsonAsync<IEnumerable<StudentModel>>("Students");
            return View(students);
        }

        /// <summary>
        /// Return detailed student view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewAsync(int id)
        {
            StudentModel? student = await _httpClient.GetFromJsonAsync<StudentModel>($"students/{id}");
            if (student == null) return NotFound();
            return View(student);
        }

        /// <summary>
        /// Return student form to create new student
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new StudentModel());
        }

        /// <summary>
        /// Submit student form to add new student to DB
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync(StudentModel student)
        {
            //future date validation
            if (student.DOB > DateTime.Now)
                ModelState.AddModelError(nameof(StudentModel.DOB), $"Please enter a value less than or equal to {DateTime.Now.ToShortDateString()}.");

            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("students", student);
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    TempData["UserId"] = response.Content.ReadFromJsonAsync<StudentModel>().Result!.Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }

            }
            return View();
        }

        /// <summary>
        /// Return edit form with student details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAsync(int id)
        {
            StudentModel? student = await _httpClient.GetFromJsonAsync<StudentModel>($"Students/{id}");
            if (student == null) return NotFound();
            return View(student);
        }

        /// <summary>
        /// Submit student form to update existing student in DB
        /// </summary>
        /// <param name="id"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAsync(int id, StudentModel student)
        {
            if (student.DOB > DateTime.Now) ModelState.AddModelError(nameof(StudentModel.DOB), $"Please enter a value less than or equal to {DateTime.Now.ToShortDateString()}.");
            if (!ModelState.IsValid) return View(student);

            HttpResponseMessage response = await _httpClient.PutAsJsonAsync<StudentModel>($"Students/{id}", student);
            if (!response.IsSuccessStatusCode) return RedirectToAction("Error", "Home");
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Remove student from DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var res = await _httpClient.DeleteAsync($"Students/{id}");
            if (res.IsSuccessStatusCode) return RedirectToAction("Index");
            return RedirectToAction("Error", "Home");
        }

        /// <summary>
        /// 404 Error page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult PageNotFound() { return View(); }
    }
}
