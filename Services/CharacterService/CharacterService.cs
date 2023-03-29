
using dotnet_rpg.Models;

namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character>() {
            new Character(),
            new Character{Id = 1, Name = "Sam"}
        };

        private readonly IMapper Mapper;

        public CharacterService(IMapper mapper)
        {
            Mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var charactre = Mapper.Map<Character>(newCharacter);

            charactre.Id = characters.Max(c => c.Id) + 1;
            characters.Add(charactre);
            serviceResponse.Data = characters.Select(c => Mapper.Map<GetCharacterDto>(c)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteAllCharacters(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = characters.First(c => c.Id == id);

                if (character is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                characters.Remove(character);

                serviceResponse.Data = characters.Select(c => Mapper.Map<GetCharacterDto>(c)).ToList();

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            serviceResponse.Data = characters.Select(c => Mapper.Map<GetCharacterDto>(c)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c => c.Id == id);

            serviceResponse.Data = Mapper.Map<GetCharacterDto>(character);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = characters.FirstOrDefault(c => c.Id == updatedCharacter.Id);

                if (character is null)
                    throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");

                Mapper.Map(updatedCharacter, character);

                serviceResponse.Data = Mapper.Map<GetCharacterDto>(character);

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
    }
}
