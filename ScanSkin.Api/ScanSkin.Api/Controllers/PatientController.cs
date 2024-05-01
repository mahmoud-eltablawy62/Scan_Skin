using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MimeKit.Text;
using ScanSkin.Api.Dtos;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScanSkin.Api.Controllers
{
    public class PatientController : BController
    {
        private readonly HttpClient _httpClient;
       
        public PatientController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://deployment-2-fvni.onrender.com/");
        }
        [HttpPost]
        public async Task<IActionResult> RespondingToillness([FromForm] illnessDto Dto)
        {


            var image = Dto.Illness_Pucture;

            // Create a new HTTP multipart form content
            var content = new MultipartFormDataContent();

            // Add the photo to the form content
            var photoContent = new StreamContent(image.OpenReadStream());
            content.Add(photoContent, "image", image.FileName);

            // Send the HTTP POST request to the Flask API endpoint
            
            HttpResponseMessage response = await _httpClient.PostAsync("predict",content);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                int indexOfPTag = result.IndexOf("<p>") + 3;

                int indexOfTage2 = result.IndexOf("</p>");

                int Len = indexOfTage2 - indexOfPTag;

                string extractedSubstring = result.Substring(indexOfPTag, Len);

                return Ok(extractedSubstring);
            }
            else
            {
                return StatusCode((int)response.StatusCode);
            }
        }


    }
}
