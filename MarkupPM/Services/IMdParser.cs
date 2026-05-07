using MarkupPM.Models;

namespace MarkupPM.Services;

public interface IMdParser
{
    Proyecto Parse(string markdown);
}
