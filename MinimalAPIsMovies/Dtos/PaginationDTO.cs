namespace MinimalAPIsMovies.Dtos
{
    public class PaginationDTO
    {
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
    }
}
