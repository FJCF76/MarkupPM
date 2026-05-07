using MarkupPM.Models;

namespace MarkupPM.Services;

public interface IMdSerializer
{
    string Serialize(Proyecto proyecto);
}
