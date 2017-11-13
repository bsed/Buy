using JiebaNet.Segmenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Buy.Models;
namespace Buy.Bll
{
    public static class Keywords
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void Add(string text)
        {
            var segmenter = new JiebaSegmenter();
            var result = segmenter.CutForSearch(text)
                .GroupBy(s => s)
                .Where(s => s.Key.Length > 1)
                .Select(s => new { Key = s.Key, Count = s.Count() })
                .ToList();
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            var temp = result.Select(s => s.Key).ToList();
            var keys = db.Keywords.Where(s => temp.Contains(s.Word)).ToList();
            var words = keys.Select(s => s.Word).ToList();
            foreach (var item in keys)
            {
                item.CouponNameCount += result.FirstOrDefault(s => s.Key == item.Word)?.Count ?? 0;
            }
            var addKeys = result.Where(s => !words.Contains(s.Key)).Select(s => new Keyword
            {
                CouponNameCount = s.Count,
                Word = s.Key
            }).ToList();
            db.Keywords.AddRange(addKeys);
            db.SaveChanges();
            //}
        }

        public static void UpdateSearchCount(string text)
        {
            var segmenter = new JiebaSegmenter();
            var result = segmenter.CutForSearch(text)
                .GroupBy(s => s)
                .Where(s => s.Key.Length > 1)
                .Select(s => new { Key = s.Key, Count = s.Count() })
                .ToList();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var temp = result.Select(s => s.Key).ToList();
                var keys = db.Keywords.Where(s => temp.Contains(s.Word)).ToList();
                foreach (var item in keys)
                {
                    item.SearchCount += result.FirstOrDefault(s => s.Key == item.Word).Count;
                }
                db.SaveChanges();
            }
        }

        public static IEnumerable<string> Split(string text)
        {
            var segmenter = new JiebaSegmenter();
            return segmenter.CutForSearch(text);
        }

        public static List<Keyword> HotKeyword()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var keys = db.Keywords.OrderByDescending(s => s.SearchCount)
                    .ThenByDescending(s => s.CouponNameCount)
                    .Take(10)
                    .ToList();
                return keys;
            }
        }
    }


}