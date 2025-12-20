using Microsoft.AspNetCore.Mvc;
using WebAppodev.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebAppodev.Controllers
{
	public class AIController : Controller
	{
		// 1. SET YOUR KEYS HERE
		private const string GithubToken = "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";
		private const string StabilityApiKey = "sk-TgAQK0g7YHHoXfeW17DVy5ZVm26FM78XpzSq95pCNkK215FD";

		// ==========================================
		// PAGE 1: TEXT WORKOUT PLANNER
		// ==========================================
		public IActionResult Index()
		{
			return View(new UserProfile());
		}

		[HttpPost]
		public async Task<IActionResult> GeneratePlan(UserProfile profile)
		{
			// Clean up model state for text generation
			ModelState.Remove("ImageUpload");
			ModelState.Remove("GeneratedImageBase64");
			ModelState.Remove("AIResponse");

			if (!ModelState.IsValid) return View("Index", profile);

			try
			{
				using (var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GithubToken);

					var requestBody = new
					{
						model = "gpt-4o",
						messages = new[]
						{
							new { role = "system", content = "You are a fitness trainer. Keep advice concise." },
							new { role = "user", content = $"Create a workout plan for a {profile.Age} year old, {profile.Weight}kg, {profile.Height}cm person. Goal: {profile.Goal}." }
						}
					};

					var response = await client.PostAsync(
						"https://models.github.ai/inference/chat/completions",
						new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json"));

					if (response.IsSuccessStatusCode)
					{
						var json = await response.Content.ReadAsStringAsync();
						using (JsonDocument doc = JsonDocument.Parse(json))
						{
							profile.AIResponse = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
						}
					}
				}
			}
			catch (Exception ex)
			{
				profile.AIResponse = "Error: " + ex.Message;
			}

			return View("Result", profile);
		}

		// ==========================================
		// PAGE 2: AI IMAGE TRANSFORMER (NEW)
		// ==========================================
		public IActionResult ImageGenerator()
		{
			return View(new UserProfile());
		}

		[HttpPost]
		public async Task<IActionResult> GenerateImageOnly(UserProfile profile)
		{
			// We only care about the image upload here
			ModelState.Remove("AIResponse");

			if (profile.ImageUpload != null && profile.ImageUpload.Length > 0)
			{
				try
				{
					// 1. Read file into memory safely
					byte[] imageBytes;
					using (var ms = new MemoryStream())
					{
						await profile.ImageUpload.CopyToAsync(ms);
						imageBytes = ms.ToArray();
					}

					// 2. Send to Stability AI
					using (var client = new HttpClient())
					{
						client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", StabilityApiKey);
						client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

						using (var content = new MultipartFormDataContent())
						{
							// Prompt settings
							content.Add(new StringContent($"Transform body to {profile.Goal} physique, keep face, gym lighting, 8k resolution"), "text_prompts[0][text]");
							content.Add(new StringContent("1.0"), "text_prompts[0][weight]");
							content.Add(new StringContent("0.35"), "image_strength");
							content.Add(new StringContent("IMAGE_STRENGTH"), "init_image_mode");

							// File Upload
							var imageContent = new ByteArrayContent(imageBytes);
							imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(profile.ImageUpload.ContentType);
							content.Add(imageContent, "init_image", profile.ImageUpload.FileName);

							var response = await client.PostAsync("https://api.stability.ai/v1/generation/stable-diffusion-xl-1024-v1-0/image-to-image", content);

							if (response.IsSuccessStatusCode)
							{
								var json = await response.Content.ReadAsStringAsync();
								using (JsonDocument doc = JsonDocument.Parse(json))
								{
									profile.GeneratedImageBase64 = doc.RootElement.GetProperty("artifacts")[0].GetProperty("base64").GetString();
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", "Error generating image: " + ex.Message);
				}
			}
			else
			{
				ModelState.AddModelError("", "Please upload an image to transform.");
				return View("ImageGenerator", profile);
			}

			// Return the separate result view for images
			return View("ImageResult", profile);
		}
	}
}