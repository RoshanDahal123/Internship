using VideoGameCharacterApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
namespace VideoGameCharacterApi.Services
{
    public class VideoGameCharacterService:IVideoGameCharacterService
    {
        static List<Character> characters = new List<Character>
    {
        new Character { Id= 1, Name = "mario", Game = "Super mario Bros", Role = "Protagonist"},
        new Character  { Id = 3, Name = "Bowser", Game = "Super Mario Bros", Role = "Antagonist" },
        new Character { Id = 4, Name = "Princess Peach", Game = "Super Mario Bros", Role = "Supporting" },
    };
        public async Task<List<Character>> GetAllCharactersAsync() => await Task.FromResult(characters);



        public async Task<Character?> GetCharacterByIdAsync(int id)
        {
            var result = characters.FirstOrDefault(c => c.Id == id);
            return await Task.FromResult(result);
        }

        public async Task<Character> AddCharacterAsync(Character character)
        {
            
            throw new NotImplementedException ();

        }

        public async Task<bool> UpdateCharacterAsync(int id,Character character)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteCharacterAsync(int id)
        {
            throw new NotImplementedException();
        }



    }
}
