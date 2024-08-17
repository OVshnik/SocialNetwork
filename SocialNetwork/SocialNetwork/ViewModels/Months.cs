using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.ViewModels
{
    public enum Months
    {
        [Display(Name="Январь")]
        Jan=1,
        Feb,
        Mar,
        Apr,
        May,
        Jun,
        Jul,
        Aug,
        Sep,
        Oct,
        Nov,
        Dec
    }
}
