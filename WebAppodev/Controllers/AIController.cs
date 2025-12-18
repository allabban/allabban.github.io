using Microsoft.AspNetCore.Mvc;
using WebAppodev.Models;
using Azure;
using Azure.AI.Inference;
using System;

namespace WebAppodev.Controllers
{
	public class AIController : Controller
	{
		// GET: Show the form
		public IActionResult Index()
		{
			return View(new UserProfile());
		}

		// POST: Process the form
		[HttpPost]
		public async Task<IActionResult> GeneratePlan(UserProfile profile)
		{
			// Note: We remove "ImageUpload" from validation because it's optional
			ModelState.Remove("ImageUpload");

			if (!ModelState.IsValid)
				return View("Index", profile);

			try
			{
				var githubToken = "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"; // Remember to handle secrets safely!
				var endpoint = new Uri("https://models.github.ai/inference");
				var credential = new AzureKeyCredential(githubToken);
				var modelName = "gpt-4o";

				var client = new ChatCompletionsClient(endpoint, credential, new AzureAIInferenceClientOptions());

				// 1. PREPARE THE TEXT PROMPT
				string textPrompt = $"Create a fitness plan for a {profile.Age} year old, " +
									$"{profile.Weight}kg, {profile.Height}cm person. " +
									$"Goal: {profile.Goal}.";

				// 2. CREATE THE MESSAGE CONTENT
				// We start with the text part
				var messageItems = new List<ChatMessageContentItem>
		{
			new ChatMessageTextContentItem(textPrompt)
		};

				// 3. HANDLE IMAGE UPLOAD (If exists)
				if (profile.ImageUpload != null && profile.ImageUpload.Length > 0)
				{
					using (var memoryStream = new MemoryStream())
					{
						await profile.ImageUpload.CopyToAsync(memoryStream);
						byte[] bytes = memoryStream.ToArray();
						string base64Image = Convert.ToBase64String(bytes);

						// Create the image content item
						// format: "data:image/jpeg;base64,....."
						string imageUrl = $"data:{profile.ImageUpload.ContentType};base64,{base64Image}";

						// Add image to the message list
						messageItems.Add(new ChatMessageImageContentItem(new Uri(imageUrl), ChatMessageImageDetailLevel.Low));

						// Append text to tell AI what to do with the image
						messageItems.Add(new ChatMessageTextContentItem(" Also, analyze the uploaded body photo to suggest specific posture corrections or focus areas."));
					}
				}

				// 4. SEND REQUEST
				var requestOptions = new ChatCompletionsOptions()
				{
					Messages =
			{
				new ChatRequestSystemMessage("You are an expert fitness trainer."),
                // The User Message now takes the LIST of items (Text + Image)
                new ChatRequestUserMessage(messageItems)
			},
					Model = modelName
				};

				Response<ChatCompletions> response = await client.CompleteAsync(requestOptions);
				profile.AIResponse = response.Value.Content;
			}
			catch (Exception ex)
			{
				profile.AIResponse = $"Error: {ex.Message}";
			}

			return View("Result", profile);
		}
	}
}