using BPWA.Common.Extensions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BPWA.Web.Services.Models
{
    public class DropdownItem : DropdownItem<int> { }

    public class DropdownItem<TKey>
    {
        public TKey Id { get; set; }
        public string Text { get; set; }
    }

    public static class DropdownItemExtensions
    {
        public static List<int> GetIds(this List<DropdownItem> list) => list?.Select(x => x.Id)?.ToList() ?? new List<int>();
        public static List<TKey> GetIds<TKey>(this List<DropdownItem<TKey>> list) => list?.Select(x => x.Id)?.ToList() ?? new List<TKey>();
        public static string GetDropdownItemsJson(this List<DropdownItem> list) => list.IsEmpty() ? "" : JsonConvert.SerializeObject(list.Select(x => new { id = x.Id, text = x.Text }).ToList());
        public static string GetDropdownItemsJson<TKey>(this List<DropdownItem<TKey>> list) => list.IsEmpty() ? "" : JsonConvert.SerializeObject(list.Select(x => new { id = x.Id, text = x.Text }).ToList());
    }
}
