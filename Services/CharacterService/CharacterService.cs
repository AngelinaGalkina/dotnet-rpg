namespace dotnet_rpg.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper Mapper;

        public readonly DataContext Context;
        
        private readonly IHttpContextAccessor HttpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            Mapper = mapper;
            Context = context;
            HttpContextAccessor = httpContextAccessor;
        }

        //private int GetUserId() => int.TryParse();

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = Mapper.Map<Character>(newCharacter);

            
            Context.Characters.Add(character);
            await Context.SaveChangesAsync();

            serviceResponse.Data = 
                 await Context.Characters.Select(c => Mapper.Map<GetCharacterDto>(c)).ToListAsync();

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteAllCharacters(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var character = await Context.Characters.FirstOrDefaultAsync(c => c.Id == id);

                if (character is null)
                    throw new Exception($"Character with Id '{id}' not found.");

                Context.Characters.Remove(character);

                await Context.SaveChangesAsync();

                serviceResponse.Data = 
                    await Context.Characters.Select(c => Mapper.Map<GetCharacterDto>(c)).ToListAsync();

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters(int userId)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacter = await Context.Characters.Where(c => c.User!.Id == userId).ToListAsync();

            serviceResponse.Data = dbCharacter.Select(c => Mapper.Map<GetCharacterDto>(c)).ToList();

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await Context.Characters.FirstOrDefaultAsync(c => c.Id == id);

            serviceResponse.Data = Mapper.Map<GetCharacterDto>(dbCharacter);

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = 
                    await Context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                if (character is null)
                    throw new Exception($"Character with Id '{updatedCharacter.Id}' not found.");

                Mapper.Map(updatedCharacter, character);

                await Context.SaveChangesAsync();
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
