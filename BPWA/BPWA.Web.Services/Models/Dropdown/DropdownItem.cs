using BPWA.Common.Extensions;
using Newtonsoft.Json;
using System;
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
        public static string GetDropdownItemJson(this Enum enumeration) => enumeration == null ? "" : JsonConvert.SerializeObject(new { id = enumeration.ToString(), text = TranslationsHelper.Translate(enumeration.ToString()) });
        public static string GetDropdownItemJson(this DropdownItem item) => item == null ? "" : JsonConvert.SerializeObject(new { id = item.Id, text = item.Text });
        public static string GetDropdownItemsJson(this List<DropdownItem> list) => list.IsEmpty() ? "" : JsonConvert.SerializeObject(list.Select(x => new { id = x.Id, text = x.Text }).ToList());
        public static string GetDropdownItemJson<TKey>(this DropdownItem<TKey> item) => item == null ? "" : JsonConvert.SerializeObject(new { id = item.Id, text = item.Text });
        public static string GetDropdownItemsJson<TKey>(this List<DropdownItem<TKey>> list) => list.IsEmpty() ? "" : JsonConvert.SerializeObject(list.Select(x => new { id = x.Id, text = x.Text }).ToList());
    }
}
