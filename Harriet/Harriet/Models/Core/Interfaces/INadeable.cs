
namespace Harriet.Models.Core
{
    /// <summary>撫でた値を保持しているモノを定義します。</summary>
    public interface INadeable
    {
        /// <summary>現在までに積算された撫で値を表します。</summary>
        double Nadenade { get; set; }

    }
}
