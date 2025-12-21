using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebAppodev.Controllers
{
	public class ImageController : Controller
	{
		private readonly IConfiguration _configuration;
		private readonly HttpClient _httpClient;

		public ImageController(IConfiguration configuration)
		{
			_configuration = configuration;
			_httpClient = new HttpClient();
		}

		// GET: Image/Index
		public IActionResult Index()
		{
			return View();
		}
		// Inside ImageController.cs
		public IActionResult ImageGenerator()
		{
			return View();
		}

		// POST: Image/Generate
		[HttpPost]
		public async Task<IActionResult> Generate(string prompt)
		{
			if (string.IsNullOrWhiteSpace(prompt))
			{
				ViewBag.Error = "Please enter a prompt.";
				return View("Index");
			}

			string apiKey = _configuration["StabilityAI:ApiKey"];
			if (string.IsNullOrEmpty(apiKey))
			{
				ViewBag.Error = "API Key is missing in appsettings.json";
				return View("Index");
			}

			// 1. Prepare the API Request
			// SDXL 1.0 Endpoint
			string url = "https://api.stability.ai/v1/generation/stable-diffusion-xl-1022461/text-to-image";

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
			_httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			// 2. Create the Payload (Matches Stability AI Docs)
			var payload = new
			{
				text_prompts = new[] {
					new { text = prompt, weight = 1 }
				},
				cfg_scale = 7,
				height = 1024,
				width = 1024,
				samples = 1,
				steps = 30
			};

			var jsonPayload = JsonSerializer.Serialize(payload);
			var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

			// 3. Send Request
			try
			{
				var response = await _httpClient.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					var responseString = await response.Content.ReadAsStringAsync();

					// Deserialize the response to get the Base64 string
					var result = JsonSerializer.Deserialize<StabilityResponse>(responseString);

					// Pass the first image to the view
					if (result != null && result.artifacts != null && result.artifacts.Count > 0)
					{
						ViewBag.ImageBase64 = result.artifacts[0].base64;
						ViewBag.Prompt = prompt;
					}
				}
				else
				{
					var errorMsg = await response.Content.ReadAsStringAsync();
					ViewBag.Error = $"API Error: {response.StatusCode} - {errorMsg}";
				}
			}
			catch (Exception ex)
			{
				ViewBag.Error = $"System Error: {ex.Message}";
			}

			return View("Index");
		}

		// Helper classes to match Stability AI JSON response structure
		public class StabilityResponse
		{
			public List<Artifact> artifacts { get; set; }
		}

		public class Artifact
		{
			public string base64 { get; set; }
			public long seed { get; set; }
			public string finishReason { get; set; }
		}
	}
}