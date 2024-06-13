using Microsoft.IdentityModel.Tokens;
using MinimalAPIsMovies.Extensions;

namespace MinimalAPIsMovies.Dtos
{
    public class PaginationDTO
    {
        private const int pageInitialValue = 1;
        private const int recordPerPageInitialValue = 10;
        public int Page { get; set; } = 1;
        private int _recordsPerPage { get; set; } = 10;
        private readonly int _recordsPerPageMax = 50;

        public int RecordsPerPage
        {
            get
            {
                return _recordsPerPage;
            }
            set
            {
                if(value > _recordsPerPageMax)
                {
                    _recordsPerPage = _recordsPerPageMax;
                }
                else
                {
                    _recordsPerPage = value;
                }
            }
        }

        public static ValueTask<PaginationDTO> BindAsync(HttpContext context)
        {
            var page = context.ExtractValueOrDefault(nameof(Page),pageInitialValue);
            var recordPerPage = context.ExtractValueOrDefault(nameof(RecordsPerPage),recordPerPageInitialValue);

            var response = new PaginationDTO { Page = page,RecordsPerPage = recordPerPage };  
            return new ValueTask<PaginationDTO>(response);  
        }
    }
}
