namespace ZambeziDigital.AspNetCore.Implementations.Generics.Services;

// public class AttachmentService(IServiceScopeFactory serviceScopeFactory)
//     : BaseService<Attachment, AttachmentDto, string>(serviceScopeFactory), IAttachmentService
// {
//     HttpClient _httpClient = serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient("Auth");
//
//     public async Task<Attachment> Update(string dbImageId, AttachmentDto entityImage)
//     {
//         var result = await _httpClient.PutAsJsonAsync($"api/Attachment/{dbImageId}", entityImage);
//         if (result.IsSuccessStatusCode)
//             return await result.Content.ReadFromJsonAsync<Attachment>() ?? throw new Exception("Could not update attachment");
//         var error = await result.Content.ReadFromJsonAsync<BasicResult>();
//         throw new Exception(error?.Errors?[0] ?? result.ReasonPhrase);
//         
//     }
//
//
// }
