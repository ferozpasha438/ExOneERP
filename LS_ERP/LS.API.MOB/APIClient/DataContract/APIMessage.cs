using System.Collections.Generic;

namespace LS.API
{
    public class APIMessageDto<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }

    }
    public class APIListMessageDto<T>
    {
        public T Data { get; set; }
        public string Message { get; set; }

    }


    public class APIMessage<T>
    {
        public T Data { get; set; }
    }
    public class APIListMessage<T>
    {
        public PaginatedList<T> data { get; set; }
    }

    public class PaginatedList<T>
    {
        public List<T> items { get; set; }
        public int pageIndex { get; set; }
        public int totalPages { get; set; }
        public int totalCount { get; set; }
    }
}
