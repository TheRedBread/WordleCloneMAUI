using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace WordleCloneMAUI.Services;

public class DictionaryService
{
    private readonly HttpClient _httpClient;

    public DictionaryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> DoesWordExist(string word)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "https://wordotron.com/api/v1/check-word",
            new { word });

        if (!response.IsSuccessStatusCode)
            return false;

        var result = await response.Content
            .ReadFromJsonAsync<WordCheckResult>();

        return result?.Valid ?? false; 
    }
    public async Task<string?> GetRandomFiveLetterWord()
    {
        var words = await _httpClient.GetFromJsonAsync<List<string>>(
            "https://random-word-api.herokuapp.com/word?number=1&length=5");

        return words?.FirstOrDefault();
    }
}
public class WordCheckResult
{
    public bool Valid { get; set; }
}
