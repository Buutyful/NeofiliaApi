using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeofiliaDomain;

public class PubMenu
{
    public int Id { get; private set; }
    public int PubId { get; private set; }

    [Url]
    public Uri? PdfUrl { get; private set; }

    [ForeignKey("PubId")]
    public Pub Pub { get; private set; } = null!;

    public PubMenu(int pubId, Uri? pdfUrl)
    {
        PubId = pubId;
        PdfUrl = pdfUrl;
    }
}
