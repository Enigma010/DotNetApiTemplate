using System.ComponentModel.DataAnnotations;

namespace Db
{
    public class Paging
    {
        /// <summary>
        /// Create the default, initial page
        /// </summary>
        public Paging() { }

        /// <summary>
        /// The default page size
        /// </summary>
        public const int DefaultPageSize = 25;
        
        /// <summary>
        /// The page number that you're currently on
        /// </summary>
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;
        
        /// <summary>
        /// The page size that you want to take so, by default you
        /// take 25 items
        /// </summary>
        [Range (1, int.MaxValue)]
        public int PageSize { get; set; } = DefaultPageSize;
    }
}
