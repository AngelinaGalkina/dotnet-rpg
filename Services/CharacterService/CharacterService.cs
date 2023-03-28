
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
    }
}
