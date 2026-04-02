using System.Collections.Generic;
using cTabWebApp.Recording;

namespace cTabWebApp.Models
{
    public class ReplayVM
    {
        public IReadOnlyList<StoredRecording> Recordings { get; set; } = [];
    }
}
